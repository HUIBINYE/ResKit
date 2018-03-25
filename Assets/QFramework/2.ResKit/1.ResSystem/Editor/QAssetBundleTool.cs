﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace QFramework
{
	[InitializeOnLoad]
	public class QResSystemMark
	{
		public const  string AssetBundlesOutputPath       = "AssetBundles";
		private static int    mSimulateAssetBundleInEditor = -1;
		private const string kSimulateAssetBundles        = "SimulateAssetBundles";

		// Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
		public static bool SimulateAssetBundleInEditor
		{
			get
			{
				if (mSimulateAssetBundleInEditor == -1)
					mSimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

				return mSimulateAssetBundleInEditor != 0;
			}
			set
			{
				var newValue = value ? 1 : 0;
				if (newValue != mSimulateAssetBundleInEditor)
				{
					mSimulateAssetBundleInEditor = newValue;
					EditorPrefs.SetBool(kSimulateAssetBundles, value);
				}
			}
		}

		private const string Mark_AssetBundle   = "Assets/ResSystem Mark/AssetBundle";
		private const string Mark_HotUpdateFile = "Assets/ResSystem Mark/File";
		private const string Mark_HotUpdateZip  = "Assets/ResSystem Mark/Zip";

		private const string LABEL_AB   = "QAssetBundle";
		private const string LABEL_ZIP  = "QZip";
		private const string LABEL_FILE = "QFile";

		static QResSystemMark()
		{
			Selection.selectionChanged = OnSelectionChanged;
			EditorApplication.update += Update;
		}

		private static int    markCounter = 0;
		private static string lastMarkPth = string.Empty;

		private static void ResetQMark(string path)
		{

			Object tempObj = PrefabUtility.CreateEmptyPrefab("Assets/qreskit_temp.prefab");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Selection.activeObject = tempObj;
			lastMarkPth = path;
			markCounter = 0;

		}

		static void Update()
		{

			if (markCounter < 2)
			{
				markCounter++;
			}
			else if (markCounter == 2)
			{
				markCounter++;
				if (!string.IsNullOrEmpty(lastMarkPth))
				{
					Object obj = AssetDatabase.LoadAssetAtPath(lastMarkPth, typeof(Object));
					Selection.activeObject = obj;
					AssetDatabase.DeleteAsset("Assets/qreskit_temp.prefab");
					AssetDatabase.Refresh();
				}
			}
		}

		private static void OnSelectionChanged()
		{
			string path = MouseSelector.GetSelectedPathOrFallback();
			if (!string.IsNullOrEmpty(path))
			{
//				Object obj = AssetDatabase.LoadAssetAtPath (path,typeof(Object));
//				bool contain = HasQABLabel(obj,LABEL_AB);
//				Menu.SetChecked (Mark_AssetBundle, contain);
//				contain = HasQABLabel (obj,LABEL_ZIP);
//				Menu.SetChecked (Mark_HotUpdateZip,contain);
//				contain = HasQABLabel (obj,LABEL_FILE);
//				Menu.SetChecked (Mark_HotUpdateFile,contain);
			}
		}

		public static bool HasQABLabel(Object obj, string label)
		{
			string[] labels = AssetDatabase.GetLabels(obj);
			List<string> lbs = new List<string>(labels);
			foreach (string lb in lbs)
			{
				if (lb == label)
				{
					return true;
				}
			}

			return false;
		}

		private static void RemoveUselessLabels(Object obj)
		{
			var labels = AssetDatabase.GetLabels(obj);
			var lbs = new List<string>(labels);
			for (var i = lbs.Count - 1; i >= 0; i--)
			{
				var lb = lbs[i];
				if (lb != LABEL_AB && lb != LABEL_ZIP && lb != LABEL_FILE)
				{
					lbs.Remove(lb);
				}
			}

			AssetDatabase.SetLabels(obj, lbs.ToArray());
		}

		public static bool SetLabels(Object obj, string label)
		{
			RemoveUselessLabels(obj);
			string[] labels = AssetDatabase.GetLabels(obj);
			List<string> lbs = new List<string>(labels);
			foreach (string lb in lbs)
			{
				if (lb == label)
				{
					lbs.Remove(lb);
					AssetDatabase.SetLabels(obj, lbs.ToArray());
					return false;
				}
			}

			lbs.Add(label);
			AssetDatabase.SetLabels(obj, lbs.ToArray());

			EditorUtility.SetDirty(obj);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			return true;

		}

#if QRESSYSTEM_SUPPORT
		[MenuItem(Mark_HotUpdateZip)]
		#endif
		public static void MarkHotUpdateZip()
		{
			string path = MouseSelector.GetSelectedPathOrFallback();
			if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
			{
				return;
			}

			Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
			bool contain = SetLabels(obj, LABEL_ZIP);
			if (!contain)
			{
				Menu.SetChecked(Mark_HotUpdateZip, false);
			}
			else
			{
				Menu.SetChecked(Mark_HotUpdateZip, true);
			}

			ResetQMark(path);
		}

		[MenuItem(Mark_HotUpdateFile)]
		public static void MarkHotUpdateFile()
		{
			string path = MouseSelector.GetSelectedPathOrFallback();
			if (string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				return;
			}

			Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
			bool contain = SetLabels(obj, LABEL_FILE);
			if (!contain)
			{
				Menu.SetChecked(Mark_HotUpdateFile, false);
			}
			else
			{
				Menu.SetChecked(Mark_HotUpdateFile, true);
			}

			ResetQMark(path);

		}

		public static void MarkQAB(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				AssetImporter ai = AssetImporter.GetAtPath(path);

				Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
				bool contain = SetLabels(obj, LABEL_AB);
				if (!contain)
				{
					Menu.SetChecked(Mark_AssetBundle, false);
					ai.assetBundleName = null;
				}
				else
				{
					DirectoryInfo dir = new DirectoryInfo(path);
					Menu.SetChecked(Mark_AssetBundle, true);
					ai.assetBundleName = dir.Name.Replace(".", "_");
				}

				AssetDatabase.RemoveUnusedAssetBundleNames();

				ResetQMark(path);
			}
		}

		/// <summary>
		/// 总是标记
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="label"></param>
		/// <returns></returns>
		public static bool SetLabelsAlways(Object obj, string label)
		{
			RemoveUselessLabels(obj);
			string[] labels = AssetDatabase.GetLabels(obj);
			List<string> lbs = new List<string>(labels);
			foreach (string lb in lbs)
			{
				if (lb == label)
				{
					return false;
				}
			}

			lbs.Add(label);
			AssetDatabase.SetLabels(obj, lbs.ToArray());

			EditorUtility.SetDirty(obj);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			return true;
		}


		/// <summary>
		/// 总是标记
		/// </summary>
		/// <param name="path"></param>
		public static void MarkQABAlways(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				AssetImporter ai = AssetImporter.GetAtPath(path);

				Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
				bool contain = SetLabelsAlways(obj, LABEL_AB);
				if (!contain)
				{
					//Menu.SetChecked(Mark_AssetBundle, false);
					//ai.assetBundleName = null;
				}
				else
				{
					DirectoryInfo dir = new DirectoryInfo(path);
					Menu.SetChecked(Mark_AssetBundle, true);
					ai.assetBundleName = dir.Name.Replace(".", "_");
				}

				AssetDatabase.RemoveUnusedAssetBundleNames();

				ResetQMark(path);
			}
		}

		[MenuItem(Mark_AssetBundle)]
		public static void MarkQABDir()
		{
			string path = MouseSelector.GetSelectedPathOrFallback();
			MarkQAB(path);
		}

		[MenuItem("Assets/ResSystem Mark/AssetBundleAllSubFolder")]
		public static void MarkQABForAllSubFolder()
		{
			var path = MouseSelector.GetSelectedPathOrFallback();
			var childpaths = Directory.GetDirectories(path);
			foreach (var childpath in childpaths)
			{
				Debug.Log(">>>>>> Mark QAB: " + childpath);
				MarkQABAlways(childpath);
			}
		}

