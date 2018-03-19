/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections;
using UnityEngine;

public class AssetBundleManifestLoader
{
    private static AssetBundleManifestLoader mManifestLoader;

    public static AssetBundleManifestLoader Instance
    {
        get
        {
            if (mManifestLoader == null)
            {
                mManifestLoader = new AssetBundleManifestLoader();
            }
            return mManifestLoader;
        }
    }

    /// <summary>
    /// Manifest文件
    /// </summary>
    private AssetBundleManifest mManifest;

    private string mMnifestPath;

    private bool mFinish;

    public bool Finish
    {
        get { return mFinish; }
    }

    //全局存在
    public AssetBundle mAssetBundle;


    private AssetBundleManifestLoader()
    {
        mMnifestPath = PathUtil.GetWWWPath() + "/" +PathUtil.GetPlatformName();
        mManifest = null;
        mAssetBundle = null;
        mFinish = false;
    }

    public IEnumerator Load()
    {
        WWW www = new WWW(mMnifestPath);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            if (www.progress >= 1f)
            {
                mAssetBundle = www.assetBundle;
                mManifest = mAssetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                mFinish = true;
            }
        }
    }

    /// <summary>
    /// 获取所有依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public string[] GetDependences(string bundleName)
    {
        return mManifest.GetAllDependencies(bundleName);
    }

    public void UnLoad()
    {
        mAssetBundle.Unload(true);
    }

}
