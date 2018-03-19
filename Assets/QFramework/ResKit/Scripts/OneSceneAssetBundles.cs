/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制一个场景里所有的资源包
/// </summary>
public class OneSceneAssetBundles
{
    private Dictionary<string, AssetBundleRelation> mNameBundleDict;
    
    private Dictionary<string, AssetCaching> mNameChacheDict;

    private string mSceneName;//当前场景的名字

    public OneSceneAssetBundles(string sceneName)
    {
        mSceneName = sceneName;
        mNameBundleDict = new Dictionary<string, AssetBundleRelation>();
    }

    /// <summary>
    /// 加载资源包
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="loadProgress"></param>
    /// <param name="loadAssetBundleCallback"></param>
    public void LoadAssetBundle(string bundleName,LoadProgress loadProgress,LoadAssetBundleCallback loadAssetBundleCallback)
    {
        if (mNameBundleDict.ContainsKey(bundleName))
        {
            Debug.LogWarning("已经加载了"+bundleName);
            return;
        }
        AssetBundleRelation assetBundleRelation = new AssetBundleRelation(bundleName,loadProgress);
        mNameBundleDict.Add(bundleName,assetBundleRelation);
        loadAssetBundleCallback(mSceneName,bundleName);
    }

    public IEnumerator Load(string bundleName)
    {
        while (!AssetBundleManifestLoader.Instance.Finish)
        {
            yield return null;
        }

        AssetBundleRelation assetBundleRelation =  mNameBundleDict[bundleName];

//        assetBundleRelation.GetAllDependence();

       string[] dependencesBundles =  AssetBundleManifestLoader.Instance.GetDependences(bundleName);

        foreach (var item in dependencesBundles)
        {
            assetBundleRelation.AddDependence(item);
            yield return LoadDependence(item, bundleName, assetBundleRelation.LoadProgress);
        }

        yield return assetBundleRelation.Load();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="referenceBundleName"></param>
    /// <param name="loadProgress"></param>
    /// <returns></returns>
    private IEnumerator LoadDependence(string bundleName, string referenceBundleName, LoadProgress loadProgress)
    {
        if (mNameBundleDict.ContainsKey(bundleName))
        {
            AssetBundleRelation assetBundleRelation = mNameBundleDict[bundleName];
            
            assetBundleRelation.AddReferenceBundle(referenceBundleName);
        }
        else
        {
            AssetBundleRelation assetBundleRelation = new AssetBundleRelation(bundleName,loadProgress);
            assetBundleRelation.AddReferenceBundle(referenceBundleName);
            mNameBundleDict.Add(bundleName,assetBundleRelation);
            yield return Load(bundleName);
        }
    }
    
    public T LoadAsset<T>(string bundleName,string assetName) where T:Object
    {
        if (mNameChacheDict.ContainsKey(bundleName))
        {
            Object[] assets = mNameChacheDict[bundleName].GetAsset(assetName);
            if (assets != null)
            {
                return assets[0] as T;
            }
   
        }

        if (!mNameBundleDict.ContainsKey(bundleName))
        {
            Debug.LogError("当前"+bundleName+"为空,无法获取:"+assetName);
            return null;
        }

        T asset = mNameBundleDict[bundleName].LoadAsset<T>(assetName) as T;
        TempObject tempObject = new TempObject(asset);

        if (mNameChacheDict.ContainsKey(bundleName))
        {
            mNameChacheDict[bundleName].AddAsset(assetName,tempObject);
        }
        else
        {
            AssetCaching assetCaching = new AssetCaching();
            assetCaching.AddAsset(assetName,tempObject);
            mNameChacheDict.Add(bundleName,assetCaching);
        }
        return asset;
    }
    
    public Object[] LoadAllAssets(string bundleName)
    {
        if (!mNameBundleDict.ContainsKey(bundleName))
        {
            Debug.LogError("当前"+bundleName+"没有加载,无法获取");
            return null;
        }

        return mNameBundleDict[bundleName].LoadAllAssets();
    }
    
    public Object[] LoadAssetWithSubAssets(string bundleName,string assetName)
    {
//        if (!mNameBundleDict.ContainsKey(bundleName))
//        {
//            Debug.LogError("当前"+bundleName+"没有加载,无法获取");
//            return null;
//        }
//        return mNameBundleDict[bundleName].LoadAssetWithSubAssets(assetName);
        if (mNameChacheDict.ContainsKey(bundleName))
        {
            Object[] assets = mNameChacheDict[bundleName].GetAsset(assetName);
            if (assets != null)
            {
                return assets;
            }
   
        }

        if (!mNameBundleDict.ContainsKey(bundleName))
        {
            Debug.LogError("当前"+bundleName+"为空,无法获取:"+assetName);
            return null;
        }

        Object[] asset = mNameBundleDict[bundleName].LoadAssetWithSubAssets(assetName);
        TempObject tempObject = new TempObject(asset);

        if (mNameChacheDict.ContainsKey(bundleName))
        {
            mNameChacheDict[bundleName].AddAsset(assetName,tempObject);
        }
        else
        {
            AssetCaching assetCaching = new AssetCaching();
            assetCaching.AddAsset(assetName,tempObject);
            mNameChacheDict.Add(bundleName,assetCaching);
        }
        return asset;
    }

    public void UnLoadAsset(string bundleName,Object asset)
    {
        if (!mNameBundleDict.ContainsKey(bundleName) )
        {
            Debug.LogError("当前mAssetBundleLoader为空");
            return;
        }
        mNameBundleDict[bundleName].UnLoadAsset(asset);
    }
    
    public void Dispose(string bundleName)
    {
        if (!mNameBundleDict.ContainsKey(bundleName))
        {
            return;
        }
        mNameBundleDict[bundleName].Dispose();
        mNameBundleDict[bundleName] = null;
    }

    public void GetAllAssetNames(string bundleName)
    {
        mNameBundleDict[bundleName].GetAllAssetNames();
    }
}
