## Hardware encoding (Intel)

### Packages to add inside the Jail 

- Back on the jails list find your newly created jail for jellyfin and click "Shell" :
  - Execute Theses commands : 

    ```
    pkg install libva-utils
    pkg install libva-intel-media-driver
    ```

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

### Scripts to add on the TrueNas Core Host

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

- Stop the JellyFin jail
- open the Edit for the jail
  - Navigate to the 'Jail Properties' tab
  - Look for the devfs_ruleset (should be the first option on the left)
  - Change the rulset number to 10
  - Save and start the jail
- Open a shell
  - type 'vainfo' and look for entrypoints like 'VAProfileH264Main : VAEntrypointEncSlice' (there can be multiple)
  - close the shell
- Connect to the JellyFin UI
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
