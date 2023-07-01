using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

class SecureFileDelete
{

    static void Main(string[] args)
    {
        // Parse command line arguments
        CommandLineArguments arguments = ParseArguments(args);

        // Check if the folder parameter is provided
        if (!arguments.ContainsParameter("folder"))
        {
            Console.WriteLine("Missing required parameter: --folder");
            Usage();
            Environment.Exit(1);
        }

        // Perform secure file deletion
        PerformSecureFileDeletion(arguments);
    }

    static void Usage()
    {
        Console.WriteLine("Usage: safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>]");
        Console.WriteLine("Example: safedelete --folder C:\\Data --time_limit 60 --pattern *.txt --exclude_files file1.txt,file2.txt");
        Console.WriteLine();
        Console.WriteLine("Parameters:");
        Console.WriteLine("  --folder         : The path of the folder to delete files from. (required)");
        Console.WriteLine("  --time_limit     : The time limit (in minutes) specifying the maximum file age. (optional)");
        Console.WriteLine("  --pattern        : The file name pattern to match. (optional)");
        Console.WriteLine("  --exclude_files  : Comma-separated list of files to exclude from deletion. (optional)");
    }


    static void PerformSecureFileDeletion(CommandLineArguments arguments)
    {
        string folder = arguments.ContainsParameter("folder") ? arguments.GetValue<string>("folder") : string.Empty;
        int? timeLimit = arguments.ContainsParameter("time_limit") ? arguments.GetValue<int>("time_limit") : null;
        string pattern = arguments.ContainsParameter("pattern") ? arguments.GetValue<string>("pattern") : string.Empty;
        List<string> excludeFiles = arguments.ContainsParameter("exclude_files") ? arguments.GetValue<string>("exclude_files").Split(',').ToList() : new List<string>();

        // Perform secure file deletion
        PerformSecureFileDeletion(folder, timeLimit, pattern, excludeFiles);
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

            if (arg.StartsWith("--"))
            {
                string parameterName = arg.Substring(2);

                if (i + 1 < args.Length)
                {
                    string parameterValue = args[i + 1];
                    arguments.AddParameter(parameterName, parameterValue);
                }
                else
                {
                    // Handle missing value for parameter error
                    Console.WriteLine($"Missing value for parameter: {parameterName}");
                    Environment.Exit(1);
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
                    Console.WriteLine($"Invalid value format for parameter: {name}");
                    Usage();
                    Environment.Exit(1);
                }
                catch (InvalidCastException)
                {
                    // Handle invalid value type error
                    Console.WriteLine($"Invalid value type for parameter: {name}");
                    Usage();
                    Environment.Exit(1);
                }
            }

            return default;
        }
    }

    static void PerformSecureFileDeletion(string folder, int? timeLimit, string pattern, List<string> excludeFiles)
    {
        try
        {
            // Get the files in the folder that match the specified criteria
            DirectoryInfo directory = new DirectoryInfo(folder);
            FileInfo[] files;
            int timeLimitValue;

            if (timeLimit != null)
            {
                timeLimitValue = (int)timeLimit;
                files = directory.GetFiles()
                    .Where(file => file.CreationTime > DateTime.Now.AddMinutes(-timeLimitValue) && file.Name.Contains(pattern) && !excludeFiles.Contains(file.Name))
                    .ToArray();
            }
            else
            {
                files = directory.GetFiles()
                    .Where(file => file.Name.Contains(pattern) && !excludeFiles.Contains(file.Name))
                    .ToArray();
            }

            if (files.Length == 0)
            {
                Console.WriteLine("No files found matching the specified criteria.");
                return;
            }

            Console.WriteLine("### Files to be securely deleted ###");
            foreach (FileInfo file in files)
            {
                Console.WriteLine(file.FullName);
            }

            foreach (FileInfo file in files)
            {
                SecureDeleteFile(file.FullName);
                Console.WriteLine("File securely deleted: " + file.FullName);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    /*static void SecureDeleteFile(string filePath)
    {
        // Step 1: Fill with random data
        // Step 2: Fill with null values
        // Step 3: Fill with zero values
        // Step 4: Fill with random data again
        // Step 5: Fill with alternating pattern (0xFF and 0x00)
        // Step 6: Fill with random data using encryption algorithm
        // Step 7: Fill with null values again
        // Step 8: Delete the file permanently
        try
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: File does not exist.");
                return;
            }

            // Get the file size
            long fileSize = new FileInfo(filePath).Length;

            // Overwrite the file content with multiple steps and algorithms
            //using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Seek(0, 0);
                stream.Flush();
                // Step 1: Fill with random data
                OverwriteWithRandomData(stream, fileSize);
                stream.Seek(0, 0);
                stream.Flush();
                // Step 2: Fill with null values
                OverwriteWithNullValues(stream, fileSize);
                stream.Seek(0, 0);
                stream.Flush();
                // Step 3: Fill with zero values
                OverwriteWithZeroValues(stream, fileSize);
                stream.Seek(0, 0);
                stream.Flush();
                // Step 4: Fill with random data again
                OverwriteWithRandomData(stream, fileSize);
                stream.Seek(0, 0);
                stream.Flush();
                // Step 5: Fill with alternating pattern (0xFF and 0x00)
                OverwriteWithAlternatingPattern(stream, fileSize);
                stream.Seek(0, 0);
                stream.Flush();
                // Step 6: Fill with random data using encryption algorithm
                OverwriteWithEncryptedRandomData(stream, fileSize);
                stream.Seek(0, 0);
                stream.Flush();
                // Step 7: Fill with null values again
                OverwriteWithNullValues(stream, fileSize);

                stream.Flush();
            }

            // Delete the file
            File.Delete(filePath);

            Console.WriteLine("File securely deleted: " + filePath);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }*/

    static void SecureDeleteFile(string filePath)
    {
        try
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: File does not exist.");
                return;
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

            Console.WriteLine("File securely deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
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
}
