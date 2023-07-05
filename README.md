# SafeDelete

SafeDelete is a command-line tool for securely deleting files from a specified folder. It uses secure deletion techniques to prevent file recovery.

## Usage

safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>] [--recursive] [--no_prompt] [--help]
safedelete -f <folder_path> [-t <minutes>] [-p <file_pattern>] [-e <file1,file2,...>] [-r] [-np] [-h]

### Parameters

- `--folder, -f`: The path of the folder to delete files from. (required)
- `--time_limit, -t`: The time limit (in minutes) specifying the maximum file age. (optional)
- `--pattern, -p`: The file name pattern to match. (optional)
- `--exclude_files, -e`: Comma-separated list of files to exclude from deletion. (optional)
- `--recursive, -r`: Search for files in all subfolders. (optional)
- `--no_prompt, -np`: Enable no prompt mode. Files will be securely deleted without confirmation. (optional)
- `--help, -h`: Display usage information. (optional)

### Examples

Delete all files in the folder:
safedelete --folder /path/to/folder

Delete files in the folder with a time limit:
safedelete --folder /path/to/folder --time_limit 30

Delete files with a specific pattern:
safedelete --folder /path/to/folder --pattern "*.txt"

Exclude specific files from deletion:
safedelete --folder /path/to/folder --exclude_files "important.docx,personal.jpg"

Delete files in subfolders:
safedelete --folder /path/to/folder --recursive

Enable no prompt mode:
safedelete --folder /path/to/folder --no_prompt

Display usage information:
safedelete --help

## Security Considerations

SafeDelete uses secure deletion techniques to prevent file recovery. However, ensure that you have the necessary permissions to delete files from the specified folder.

## Disclaimer

SafeDelete is provided as-is without any warranties. Use it at your own risk.
