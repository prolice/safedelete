# Technical Documentation: SecureFileDelete Tool

## Table of Contents
1. [Introduction](#introduction)
2. [Usage](#usage)
3. [Command Line Parameters](#command-line-parameters)
4. [PerformSecureFileDeletion Function](#performsecurefiledeletion-function)
5. [SecureDeleteFile Function](#securedeletefile-function)
6. [Overwrite Methods](#overwrite-methods)
7. [CommandLineArguments Class](#commandlinearguments-class)

## 1. Introduction
The SecureFileDelete tool is a command-line utility that securely deletes files from a specified folder. It provides various options to control the deletion process, such as setting a time limit, specifying file patterns, and excluding certain files from deletion. The tool ensures that the deleted files are overwritten multiple times with different data patterns to prevent data recovery.

## 2. Usage
The tool is executed from the command line with the following syntax:
safedelete --folder <folder_path> [--time_limit <minutes>] [--pattern <file_pattern>] [--exclude_files <file1,file2,...>]

Example usage:
safedelete --folder C:\Data --time_limit 60 --pattern *.txt --exclude_files file1.txt,file2.txt


## 3. Command Line Parameters
The SecureFileDelete tool accepts the following command line parameters:

- `--folder`:
  Specifies the path of the folder from which files should be deleted.
  Example: `--folder C:\\Data`

- `--time_limit`:
  Specifies the time limit (in minutes) for the maximum file age. Files older than the specified limit will be deleted.
  Example: `--time_limit 60`

- `--pattern`:
  Specifies the file name pattern to match. Only files with names containing the specified pattern will be considered for deletion.
  Example: `--pattern *.txt`

- `--exclude_files`:
  Specifies a comma-separated list of files to exclude from deletion.
  Example: `--exclude_files file1.txt,file2.txt`

## 4. PerformSecureFileDeletion Function
The `PerformSecureFileDeletion` function is responsible for coordinating the secure file deletion process based on the provided command line arguments. It retrieves the values of the command line parameters and performs the secure file deletion by calling the overloaded `PerformSecureFileDeletion` function.

## 5. SecureDeleteFile Function
The `SecureDeleteFile` function performs the secure deletion of a single file. It takes the file path as input and performs the following steps:
- Checks if the file exists.
- Gets the directory of the original file.
- Creates a temporary file path in the same directory using a random file name.
- Iterates 7 times, each time renaming the temporary file with a new random file name.
- Gets the file size.
- Overwrites the file content using different steps and algorithms.
- Deletes the renamed file.

## 6. Overwrite Methods
The `SecureDeleteFile` function uses several overwrite methods to securely delete the file contents. These methods include:
- `OverwriteWithRandomData`: Fills the file with random data.
- `OverwriteWithNullValues`: Fills the file with null values.
- `OverwriteWithZeroValues`: Fills the file with zero values.
- `OverwriteWithAlternatingPattern`: Fills the file with an alternating pattern of 0xFF and 0x00.
- `OverwriteWithEncryptedRandomData`: Fills the file with random data using an encryption algorithm.

## 7. CommandLineArguments Class
The `CommandLineArguments` class represents a collection of command line parameters and provides methods to parse and retrieve the parameter values. It includes the following methods:
- `AddParameter`: Adds a parameter and its value to the collection.
- `ContainsParameter`: Checks if a parameter exists in the collection.
- `GetValue<T>`: Retrieves the value of a parameter and converts it to the specified type.

