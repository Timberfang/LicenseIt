# LicenseIt

A generator for your LICENSE files.

LicenseIt is a command-line tool for generating LICENSE files for open-source projects. Most of the licenses featured on [choosealicense.com](https://choosealicense.com), are available, whether for software (MIT, MPL, GPL, etc.), media (Creative Commons), or fonts (OFL).

## Install

If available, download an executable from the [releases](https://github.com/Timberfang/LicenseIt/releases/latest) page, then extract it anywhere on your computer.

## Usage

LicenseIt is a command-line application. Open your favorite shell, and type `LicenseIt.exe --help` to list the commands available.

For example, to create a license, use the following:

```powershell
LicenseIt.exe new --author-name 'John Doe' --project-name 'Example Project' --license-name 'MIT'
```

Note the location given for the output file. Put that file in the correct location within your project, and you are good to go!

## Adding More License Templates

License templates can be added or updated in the `Templates` directory:

1. Anywhere within that directory or its children, create a `.txt` file for your license
2. Paste the full contents of your license in the file.
3. Replace any placeholders for author name with `${AUTHOR_NAME}`, project name with `${PROGRAM_NAME}`, and year with `${YEAR}`.

Please ensure that your license file's name is unique. While the existing licenses use the [SPDX Codes](https://spdx.org/licenses), any name will be accepted by the program.

## Building

To build this project from source, you'll need the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) version 9 or any later version. For Native Ahead-of-Time (AOT) compilation (produces a native machine-code binary instead of one needing a Runtime), you'll additionally need the following:

- **Windows:** [Build Tools for Visual Studio 2022](https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022) with the `MSVC - x64/x86 - Latest version` and `Windows 11 SDK - Latest version` components
- **Alpine Linux (3.15+):** `sudo apk add clang build-base zlib-dev`
- **Fedora Linux (39+):** `sudo dnf install clang zlib-devel`
- **RHEL (8+):** `sudo dnf install clang zlib-devel zlib-ng-devel zlib-ng-compat-devel`
- **Ubuntu (18.04+):** `sudo apt-get install clang zlib1g-dev`
- **MacOS:** Latest [Command Line Tools for XCode](https://developer.apple.com/download)

Only Windows has pre-compiled builds at the moment. For Linux and MacOS, you'll need to compile from source. Make sure you have the dependencies installed, then get the correct [RID](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog#known-rids) for your operating system. Run the following commands, replacing `$RID` with your RID:

```powershell
git clone https://github.com/Timberfang/LicenseIt.git
cd LicenseIt
dotnet publish -r $RID -c Release
```

The .NET SDK should automatically download and install any necessary libraries, then compile the program for your platform.

## License

LicenseIt is Copyright (c) 2025 Timberfang, under the MIT License (MIT). For more information, see the [LICENSE file](LICENSE).
