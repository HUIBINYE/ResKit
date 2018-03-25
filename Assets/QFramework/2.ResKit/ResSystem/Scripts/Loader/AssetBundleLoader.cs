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
        private readonly string mBundleName;

        /// <summary>
        /// 路径
        /// </summary>
        private string mBundlePath;

        private float mProgress;

        private LoadProgress mLoadProgress;

        private LoadComplete mLoadComplete;

        public AssetBundleLoader(LoadComplete loadComplete, LoadProgress loadProgress, string bundleName)
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
                    mLoadProgress(mBundleName, mProgress);
                }

                yield return mWWW.progress;
            }

            mProgress = mWWW.progress;

            if (mProgress >= 1f)
            {
                //加载完成
                mAssetLoader = new AssetLoader {AssetBundle = mWWW.assetBundle};

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

        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (mAssetLoader == null)
            {
                Debug.LogError("当前资源包为空,无法获取:" + assetName);
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
                Debug.LogError("当前资源包为空,无法获取:" + assetName);
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
}