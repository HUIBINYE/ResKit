/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
    private static AssetBundleManager instance;

    public static AssetBundleManager Instance
    {
        get{ return instance;}
    }
    private Dictionary<string, SceneManager> nameScenesDict = new Dictionary<string, SceneManager>();

    private void Awake()
    {
        instance = this;

        //第一步：
        StartCoroutine(AssetBundleManifestLoader.Instance.Load());
    }

    private void OnDestroy()
    {
        nameScenesDict.Clear();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void LoadAssetBundle(string sceneName, string folderName, LoadProgress loadProgress)
    {
        if (!nameScenesDict.ContainsKey(sceneName))
        {
            //先创建场景
            CreateScene(sceneName);
        }
        SceneManager sceneManager = nameScenesDict[sceneName];
        sceneManager.LoadAssetBundle(folderName,loadProgress,LoadAssetBundleCallBack);
    }

    private void CreateScene(string sceneName)
    {
        SceneManager sceneManager = new SceneManager(sceneName);
        sceneManager.ReadRecord(sceneName);
        nameScenesDict.Add(sceneName,sceneManager);
    }

    private void LoadAssetBundleCallBack(string sceneName,string bundleName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            StartCoroutine(sceneManager.Load(bundleName));
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
        }
    }
    
    public string GetBundleName(string sceneName,string folderName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            return sceneManager.GetBundleName(folderName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
            return null;
        }
    }
    
    public bool IsLoading(string sceneName,string folderName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            return sceneManager.IsLoading(folderName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
            return false;
        }
    }

    public bool IsFinish(string sceneName,string folderName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            return sceneManager.IsFinish(folderName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
            return false;
        }
    }
    
    public Object LoadAsset(string sceneName,string folderName, string assetName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            return sceneManager.LoadAsset(folderName,assetName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
            return null;
        }
    }
    public Object[] LoadAllAssets(string sceneName,string folderName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            return sceneManager.LoadAllAssets(folderName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
            return null;
        }
    }

    public Object[] LoadAssetWithSubAssets(string sceneName,string folderName,string assetName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            return sceneManager.LoadAssetWithSubAssets(folderName,assetName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
            return null;
        }
    }
    
    public void UnLoadAsset(string sceneName,string folderName,string assetName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            sceneManager.UnLoadAsset(folderName,assetName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
        }
    }
    
    public void UnLoadAllAsset(string sceneName,string folderName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            sceneManager.UnLoadAllAsset(folderName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
        }
    }

    public void UnLoadAll(string sceneName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            sceneManager.UnLoadAll();
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
        }
    }

    public void Dispose(string sceneName,string folderName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            sceneManager.Dispose(folderName);
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
        }
    }

    public void DisposeAll(string sceneName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            sceneManager.DisposeAll();
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
        }
    }

    public void DisposeAllAndUnLoadAll(string sceneName)
    {
        if (nameScenesDict.ContainsKey(sceneName))
        {
            SceneManager sceneManager = nameScenesDict[sceneName];

            sceneManager.DisposeAllAndUnLoadAll();
        }
        else
        {
            Debug.LogError("不存在场景"+sceneName);
        }
    }
}
