/****************************************************************************
 * Copyright (c) 2018 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections;
using System.Net;
using UnityEngine;

public class NoCachingLoadExample : MonoBehaviour
{
	public string BundleURL;
	
	public string AssetName;

	private IEnumerator Start()
	{
		using (WWW www = new WWW(BundleURL))
		{
			yield return www;
			if (www.error != null)
			{
				Debug.LogError("WWW download had an error:" + www.error);
			}

			AssetBundle bundle = www.assetBundle;

			if (string.IsNullOrEmpty(AssetName))
			{
				Instantiate(bundle.mainAsset);
			}
			else
			{
				Instantiate(bundle.LoadAsset(AssetName));
			}
			bundle.Unload(false);
		}
	}
}
