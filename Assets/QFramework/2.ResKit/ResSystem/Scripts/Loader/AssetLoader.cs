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
    using UnityEngine;


    /// <summary>
    /// 加载包里的资源
    /// </summary>
    public class AssetLoader : System.IDisposable
    {
        private AssetBundle mAssetBundle;

        public AssetBundle AssetBundle
        {
            set { mAssetBundle = value; }
        }

        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (mAssetBundle == null)
            {
                Debug.LogError("当前资源包为空,无法获取:" + assetName);
                return null;
            }

            if (!mAssetBundle.Contains(assetName))
            {
                Debug.LogError("当前资源包不包括:" + assetName);
                return null;
            }

            return mAssetBundle.LoadAsset(assetName) as T;
        }

        public Object[] LoadAllAssets()
        {
            if (mAssetBundle == null)
            {
                Debug.LogError("当前资源包为空,无法获取");
                return null;
            }

            return mAssetBundle.LoadAllAssets();
        }

        public Object[] LoadAssetWithSubAssets(string assetName)
        {
            if (mAssetBundle == null)
            {
                Debug.LogError("当前资源包为空,无法获取:" + assetName);
                return null;
            }

            if (!mAssetBundle.Contains(assetName))
            {
                Debug.LogError("当前资源包不包括:" + assetName);
                return null;
            }

            return mAssetBundle.LoadAssetWithSubAssets(assetName);
        }

        public void UnLoadAsset(Object asset)
        {
            Resources.UnloadAsset(asset);
        }

        public void Dispose()
        {
            if (mAssetBundle == null)
            {
                return;
            }

            mAssetBundle.Unload(false);
        }

        public void GetAllAssetNames()
        {
            string[] names = mAssetBundle.GetAllAssetNames();

            foreach (var itemName in names)
            {
                Debug.Log(itemName);
            }
        }
    }
}