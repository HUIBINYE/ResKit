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

namespace QFramework.ResKit
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 管理资源包的依赖关系
    /// </summary>
    public class AssetBundleRelation
    {
        private AssetBundleLoader mAssetBundleLoader;

        private readonly string mBundleName;

        public string BundleName
        {
            get { return mBundleName; }
        }

        private LoadComplete mLoadComplete;

        public LoadProgress LoadProgress { get; private set; }

        private bool mFinish;

        public bool Finish
        {
            get { return mFinish; }
        }

        public AssetBundleRelation(string bundleName, LoadProgress loadProgress)
        {
            mBundleName = bundleName;
            LoadProgress = loadProgress;
            mFinish = false;
            mDependenceBundle = new List<string>();
            mReferenceBundle = new List<string>();
            mAssetBundleLoader = new AssetBundleLoader(OnLoadComplete, LoadProgress, mBundleName);
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
        private readonly List<string> mDependenceBundle;

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

                return mReferenceBundle.Count <= 0;
            }

            return false;
        }

        //++++++++++++++++++++++++++
        public string[] GetAllDependence()
        {
            return mDependenceBundle.ToArray();
        }


        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (mAssetBundleLoader == null)
            {
                Debug.LogError("当前mAssetBundleLoader为空,无法获取:" + assetName);
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
                Debug.LogError("当前mAssetBundleLoader为空,无法获取:" + assetName);
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
}