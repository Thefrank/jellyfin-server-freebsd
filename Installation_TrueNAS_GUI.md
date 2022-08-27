# Installation overview using TrueNAS GUI
## Overview
Jellyfin currently does not use the plugin system that TrueNAS uses so this will require more interaction from the user than what normally would be expected.

If you are not comfortable setting up a jail or using the shell please wait until someone (maybe me?) adds it to the community plugin libary.

If you are installing from base FreeBSD, I have included some general details that you can use for you jail at the end of the "Jail Setup" section

As of Jellyfin >= 10.7.7, installation requires `pkg` >= 1.17.0. This is for the newer `.pkg` format.

## Jail Setup
1. From the main screen select Jails

2. Click ADD

3. Click Advanced Jail Creation

4. Name (any name will work): Jellyfin

5. Jail Type: Default (Clone Jail)

6. Release: 12.2-Release (or newer)

7. Configure Basic Properties to your liking but add AT LEAST ONE of
- [x] VNET 
- [X] IPv6

  * (Not having VNET means things like DLNA will not work)
  * (You may also encouter other networking issues! See: troubleshooting)
  * (The service file will try and work around user configuration issues but is not perfect)
 
8. Configure Jail Properties to your liking but add
- [x] allow_raw_sockets
  
  * (Highly suggested for easier troubleshooting)
- [x] allow_mlock

  * (This is REQUIRED)

9. Configure Network Properties to your liking

10. Configure Custom Properties to your liking

11. Click Save


Summary for base FreeBSD users:

Required:
- Unprivileged mlock (usually allow_mlock=1, allow.mlock=1, or security.bsd.unprivileged_mlock: 1)
- At least one of:
  - VNET (usually, vnet=1 or security.jail.vnet: 1)
  - If using IPv6: ip6=inherit

Suggested:
- raw sockets (usually allow_raw_sockets=1 or security.jail.allow_raw_sockets: 1)

## Jellyfin Installation

Back on the jails list find your newly created jail for jellyfin and click "Shell"

Download the version you want you can find the releases of jellyfinserver here: https://github.com/Thefrank/jellyfin-server-freebsd/releases/

You can just copy and paste the full download URL and `fetch` will be able to download it:

`fetch https://github.com/Thefrank/jellyfin-server-freebsd/releases/download/v10.8.4/jellyfinserver-10.8.4.pkg`

Now we install it:

`pkg install jellyfinserver-10.8.4.pkg`

Don't close the shell out yet we still have a few more things!

## Configuring Jellyfin

Now that we have it installed a few more steps are required.

### User and Group Setup
Creation of a user and group is handled by the package on installation but if you want to use your own, set that up now.

We still are not done with the shell!

### Service Setup

Time to enable the service

`sysrc jellyfinserver_enable=TRUE`

If you want to use a different UID/GID please change that now:

`sysrc jellyfinserver_user="USER_YOU_WANT"`

`sysrc jellyfinserver_group="GROUP_YOU_WANT"`

Almost done, let's start the service:

`service jellyfinserver start`

If everything went according to plan then jellyfin should be up and running on the IP of the jail (port 8096)!

(You can now safely close the shell)

## Updating Jellyfin Installation

This is similar to installing Jellyfin but with fewer steps:

- Stop the current jellyfin server (NOT THE JAIL)

- Use `fetch` to download the newest pkg from the releases page found here: https://github.com/Thefrank/jellyfin-server-freebsd/releases/latest

- Use `pkg` to install said package

- Start the jellyfin server

## Troubleshooting and other things to note
- /Confusing HTTP/S messages/ (or other problems with jellyfin not being able to use the internet)
   - Make sure you have VNET turned on for your jail.
     - I have VNET turned on!
       - Don't try and have jellyfin monitor SMB shares `libinotify` does not impliment this well
          - I am not using SMB shares!
             - Turn off file monitor if you have more than ~5k files that it needs to monitor. This is a known issue in `libinotify`
     - I don't want to use VNET!
       - Your jail needs `ip6=inherit` if using ipv6. Using `ip6=new` WILL NOT WORK. Blame a combination of how FreeBSD exposes its network stack to jails and how dotNET handles the responses it gets.
- Something SQL related
  - This should be done automatically on install but try: `ln -s /usr/local/lib/libsqlite3.so /usr/local/lib/libe_sqlite3`
