/****************************************************************************
 * Copyright (c) 2018 yuanhuibin@putao.com
 ****************************************************************************/

using System.IO;
using UnityEngine;

public static class PathUtil 
{
    public static string GetAssetBundleOutPath()
    {
        string outPath = GetPlatformPath() + "/" + GetPlatformName() + "/";

        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        return outPath;
    }

    private static string GetPlatformPath()
    {
        switch(Application.platform)
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
        switch(Application.platform)
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
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                return "jar:file://"+GetAssetBundleOutPath();
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXPlayer:
                return "file:///" + GetAssetBundleOutPath();
            // Add more build platform for your own.
            // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
            default:
                return null;
        }
    }
}
