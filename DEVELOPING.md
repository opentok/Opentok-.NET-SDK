# Development Guidelines

This document describes tools, tasks and workflow that one needs to be familiar with in order to effectively maintain
this project. If you use this package within your own software as is but don't plan on modifying it, this guide is
**not** for you.

## Tools

*  [Visual Studio](http://www.visualstudio.com/): the IDE provided by Microsoft with which the product is built. The
   code may work with other IDEs and tools but Visual Studio is required to effectively maintaiin the project.

*  [NuGet Package Manager](http://www.nuget.org/): an open source package manager that is built into many newer
   versions of Visual Studio. If you do not already have this extension, you can install it via
   Tools > Extensions and Updates. Make sure the command line version of the tools are available.

## Tasks

### Building

In Visual Studio, first select the appropriate Build Configuration (either "Debug" or "Release"). Next, in the Solution
Explorer panel, right-click on the OpenTok project, and then choose Build.

The output assemblies (OpenTok.dll and its dependencies) will be placed in the `OpenTok\bin\{Configuration}\` directory.

### Testing

This project's tests are written as Xunit test cases. In Visual Studio, open the Test Explorer panel (Test > Windows >
Test Explorer). If you have not already built the solution (F6), you will need to do so in order for the tests to appear.
You can now either click the Run All command or use the keyboard shortcut (Ctrl+R, A).

### Generating Documentation

**TODO** Document Doxygen process or [change things up](https://github.com/opentok/Opentok-.NET-SDK/issues/31)

### Releasing

In order to create a release, the following should be completed in order.

1. Build the solution using the Release configuration. Ensure all the tests are passing and that there is enough test coverage.
1. Make sure you are on the `dev` branch of the repository, with all changes merged/commited already.
1. Update the version number in the source code and the README. See [Versioning](#versioning) for information
   about selecting an appropriate version number. Files to change:
   - OpenTok\Properties\AssemblyInfo.cs (AssemblyInformationalVersion contains full semver, AssemblyVersion only
     only contains major.minor.*)
   - README.md
1. Commit the version number change with the message "Update to version x.y.z", substituting the new version number.
1. Create a git tag: `git tag -a vx.y.z -m "Release vx.y.z"`
1. Ensure that you have permissions to publish an update to [the NuGet package](https://www.nuget.org/packages/OpenTok/)
   and that the command line tools have been configured using your account's API Key.
1. Rebuild the solution with the new version number.
1. Publish a NuGet package using these commands at the command line (substituting for {version}):
   `NuGet.exe pack -sym OpenTok\OpenTok.csproj; nuget push OpenTok.{version}.nupkg`
1. Change the version number for future development by incrementing the patch number and
   adding "-Alpha1" in each file except the README. Then stage the remaining files and commit with the message
   "Begin development on next version".
1. Push the changes to the source repository: `git push origin dev; git push --tags origin`
1. Compress the contents of the `OpenTok\bin\Release\` directory, name it using the following template: `OpenTokSDK_x.y.z.zip`
   Upload the zip as an attached file in the latest GitHub Release. Add release notes with a description of changes and fixes.

## Workflow

### Versioning

The project uses [semantic versioning](http://semver.org/) as a policy for incrementing version numbers. For planned
work that will go into a future version, there should be a Milestone created in the Github Issues named with the version
number (e.g. "v2.2.1").

During development the version number should end in "-Alphax" or "-Betax", where x is an increasing number starting from 1.
Using a "." in the prerelease tag is not allowed in the .NET platform.

### Branches

*  `dev` - the main development branch.
*  `master` - reflects the latest stable release.
*  `feat.foo` - feature branches. these are used for longer running tasks that cannot be accomplished in one commit.
   once merged into dev, these branches should be deleted.
*  `vx.x.x` - if development for a future version/milestone has begun while dev is working towards a sooner
   release, this is the naming scheme for that branch. once merged into dev, these branches should be deleted.

### Tags

*  `vx.x.x` - commits are tagged with a final version number during release.

### Issues

Issues are labelled to help track their progress within the pipeline.

*  no label - these issues have not been triaged.
*  `bug` - confirmed bug. aim to have a test case that reproduces the defect.
*  `enhancement` - contains details/discussion of a new feature. it may not yet be approved or placed into a
   release/milestone.
*  `wontfix` - closed issues that were never addressed.
*  `duplicate` - closed issue that is the same to another referenced issue.
*  `question` - purely for discussion

### Management

When in doubt, find the maintainers and ask.
