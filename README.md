# Dimmy

*this is not much more than a thought experemt 

Dimmy is a tool to help you with a docker based development workflow. 

Built on the .NET Core stack, it is at home on Ubuntu as much as it is on Windows 10. Even though initially conceived to help with .NET development, it has a pluggable architecture allowing you to bring support for your favourite platforms; Drupal, Umbraco, or Sitecore. Do you know how to write a docker-compose file and publish to NuGet? Grate you now know how to build and distribute a Dimmy plugin.

Under the hood, Dimmy is not much more than plugin management logic, a templating engine for docker-compose files, and a way to manage public and non-public settings.  It uses the power of Octostache to generate docker-compose files that are then executed by docker-compose. The best thing is you do not need to commit to Dimmy, if you find you dislike dimsims take the generated docker-compose file and remove the tool.

There are two essential parts of how Dimmy works. The first is your project directory; this is where the public development settings and a docker-compose template are stored and ultimately checked to source control. Next is the runtime directory where non-public development settings, the generated docker-compose files, and any bind mounts are found; this directory would generally not be committed to source control.

Getting started:
To get started there are a few prerequisites:
.NET Core 3.1
Docker
Docker Compose

With that out of the way we can install the tool

`dotnet tool install --global Dimmy --version 0.0.3`

See the [nuget page]( https://www.nuget.org/packages/Dimmy/) for the latest version. Also, note you may need to restart your shell to refresh the path environment variables.

## Plugins

Running dimmy now you see a list of root commands, the one we are interested in is `plugins`.

You can see what plugins are available by running

`dimmy plugins ls --remote`

This makes a search request to NuGet for any packages tagged with "dimmyplugin" equivalent to https://www.nuget.org/packages?q=Tags%3A%22dimmyplugin%22

Installing is also one line

`dimmy plugins install --package-id Dimmy.HelloWorld.Plugin --package-version 0.0.1`

This downloads and extracts the NuGet package into a plugin directory. On the next execution of dimmy plugins are discovered and bootstrapped, allowing that plugin to register root and sub commands. **Please note that this will allow 3rd party code to execute so use this at your own risk**.

Running dimmy again you will see a new root command "hello"

### Current plugins:
* https://github.com/DeloitteDigitalAPAC/Dimmy.Sitecore.Plugin

## Whats Next

No official road map yet but you can see what [enhancements](https://github.com/gravypower/Dimmy/labels/enhancement) may be on the way. If you wish to contribute, please, by all means, open a PR. Also goes for any bugs you may find open a new git issue.

## 終わり

This is not much more than a thought experiment currently, use at your own risk and thank you for taking the time to look at this project. 

Made with :heart: in Bendigo.
