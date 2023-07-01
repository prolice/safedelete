# Release Note 0.0.1

## Overview
Release 0.0.1 of the `SecureFileDelete` code provides a command-line utility for securely deleting files from a specified folder. It offers various options for specifying the file deletion criteria, including time limit, file pattern, and exclusion of specific files.

## Features
- Securely delete files from a specified folder.
- Support for specifying a time limit to delete files created within a certain duration.
- Ability to specify a file name pattern to match files for deletion.
- Option to exclude specific files from deletion.

## Usage
The command-line syntax for using the `safedelete` utility is as follows:
safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>]

### Parameters
- `--folder`: The path of the folder to delete files from. (required)
- `--time_limit`: The time limit (in minutes) specifying the maximum file age. (optional)
- `--pattern`: The file name pattern to match. (optional)
- `--exclude_files`: Comma-separated list of files to exclude from deletion. (optional)

### Example
safedelete --folder C:\Data --time_limit 60 --pattern *.txt --exclude_files file1.txt,file2.txt

## Changes
- Initial release of the `SecureFileDelete` code.
- Provides basic functionality to securely delete files from a specified folder.
- Supports command-line arguments for specifying deletion criteria.
- Performs secure deletion using multiple steps and algorithms.

Please note that this is an early version of the code, and additional features and improvements may be added in future releases.
