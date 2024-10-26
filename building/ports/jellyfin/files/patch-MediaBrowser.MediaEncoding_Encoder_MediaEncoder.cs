--- MediaBrowser.MediaEncoding/Encoder/MediaEncoder.cs.orig	2024-10-26 18:17:18 UTC
+++ MediaBrowser.MediaEncoding/Encoder/MediaEncoder.cs
@@ -221,7 +221,7 @@ namespace MediaBrowser.MediaEncoding.Encoder
                 _isLowPriorityHwDecodeSupported = validator.CheckSupportedHwaccelFlag("low_priority");
 
                 // Check the Vaapi device vendor
-                if (OperatingSystem.IsLinux()
+                if ((OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
                     && SupportsHwaccel("vaapi")
                     && !string.IsNullOrEmpty(options.VaapiDevice)
                     && options.HardwareAccelerationType == HardwareAccelerationType.vaapi)
