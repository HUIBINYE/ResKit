/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源缓存
/// </summary>
public class AssetCaching
{
    /// <summary>
    /// 已经加载过的资源名字和资源的映射关系
    /// </summary>
    private Dictionary<string, TempObject> mNameAssetDict;

    public AssetCaching()
    {
        mNameAssetDict = new Dictionary<string, TempObject>();
    }

    public void AddAsset(string assetName, TempObject asset)
    {
        if (!mNameAssetDict.ContainsKey(assetName))
        {
            mNameAssetDict.Add(assetName,asset);
        }
        else
        {
            Debug.LogError("已经加载过了:"+assetName);
        }
    }

    public Object[] GetAsset(string assetName)
    {
        if (mNameAssetDict.ContainsKey(assetName))
        {
            return mNameAssetDict[assetName].Asset.ToArray();
        }
        else
        {
            Debug.LogError("尚未加载:"+assetName);
            return null;
        }
    }

    public void UnLoadAsset(string assetName)
    {
        if (mNameAssetDict.ContainsKey(assetName))
        {
            mNameAssetDict[assetName].UnLoadAsset();
        }
        else
        {
            Debug.LogError("尚未加载:"+assetName);
        }
    }

    public void UnLoadAllAsset()
    {
        foreach (var item in mNameAssetDict.Keys)
        {
            UnLoadAsset(item);
        }
        mNameAssetDict.Clear();
    }
}
