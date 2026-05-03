# GUnit
A small library to Test your .Net Godot Projects with unit tests.

## Installation
Run the following command to install the dotnet tool globaly.
```
dotnet tool install --global samuelmatarese.GUnit.CLI
```

# Development

## GUnit.CLI

To install a local version run the following commands. Also ensure that dotnet tools are registered in the PATH.

```
dotnet pack -c Release
```

```
dotnet tool install --global samuelmatarese.GUnit.CLI --add-source ./bin/Release
```

To uninstall run
```
dotnet tool uninstall -g samuelmatarese.GUnit.CLI
```
