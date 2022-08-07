using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AssetBundleMgr : MonoBehaviour
{
    /// <summary>
    /// 主包
    /// </summary>
    private AssetBundle mainAB = null;
    /// <summary>
    /// 依赖包获取用的配置文件
    /// </summary>
    private AssetBundleManifest manifest = null;
    /// <summary>
    /// 用字典存储加载过的AB包
    /// </summary>
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// 这个AB包存放路径 方便修改
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
            //return Application.dataPath + "!assets/";
        }
    }
    private string MainABName
    {
        get
        {
#if UNITY_IOS
        return "IOS";
#elif UNITY_ANDROID
        return "Android";
#else
            return "PC";
#endif
        }
    }
    public void LoadAB(string abName)
    {
        //加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        }
        //获取依赖包的相关信息
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //加载资源来源包
        //如果没有加载过 再加载

        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }
    /// <summary>
    /// 同步加载，如果类型为GameObject，则直接创建
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    public Object LoadRes(string abName, string resName, bool isCreateObj = true)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便，在加载资源时，判断是否为GameObject
        Object obj = abDic[abName].LoadAsset(resName);
        //加载资源
        if (obj is GameObject)
        {
            if (isCreateObj)
            {
                return Instantiate(obj);
            }
            else
            {
                return obj;
            }
        }
        else
        {
            return obj;
        }

    }
    /// <summary>
    /// 同步泛型加载，如果类型为GameObject，则直接创建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public T LoadRes<T>(string abName, string resName, bool isCreateObj = true) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便，在加载资源时，判断是否为GameObject
        T obj = abDic[abName].LoadAsset<T>(resName);
        //加载资源
        if (obj is GameObject)
        {
            if (isCreateObj)
            {
                return Instantiate(obj);
            }
            else
            {
                return obj;
            }
        }
        else
        {
            return obj;
        }

    }
    /// <summary>
    /// 根据名字异步加载
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便，在加载资源时，判断是否为GameObject
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;

        //异步加载结束后，通过委托给外部使用

        //加载资源
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        else
        {
            callBack(abr.asset);
        }
        yield return null;
    }
    /// <summary>
    /// 根据类型异步加载
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便，在加载资源时，判断是否为GameObject
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;

        //异步加载结束后，通过委托给外部使用

        //加载资源
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset));
        }
        else
        {
            callBack(abr.asset);
        }
        yield return null;
    }
    /// <summary>
    /// 根据名字泛型异步加载
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便，在加载资源时，判断是否为GameObject
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;

        //异步加载结束后，通过委托给外部使用

        //加载资源
        if (abr.asset is GameObject)
        {
            callBack(Instantiate(abr.asset) as T);
        }
        else
        {
            callBack(abr.asset as T);
        }
        yield return null;
    }
    //单个包卸载
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }
    //所有包卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }

}
