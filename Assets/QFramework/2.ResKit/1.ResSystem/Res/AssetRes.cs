﻿/****************************************************************************
 * Copyright (c) 2017 snowcold
 * Copyright (c) 2017 liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 * https://github.com/liangxiegame/QSingleton
 * https://github.com/liangxiegame/QChain
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

namespace QFramework
{
	using System;
	using UnityEngine;
	using System.Collections;
    	
    public class AssetRes : BaseRes
    {
        protected string[]            mAssetBundleArray;
        protected AssetBundleRequest  mAssetBundleRequest;

        public static AssetRes Allocate(string name,string onwerBundleName = null)
        {
            AssetRes res = SafeObjectPool<AssetRes>.Instance.Allocate();
            if (res != null)
            {
                res.AssetName = name;
	            res.mOwnerBundleName = onwerBundleName;
                res.InitAssetBundleName();
            }
            return res;
        }

        protected string AssetBundleName
        {
            get
            {
	            return mAssetBundleArray == null ? null : mAssetBundleArray[0];
            }
        }
	    
        public AssetRes(string assetName) : base(assetName)
        {
            
        }

        public AssetRes()
        {

        }

        public override void AcceptLoaderStrategySync(IResLoader loader, IResLoaderStrategy strategy)
        {
            strategy.OnSyncLoadFinish(loader, this);
        }

        public override void AcceptLoaderStrategyAsync(IResLoader loader, IResLoaderStrategy strategy)
        {
            strategy.OnAsyncLoadFinish(loader, this);
        }

        public override bool LoadSync()
        {
            if (!CheckLoadAble())
            {
                return false;
            }

            if (string.IsNullOrEmpty(AssetBundleName))
            {
                return false;
            }

            AssetBundleRes abR = ResMgr.Instance.GetRes<AssetBundleRes>(AssetBundleName);

	        if (abR == null || abR.assetBundle == null)
            {
                Log.E("Failed to Load Asset, Not Find AssetBundleImage:" + AssetBundleName);
                return false;
            }

            State = ResState.Loading;

            //TimeDebugger timer = ResMgr.S.timeDebugger;

            //timer.Begin("LoadSync Asset:" + mName);


            HoldDependRes();


			UnityEngine.Object obj = null;

			#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor && !string.Equals(mAssetName,"assetbundlemanifest")) {
				string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName (abR.AssetName, mAssetName);
				if (assetPaths.Length == 0) {
					Log.E("Failed Load Asset:" + mAssetName);
					OnResLoadFaild();
					return false;
				}
				obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object> (assetPaths [0]);
//				obj = UnityEngine.Object.Instantiate(obj);
//				UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();
			}
			else
			#endif
			{	
				obj = abR.assetBundle.LoadAsset (mAssetName);
			}
            //timer.End();

            UnHoldDependRes();

            if (obj == null)
            {
                Log.E("Failed Load Asset:" + mAssetName);
                OnResLoadFaild();
                return false;
            }

            mAsset = obj;

            State = ResState.Ready;
            //Log.I(string.Format("Load AssetBundle Success.ID:{0}, Name:{1}", mAsset.GetInstanceID(), mName));

            //timer.Dump(-1);
            return true;
        }

        public override void LoadAsync()
        {
            if (!CheckLoadAble())
            {
                return;
            }

            if (string.IsNullOrEmpty(AssetBundleName))
            {
                return;
            }

            State = ResState.Loading;

            ResMgr.Instance.PostIEnumeratorTask(this);
        }

        public override IEnumerator StartIEnumeratorTask(System.Action finishCallback)
        {
            if (RefCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            AssetBundleRes abR = ResMgr.Instance.GetRes<AssetBundleRes>(AssetBundleName);

            if (abR == null || abR.assetBundle == null)
            {
                Log.E("Failed to Load Asset, Not Find AssetBundleImage:" + AssetBundleName);
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            //确保加载过程中依赖资源不被释放:目前只有AssetRes需要处理该情况
            HoldDependRes();


			#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor && !string.Equals(mAssetName,"assetbundlemanifest")) {
				string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName (abR.AssetName, mAssetName);
				if (assetPaths.Length == 0) {
					Log.E("Failed Load Asset:" + mAssetName);
					OnResLoadFaild();
					finishCallback();
					yield break;
				}
				mAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object> (assetPaths [0]);
			}
			else
			#endif
			{	
				AssetBundleRequest abQ = abR.assetBundle.LoadAssetAsync(mAssetName);
				mAssetBundleRequest = abQ;

				yield return abQ;

				mAssetBundleRequest = null;

				UnHoldDependRes();

				if (!abQ.isDone)
				{
					Log.E("Failed Load Asset:" + mAssetName);
					OnResLoadFaild();
					finishCallback();
					yield break;
				}

				mAsset = abQ.asset;
			}

            State = ResState.Ready;

            finishCallback();
        }

        public override string[] GetDependResList()
        {
            return mAssetBundleArray;
        }

        public override void OnRecycled()
        {
            mAssetBundleArray = null;
        }
        
        public override void Recycle2Cache()
        {
            SafeObjectPool<AssetRes>.Instance.Recycle(this);
        }

        protected override float CalculateProgress()
        {
            if (mAssetBundleRequest == null)
            {
                return 0;
            }

            return mAssetBundleRequest.progress;
        }

		protected void InitAssetBundleName()
		{
			mAssetBundleArray = null;

			AssetData config = mOwnerBundleName != null
				? AssetDataTable.Instance.GetAssetData(mAssetName, mOwnerBundleName)
				: AssetDataTable.Instance.GetAssetData(mAssetName);

			if (config == null)
			{
				Log.E("Not Find AssetData For Asset:" + mAssetName);
				return;
			}

			string assetBundleName = AssetDataTable.Instance.GetAssetBundleName(config.AssetName, config.AssetBundleIndex,mOwnerBundleName);

			if (string.IsNullOrEmpty(assetBundleName))
			{
				Log.E("Not Find AssetBundle In Config:" + config.AssetBundleIndex);
				return;
			}
			mAssetBundleArray = new string[1];
			mAssetBundleArray[0] = assetBundleName;
		}
    }
}
