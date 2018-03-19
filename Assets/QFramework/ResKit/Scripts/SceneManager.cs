/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SceneManager
{
    private OneSceneAssetBundles sceneAssetBundles;

    private Dictionary<string, string> folderBundleDict;

    public SceneManager(string sceneName)
    {
        sceneAssetBundles = new OneSceneAssetBundles(sceneName);
        folderBundleDict = new Dictionary<string, string>();
    }

    public void ReadRecord(string sceneName)
    {
        string path = PathUtil.GetAssetBundleOutPath() + "/" + sceneName + "Record.config";

        using (FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                int count = int.Parse(sr.ReadLine());

                for (int i = 0; i < count; i++)
                {
                    string line = sr.ReadLine();
                    string[] kv = line.Split(' ');
                    folderBundleDict.Add(kv[0],kv[1]);
                }
            }
        }
    }

    public string GetBundleName(string folderName)
    {
        if (!folderBundleDict.ContainsKey(folderName))
        {
            Debug.LogError("没有当前文件夹"+folderName);
        }

        return folderBundleDict[folderName];
    }

    public void LoadAssetBundle(string folderName,LoadProgress loadProgress,LoadAssetBundleCallback loadAssetBundleCallback)
    {
        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            sceneAssetBundles.LoadAssetBundle(bundleName,loadProgress,loadAssetBundleCallback);
        }
        else
        {
            Debug.LogError("没有当前文件夹"+folderName+"对应的包");
        }
    }

    public bool IsLoading(string folderName)
    {
        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            return sceneAssetBundles.IsLoading(bundleName);
        }
        else
        {
            Debug.LogError("没有当前文件夹"+folderName+"对应的包");
            return false;
        }
    }

    public bool IsFinish(string folderName)
    {
        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            return sceneAssetBundles.IsFinish(bundleName);
        }
        else
        {
            Debug.LogError("没有当前文件夹"+folderName+"对应的包");
            return false;
        }
    }

    public IEnumerator Load(string bundleName)
    {
        yield return sceneAssetBundles.Load(bundleName);
    }

    public Object LoadAsset(string folderName, string assetName)
    {
        if (sceneAssetBundles == null)
        {
            Debug.LogError("sceneAssetBundles为空");
            return null;
        }

        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            return sceneAssetBundles.LoadAsset<Object>(bundleName,assetName);
        }
        else
        {
            Debug.LogError("sceneAssetBundles为空");
            return null;
        }
    }
    public Object[] LoadAllAssets(string folderName)
    {
        if (sceneAssetBundles == null)
        {
            Debug.LogError("sceneAssetBundles为空");
            return null;
        }

        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            return sceneAssetBundles.LoadAllAssets(bundleName);
        }
        else
        {
            Debug.LogError("没有当前文件夹"+folderName+"对应的包");
            return null;
        }
    }

    public Object[] LoadAssetWithSubAssets(string folderName,string assetName)
    {
        if (sceneAssetBundles == null)
        {
            Debug.LogError("sceneAssetBundles为空");
            return null;
        }

        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            return sceneAssetBundles.LoadAssetWithSubAssets(bundleName,assetName);
        }
        else
        {
            Debug.LogError("没有当前文件夹"+folderName+"对应的包");
            return null;
        }
    }
    
    public void UnLoadAsset(string folderName,string assetName)
    {
        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            sceneAssetBundles.UnLoadAsset(bundleName,assetName);
        }
        else
        {
            Debug.LogError("111");
        }
    }
    
    public void UnLoadAllAsset(string folderName)
    {
        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            sceneAssetBundles.UnLoadAllAsset(bundleName);
        }
        else
        {
            Debug.LogError("找不到文件夹:"+folderName+"对应的包");
        }
    }

    public void UnLoadAll()
    {
        sceneAssetBundles.UnLoadAll();
    }

    public void Dispose(string folderName)
    {
        if (folderBundleDict.ContainsKey(folderName))
        {
            string bundleName = folderBundleDict[folderName];
            sceneAssetBundles.Dispose(bundleName);
        }
        else
        {
            Debug.LogError("找不到文件夹:"+folderName+"对应的包");
        }
    }

    public void DisposeAll()
    {
        sceneAssetBundles.DisposeAll();
    }

    public void DisposeAllAndUnLoadAll()
    {
        sceneAssetBundles.DisposeAllAndUnLoadAll();
    }
}
