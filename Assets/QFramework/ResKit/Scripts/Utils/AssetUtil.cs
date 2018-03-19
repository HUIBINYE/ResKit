/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

public delegate void LoadProgress(string bundleName, float progress);

public delegate void LoadComplete(string bundleName);

public delegate void LoadAssetBundleCallback(string sceneName,string bundleName);

public class AssetUtil
{
//    public Action<float> LoadProgress;//加载进度
}
