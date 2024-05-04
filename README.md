# (Unofficial) Jellyfin Server for FreeBSD

Microsoft does not have an official build of dotNET for FreeBSD. See [HERE](https://github.com/dotnet/runtime/issues/14537) for more details.
This repo currently only contains binary components for [Jellyfin-Server](https://github.com/jellyfin/jellyfin) (FreeBSD AMD64) and [Jellyfin-Web](https://github.com/jellyfin/jellyfin-web/) (Portable) in addition to the required and pre-built libSkiaSharp.

## Divergence from main project

This product builds cleanly with a working dotNET SDK under FreeBSD 12.2+. No code modification is required unless using the `build.sh` or `build.yaml`
Check "building" if you would like to learn more.

# Updates

This package will lag behind the official Jellyfin product as it is not a part their CI/CD process and relies on me manually building it. If it lags more than a week behind and you want updates please **OPEN A TICKET**.

# Known Issues

 - `libinotify` runs into resource limitations when used to monitor a large number of files. This is typically experienced as DNS or SSL errors.
	- Cause: Oversimplified as [Everything is a file](https://en.wikipedia.org/wiki/Everything_is_a_file)
	- Workaround: Make sure that "Enable real time monitoring" is disabled for every library. This option is checked by default.
 - `ffmpeg` sees very slow transcode rates when compared to other platforms. See [HERE](https://github.com/Thefrank/jellyfin-server-freebsd/issues/67).
	- Cause: Unknown.
	- Workaround: There is no known workaround for this. This appears to be a FreeBSD or build issue. 
 - Hardware acceleration does not work. `ffmpeg` detects suitable hardware.
	- Cause: Explicit FreeBSD cases not included in code.
	- Workaround: The current experimental [build](https://github.com/Thefrank/jellyfin-server-freebsd/releases/tag/jellyfin-vaapi-test) based on Jellyfin 10.9.0 master branch includes patching to add more supported hardware types. Please let me know if this works!
 - IPv6 support does not work in all jail situations (e.g., TrueNAS CORE created jails, `ipv6=new`, or `vnet=OFF`)
	- Cause(s): 
	  - TrueNAS CORE sets up jails in a way that might cause discovery to fail on ipv6 networks.
	  - FreeBSD exposes a limited amount of data to the jail based on OS or jail security level.
	  - dotNET uses dual-sockets and runs into issues if only 4 or 6 is visible.
	- Workaround: See the [FAQ](FAQ.md) for various workarounds. In some cases, a workaround might not be available.

# Bugs

Please **DO NOT** bring bugs about this build or platform to the main jellyfin team. This is **Unofficial**. Open a ticket if you are having issues but please check their official [Issues](https://github.com/jellyfin/jellyfin/issues) first and make sure you have read the [FAQ.md](FAQ.md) first.

# License(s)

 - Jellyfin is under GPLv2 
 - Skia is under BSD-3-CLAUSE 
 - Bundled combination of the above would likely fall under GPLv2 or some dual license?

# One more thing

Microsoft does not currently support dotNET5+ on FreeBSD so this package and its binaries might have limited support across FreeBSD versions. If you are knowledgeable in the inner-workings of FreeBSD please drop over to https://github.com/dotnet/runtime/issues/14537 if you would like to help out getting dotNET on to FreeBSD.
