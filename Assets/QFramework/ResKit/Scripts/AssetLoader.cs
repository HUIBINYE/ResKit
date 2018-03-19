/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using UnityEngine;

/// <summary>
/// 加载包里的资源
/// </summary>
public class AssetLoader:System.IDisposable
{
    private AssetBundle mAssetBundle;

    public AssetBundle AssetBundle
    {
        set { mAssetBundle = value; }
    }

    public T LoadAsset<T>(string assetName) where T:Object
    {
        if (mAssetBundle == null)
        {
            Debug.LogError("当前资源包为空,无法获取:"+assetName);
            return null;
        }
        if (!mAssetBundle.Contains(assetName))
        {
            Debug.LogError("当前资源包不包括:"+assetName);
            return null;
        }
        return mAssetBundle.LoadAsset(assetName) as T;
    }
    
    public Object[] LoadAllAssets()
    {
        if (mAssetBundle == null)
        {
            Debug.LogError("当前资源包为空,无法获取");
            return null;
        }

        return mAssetBundle.LoadAllAssets();
    }
    
    public Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (mAssetBundle == null)
        {
            Debug.LogError("当前资源包为空,无法获取:"+assetName);
            return null;
        }
        if (!mAssetBundle.Contains(assetName))
        {
            Debug.LogError("当前资源包不包括:"+assetName);
            return null;
        }
        return mAssetBundle.LoadAssetWithSubAssets(assetName);
    }

    public void UnLoadAsset(Object asset)
    {
        Resources.UnloadAsset(asset);
    }

    public void Dispose()
    {
        if (mAssetBundle == null)
        {
            return;
        }
        mAssetBundle.Unload(false);
    }

    public void GetAllAssetNames()
    {
        string[] names = mAssetBundle.GetAllAssetNames();

        foreach (var itemName in names)
        {
            Debug.Log(itemName);
        }
    }

}
