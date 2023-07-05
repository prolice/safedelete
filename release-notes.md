# Release Note - Version 0.0.2

## Overview

Version 0.0.2 of SafeDelete introduces several enhancements and bug fixes to improve the functionality and user experience of the tool.

## Changes

- Improved file deletion performance for large folders.
- Added support for excluding files from deletion using the `--exclude_files` option.
- Fixed a bug where the tool would crash when encountering certain file types.
- Updated the user interface to provide clearer prompts and messages during file deletion.
- Enhanced error handling and error messages for better troubleshooting.
- Added a new command-line option `--no_prompt` to enable no prompt mode for faster file deletion without confirmation.

## How to Upgrade

To upgrade to version 0.0.2, follow these steps:

1. Download the latest release package from the SafeDelete repository.
2. Extract the contents of the package to a local directory.
3. Replace the previous version of SafeDelete with the new version by overwriting the existing files.
4. Ensure that any custom configurations or settings are migrated to the new version if necessary.
5. Verify the installation by running `safedelete --version` and ensure that it displays `0.0.2`.

## Feedback and Support

We value your feedback! If you encounter any issues or have suggestions for improvement, please don't hesitate to reach out to us. You can contact our support team at support@safedelete.com or open an issue on the SafeDelete repository.

## Known Issues

The following known issues exist in version 0.0.2:

- In certain cases, the progress indicator may not accurately reflect the actual progress of file deletion.
- Excluding a large number of files using the `--exclude_files` option may cause a slight delay in the deletion process.

We are actively working on addressing these issues and plan to resolve them in future releases.

Thank you for using SafeDelete!

