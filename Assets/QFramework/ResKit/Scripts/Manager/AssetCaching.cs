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
                mNameAssetDict.Add(assetName, asset);
            }
            else
            {
                Debug.LogError("已经加载过了:" + assetName);
            }
        }

        public Object[] GetAsset(string assetName)
        {
            if (mNameAssetDict.ContainsKey(assetName))
            {
                return mNameAssetDict[assetName].Asset.ToArray();
            }

            Debug.LogError("尚未加载:" + assetName);
            return null;
        }

        public void UnLoadAsset(string assetName)
        {
            if (mNameAssetDict.ContainsKey(assetName))
            {
                mNameAssetDict[assetName].UnLoadAsset();
            }
            else
            {
                Debug.LogError("尚未加载:" + assetName);
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
}