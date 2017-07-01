# DevelopmentConsole

WIP (likely never to be finished)

*DevelopmentConsole* extension for Unity. It's a run-time console (Quake like) that allows you execute predefined commands inside the game. You can execute any method or property that has specified a `[CommandHandler]` attribute.

Licensed under MIT license. See LICENSE file in the project root folder.   

![DevelopmentConsole](/Resources/cover_screenshot.png?raw=true)

## Features

* Visualize any value on a graph
* Execute any method with command line

## Resources

* [FPS graph (video)](https://goo.gl/photos/MjjTNWkosQeZHL217)
* [Command line (video)](https://youtu.be/-Jqn67aOXt0)

## Quick Start

1. Clone or download (with the *Download* button) the repository into the *Assets* folder.
2. Drag *DevelopmentConsole* prefab from *DevelopmentConsole/Core/* onto the scene.
4. Register any void method in your code by attaching a `[CommandHandler]` attribute to it. To make
  the method registered, you must also call `CommandHandlerManager.RegisterCommandHandlers(typeof(<className>), this);`
  in the same class (for eg. in _Awake()_)
5. Run the game and open console wity the back quote button (tilde)
6. Type command name (same as the registered method name but lowercase) and press Enter to execute.

You can also open *DevelopmentConsole/Examples/Test.unity* scene, press play and open command line console (with tilde key) to check how it works. Check the video for example commands.

## Help

Just create an issue and I'll do my best to help.

## Contributions

Pull requests, ideas, questions or any feedback at all are welcome.

## Versioning

Example: `v0.2.3+1`

- `0` Major version. Introduces breaking changes.
- `2` Minor version. Adds new features.
- `3` Patch version. Bug fixes.
- `+1` Metadata version.

[Semantic Versioning Specification](http://semver.org/)
