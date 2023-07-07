# Release Note - Version 0.1-alpha1

## Features

- Securely delete files from a specified folder.
- Support for specifying a time limit to delete files based on their age.
- Option to provide a file name pattern to match specific files for deletion.
- Exclude specific files from deletion using a comma-separated list.
- Ability to search for files in all subfolders recursively.
- No prompt mode to enable automatic file deletion without confirmation.
- Simulate mode to test the file deletion process without actually deleting files.
- Display usage information with the `--help` or `-h` parameter.
- Display the application version with the `--version` parameter.

## Usage

```bash
safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>] [--recursive] [--no_prompt] [--simulate]
safedelete -f <folder_path> [-t <minutes>] [-p <file_pattern>] [-e <file1,file2,...>] [-r] [-np] [-s]

## Parameters

- `--folder`, `-f`: The path of the folder to delete files from. (required)
- `--time_limit`, `-t`: The time limit (in minutes) specifying the maximum file age. (optional)
- `--pattern`, `-p`: The file name pattern to match. (optional)
- `--exclude_files`, `-e`: Comma-separated list of files to exclude from deletion. (optional)
- `--recursive`, `-r`: Search for files in all subfolders. (optional)
- `--no_prompt`, `-np`: Enable no prompt mode. Files will be securely deleted without confirmation. (optional)
- `--simulate`, `-s`: Enable simulate mode. Files will not be securely deleted, they will be simulated. (optional)
- `--help`, `-h`: Display usage information. (optional)
- `--version`: Display the application version. (optional)


Please note that this is an alpha version of the SecureFileDelete application and may contain bugs or incomplete features. Use it with caution.

Thank you for using SecureFileDelete version 0.1-alpha1! We appreciate your feedback and contributions to improve the application.

If you have any questions or need assistance, please reach out to our support team.