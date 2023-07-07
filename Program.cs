using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

class SecureFileDelete
{
    // Define the version of the app
    private static readonly string AppVersion = typeof(SecureFileDelete).Assembly.GetName().Version.ToString();
    private static int progressBarWidth = 40; // Width of the progress bar
    static void Main(string[] args)
    {
        if (args.Contains("--version"))
        {
            Console.WriteLine("App Version: [ALPHA]" + AppVersion);
            return;
        }
        // Parse command line arguments
        CommandLineArguments arguments = ParseArguments(args);

        if (arguments.ContainsParameter("help") || arguments.ContainsParameter("h"))
        {
            Usage();
            return;
        }

        // Check if the folder parameter is provided
        if (!(arguments.ContainsParameter("folder") || arguments.ContainsParameter("f")))
        {
            Console.WriteLine("Missing required parameter: --folder or -f");
            Usage();
            return;
        }

        

        // Perform secure file deletion
        PerformSecureFileDeletion(arguments);
    }

    static void Usage()
    {

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Usage: safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>] [--recursive]");
        Console.WriteLine("       safedelete -f <folder_path> [-t <minutes>] [-p <file_pattern>] [-e <file1,file2,...>] [-r]");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Parameters:");
        Console.WriteLine("  --folder, -f          : The path of the folder to delete files from. (required)");
        Console.WriteLine("  --time_limit, -t      : The time limit (in minutes) specifying the maximum file age. (optional)");
        Console.WriteLine("  --pattern, -p         : The file name pattern to match. (optional)");
        Console.WriteLine("  --exclude_files, -e   : Comma-separated list of files to exclude from deletion. (optional)");
        Console.WriteLine("  --recursive, -r       : Search for files in all subfolders. (optional)");
        Console.WriteLine("  --no_prompt, -np      : Enable no prompt mode. Files will be securely deleted without confirmation. (optional)");
        Console.WriteLine("  --simulate, -s        : Enable simulate mode. Files will not be securely deleted, they will be simulated. (optional)");
        Console.WriteLine("  --help, -h            : Display usage information. (optional)");
        Console.ForegroundColor = ConsoleColor.White;
    }



    static void PerformSecureFileDeletion(CommandLineArguments arguments)
    {
        // Map short parameter aliases to their corresponding full parameter names
        Dictionary<string, string> shortToFull = new Dictionary<string, string>()
    {
        { "f", "folder" },
        { "t", "time_limit" },
        { "p", "pattern" },
        { "e", "exclude_files" },
        { "r", "recursive" },
        { "h", "help"},
        { "np", "no_prompt" },
        { "s", "simulate" } // Added "s" as a short alias for simulate
    };

        // Check short parameters and update the corresponding full parameter name
        foreach (var entry in shortToFull)
        {
            string shortParam = entry.Key;
            string fullParam = entry.Value;

            if (arguments.ContainsParameter(shortParam))
            {
                arguments.AddParameter(fullParam, arguments.GetValue<string>(shortParam));
                arguments.RemoveParameter(shortParam);
            }
        }

        string folder = arguments.ContainsParameter("folder") ? arguments.GetValue<string>("folder") : string.Empty;
        int? timeLimit = arguments.ContainsParameter("time_limit") ? arguments.GetValue<int>("time_limit") : null;
        string pattern = arguments.ContainsParameter("pattern") ? arguments.GetValue<string>("pattern") : string.Empty;
        List<string> excludeFiles = arguments.ContainsParameter("exclude_files") ? arguments.GetValue<string>("exclude_files").Split(',').ToList() : new List<string>();
        bool recursive = arguments.ContainsParameter("recursive");
        bool noPromptMode = arguments.ContainsParameter("no_prompt");
        bool simulate = arguments.ContainsParameter("simulate"); // Check if simulate parameter is provided

        // Perform secure file deletion
        PerformSecureFileDeletion(folder, timeLimit, pattern, excludeFiles, recursive, noPromptMode, simulate);
    }


    static CommandLineArguments ParseArguments(string[] args)
    {
        if (args.Length == 0)
        {
            Usage();
            Environment.Exit(1);
        }

        CommandLineArguments arguments = new CommandLineArguments();

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];

            if (arg.StartsWith("--") || arg.StartsWith("-"))
            {
                string parameterName = arg.StartsWith("--") ? arg.Substring(2) : arg.Substring(1);

                if (i + 1 < args.Length && !args[i + 1].StartsWith("--") && !args[i + 1].StartsWith("-"))
                {
                    string parameterValue = args[i + 1];
                    arguments.AddParameter(parameterName, parameterValue);
                    i++; // Skip the next argument as it is the value for the current parameter
                }
                else
                {
                    // Handle boolean parameters with no value
                    arguments.AddParameter(parameterName, string.Empty);
                }
            }
        }

        return arguments;
    }


    class CommandLineArguments
    {
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        public void AddParameter(string name, string value)
        {
            parameters[name] = value;
        }

        public bool ContainsParameter(string name)
        {
            return parameters.ContainsKey(name);
        }

        public T GetValue<T>(string name)
        {
            if (parameters.TryGetValue(name, out string value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (FormatException)
                {
                    // Handle invalid value format error
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Invalid value format for parameter: {name}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Usage();
                    Environment.Exit(1);
                }
                catch (InvalidCastException)
                {
                    // Handle invalid value type error
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Invalid value type for parameter: {name}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Usage();
                    Environment.Exit(1);
                }
            }

            return default;
        }

        public void RemoveParameter(string name)
        {
            parameters.Remove(name);
        }
    }

    static void PerformSecureFileDeletion(string folder, int? timeLimit, string pattern, List<string> excludeFiles, bool recursive, bool noPromptMode, bool simulate)
    {
        try
        {
            // Get the files in the folder (and subfolders if recursive is true) that match the specified criteria
            DirectoryInfo directory = new DirectoryInfo(folder);
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            List<FileInfo> files = new List<FileInfo>();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("### Files to be securely deleted ###");

            ProcessFolder(directory, files, timeLimit, pattern, excludeFiles, searchOption);
            
            // Process subfolders recursively
            foreach (var subDir in directory.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    ProcessFolder(subDir, files, timeLimit, pattern, excludeFiles, searchOption);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Access denied: {subDir.FullName}");
                    // Continue processing other subfolders
                }
            }

            // Remove duplicates from the files list
            files = files.Distinct(new FileInfoEqualityComparer()).ToList();


            if (files.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No files found matching the specified criteria.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            else
            {
                foreach (FileInfo file in files)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen; 
                    Console.WriteLine(file.FullName);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"#Files found matching the specified criteria: {files.Count}");

                long totalSize = files.Sum(file => file.Length);
                Console.WriteLine($"Total size of files to be deleted: {FormatSize(totalSize)}");
            }
            
            

            string confirmation = "y";
            if (!noPromptMode)
            {
                // Confirm file deletion
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Are you sure you want to delete these files? (yes/no): ");
                confirmation = Console.ReadLine()?.Trim().ToLower();
            }

            if (confirmation.Contains("y"))
            {
                int numFiles = files.Count;
                int filesDeleted = 0;
                int skippedFolders = 0;

                // Initialize the progress bar
                Console.Write("[");
                Console.CursorVisible = false;

                //foreach (FileInfo file in files)
                Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (file, state) =>
                {
                    try
                    {
                        if (!simulate)
                        {
                            SecureDeleteFile(file.FullName);
                        }
                        int currentFilesDeleted = Interlocked.Increment(ref filesDeleted);

                        // Update the progress bar
                        UpdateProgressBar(currentFilesDeleted, numFiles);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Interlocked.Increment(ref skippedFolders);
                    }
                });

                // Complete the progress bar
                Console.WriteLine("] Done");
                Console.CursorVisible = true;

                if (skippedFolders > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Skipped {skippedFolders} folder(s) due to access errors.");
                }

                if (recursive)
                {
                    DeleteEmptySubfolders(folder);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("File deletion canceled.");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Error: " + ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    static void ProcessFolder(DirectoryInfo folder, List<FileInfo> files, int? timeLimit, string pattern, List<string> excludeFiles, SearchOption searchOption)
    {
        try
        {
            FileInfo[] folderFiles = folder.GetFiles(pattern, searchOption)
                .Where(file => !excludeFiles.Contains(file.Name)).ToArray();

            if (timeLimit != null)
            {
                int timeLimitValue = (int)timeLimit;
                folderFiles = folderFiles
                    .Where(file => file.CreationTime > DateTime.Now.AddMinutes(-timeLimitValue)).ToArray();
            }

            files.AddRange(folderFiles);

            foreach (var subfolder in folder.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    ProcessFolder(subfolder, files, timeLimit, pattern, excludeFiles, searchOption);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Access denied: {subfolder.FullName}");
                    Console.ForegroundColor = ConsoleColor.White;
                    // Continue processing other subfolders
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Access denied: {folder.FullName}");
            Console.ForegroundColor = ConsoleColor.White;
            // Continue processing other subfolders
        }
    }

    static object lockObject = new object();

    static void UpdateProgressBar(int currentFilesDeleted, int totalFiles)
    {
        double progress = (double)currentFilesDeleted / totalFiles;
        int progressBarLength = (int)(progressBarWidth * progress);
        string progressBar = new string('#', progressBarLength).PadRight(progressBarWidth);

        lock (lockObject)
        {
            Console.SetCursorPosition(1, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(progressBar);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }


    class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return string.Equals(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }

    static string FormatSize(long size)
    {
        string[] suffixes = { "B", "Ko", "Mo", "Go", "To" };
        int suffixIndex = 0;
        double formattedSize = size;

        while (formattedSize >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            formattedSize /= 1024;
            suffixIndex++;
        }

        return $"{formattedSize:0.##} {suffixes[suffixIndex]}";
    }

    static void SecureDeleteFile(string filePath)
    {
        try
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File does not exist.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            // Remove the read-only attribute if it's set
            if (File.GetAttributes(filePath).HasFlag(FileAttributes.ReadOnly))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            // Get the directory of the original file
            string fileDirectory = Path.GetDirectoryName(filePath);

            // Create a temporary file path in the same directory with the random file name
            string tempFilePath = filePath;

           
            // Perform 7 iterations of secure deletion steps
            for (int i = 0; i < 7; i++)
            {
                // Generate a new random file name for the next iteration
                string randomFileName = Path.GetRandomFileName();
                randomFileName = Path.Combine(fileDirectory, randomFileName);

                // Create a new file with the random file name
                File.Move(tempFilePath, randomFileName);

                tempFilePath = randomFileName;
            }

            // Get the file size
            long fileSize = new FileInfo(tempFilePath).Length;

            // Overwrite the file content with multiple steps and algorithms
            using (FileStream stream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Write))
            {
                // Step 1: Fill with random data
                OverwriteWithRandomData(stream, fileSize);
                stream.Seek(0, SeekOrigin.Begin);

                // Step 2: Fill with null values
                OverwriteWithNullValues(stream, fileSize);
                stream.Seek(0, SeekOrigin.Begin);

                // Step 3: Fill with zero values
                OverwriteWithZeroValues(stream, fileSize);
                stream.Seek(0, SeekOrigin.Begin);

                // Step 4: Fill with random data again
                OverwriteWithRandomData(stream, fileSize);
                stream.Seek(0, SeekOrigin.Begin);

                // Step 5: Fill with alternating pattern (0xFF and 0x00)
                OverwriteWithAlternatingPattern(stream, fileSize);
                stream.Seek(0, SeekOrigin.Begin);

                // Step 6: Fill with random data using encryption algorithm
                OverwriteWithEncryptedRandomData(stream, fileSize);
                stream.Seek(0, SeekOrigin.Begin);

                // Step 7: Fill with null values again
                OverwriteWithNullValues(stream, fileSize);
                stream.Seek(0, SeekOrigin.Begin);
            }

            // Delete the renamed file
            File.Delete(tempFilePath);

            //Console.WriteLine("File securely deleted.");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Error: " + ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }


    static string GetParameterValue(Dictionary<string, string> arguments, string parameter)
    {
        return arguments.ContainsKey(parameter) ? arguments[parameter] : string.Empty;
    }

    static void OverwriteWithRandomData(FileStream stream, long fileSize)
    {
        byte[] buffer = new byte[4096];
        Random random = new Random();
        long remainingSize = fileSize;

        while (remainingSize > 0)
        {
            random.NextBytes(buffer);

            int bytesToWrite = (int)Math.Min(remainingSize, buffer.Length);
            stream.Write(buffer, 0, bytesToWrite);

            remainingSize -= bytesToWrite;
        }
    }

    static void OverwriteWithNullValues(FileStream stream, long fileSize)
    {
        byte[] buffer = new byte[4096];
        long remainingSize = fileSize;

        while (remainingSize > 0)
        {
            int bytesToWrite = (int)Math.Min(remainingSize, buffer.Length);
            stream.Write(buffer, 0, bytesToWrite);

            remainingSize -= bytesToWrite;
        }
    }

    static void OverwriteWithZeroValues(FileStream stream, long fileSize)
    {
        byte[] buffer = new byte[4096];
        long remainingSize = fileSize;

        while (remainingSize > 0)
        {
            Array.Clear(buffer, 0, buffer.Length);

            int bytesToWrite = (int)Math.Min(remainingSize, buffer.Length);
            stream.Write(buffer, 0, bytesToWrite);

            remainingSize -= bytesToWrite;
        }
    }

    static void OverwriteWithAlternatingPattern(FileStream stream, long fileSize)
    {
        byte[] buffer = new byte[4096];
        long remainingSize = fileSize;
        byte value = 0xFF;

        while (remainingSize > 0)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = value;
                value = (byte)(~value);
            }

            int bytesToWrite = (int)Math.Min(remainingSize, buffer.Length);
            stream.Write(buffer, 0, bytesToWrite);

            remainingSize -= bytesToWrite;
        }
    }

    static void OverwriteWithEncryptedRandomData(FileStream stream, long fileSize)
    {
        byte[] buffer = new byte[4096];
        long remainingSize = fileSize;

        using (AesManaged aes = new AesManaged())
        {
            aes.GenerateKey();
            aes.GenerateIV();

            while (remainingSize > 0)
            {
                byte[] randomData = new byte[buffer.Length];
                RandomNumberGenerator.Fill(randomData);

                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(randomData, 0, randomData.Length);
                    cryptoStream.FlushFinalBlock();

                    memoryStream.Position = 0;
                    int bytesRead = memoryStream.Read(buffer, 0, buffer.Length);

                    int bytesToWrite = (int)Math.Min(remainingSize, bytesRead);
                    stream.Write(buffer, 0, bytesToWrite);

                    remainingSize -= bytesToWrite;
                }
            }
        }
    }

    static void DeleteEmptySubfolders(string rootFolder)
    {
        string[] subfolders = Directory.GetDirectories(rootFolder);

        foreach (string subfolder in subfolders)
        {
            DeleteEmptySubfolders(subfolder);

            if (Directory.GetFiles(subfolder).Length == 0 && Directory.GetDirectories(subfolder).Length == 0)
            {
                Directory.Delete(subfolder);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Empty subfolder deleted: " + subfolder);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

}
