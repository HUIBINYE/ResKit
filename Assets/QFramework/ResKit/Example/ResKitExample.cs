using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResKitExample : MonoBehaviour 
{
	// Use this for initialization
	void Start ()
	{
		AssetBundleManager.Instance.LoadAssetBundle("Scene1", "Buildings", OnLoadProgress);
	}
	void OnLoadProgress(string bundleName, float progress)
	{
		Debug.LogError(progress + ":" + bundleName);

		if (progress >= 1)
		{
			Instantiate(AssetBundleManager.Instance.LoadAsset("Scene1", "Buildings", "Sphere"));
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
