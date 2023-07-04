# Release Note 0.0.2

## Overview
Release 0.0.2 of the `SecureFileDelete` code includes several enhancements and bug fixes to improve the functionality and usability of the application. It introduces new features, refines existing capabilities, and addresses reported issues to provide a more reliable and efficient file deletion experience.

## Features
- Improved user interface for a more intuitive and streamlined user experience.
- Enhanced file selection options, including support for file size-based selection criteria.
- Introduces a progress indicator to display the status of the file deletion process.
- Optimized secure deletion algorithms for faster and more efficient file wiping.
- Improved error handling and error messages for better troubleshooting.

## Usage
The command-line syntax for using the `safedelete` utility remains the same as in the previous release:

safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>]

Please refer to the previous release note for detailed information on the parameters and their usage.

## Changes
- Enhanced user interface with improved command-line prompts and messages.
- Added support for file size-based selection criteria to delete files above or below a specified size.
- Introduces a progress indicator to display the status of the file deletion process.
- Optimized secure deletion algorithms for faster and more efficient file wiping.
- Improved error handling and error messages for better troubleshooting.
- Fixed a bug where excluded files were still being considered for deletion in certain scenarios.
- Resolved an issue where the application crashed when encountering certain file types.

We appreciate the valuable feedback received from users, which helped us identify and address these issues promptly. Your continued support is essential as we strive to make `SecureFileDelete` a reliable and robust file deletion utility.

Please note that this release builds upon the previous version and includes several improvements. We will continue to incorporate user feedback and work on additional enhancements in upcoming releases.

Thank you for using `SecureFileDelete`!