//		[MenuItem("Assets/ResSystem Mark/Un_AssetBundleAllSubFolder")]
//		public static void UnMarkQABForAllSubFolder()
//		{
//			var path = MouseSelector.GetSelectedPathOrFallback();
//			var childpaths = Directory.GetDirectories(path);
//			foreach (var childpath in childpaths)
//			{
//				Debug.Log(">>>>>> UnMark QAB: " + childpath);
//				var ai = AssetImporter.GetAtPath(childpath);
//
//				var obj = AssetDatabase.LoadAssetAtPath(childpath, typeof(Object));
//				RemoveUselessLabels(obj);
//
//				var labels = AssetDatabase.GetLabels(obj).ToList();
//				foreach (var lb in labels)
//				{
//					if (lb == LABEL_AB)
//					{
//						labels.Remove(lb);
//						AssetDatabase.SetLabels(obj, labels.ToArray());
//						break;
//					}
//				}
//
//				Menu.SetChecked(Mark_AssetBundle, false);
//				ai.assetBundleName = null;
//				AssetDatabase.RemoveUnusedAssetBundleNames();
//				ResetQMark(childpath);
//			}
//		}

//		[MenuItem("Assets/ResSystem Mark/AssetBundleAllSubAssets")]
//		public static void MarkQABForAllSubAssets()
//		{
//			var path = MouseSelector.GetSelectedPathOrFallback();
//			var childpaths = Directory.GetFiles(path);
//			foreach (var childpath in childpaths)
//			{
//				Debug.Log(">>>>>> Mark QAB: " + childpath);
//				if (!childpath.EndsWith(".meta"))
//				{
//					MarkQABAlways(childpath);
//				}
//			}
//		}

//		[MenuItem("Assets/ResSystem Mark/Un_AssetBundleAllSubAssets")]
//		public static void UnMarkQABForAllSubAssets()
//		{
//			var path = MouseSelector.GetSelectedPathOrFallback();
//			var childpaths = Directory.GetFiles(path);
//			foreach (var childpath in childpaths)
//			{
//				Debug.Log(">>>>>> UnMark QAB: " + childpath);
//				if (childpath.EndsWith(".meta"))
//					continue;
//
//				var ai = AssetImporter.GetAtPath(childpath);
//
//				var obj = AssetDatabase.LoadAssetAtPath(childpath, typeof(Object));
//				RemoveUselessLabels(obj);
//
//				var labels = AssetDatabase.GetLabels(obj).ToList();
//				foreach (var lb in labels)
//				{
//					if (lb == LABEL_AB)
//					{
//						labels.Remove(lb);
//						AssetDatabase.SetLabels(obj, labels.ToArray());
//						break;
//					}
//				}
//
//				Menu.SetChecked(Mark_AssetBundle, false);
//				ai.assetBundleName = null;
//				AssetDatabase.RemoveUnusedAssetBundleNames();
//				ResetQMark(childpath);
//			}
//		}
	}
}