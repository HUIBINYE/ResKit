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
            mMnifestPath = PathUtil.GetWWWPath() + "/" + PathUtil.GetPlatformName();
            Debug.Log("mMnifestPath" + mMnifestPath);
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
}