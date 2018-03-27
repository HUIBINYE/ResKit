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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ResKit
{

    /// <summary>
    /// 控制一个场景里所有的资源包
    /// </summary>
    public class OneSceneAssetBundles
    {
        private Dictionary<string, AssetBundleRelation> mNameBundleDict;

        private Dictionary<string, AssetCaching> mNameChacheDict;

        private string mSceneName; //当前场景的名字

        public OneSceneAssetBundles(string sceneName)
        {
            mSceneName = sceneName;
            mNameBundleDict = new Dictionary<string, AssetBundleRelation>();
            mNameChacheDict = new Dictionary<string, AssetCaching>();
        }

        public bool IsLoading(string bundleName)
        {
            return mNameBundleDict.ContainsKey(bundleName);
        }

        public bool IsFinish(string bundleName)
        {
            if (IsLoading(bundleName))
            {
                return mNameBundleDict[bundleName].Finish;
            }

            return false;
        }

        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="loadProgress"></param>
        /// <param name="loadAssetBundleCallback"></param>
        public void LoadAssetBundle(string bundleName, LoadProgress loadProgress,
            LoadAssetBundleCallback loadAssetBundleCallback)
        {
            if (mNameBundleDict.ContainsKey(bundleName))
            {
                Debug.LogWarning("已经加载了" + bundleName);
                return;
            }

            AssetBundleRelation assetBundleRelation = new AssetBundleRelation(bundleName, loadProgress);
            mNameBundleDict.Add(bundleName, assetBundleRelation);
            loadAssetBundleCallback(mSceneName, bundleName);
        }

        public IEnumerator Load(string bundleName)
        {
            while (!AssetBundleManifestLoader.Instance.Finish)
            {
                yield return null;
            }

            AssetBundleRelation assetBundleRelation = mNameBundleDict[bundleName];

//        assetBundleRelation.GetAllDependence();

            string[] dependencesBundles = AssetBundleManifestLoader.Instance.GetDependences(bundleName);

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
                AssetBundleRelation assetBundleRelation = new AssetBundleRelation(bundleName, loadProgress);
                assetBundleRelation.AddReferenceBundle(referenceBundleName);
                mNameBundleDict.Add(bundleName, assetBundleRelation);
                yield return Load(bundleName);
            }
        }

        public T LoadAsset<T>(string bundleName, string assetName) where T : Object
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
                Debug.LogError("当前" + bundleName + "为空,无法获取:" + assetName);
                return null;
            }

            T asset = mNameBundleDict[bundleName].LoadAsset<T>(assetName) as T;
            TempObject tempObject = new TempObject(asset);

            if (mNameChacheDict.ContainsKey(bundleName))
            {
                mNameChacheDict[bundleName].AddAsset(assetName, tempObject);
            }
            else
            {
                AssetCaching assetCaching = new AssetCaching();
                assetCaching.AddAsset(assetName, tempObject);
                mNameChacheDict.Add(bundleName, assetCaching);
            }

            return asset;
        }

        public Object[] LoadAllAssets(string bundleName)
        {
            if (!mNameBundleDict.ContainsKey(bundleName))
            {
                Debug.LogError("当前" + bundleName + "没有加载,无法获取");
                return null;
            }

            return mNameBundleDict[bundleName].LoadAllAssets();
        }

        public Object[] LoadAssetWithSubAssets(string bundleName, string assetName)
        {
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
                Debug.LogError("当前" + bundleName + "为空,无法获取:" + assetName);
                return null;
            }

            Object[] asset = mNameBundleDict[bundleName].LoadAssetWithSubAssets(assetName);
            TempObject tempObject = new TempObject(asset);

            if (mNameChacheDict.ContainsKey(bundleName))
            {
                mNameChacheDict[bundleName].AddAsset(assetName, tempObject);
            }
            else
            {
                AssetCaching assetCaching = new AssetCaching();
                assetCaching.AddAsset(assetName, tempObject);
                mNameChacheDict.Add(bundleName, assetCaching);
            }

            return asset;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="asset"></param>
        public void UnLoadAsset(string bundleName, string assetName)
        {
            if (!mNameChacheDict.ContainsKey(bundleName))
            {
                Debug.LogError("没有加载" + bundleName + "没有缓存资源，无法卸载");
                return;
            }

            mNameChacheDict[bundleName].UnLoadAsset(assetName);

            Resources.UnloadUnusedAssets();
        }

        public void UnLoadAllAsset(string bundleName)
        {
            if (!mNameChacheDict.ContainsKey(bundleName))
            {
                Debug.LogError("没有加载" + bundleName + "没有缓存资源，无法卸载");
                return;
            }

            mNameChacheDict[bundleName].UnLoadAllAsset();
            mNameChacheDict.Remove(bundleName);
            Resources.UnloadUnusedAssets();
        }

        public void UnLoadAll()
        {
            foreach (var item in mNameChacheDict.Keys)
            {
                UnLoadAllAsset(item);
            }

            mNameChacheDict.Clear();
        }

        public void Dispose(string bundleName)
        {
            if (!mNameBundleDict.ContainsKey(bundleName))
            {
                Debug.LogError("没有加载" + bundleName + "无法加载");
                return;
            }

            AssetBundleRelation assetBundleRelation = mNameBundleDict[bundleName];

            string[] allDependences = assetBundleRelation.GetAllDependence();

            foreach (var dependenceBundleName in allDependences)
            {
                AssetBundleRelation tempAssetBundleRelation = mNameBundleDict[bundleName];
                bool resualt = tempAssetBundleRelation.RemoveReferenceBundle(bundleName);

                if (resualt)
                {
                    //递归
                    Dispose(tempAssetBundleRelation.BundleName);
                }
            }

            //TODO:有错误
            mNameBundleDict[bundleName].Dispose();
            mNameBundleDict[bundleName] = null;
        }

        public void DisposeAll()
        {
            foreach (var item in mNameBundleDict.Keys)
            {
                Dispose(item);
            }

            mNameBundleDict.Clear();
        }

        public void DisposeAllAndUnLoadAll()
        {
            UnLoadAll();
            DisposeAll();
        }

        public void GetAllAssetNames(string bundleName)
        {
            mNameBundleDict[bundleName].GetAllAssetNames();
        }
    }
}
