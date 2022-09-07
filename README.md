# (Unofficial) Jellyfin Server for FreeBSD

Microsoft does not have an official build of dotNET for FreeBSD. See [HERE](https://github.com/dotnet/runtime/issues/14537) for more details.
This repo currently only contains binary components for [Jellyfin-Server](https://github.com/jellyfin/jellyfin) (FreeBSD AMD64) and [Jellyfin-Web](https://github.com/jellyfin/jellyfin-web/) (Portable) in addition to the required and pre-built libSkiaSharp. An installable package (.pkg) is provided for ease of installation.

# Dependencies 

```
pkg install sqlite3 openssl mediainfo libunwind icu ffmpeg
```

## Divergence from main project
This product builds cleanly with a working dotNET SDK under FreeBSD 12.2+. No code modification is required unless using the `build.sh` or `build.yaml`


# Updates

This package will lag behind the official Jellyfin product as it is not a part their CI/CD process and relies on me manually building it. If it lags more than a week behind and you want updates please **OPEN A TICKET**.

# TODO

 - CI/CD system which will likely be Azure DevOps as it does not appear Github actions has FreeBSD systems 
 - Figure out how to wire FreeBSD ports system into autopackaging updates from here
 - Figure out how to use libmap in ports system instead of making a symlink for library
 
# Bugs
Please **DO NOT** bring bugs about this build or platform to the main jellyfin team. This is **Unofficial**. Open a ticket if you are having issues but please check their official [Issues](https://github.com/jellyfin/jellyfin/issues) first and make sure you have read the [Troubleshooting section](https://github.com/Thefrank/jellyfin-server-freebsd/blob/main/Installation_TrueNAS_GUI.md#troubleshooting-and-other-things-to-note) in the TrueNAS guide (yes, even if using base FreeBSD) as it cover most issues/bugs/quirks you might hit.

# License(s)

 - Jellyfin is under GPLv2 
 - Skia is under BSD-3-CLAUSE 
 - Bundled combination of the above would likely fall under GPLv2 or some dual
   license. 
 - IANAL so I am not 100% sure.

# One more thing

Microsoft does not currently support dotNET5+ on FreeBSD so this package and its binaries might have limited support across FreeBSD versions. If you are knowledgeable in the inner-workings of FreeBSD please drop over to https://github.com/dotnet/runtime/issues/14537 if you would like to help out getting dotNET on to FreeBSD.
