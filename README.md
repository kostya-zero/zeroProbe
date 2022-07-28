# zeroProbe
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ee24203115c542b08553b7e071a14b88)](https://www.codacy.com/gl/kostya-zero/zeroprobe/dashboard?utm_source=gitlab.com&amp;utm_medium=referral&amp;utm_content=kostya-zero/zeroprobe&amp;utm_campaign=Badge_Grade)
[![.github/workflows/dotnet.yml](https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml)

Advanced utility to create, run and manage tests. 
This tool allows you to create your own pipelines and run it.
Tool written on C# with .NET 6.
To run zeroProbe `dotnet-runtime` are required.
It functionality will increase with every update.

![img.png](img.png)
> Not final version. Look may change with release.

## Features
### Test local
zeroProbe tell you where error occurs. 
It runs very fast an easy to build your own configuration file.

### Easy to use
First task in development of zeroProbe was to make easy to use tool for test. 
Well, we did it!
You don't need any other skills. 
Nothing hard to build your own tests.

### .NET 6
.NET 6 provides the best run speed in C# applications. 
We choose .NET for speed and ability to make zeroProbe cross-platform in the future.

## Bridge for zeroProbe (soon)
Bridge is internal program to interact with zeroProbe fast and easier with less typing.
How it works?
- zeroProbe will has `--bridge` argument. To enable Bridge mode, use `--bridge=1`. 
- Run commands like in terminal, but without zeroProbe at start.
- You can interact with your project in real-time.

## Build from source
### Install build dependencies
zeroProbe use .NET 6 framework to run. To build you must have `dotnet-runtime` and `dotnet-sdk` installed.
- Ubuntu, Debian, Debian-based and Ubuntu-based:
```shell
sudo apt-get update && apt-get upgrade     # Update repositories and packages
sudo apt-get install -y dotnet-runtime-6.0 # Install runtime
sudo apt-get install -y dotnet-sdk-6.0     # Install SDK
```
- Fedora:
```shell
sudo dnf install dotnet-runtime-6.0 # Install runtime
sudo dnf install dotnet-sdk-6.0     # Install SDK
```
- ArchLinux, Manjaro and ArchLinux-based:
```shell
sudo pacman -Syu dotnet-runtime dotnet-sdk # Install runtime and SDK
```
- openSUSE:
```shell
sudo zypper install dotnet-sdk-6.0     # Install SDK
sudo zypper install dotnet-runtime-6.0 # Install runtime
```

### Prepare workspace 
Firstly, clone official repository from [GitLab](https://gitlab.com/kostya-zero/zeroprobe).
```shell
git clone https://gitlab.com/kostya-zero/zeroprobe # Cloning GitLab repo
cd zeroprobe                                       # Moving to cloned repo directory
```

### Run build 
Now, the final moment, running build of zeroProbe.
```shell
dotnet restore            # Restore dependencies
dotnet build --no-restore # Building
```

## Credits
Developers:
- Konstantin ".ZERO" Zhigaylo - main developer

Tools and websites used in development:
- Codacy
- JetBrains Rider 2022.1.3
- GitHub Workflows
