# SecureFileDelete

SecureFileDelete is a command-line application that securely deletes files from a specified folder. It uses multiple steps and algorithms to overwrite the file content, making it unrecoverable.

## Usage

safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>] [--recursive] [--no_prompt] [--simulate] [--help] [--version]
or
safedelete -f <folder_path> [-t <minutes>] [-p <file_pattern>] [-e <file1,file2,...>] [-r] [-np] [-s] [-h]


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

## Examples

- Delete all files from a folder:
safedelete --folder C:\path\to\folder

- Delete files from a folder matching a specific pattern:
safedelete --folder C:\path\to\folder --recursive


## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
