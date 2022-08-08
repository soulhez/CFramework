using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringHelper
{
    /// <summary>
    /// ��ȡ�����ַ���֮����ַ���
    /// </summary>
    /// <param name="sourse">Դ�ַ���</param>
    /// <param name="startstr">��ʼ�ַ���</param>
    /// <param name="endstr">�����ַ���</param>
    /// <returns></returns>
    public static string MidStrEx(string sourse, string startstr, string endstr)
    {
        string result = string.Empty;
        int startindex, endindex;
        try
        {
            startindex = sourse.IndexOf(startstr);
            if (startindex == -1)
                return result;
            string tmpstr = sourse.Substring(startindex + startstr.Length);
            endindex = tmpstr.IndexOf(endstr);
            if (endindex == -1)
                return result;
            result = tmpstr.Remove(endindex);
        }
        catch (Exception ex)
        {
            Debug.LogError("MidStrEx Err:" + ex.Message);
        }
        return result;
    }
}
