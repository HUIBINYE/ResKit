/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using UnityEditor;
using UnityEngine;

public class AssetBundleEditor 
{
	[MenuItem("AssetBundle/Set Asset Bundle Lables")]
	public static void SetAssetBundleLables()
	{
		AssetDatabase.RemoveUnusedAssetBundleNames();

		string assetDirectory = Application.dataPath + "/Res";
		Debug.Log(assetDirectory);
		
		DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);
		DirectoryInfo[] sceneDirectories = directoryInfo.GetDirectories();

		foreach (var itemInfo in sceneDirectories)
		{
			string sceneDirectory = assetDirectory + "/" + itemInfo.Name;
			DirectoryInfo sceneDirectoryInfo = new DirectoryInfo(sceneDirectory);
			if (sceneDirectoryInfo == null)
			{
				Debug.LogError("sceneDirectory" + "not exit!");
				return;
			}
			else
			{
				Dictionary<string,string> namePathDic = new Dictionary<string, string>();
				int index = sceneDirectory.LastIndexOf("/");
				string sceneName = sceneDirectory.Substring(index + 1);
//				Debug.LogError("DirInfo:"+sceneDirectoryInfo);
//				Debug.LogError("SceneName:"+sceneName);
				OnSceneFileSystemInfo(sceneDirectoryInfo,sceneName);
				OnWriteConfig(sceneName,namePathDic);
			}
		}
		Debug.Log("设置成功");
	}

	public static void OnWriteConfig(string sceneName,Dictionary<string,string> namePathDict)
	{
		string path = Application.dataPath + "/AssetBundle/" + sceneName + "Record.config";
		Debug.Log(path);
		using (FileStream fs = new FileStream(path,FileMode.OpenOrCreate,FileAccess.Write))
		{
			using (StreamWriter sw = new StreamWriter(fs))
			{
				sw.WriteLine(namePathDict.Count);
				foreach (var kv in namePathDict)
				{
					sw.Write(kv.Key + "-"+kv.Value);
				}
			}
		}
	}

	public static void OnSceneFileSystemInfo(FileSystemInfo info, string sceneName)
	{
		if (!info.Exists)
		{
			Debug.LogError(info.FullName + "not exit!");
			return;
		}
		DirectoryInfo directoryInfo = info as DirectoryInfo;
		FileSystemInfo[] fileSystemInfo = directoryInfo.GetFileSystemInfos();
		foreach (var itemInfo in fileSystemInfo)
		{
			FileInfo fileInfo = itemInfo as FileInfo;
			if (fileInfo == null)
			{
				OnSceneFileSystemInfo(itemInfo,sceneName);
			}
			else
			{
				SetLables(fileInfo,sceneName);
			}
		}
	}

	public static void SetLables(FileInfo fileInfo,string sceneName)
	{
		if (fileInfo.Extension == ".meta")
		{
			return;
		}

		string bundleName = GetBundleName(fileInfo,sceneName);
		int index = fileInfo.FullName.IndexOf("Assets");
		string assetPath = fileInfo.FullName.Substring(index);
		AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
		assetImporter.assetBundleName = bundleName.ToLower();
		if (fileInfo.Extension == ".Unity")
		{
			assetImporter.assetBundleVariant = "unity3d";
		}
		else
		{
			assetImporter.assetBundleVariant = "assetbundle";
		}

//		Debug.LogError("bundleName"+bundleName);
	}

	public static string GetBundleName(FileInfo fileInfo, string sceneName)
	{
		string path = fileInfo.FullName;
//		Debug.LogError("path:"+path);
		int index = path.IndexOf(sceneName) + sceneName.Length;
		string bundlePath = path.Substring(index + 1);
		if (bundlePath.Contains("/"))
		{
			string[] temp = bundlePath.Split('/');
			return sceneName + "/" + temp[0];
		}
		else
		{
			return sceneName;
		}
	}
}
