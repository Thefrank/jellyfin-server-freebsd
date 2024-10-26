--- MediaBrowser.Controller/MediaEncoding/EncodingHelper.cs.orig	2024-10-26 18:17:18 UTC
+++ MediaBrowser.Controller/MediaEncoding/EncodingHelper.cs
@@ -894,7 +894,7 @@ namespace MediaBrowser.Controller.MediaEncoding
         private string GetQsvDeviceArgs(string renderNodePath, string alias)
         {
             var arg = " -init_hw_device qsv=" + (alias ?? QsvAlias);
-            if (OperatingSystem.IsLinux())
+            if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
             {
                 // derive qsv from vaapi device
                 return GetVaapiDeviceArgs(renderNodePath, "iHD", "i915", "0x8086", null, VaapiAlias) + arg + "@" + VaapiAlias;
@@ -971,6 +971,7 @@ namespace MediaBrowser.Controller.MediaEncoding
             var args = new StringBuilder();
             var isWindows = OperatingSystem.IsWindows();
             var isLinux = OperatingSystem.IsLinux();
+            var isFreeBSD = OperatingSystem.IsFreeBSD();
             var isMacOS = OperatingSystem.IsMacOS();
             var optHwaccelType = options.HardwareAccelerationType;
             var vidDecoder = GetHardwareVideoDecoder(state, options) ?? string.Empty;
@@ -978,7 +979,7 @@ namespace MediaBrowser.Controller.MediaEncoding
 
             if (optHwaccelType == HardwareAccelerationType.vaapi)
             {
-                if (!isLinux || !_mediaEncoder.SupportsHwaccel("vaapi"))
+                if ((!isLinux && !isFreeBSD) || !_mediaEncoder.SupportsHwaccel("vaapi"))
                 {
                     return string.Empty;
                 }
@@ -1052,7 +1053,7 @@ namespace MediaBrowser.Controller.MediaEncoding
             }
             else if (optHwaccelType == HardwareAccelerationType.qsv)
             {
-                if ((!isLinux && !isWindows) || !_mediaEncoder.SupportsHwaccel("qsv"))
+                if ((!isLinux && !isWindows && !isFreeBSD) || !_mediaEncoder.SupportsHwaccel("qsv"))
                 {
                     return string.Empty;
                 }
@@ -4010,6 +4011,7 @@ namespace MediaBrowser.Controller.MediaEncoding
 
             var isWindows = OperatingSystem.IsWindows();
             var isLinux = OperatingSystem.IsLinux();
+            var isFreeBSD = OperatingSystem.IsFreeBSD();
             var vidDecoder = GetHardwareVideoDecoder(state, options) ?? string.Empty;
             var isSwDecoder = string.IsNullOrEmpty(vidDecoder);
             var isSwEncoder = !vidEncoder.Contains("qsv", StringComparison.OrdinalIgnoreCase);
@@ -4017,7 +4019,7 @@ namespace MediaBrowser.Controller.MediaEncoding
             var isIntelDx11OclSupported = isWindows
                 && _mediaEncoder.SupportsHwaccel("d3d11va")
                 && isQsvOclSupported;
-            var isIntelVaapiOclSupported = isLinux
+            var isIntelVaapiOclSupported = (isLinux || isFreeBSD)
                 && IsVaapiSupported(state)
                 && isQsvOclSupported;
 
@@ -4599,10 +4601,11 @@ namespace MediaBrowser.Controller.MediaEncoding
             }
 
             var isLinux = OperatingSystem.IsLinux();
+            var isFreeBSD = OperatingSystem.IsFreeBSD();
             var vidDecoder = GetHardwareVideoDecoder(state, options) ?? string.Empty;
             var isSwDecoder = string.IsNullOrEmpty(vidDecoder);
             var isSwEncoder = !vidEncoder.Contains("vaapi", StringComparison.OrdinalIgnoreCase);
-            var isVaapiFullSupported = isLinux && IsVaapiSupported(state) && IsVaapiFullSupported();
+            var isVaapiFullSupported = (isLinux || isFreeBSD) && IsVaapiSupported(state) && IsVaapiFullSupported();
             var isVaapiOclSupported = isVaapiFullSupported && IsOpenclFullSupported();
             var isVaapiVkSupported = isVaapiFullSupported && IsVulkanFullSupported();
 
@@ -6129,11 +6132,12 @@ namespace MediaBrowser.Controller.MediaEncoding
         {
             var isWindows = OperatingSystem.IsWindows();
             var isLinux = OperatingSystem.IsLinux();
+            var isFreeBSD = OperatingSystem.IsFreeBSD();
             var isMacOS = OperatingSystem.IsMacOS();
             var isD3d11Supported = isWindows && _mediaEncoder.SupportsHwaccel("d3d11va");
-            var isVaapiSupported = isLinux && IsVaapiSupported(state);
+            var isVaapiSupported = (isLinux || isFreeBSD) && IsVaapiSupported(state);
             var isCudaSupported = (isLinux || isWindows) && IsCudaFullSupported();
-            var isQsvSupported = (isLinux || isWindows) && _mediaEncoder.SupportsHwaccel("qsv");
+            var isQsvSupported = (isLinux || isWindows || isFreeBSD) && _mediaEncoder.SupportsHwaccel("qsv");
             var isVideotoolboxSupported = isMacOS && _mediaEncoder.SupportsHwaccel("videotoolbox");
             var isRkmppSupported = isLinux && IsRkmppFullSupported();
             var isCodecAvailable = options.HardwareDecodingCodecs.Contains(videoCodec, StringComparison.OrdinalIgnoreCase);
@@ -6282,8 +6286,9 @@ namespace MediaBrowser.Controller.MediaEncoding
         {
             var isWindows = OperatingSystem.IsWindows();
             var isLinux = OperatingSystem.IsLinux();
+            var isFreeBSD = OperatingSystem.IsFreeBSD();
 
-            if ((!isWindows && !isLinux)
+            if ((!isWindows && !isLinux && !isFreeBSD)
                 || options.HardwareAccelerationType != HardwareAccelerationType.qsv)
             {
                 return null;
@@ -6293,7 +6298,7 @@ namespace MediaBrowser.Controller.MediaEncoding
             var isIntelDx11OclSupported = isWindows
                 && _mediaEncoder.SupportsHwaccel("d3d11va")
                 && isQsvOclSupported;
-            var isIntelVaapiOclSupported = isLinux
+            var isIntelVaapiOclSupported = (isLinux || isFreeBSD)
                 && IsVaapiSupported(state)
                 && isQsvOclSupported;
             var hwSurface = (isIntelDx11OclSupported || isIntelVaapiOclSupported)
@@ -6492,7 +6497,7 @@ namespace MediaBrowser.Controller.MediaEncoding
 
         public string GetVaapiVidDecoder(EncodingJobInfo state, EncodingOptions options, MediaStream videoStream, int bitDepth)
         {
-            if (!OperatingSystem.IsLinux()
+            if ((!OperatingSystem.IsLinux() && !OperatingSystem.IsFreeBSD())
                 || options.HardwareAccelerationType != HardwareAccelerationType.vaapi)
             {
                 return null;
