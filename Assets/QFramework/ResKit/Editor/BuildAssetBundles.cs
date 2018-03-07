/****************************************************************************
 * Copyright (c) 2018 yuanhuibin@putao.com
 ****************************************************************************/

using System.IO;
using UnityEditor;

/// <summary>
/// 打包AssetBundles
/// from:https://docs.unity3d.com/Manual/AssetBundles-Workflow.html(内容很新，很受用)
/// </summary>
public class BuildAssetBundles
{
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/AssetBundles";
		if (!Directory.Exists(assetBundleDirectory))
		{
			Directory.CreateDirectory(assetBundleDirectory);
		}

		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
	}
}
