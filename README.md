<div align="center">
    <img src="poster.png"/>
    <h3>Lightweight and simple solution for projects testing.</h3>
</div>

<div id="badges" align="center">
    <a href="https://www.codacy.com/gl/kostya-zero/zeroprobe/dashboard?utm_source=gitlab.com&amp;utm_medium=referral&amp;utm_content=kostya-zero/zeroprobe&amp;utm_campaign=Badge_Grade">
        <img src="https://app.codacy.com/project/badge/Grade/ee24203115c542b08553b7e071a14b88"/>
    </a>
    <a href="https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml">
        <img src="https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml/badge.svg?branch=main&"/>
    </a>
    <a href="https://gitlab.com/kostya-zero/zeroprobe">
        <img src="https://img.shields.io/badge/GitLab-repository-orange?logo=gitlab&"/>
    </a>
    <a href="https://github.com/kostya-zero/zeroProbe">
        <img src="https://img.shields.io/badge/GitHub-repository-232323?logo=github&"/>
    </a>
 </div>

### :blue_book: What is this?

zeroProbe is a compact and simple application focus on testing things.
You can test everything, from building an application to test runs with different arguments.
zeroProbe are open-source project and absolutly free.
It has not too much functionallity, but in the feature it will increase.
You can use it on your server for testing after each change.
The configuration allows you to make test scripts for everything you want.
Just try it.

### :scroll: Configuration

zeroProbe use it own configuration system named ProbeConfig.
It looks similliar to INI files.
ProbeConfig are focus on being simple, flexible and have pretty look.

```text
project: .NET Build Test
stages: restore build

!restore.add_command: dotnet restore
!build.add_command: dotnet build --no-restore
```

```text
project: Build File Lister
check_for: g++ gcc
stages: build test

!build.add_command: g++ main.cpp src/io.cpp src/io.h src/fs.cpp src/fs.h -o file_lister
!test.add_command: ./file_lister
```

Need a ready template?
zeroProbe contains templates for your projects inside.
Generate and start making your work more productive!

### :rocket: Fast as possible

zeroProbe was developed in C#. It works on .NET and it so-o-o fast. Really, just try it.
.NET 6 allows us to build more faster and feature-rich applications with C#.
zeroProbe are fast tool for your tests.
It make your test automated, it works so fast and has an easy configuration style - that's so cool!

### :floppy_disk: Installation

- Download latest version of zeroProbe that's match your OS and architecture.
- Unpack archive.
- Place executable file where you want.
- :rocket: You are ready to use zeroProbe and make your work a little bit easier!

### :package: Contributing

Want to help us in development? Sounds good!
Feel free to fork this repository and make a pull request.
Also, if you have problems, tell us in issues on GitHub or GitLab about what's wrong or where you got stuck.
You can check step-by-step guide [here](CONTRIBUTING.md).

### :earth_asia: Links

- [GitLab Repository](https://gitlab.com/kostya-zero/zeroprobe) - main repository of zeroProbe.
- [GitHub Repository](https://github.com/kostya-zero/zeroProbe) - mirror of zeroProbe for GitHub.
- [GitHub Workflow: .NET](https://github.com/kostya-zero/zeroProbe/actions/workflows/dotnet.yml) - build status.
- [Trello Board](https://trello.com/b/jLdiw40c/zeroprobe) - check out what we are planing and what we are doing now.
- [Codacy](https://www.codacy.com/gl/kostya-zero/zeroprobe/dashboard?utm_source=gitlab.com&amp;utm_medium=referral&amp;utm_content=kostya-zero/zeroprobe&amp;utm_campaign=Badge_Grade) - code ranking and issues.

### :envelope: Message from Author

I started learning C# in 2021 and now, I try to make an application for testing.
That's a practice for my skills, and it will be cool if you will support my project with contribution or starring this repository or leaving a review for this project.
Thanks!