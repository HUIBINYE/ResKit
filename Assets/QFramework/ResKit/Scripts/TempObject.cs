/****************************************************************************
 * Copyright (c) 2017 yuanhuibin@putao.com
 ****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class TempObject
{
    private List<Object> mAssets;

    public List<Object> Asset
    {
        get { return mAssets; }
    }

    public TempObject(params Object[] objects)
    {
        mAssets = new List<Object>();
        mAssets.AddRange(objects);
    }

//    public void AddAsset(params Object[] objects)
//    {
//        
//    }

    public void UnLoadAsset()
    {
        for (int i = mAssets.Count - 1; i >= 0; i--)
        {
            Resources.UnloadAsset(mAssets[i]);
        }
    }
}
