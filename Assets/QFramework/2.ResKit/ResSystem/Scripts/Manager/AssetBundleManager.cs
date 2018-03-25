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

    public class AssetBundleManager : MonoBehaviour
    {
        private static AssetBundleManager instance;

        public static AssetBundleManager Instance
        {
            get { return instance; }
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

            var sceneManager = nameScenesDict[sceneName];
            sceneManager.LoadAssetBundle(folderName, loadProgress, LoadAssetBundleCallBack);
        }

        private void CreateScene(string sceneName)
        {
            var sceneManager = new SceneManager(sceneName);
            sceneManager.ReadRecord(sceneName);
            nameScenesDict.Add(sceneName, sceneManager);
        }

        private void LoadAssetBundleCallBack(string sceneName, string bundleName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                StartCoroutine(sceneManager.Load(bundleName));
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
            }
        }

        public string GetBundleName(string sceneName, string folderName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                return sceneManager.GetBundleName(folderName);
            }

            Debug.LogError("不存在场景" + sceneName);
            return null;
        }

        public bool IsLoading(string sceneName, string folderName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                SceneManager sceneManager = nameScenesDict[sceneName];

                return sceneManager.IsLoading(folderName);
            }

            Debug.LogError("不存在场景" + sceneName);
            return false;
        }

        public bool IsFinish(string sceneName, string folderName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                return sceneManager.IsFinish(folderName);
            }

            Debug.LogError("不存在场景" + sceneName);
            return false;
        }

        public Object LoadAsset(string sceneName, string folderName, string assetName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                return sceneManager.LoadAsset(folderName, assetName);
            }

            Debug.LogError("不存在场景" + sceneName);
            return null;
        }

        public Object[] LoadAllAssets(string sceneName, string folderName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                SceneManager sceneManager = nameScenesDict[sceneName];

                return sceneManager.LoadAllAssets(folderName);
            }

            Debug.LogError("不存在场景" + sceneName);
            return null;
        }

        public Object[] LoadAssetWithSubAssets(string sceneName, string folderName, string assetName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                SceneManager sceneManager = nameScenesDict[sceneName];

                return sceneManager.LoadAssetWithSubAssets(folderName, assetName);
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
                return null;
            }
        }

        public void UnLoadAsset(string sceneName, string folderName, string assetName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                SceneManager sceneManager = nameScenesDict[sceneName];

                sceneManager.UnLoadAsset(folderName, assetName);
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
            }
        }

        public void UnLoadAllAsset(string sceneName, string folderName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                sceneManager.UnLoadAllAsset(folderName);
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
            }
        }

        public void UnLoadAll(string sceneName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                sceneManager.UnLoadAll();
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
            }
        }

        public void Dispose(string sceneName, string folderName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                sceneManager.Dispose(folderName);
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
            }
        }

        public void DisposeAll(string sceneName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                sceneManager.DisposeAll();
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
            }
        }

        public void DisposeAllAndUnLoadAll(string sceneName)
        {
            if (nameScenesDict.ContainsKey(sceneName))
            {
                var sceneManager = nameScenesDict[sceneName];

                sceneManager.DisposeAllAndUnLoadAll();
            }
            else
            {
                Debug.LogError("不存在场景" + sceneName);
            }
        }
    }
}