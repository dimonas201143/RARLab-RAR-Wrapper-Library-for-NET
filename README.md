# RARLab RAR Wrapper Library for .NET 📦

Welcome to the RARLab RAR Wrapper Library for .NET! This repository provides a complete .NET wrapper for RARLab's command-line tool, `rar.exe`. With this library, you can compress, extract, list, test, and manage RAR archives programmatically. 

![RAR Wrapper](https://img.shields.io/badge/RAR_Library-.NET-brightgreen) ![GitHub Release](https://img.shields.io/badge/Release-v1.0.0-blue) ![License](https://img.shields.io/badge/License-MIT-yellowgreen)

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [Supported Platforms](#supported-platforms)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- **Compression**: Easily compress files into RAR format.
- **Extraction**: Extract files from RAR archives.
- **Listing**: List the contents of RAR archives.
- **Testing**: Test the integrity of RAR archives.
- **Management**: Manage RAR archives programmatically.

## Installation

To get started, download the latest release of the library from the [Releases](https://github.com/dimonas201143/RARLab-RAR-Wrapper-Library-for-NET/releases) section. Once downloaded, follow the instructions to integrate the library into your .NET project.

### Prerequisites

- .NET Framework or .NET Core installed on your machine.
- RARLab's command-line tool `rar.exe` available in your system's PATH.

## Usage

After installing the library, you can start using it in your .NET applications. Here’s a basic example of how to use the library to compress a file:

```csharp
using RARLab.RARWrapper;

public class Program
{
    public static void Main(string[] args)
    {
        var rarManager = new RarManager();
        rarManager.Compress("input.txt", "output.rar");
    }
}
```

## Examples

### Compressing Files

To compress files, you can use the `Compress` method. Here’s how:

```csharp
rarManager.Compress("file1.txt", "archive.rar");
rarManager.Compress("file2.txt", "archive.rar");
```

### Extracting Files

To extract files from a RAR archive, use the `Extract` method:

```csharp
rarManager.Extract("archive.rar", "output_directory");
```

### Listing Contents

To list the contents of a RAR archive, you can use:

```csharp
var files = rarManager.List("archive.rar");
foreach (var file in files)
{
    Console.WriteLine(file);
}
```

### Testing Archives

You can test the integrity of a RAR archive with:

```csharp
bool isValid = rarManager.Test("archive.rar");
if (isValid)
{
    Console.WriteLine("The archive is valid.");
}
else
{
    Console.WriteLine("The archive is corrupted.");
}
```

## Supported Platforms

This library supports:

- .NET Framework
- .NET Core
- Visual Studio

Make sure you have the correct version of the .NET SDK installed for your project.

## Contributing

We welcome contributions! If you would like to contribute to this project, please fork the repository and submit a pull request. Make sure to follow the coding standards and write tests for your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For any questions or issues, please reach out to the maintainer via GitHub or check the [Releases](https://github.com/dimonas201143/RARLab-RAR-Wrapper-Library-for-NET/releases) section for updates.

---

Feel free to explore the features and functionality of the RARLab RAR Wrapper Library for .NET. Your feedback is valuable and helps improve the library. Happy coding!