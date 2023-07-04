# Secure File Delete

Secure File Delete is a command-line application written in C# that securely deletes files from a specified folder. It uses multiple steps and algorithms to overwrite the file content, making it difficult to recover the deleted files.

## Usage

safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>] [--recursive]
safedelete -f <folder_path> [-t <minutes>] [-p <file_pattern>] [-e <file1,file2,...>] [-r]

### Parameters

- `--folder, -f` : The path of the folder to delete files from. (required)
- `--time_limit, -t` : The time limit (in minutes) specifying the maximum file age. (optional)
- `--pattern, -p` : The file name pattern to match. (optional)
- `--exclude_files, -e` : Comma-separated list of files to exclude from deletion. (optional)
- `--recursive, -r` : Search for files in all subfolders. (optional)
- `--help, -h` : Display usage information. (optional)

## Getting Started

1. Clone the repository or download the code.
2. Build the application using Visual Studio or the .NET command-line tools.
3. Open a command prompt or terminal and navigate to the build output directory.
4. Run the `safedelete` command with the desired parameters.

## Example

To delete all text files from the "Documents" folder and its subfolders:
safedelete --folder C:\Documents --pattern *.txt --recursive


This will securely delete all text files in the specified folder and its subfolders.

## How It Works

The application performs the following steps:

1. Parses the command-line arguments and checks for required parameters.
2. Maps short parameter aliases to their corresponding full parameter names.
3. Performs secure file deletion based on the provided parameters.
4. Retrieves the list of files in the specified folder that match the criteria.
5. Displays the list of files to be securely deleted and prompts for confirmation.
6. If confirmed, securely deletes the files using multiple steps and algorithms:
   - Renames the file multiple times with random names.
   - Overwrites the file content with random data, null values, zero values, alternating pattern, encrypted random data, and null values again.
   - Deletes the renamed file.

## License

This project is licensed under the [MIT License](LICENSE). Feel free to modify and distribute it as needed.