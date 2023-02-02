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

`fetch https://github.com/Thefrank/jellyfin-server-freebsd/releases/download/v10.8.8/jellyfinserver-10.8.8.pkg`

Now we install it:

`pkg install jellyfinserver-10.8.8.pkg`

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

## Hardware encoding (Intel)

### Packages and script to add inside the Jail 

- Back on the jails list find your newly created jail for jellyfin and click "Shell" :
  - Install vautils : `pkg install libva-utils`
  - Install intel driver : [VAAPI driver support page](https://github.com/intel/media-driver#decodingencoding-features)
    - 4th gen and older : `pkg install libva-intel-driver`
    - 5th gen and newer : `pkg install libva-intel-media-driver`

  - Add lffmpeg script to add missing vaapi command

     Create the lffmpeg script file (available here : [url](script/lffmpeg))

     ```
     cd /usr/local/bin
     ee lffmpeg
     ```

     Paste the file content :

     ```
     #!/bin/sh
     ffmpeg -hwaccel vaapi "$@"
     ```

     Close the editor with [ESC] and enter

  - Make lffmpeg script executable

     `chmod +x lffmpeg`

### Script to add on the TrueNas Core Host

- Loading kernel module + adding jail config to pass /dev/dri and /dev/drm
  - Open a ssh shell on the TrueNas host 
    Create a script file in the root folder (available here : [url](script/enable_gpu_jails.sh)) :

    `ee /root/enable_gpu_jails.sh`

    Paste the file content :

    ```
    #!/bin/sh

    echo '[devfsrules_bpfjail=101]
    add path 'bpf*' unhide
    [plex_drm=10]
    add include $devfsrules_hide_all
    add include $devfsrules_unhide_basic
    add include $devfsrules_unhide_login
    add include $devfsrules_jail
    add include $devfsrules_bpfjail
    add path 'dri*' unhide
    add path 'dri/*' unhide
    add path 'drm*' unhide
    add path 'drm/*' unhide' >> /etc/devfs.rules

    service devfs restart

    kldload /boot/modules/i915kms.ko
    ```

    Close the editor with [ESC] and enter

  - Make script executable
    `chmod +x /root/enable_gpu_jails.sh`

  - Run the script 
    `/root/enable_gpu_jails.sh`
    
### Required editing to the Jail and Test

- Stop the Jellyfin jail
- open the Edit for the jail
  - Navigate to the 'Jail Properties' tab
  - Look for the devfs_ruleset (should be the first option on the left)
  - Change the rulset number to 10
  - Save and start the jail
- Open a shell
  - type 'vainfo' and look for entrypoints like : 'VAProfileH264Main : VAEntrypointEncSlice' (there can be multiple)
  - add the group video to the Jellyfin user (replace jellyfinserver if the user was changed with the sys_rc command during service setup
    - `pw group mod video -m jellyfinserver`
  - close the shell
- Connect to the Jellyfin UI
  - Verify that playback does work before enabling hardware encoding
  - Once software playback is working go to Server --> Playback from the Dashboard
    - Change the 'Hardware acceleration:' option from 'None' to 'VAAPI'
    - Find the "ffmpeg path" option (arround the middle of the page)
    - Change the value from 'ffmpeg' to '/usr/local/bin/lffmpeg'
    - Save
  - Hardware encoding should now be working
  - If everthing now works
    - Return to the TrueNas Core UI and navigate to Tasks --> Init/Shutdown Scripts
    - Click [ADD]
    - Change Type to 'Script'
    - Navigate to '/root/enable_gpu_jails.sh'
    - Change When to 'PostInit'
    - Click [Submit]

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
- Jellyfin can't see my mount points / files!
  - Double check your permissions. Jellyfin does not need to be the owner, but its GID should be a part of a group that can access the mount/file. This is also your reminder to keep UID/GID consistent between host and guest systems.
- Posters aren't rendering! Something `libSkiaSharp` version related in the log!
  - Tests don't seem to cover if the right version of `libSkipSharp` is bundled with Jellyfin. I try and check the upstream log for what version is now required but I may have missed it. Open a ticket to remind me to rebuild the library. 
- System kernel panic and reset when using VAAPI
  - In the Bios :
    - Try forcing the integrated GPU as first GPU
    - Set at least 128Mb of ram to the integrated GPU and 256 of pre allocated DMVT
  - Plug a monitor or a Dummy HDMI (some integrated GPU seem to require a monitor for memory allocation)
