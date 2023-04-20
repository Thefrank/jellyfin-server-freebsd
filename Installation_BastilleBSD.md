# Installation Overview using BestilleBSD
## Overview

There is now a community Template for BastilleBSD users made by @leandroscardua !

If you already familiar with BastilleBSD, you can go directly to deploy step

As of Jellyfin >= 10.7.7, installation requires `pkg` >= 1.17.0. This is for the newer `.pkg` format.

### Before starts the deploy step
https://bastillebsd.org/getting-started/

jellyfin = will be the name of the jail, if you want you can change the name.
step 4 = That configuration will open the port 8096 on host and redirect to the 8096 on the jails.

### Deploy step

1. bastille create jellyfin

2. bastille bootstrap https://gitlab.com/bastillebsd-templates/jellyfin

3. bastille template jellyfin https://gitlab.com/bastillebsd-templates/jellyfin

4. bastille rdr jellyfin tcp 8096 8096

## Updating Jellyfin Installation

This is similar to installing Jellyfin but with fewer steps:

- Connect to the jail
  bastille console jellyfin
  
- Stop the  jellyfin service
  service stop jellyfin

- Use `pkg` to install said package
  pkg install jellyfin

- Start the jellyfin server
  service start jellyfin
