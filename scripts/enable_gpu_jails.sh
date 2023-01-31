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
