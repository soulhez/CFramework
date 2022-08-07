using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AssetBundleMgr : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    private AssetBundle mainAB = null;
    /// <summary>
    /// ��������ȡ�õ������ļ�
    /// </summary>
    private AssetBundleManifest manifest = null;
    /// <summary>
    /// ���ֵ�洢���ع���AB��
    /// </summary>
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// ���AB�����·�� �����޸�
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
        //����AB��
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        }
        //��ȡ�������������Ϣ
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //�жϰ��Ƿ���ع�
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //������Դ��Դ��
        //���û�м��ع� �ټ���

        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }
    /// <summary>
    /// ͬ�����أ��������ΪGameObject����ֱ�Ӵ���
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    public Object LoadRes(string abName, string resName, bool isCreateObj = true)
    {
        //����AB��
        LoadAB(abName);
        //Ϊ�����淽�㣬�ڼ�����Դʱ���ж��Ƿ�ΪGameObject
        Object obj = abDic[abName].LoadAsset(resName);
        //������Դ
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
    /// ͬ�����ͼ��أ��������ΪGameObject����ֱ�Ӵ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public T LoadRes<T>(string abName, string resName, bool isCreateObj = true) where T : Object
    {
        //����AB��
        LoadAB(abName);
        //Ϊ�����淽�㣬�ڼ�����Դʱ���ж��Ƿ�ΪGameObject
        T obj = abDic[abName].LoadAsset<T>(resName);
        //������Դ
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
    /// ���������첽����
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
        //����AB��
        LoadAB(abName);
        //Ϊ�����淽�㣬�ڼ�����Դʱ���ж��Ƿ�ΪGameObject
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;

        //�첽���ؽ�����ͨ��ί�и��ⲿʹ��

        //������Դ
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
    /// ���������첽����
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
        //����AB��
        LoadAB(abName);
        //Ϊ�����淽�㣬�ڼ�����Դʱ���ж��Ƿ�ΪGameObject
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;

        //�첽���ؽ�����ͨ��ί�и��ⲿʹ��

        //������Դ
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
    /// �������ַ����첽����
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
        //����AB��
        LoadAB(abName);
        //Ϊ�����淽�㣬�ڼ�����Դʱ���ж��Ƿ�ΪGameObject
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;

        //�첽���ؽ�����ͨ��ί�и��ⲿʹ��

        //������Դ
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
    //������ж��
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }
    //���а�ж��
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }

}
