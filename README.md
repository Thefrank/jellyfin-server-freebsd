# (Unofficial) Jellyfin Server for FreeBSD

Microsoft does not have an official build of dotNET for FreeBSD. See [HERE](https://github.com/dotnet/runtime/issues/14537) for more details.
This repo currently only contains binary components for [Jellyfin-Server](https://github.com/jellyfin/jellyfin) (FreeBSD AMD64) and [Jellyfin-Web](https://github.com/jellyfin/jellyfin-web/) (Portable) in addition to the required and pre-built libSkiaSharp. An installable package (.txz) is provided for ease of installation.

## Divergence from main project
This product builds cleanly with a working dotNET SDK under FreeBSD 12.2. No code modification is required unless using the `build.sh` or `build.yaml`


# Updates

This package will lag behind the official Jellyfin product as it is not a part their CI/CD process and relies on me manually building. If it lags more than a week behind and you want updates please **OPEN A TICKET**.

# TODO

 - CI/CD system which will likely be Azure DevOps as it does not appear Github actions has FreeBSD systems 
  - Figure out how to wire FreeBSD ports system into autopackaging updates from here
 
# Bugs
Please **DO NOT** bring bugs about this build or platform to the main jellyfin team. This is **Unofficial**. Open a ticket if you are having issues but please check their official [Issues](https://github.com/jellyfin/jellyfin/issues) first. 

# License(s)

 - Jellyfin is under GPLv2 
 - Skia is under BSD-3-CLAUSE 
 - Bundled combination of the above would likely fall under GPLv2 or some dual
   license. 
	  - IANAL so I am not 100% sure.
