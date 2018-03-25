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
using System.IO;
using UnityEngine;

namespace QFramework.ResKit
{
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

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    int count = int.Parse(sr.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        string line = sr.ReadLine();
                        string[] kv = line.Split(' ');
                        folderBundleDict.Add(kv[0], kv[1]);
                    }
                }
            }
        }

        public string GetBundleName(string folderName)
        {
            if (!folderBundleDict.ContainsKey(folderName))
            {
                Debug.LogError("没有当前文件夹" + folderName);
            }

            return folderBundleDict[folderName];
        }

        public void LoadAssetBundle(string folderName, LoadProgress loadProgress,
            LoadAssetBundleCallback loadAssetBundleCallback)
        {
            if (folderBundleDict.ContainsKey(folderName))
            {
                string bundleName = folderBundleDict[folderName];
                sceneAssetBundles.LoadAssetBundle(bundleName, loadProgress, loadAssetBundleCallback);
            }
            else
            {
                Debug.LogError("没有当前文件夹" + folderName + "对应的包");
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
                Debug.LogError("没有当前文件夹" + folderName + "对应的包");
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
                Debug.LogError("没有当前文件夹" + folderName + "对应的包");
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
                return sceneAssetBundles.LoadAsset<Object>(bundleName, assetName);
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
                Debug.LogError("没有当前文件夹" + folderName + "对应的包");
                return null;
            }
        }

        public Object[] LoadAssetWithSubAssets(string folderName, string assetName)
        {
            if (sceneAssetBundles == null)
            {
                Debug.LogError("sceneAssetBundles为空");
                return null;
            }

            if (folderBundleDict.ContainsKey(folderName))
            {
                string bundleName = folderBundleDict[folderName];
                return sceneAssetBundles.LoadAssetWithSubAssets(bundleName, assetName);
            }
            else
            {
                Debug.LogError("没有当前文件夹" + folderName + "对应的包");
                return null;
            }
        }

        public void UnLoadAsset(string folderName, string assetName)
        {
            if (folderBundleDict.ContainsKey(folderName))
            {
                string bundleName = folderBundleDict[folderName];
                sceneAssetBundles.UnLoadAsset(bundleName, assetName);
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
                Debug.LogError("找不到文件夹:" + folderName + "对应的包");
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
                Debug.LogError("找不到文件夹:" + folderName + "对应的包");
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
}