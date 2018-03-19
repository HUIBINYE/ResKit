/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections;
using UnityEngine;

/// <summary>
/// 加载AssetBundle
/// </summary>
public class AssetBundleLoader
{
    private AssetLoader mAssetLoader;
    
    /// <summary>
    /// WWW对象
    /// </summary>
    private WWW mWWW;

    /// <summary>
    /// 包名称
    /// </summary>
    private string mBundleName;

    /// <summary>
    /// 路径
    /// </summary>
    private string mBundlePath;

    private float mProgress;

    private LoadProgress mLoadProgress;

    private LoadComplete mLoadComplete;

    public AssetBundleLoader(LoadComplete loadComplete, LoadProgress loadProgress,string bundleName)
    {
        mLoadComplete = loadComplete;
        mLoadProgress = loadProgress;
        mBundleName = bundleName;
        mProgress = 0f;
        mBundlePath = PathUtil.GetWWWPath() + "/" + bundleName;
        mWWW = null;
        mAssetLoader = null;
    }

    public IEnumerator Load()
    {
        mWWW = new WWW(mBundlePath);
        while (!mWWW.isDone)
        {
            mProgress = mWWW.progress;
            if (mLoadProgress != null)
            {
                mLoadProgress(mBundleName,mProgress);
            }

            yield return mWWW.progress;
        }

        mProgress = mWWW.progress;

        if (mProgress >= 1f)
        {
            //加载完成
            mAssetLoader = new AssetLoader();
            mAssetLoader.AssetBundle = mWWW.assetBundle;

            if (mLoadProgress != null)
            {
                mLoadProgress(mBundleName, mProgress);
            }

            if (mLoadComplete != null)
            {
                mLoadComplete(mBundleName);
            }
        }
    }
    
    public T LoadAsset<T>(string assetName) where T:Object
    {
        if (mAssetLoader == null)
        {
            Debug.LogError("当前资源包为空,无法获取:"+assetName);
            return null;
        }
        return mAssetLoader.LoadAsset<T>(assetName) as T;
    }
    
    public Object[] LoadAllAssets()
    {
        if (mAssetLoader == null)
        {
            Debug.LogError("当前资源包为空,无法获取");
            return null;
        }

        return mAssetLoader.LoadAllAssets();
    }
    
    public Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (mAssetLoader == null)
        {
            Debug.LogError("当前资源包为空,无法获取:"+assetName);
            return null;
        }
        return mAssetLoader.LoadAssetWithSubAssets(assetName);
    }

    public void UnLoadAsset(Object asset)
    {
        if (mAssetLoader == null)
        {
            Debug.LogError("当前mAssetLoader为空");
            return;
        }
        mAssetLoader.UnLoadAsset(asset);
    }
    
    public void Dispose()
    {
        if (mAssetLoader == null)
        {
            return;
        }
        mAssetLoader.Dispose();
        mAssetLoader = null;
    }

    public void GetAllAssetNames()
    {
        mAssetLoader.GetAllAssetNames();
    }


}
