# Installation overview using TrueNAS GUI
## Overview
Jellyfin currently does not use the plugin system that TrueNAS uses so this will require more interaction from the user than what normally would be expected.

If you are not comfortable setting up a jail or using the shell please wait until someone (maybe me?) adds it to the community plugin libary.

If you are installing from base FreeBSD there is a different guide for that!

## Jail Setup
1. From the main screen select Jails

2. Click ADD

3. Click Advanced Jail Creation

4. Name (any name will work): Jellyfin

5. Jail Type: Default (Clone Jail)

6. Release: 12.2-Release (or newer)

7. Configure Basic Properties to your liking

8. Configure Jail Properties to your liking but add
- [x] allow_raw_sockets
- [x] allow_mlock

9. Configure Network Properties to your liking

10. Configure Custom Properties to your liking

11. Click Save

## Jellyfin Installation

Back on the jails list find your newly created jail for jellyfin and click "Shell"

Download the version you want you can find the releases of jellyfinserver here: https://github.com/Thefrank/jellyfin-server-freebsd/releases/

You can just copy and paste the full download URL and `fetch` will be able to download it:

`fetch https://github.com/Thefrank/jellyfin-server-freebsd/releases/download/v10.7.6/jellyfinserver-10.7.6.txz`

Now we install it:

`pkg install jellyfinserver-10.7.6.txz`

Currently, jellyfinserver looks for a library that is not able to be built for FreeBSD however, we have a "close enough" alternative so we symlink to it.

`ln -s /usr/local/lib/libsqlite3.so /usr/local/lib/libe_sqlite3`

Don't close the shell out yet we still have a few more things!

## Configuring Jellyfin

Now that we have it installed a few more steps are required.

### User and Group Setup
Creation of a user is usually handled by the package on installation but is left out here because we don't have a reserved UID/GID for jellyfin so...

I suggest creating a user/group named `jellyfin`

`pw user add jellyfin -c jellyfin -u 710 -d /nonexistent -s /usr/bin/nologin`

The above will create a "jellyfin" user with UID 710.

We still are not done with the shell!

### Service Setup

Time to enable the service

`sysrc jellyfinserver_enable=TRUE`

If you did not create a user/group named `jellyfin` you will need to tell the service file what user/group it should be running under

`sysrc jellyfinserver_user="USER_YOU_WANT"`

`sysrc jellyfinserver_group="GROUP_YOU_WANT"`

Almost done, let's start the service:

`service jellyfinserver start`

If everything went according to plan then jellyfin should be up and running on the IP of the jail!

(You can now safely close the shell)

## Troubleshooting and other things to note
- /Confusing HTTP/S messages/ (or other problems with jellyfin not being able to use the internet)
   - Make sure you have VNET turned on for your jail.
     - I have VNET turned on!
       - Don't try and have jellyfin monitor SMB shares `libinotify` does not impliment this well
          - I am not using SMB shares!
             - Turn off file monitor if you have more than ~5k files that it needs to monitor. This is a known issue in `libinotify`
