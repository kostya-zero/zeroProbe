<div align="center">
    <img src="poster.png"/>
    <h3>Powerful, advanced and open-source utility for tests automation.</h3>
</div>

<div id="badges" align="center">
    <a href="https://www.codacy.com/gl/kostya-zero/zeroprobe/dashboard?utm_source=gitlab.com&amp;utm_medium=referral&amp;utm_content=kostya-zero/zeroprobe&amp;utm_campaign=Badge_Grade">
        <img src="https://app.codacy.com/project/badge/Grade/ee24203115c542b08553b7e071a14b88"/>
    </a>
    <a href="https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml">
        <img src="https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml/badge.svg?branch=main&"/>
    </a>
    <a href="https://img.shields.io/github/commit-activity/w/kostya-zero/zeroProbe">
        <img src="https://img.shields.io/github/commit-activity/w/kostya-zero/zeroProbe"/>
    </a>
    <a href="https://img.shields.io/github/last-commit/kostya-zero/zeroProbe">
        <img src="https://img.shields.io/github/last-commit/kostya-zero/zeroProbe"/>
    </a>
    <a href="https://gitlab.com/kostya-zero/zeroprobe">
        <img src="https://img.shields.io/badge/GitLab-repository-orange?logo=gitlab&"/>
    </a>
    <a href="https://github.com/kostya-zero/zeroProbe">
        <img src="https://img.shields.io/badge/GitHub-repository-232323?logo=github&"/>
    </a>
    <a>
        <img src="https://img.shields.io/github/downloads/kostya-zero/zeroprobe/total?color=grey">
    </a>
 </div>

<div align="center">
    <img src="img.png" alt="Demonstration of zeroProbe.">
    <em>Demonstration of zeroProbe.</em>
</div>

## Features
### Your powerful tool
zeroProbe allows you to automate your test or scripts.
You can make your own configuration with **Probe Config**.
It allows you to create configurations for any tasks.
If got any errors, zeroProbe will describe it in error message.

### Easy-to-use
**Probe Config** are not require any skills.
You can make configurations for any tasks.
Test, build scripts, install scripts, etc - anything what you want.
Take a look:
```
project: Build File Lister
check_for: g++ gcc
stages: build test

!build.add_command: g++ main.cpp src/io.cpp src/io.h src/fs.cpp src/fs.h -o file_lister
!test.add_command: ./file_lister
```

### Speed, I'm speed...
zeroProbe uses .NET 6 for best performance.
We are trying to make zeroProbe fast as possible.
The first task of our work is to make fast utility.

### Helpful guides
zeroProbe have guides on [GitHub](https://github.com/kostya-zero/zeroProbe/wiki) and [GitLab](https://gitlab.com/kostya-zero/zeroprobe/-/wikis/home) wiki.
If you want to know more about it, you can check it.
Wiki updates with every new features in zeroProbe or if developers want to tell more about it.

### Stay up-to-date
Check our [Trello board](https://trello.com/b/jLdiw40c/zeroprobe) to see what we are planing and what we are doing now.
It helps you to know what we are gonna to add or fix in zeroProbe.

## Credits
Developers:
- Konstantin ".ZERO" Zhigaylo - main developer

Tools and websites used in development:
- Codacy
- JetBrains dotCover
- JetBrains Rider
- GitHub Workflows
