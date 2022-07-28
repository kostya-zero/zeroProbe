# zeroProbe
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ee24203115c542b08553b7e071a14b88)](https://www.codacy.com/gl/kostya-zero/zeroprobe/dashboard?utm_source=gitlab.com&amp;utm_medium=referral&amp;utm_content=kostya-zero/zeroprobe&amp;utm_campaign=Badge_Grade)
[![.github/workflows/dotnet.yml](https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml)

Advanced utility to create, run and manage tests. 
This tool allows you to create your own pipelines and run it.
Tool written on C# with .NET 6.
To run zeroProbe `dotnet-runtime` are required.
It functionality will increase with every update.

![img.png](https://gitlab.com/kostya-zero/zeroprobe/-/tree/main/img.png)

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
- zeroProbe will has `--bridge` argument. To enable Bridge mode, type `--bridge=1`.
- 