![DimSim](/Dimmy.png) 

# [Dimmy](https://en.wikipedia.org/wiki/Dim_sim) 

Dimmy is a tool to help you with a docker based development workflow. 

Built on the .NET Core stack, it is at home on Ubuntu as much as it is on Windows 10. Even though initially conceived to help with .NET development, it has a pluggable architecture allowing you to bring support for your favourite platforms; Drupal, Umbraco, or Sitecore. Do you know how to write a docker-compose file and publish to NuGet? Great you now know how to build and distribute a Dimmy plugin.


Under the hood, Dimmy is not much more than plugin management logic, a templating engine for docker-compose files, and a way to manage public and non-public settings.  It uses the power of Octostache to generate docker-compose files that are then executed by docker-compose. It also provides several shortcuts to everyday developer tasks like quickly connecting to a running container without all the stress of looking up container ids. The best thing is you do not need to commit to Dimmy; if you find you dislike dimsims, send the fried meat and vegetable dumpling back to the Fish & Chips shop, take the generated docker-compose file and remove the tool.

Dimmy splits working files across two directories:
1. The project directory, this contains the template for your docker-compose (*.template.yaml) file, and a .dimmy.yaml that contains public developer settings,  and some metadata about the project. This directory is intended to be your git repository so you can share Dimmy configuration with other members of your team.
1. The working directory, this contains all the bits needed to run docker-compose up, bind mounts, the generated docker-compose.yaml, and non-public developer settings. This directory is not intended to be checked into source control but can be shared between team members.

## Mission

Dimmy aims to be a tool that decorates a developers experience, support both Windows and Linux containers, be transparent in what it is doing and above all not bring lock-in. I want to ensure that everything that Dimmy does a developer can continue doing even without Dimmy but maybe a bit slower.

## Inspiration

There have been a few inspirations for this project, the main one being [Lando](https://docs.lando.dev/) and initially I wanted to use that tool, but there were two issues/roadblocks for me, and so Dimmy was conceived. The first was that it seemed to me that it was geared to Linux containers; it has a bunch of really cool features, but some of them were not available on Windows containers. This, coupled with the fact I could not get it working at all with a Windows container, made me drop this asperation but not the inspiration. The other was it is written in NodeJS, not that I think this is a bad language its just I prefer C# and the .NET Core stack. 

The other was [node-red](https://nodered.org/) I like how it uses tags on npm packages to distribute plugins, and I wanted to see if I could do something similar with NuGet.

## Getting started

To get started there are a few prerequisites:
* .NET Core 3.1
* Docker
* Docker Compose

With that out of the way we can install the tool

`dotnet tool install --global Dimmy --version 0.0.3`

Or you can use this one-liner to install .NER Core 3.1 and Dimmy:
`Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex -Command ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/gravypower/Dimmy/master/dimmy-install.ps1'))`

See the [nuget page]( https://www.nuget.org/packages/Dimmy/) for the latest version. Also, note you may need to restart your shell to refresh the path environment variables.

## Plugins

Running dimmy now you see a list of root commands, the one we are interested in is `plugins`.

You can see what plugins are available by running

`dimmy plugins ls --remote`

This makes a search request to NuGet for any packages tagged with "dimmyplugin" equivalent to https://www.nuget.org/packages?q=Tags%3A%22dimmyplugin%22

Installing is also one line

`dimmy plugins install --package-id Dimmy.HelloWorld.Plugin --package-version 0.0.1`

This downloads and extracts the NuGet package into a plugin directory. On the next execution of dimmy plugins are discovered and bootstrapped, allowing that plugin to register root and sub commands. **Please note that this will allow 3rd party code to execute so use this at your own risk**.

Running dimmy again you will see a new root command `hello`

`dimmy hello --name Aaron`

This will respond to you `Hello Aaron`

### Current plugins:
* https://github.com/DeloitteDigitalAPAC/Dimmy.Sitecore.Plugin

## Project
After plugin management, this is where you want to look. With this root command, you can create a new project with initialise, start or stop a project (docker-compose up or docker-compose down) or attach to a running container.  

### Attach
This sub command will allow you to specify a roel/container you wish to connect to, or if not supplied all options for the project is output so you can esaly choose.

### Context project
Another one of the reasons Dimmy was created was to swap between applications easily and quickly. As such Dimmy checks the context directory where the dimmy command was issued from, and if it contains a .dimmy file it will use information stored in that file to understand what the context project is. the .dimmy file can be found in your `working-path` and also contains the non-public settings. So swapping projects is as easy as issuing `dimmy project stop`, changing directory and `dimmy project start`.

## Whats Next

No official road map yet but you can see what [enhancements](https://github.com/gravypower/Dimmy/labels/enhancement) may be on the way. If you wish to contribute, please, by all means, open a PR. Also goes for any bugs you may find open a new git issue.

## 終わり

This is not much more than a thought experiment currently, use at your own risk and thank you for taking the time to look at this project. 

Made with :heart: in Bendigo.
