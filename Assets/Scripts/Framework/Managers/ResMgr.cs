using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResMgr 
{
    public static ResMgr Instance = null;
    public void Init() {
        ResMgr.Instance = this;
    }

    public T LoadAssetSync<T>(string path) where T : Object {
#if UNITY_EDITOR
        path = Path.Combine("Assets/AssetsPackage", path);
        T ret = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
        return ret;
#else
        return null;
#endif

    }
}
