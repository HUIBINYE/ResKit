/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections;
using UnityEngine;

public class CachingLoadExample : MonoBehaviour
{
	public string BundleURL;

	public string AssetName;

	public int version;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(DownloadAndCache());
	}

	IEnumerator DownloadAndCache()
	{
		while (!Caching.ready)
		{
			yield return null;
		}

		using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL,version))
		{
			yield return www;
			if (www.error != null)
			{
				Debug.LogError("www download had an error"+www.error);
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
