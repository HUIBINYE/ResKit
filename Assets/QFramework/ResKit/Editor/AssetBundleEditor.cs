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
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	public static class AssetBundleEditor
	{
		[MenuItem("AssetBundle/Set Asset Bundle Lables")]
		public static void SetAssetBundleLables()
		{
			AssetDatabase.RemoveUnusedAssetBundleNames();

			var assetDirectory = Application.dataPath + "/Res";
			Debug.Log(assetDirectory);

			var directoryInfo = new DirectoryInfo(assetDirectory);
			var sceneDirectories = directoryInfo.GetDirectories();

			foreach (var itemInfo in sceneDirectories)
			{
				var sceneDirectory = assetDirectory + "/" + itemInfo.Name;
				var sceneDirectoryInfo = new DirectoryInfo(sceneDirectory);
				{
					var namePathDic = new Dictionary<string, string>();
					var index = sceneDirectory.LastIndexOf("/");
					var sceneName = sceneDirectory.Substring(index + 1);
					OnSceneFileSystemInfo(sceneDirectoryInfo, sceneName, namePathDic);
					OnWriteConfig(sceneName, namePathDic);
				}
			}

			Debug.Log("设置成功");
		}

		private static void OnWriteConfig(string sceneName, Dictionary<string, string> namePathDict)
		{
			string path = PathUtil.GetAssetBundleOutPath() + sceneName + "Record.config";

			if (!Directory.Exists(PathUtil.GetAssetBundleOutPath()))
			{
				Directory.CreateDirectory(PathUtil.GetAssetBundleOutPath() + sceneName);
			}

			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					sw.WriteLine(namePathDict.Count);
					foreach (var kv in namePathDict)
					{
						sw.WriteLine(kv.Key + " " + kv.Value);
					}
				}
			}
		}

		private static void OnSceneFileSystemInfo(FileSystemInfo info, string sceneName,
			Dictionary<string, string> namePathDic)
		{
			if (!info.Exists)
			{
				Debug.LogError(info.FullName + "not exit!");
				return;
			}

			var directoryInfo = info as DirectoryInfo;
			var fileSystemInfo = directoryInfo.GetFileSystemInfos();
			foreach (var itemInfo in fileSystemInfo)
			{
				var fileInfo = itemInfo as FileInfo;
				if (fileInfo == null)
				{
					OnSceneFileSystemInfo(itemInfo, sceneName, namePathDic);
				}
				else
				{
					SetLabels(fileInfo, sceneName, namePathDic);
				}
			}
		}

		private static void SetLabels(FileInfo fileInfo, string sceneName, Dictionary<string, string> namePathDict)
		{
			if (fileInfo.Extension == ".meta")
			{
				return;
			}

			var bundleName = GetBundleName(fileInfo, sceneName);
			var index = fileInfo.FullName.IndexOf("Assets");
			var assetPath = fileInfo.FullName.Substring(index);
			var assetImporter = AssetImporter.GetAtPath(assetPath);
			assetImporter.assetBundleName = bundleName.ToLower();
			assetImporter.assetBundleVariant = fileInfo.Extension == ".Unity" ? "unity3d" : "assetbundle";

			var folderName = "";

			folderName = bundleName.Contains("/") ? bundleName.Split('/')[1] : bundleName;

			var bundlePath = assetImporter.assetBundleName + "." + assetImporter.assetBundleVariant;
			if (!namePathDict.ContainsKey(folderName))
			{
				namePathDict.Add(folderName, bundlePath);
			}
		}

		public static string GetBundleName(FileInfo fileInfo, string sceneName)
		{
			var path = fileInfo.FullName;
			var index = path.IndexOf(sceneName) + sceneName.Length;
			var bundlePath = path.Substring(index + 1);
			if (bundlePath.Contains("/"))
			{
				string[] temp = bundlePath.Split('/');
				return sceneName + "/" + temp[0];
			}

			return sceneName;
		}

		[MenuItem("AssetBundle/Build AssetBundles")]
		static void BuildAllAssetBundles()
		{
			var assetBundleDirectory = PathUtil.GetAssetBundleOutPath();
			if (!Directory.Exists(assetBundleDirectory))
			{
				Directory.CreateDirectory(assetBundleDirectory);
			}

			BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
		}

		[MenuItem("AssetBundle/Delete All AssetBundles")]
		static void DeleteAssetBundle()
		{
			var outPath = PathUtil.GetAssetBundleOutPath();
			Directory.Delete(outPath, true);
			AssetDatabase.Refresh();
		}
	}
}