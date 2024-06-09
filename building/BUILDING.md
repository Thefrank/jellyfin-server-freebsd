# Instructions for Jellyfin 10.9.x

## Building with ports
An experimental ports build that only uses one prebuilt (Skia) can be found in the ports folder.

Once the bugs are worked out you can use this to build / update jellyfin directly


# Instructions for Jellyfin < = 10.8.13
## Building

Of the binaries currently published, non-prerelease, versions here of Jellyfin there are no patches applied to source.

That being said...

## Patches

I have included for Jellyfin: `build.freebsd.amd64` and `jellyfin.diff`
- `build.freebsd.amd64`
  - Contains a deployment file for anyone wanting to use the build script
  - Based on old version of `build.linux.amd64`
- `jellyfin.dff`
  - A work in progress of getting Jellyfin to see/use VAAPI under FreeBSD.
  - Feedback/PRs welcome
 
I have included for Jellyfin-web: No patches required here


## Non-patches to consider when building

For Jellyfin:
- `sed -i '' '37,40d' ./build.sh`
  - removes packaging tool check

For Jellyfin-web:
- `git grep -l "/bin/bash" | xargs sed -i '' -e 's/\/bin\/bash/\/usr\/bin\/env bash/g'`
  - "shebang fixes" as FreeBSD does not store `bash` in `/bin` as `bash` is not a default shell.
  - This really only needs to hit the `build.portable` deployment file.
  - `env` can find `bash` and it removes a hard-coded path.