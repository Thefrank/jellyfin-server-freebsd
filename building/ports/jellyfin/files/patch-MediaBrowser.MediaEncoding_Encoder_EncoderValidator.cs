--- MediaBrowser.MediaEncoding/Encoder/EncoderValidator.cs.orig	2024-10-26 18:17:18 UTC
+++ MediaBrowser.MediaEncoding/Encoder/EncoderValidator.cs
@@ -382,7 +382,7 @@ namespace MediaBrowser.MediaEncoding.Encoder
 
         public bool CheckVaapiDeviceByDriverName(string driverName, string renderNodePath)
         {
-            if (!OperatingSystem.IsLinux())
+            if (!OperatingSystem.IsLinux() && !OperatingSystem.IsFreeBSD())
             {
                 return false;
             }
@@ -406,7 +406,7 @@ namespace MediaBrowser.MediaEncoding.Encoder
 
         public bool CheckVulkanDrmDeviceByExtensionName(string renderNodePath, string[] vulkanExtensions)
         {
-            if (!OperatingSystem.IsLinux())
+            if (!OperatingSystem.IsLinux() && !OperatingSystem.IsFreeBSD())
             {
                 return false;
             }
