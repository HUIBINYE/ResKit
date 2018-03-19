/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理资源包的依赖关系
/// </summary>
public class AssetBundleRelation
{
    private AssetBundleLoader mAssetBundleLoader;

    private string mBundleName;

    private LoadComplete mLoadComplete;

    private LoadProgress mLoadProgress;

    public LoadProgress LoadProgress
    {
        get { return mLoadProgress; }
    }

    private bool mFinish;

    public bool Finish
    {
        get { return mFinish; }
    }

    public AssetBundleRelation(string bundleName, LoadProgress loadProgress)
    {
        mBundleName = bundleName;
        mLoadProgress = loadProgress;
        mFinish = false;
        mDependenceBundle = new List<string>();
        mReferenceBundle = new List<string>();
        mAssetBundleLoader = new AssetBundleLoader(OnLoadComplete,mLoadProgress,mBundleName);
    }
    
    public IEnumerator Load()
    {
        yield return mAssetBundleLoader.Load();
    }

    private void OnLoadComplete(string bundleName)
    {
        mFinish = true;
    }
    
    //依赖关系
    private List<string> mDependenceBundle;

    public void AddDependence(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName)) return;
        if (mDependenceBundle.Contains(bundleName)) return;
        mDependenceBundle.Add(bundleName);
    }
    public void RemoveDependence(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName)) return;
        if (mDependenceBundle.Contains(bundleName))
        {
            mDependenceBundle.Remove(bundleName);
        }
    }
    
    //被依赖关系

    private List<string> mReferenceBundle;
    
    public void AddReferenceBundle(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName)) return;
        if (mDependenceBundle.Contains(bundleName)) return;
        mDependenceBundle.Add(bundleName);
    }
    public bool RemoveReferenceBundle(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName)) return false;
        if (mDependenceBundle.Contains(bundleName))
        {
            mDependenceBundle.Remove(bundleName);

            if (mReferenceBundle.Count > 0)
            {
                return false;
            }
            else
            {
                //shifang
                Dispose();
                return true;
            }
        }
        return false;
    }
    
    //++++++++++++++++++++++++++
    public string[] GetAllDependence()
    {
        return mDependenceBundle.ToArray();
    }
    
    
    public T LoadAsset<T>(string assetName) where T:Object
    {
        if (mAssetBundleLoader == null)
        {
            Debug.LogError("当前mAssetBundleLoader为空,无法获取:"+assetName);
            return null;
        }
        return mAssetBundleLoader.LoadAsset<T>(assetName) as T;
    }
    
    public Object[] LoadAllAssets()
    {
        if (mAssetBundleLoader == null)
        {
            Debug.LogError("当前mAssetBundleLoader为空,无法获取");
            return null;
        }

        return mAssetBundleLoader.LoadAllAssets();
    }
    
    public Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (mAssetBundleLoader == null)
        {
            Debug.LogError("当前mAssetBundleLoader为空,无法获取:"+assetName);
            return null;
        }
        return mAssetBundleLoader.LoadAssetWithSubAssets(assetName);
    }

    public void UnLoadAsset(Object asset)
    {
        if (mAssetBundleLoader == null)
        {
            Debug.LogError("当前mAssetBundleLoader为空");
            return;
        }
        mAssetBundleLoader.UnLoadAsset(asset);
    }
    
    public void Dispose()
    {
        if (mAssetBundleLoader == null)
        {
            return;
        }
        mAssetBundleLoader.Dispose();
        mAssetBundleLoader = null;
    }

    public void GetAllAssetNames()
    {
        mAssetBundleLoader.GetAllAssetNames();
    }
}
