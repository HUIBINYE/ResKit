/****************************************************************************
 * Copyright (c) 2018.3 huibinye
 * 
 * http://qframework.io
 * https://github.com/HUIBINYE/ResKit
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

namespace QFramework.ResKit
{
    using System.IO;
    using UnityEngine;
#if UNITY_EDITOR	
    using UnityEditor;
#endif
    
    public static class PathUtil
    {
        public static string GetAssetBundleOutPath()
        {
            var outPath = GetPlatformPath() + "/" + GetPlatformName() + "/";

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            return outPath;
        }

        private static string GetPlatformPath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    return Application.persistentDataPath;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return Application.streamingAssetsPath;
                default:
                    return null;
            }
        }

        public static string GetPlatformName()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "OSX";
                default:
                    return null;
            }
        }

        public static string GetWWWPath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "jar:file://" + GetAssetBundleOutPath();
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "file:///" + GetAssetBundleOutPath();
                // Add more build platform for your own.
                // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
                default:
                    return null;
            }
        }
	
#if UNITY_EDITOR
        public static string GetPlatformForAssetBundles(BuildTarget target)
        {
            switch(target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
#if UNITY_5 
                case BuildTarget.StandaloneOSXUniversal:
#else
			case BuildTarget.StandaloneOSX:
			#endif
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
#endif
	
        static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch(platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
    }
}