using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseScriptsCreater : Editor
{
    static string UIBaseTemplatePath = "Assets/CFramework/Editor/Templates/CSharpTemplates/UIBaseTemplate.txt";
    
    public static string TransformFindStr = "#Ŀ���Ա����#=transform.Find(\"#·��#\").GetComponent<#�������#>();";
    /// <summary>
    /// UI������ҹ���������׼��_xxx���磺_Img��_Sli��_Tog
    /// </summary>
    [MenuItem("GameObject/CreateUIBaseScript",false,-100)]
    public static void UIComponentFinder()
    {
        GameObject item = Selection.activeGameObject;
        string className = item.name;
        string scriptPath = Application.dataPath + "/AutoUIScripts";

        //�����Զ����ɵĽű��ļ�
        if (!Directory.Exists(scriptPath))
        {
            Directory.CreateDirectory(scriptPath);
        }
        scriptPath = scriptPath + "/" + className + ".cs";
        //�ж��ļ��Ƿ��Ѿ�����
        
        string contentStr = "";
        string variableStr = "";
        List<string> variableStrList = new List<string>();
        List<string> variableTypeList = new List<string>();
        List<string> transPathStrList = new List<string>();
        Transform[] trans = item.GetComponentsInChildren<Transform>();
        for (int i = 0; i < trans.Length; i++)
        {
            string[] str = trans[i].name.Split('_');
            if (str.Length < 2)
            {
                continue;
            }
            string componentName = "Transform";
            switch(str[str.Length-1].ToLower())
            {
                case "img":
                    componentName = "Image";
                    break;
                case "sli":
                    componentName = "Slider";
                    break;
                case "txt":
                    componentName = "Text";
                    break;
                case "btn":
                    componentName = "Button";
                    break;
            }
           
           

            //��ȡ������
            variableStrList.Add(trans[i].name);
            if(componentName!="")
            {
                //��ȡ������������
                variableTypeList.Add(componentName);
            }

            //��ȡTransform·��
            transPathStrList.Add(GetTransformPath(trans[i], item.transform));

            
        }
        //��ʼ�������ַ���
        for(int i=0;i<variableStrList.Count;i++)
        {
            variableStr += "public "+variableTypeList[i] +" "+ variableStrList[i]+";\n";

        }
        //��ʼ�������ַ���
        for(int i=0;i<transPathStrList.Count;i++)
        {
            string transformFindStr= TransformFindStr.Replace("#Ŀ���Ա����#", variableStrList[i]);
            transformFindStr=transformFindStr.Replace("#·��#", transPathStrList[i]);
            transformFindStr = transformFindStr.Replace("#�������#", variableTypeList[i]);
            contentStr += transformFindStr+"\n";
        }
        //����д�뵽�ļ��е��ַ���
        string classInfo = File.ReadAllText(UIBaseTemplatePath, System.Text.Encoding.UTF8);
        classInfo = classInfo.Replace("#ʱ��#", System.DateTime.Now.ToString("yyyy��MM��dd�� HH:mm:ss dddd"));
        classInfo = classInfo.Replace("#����#", className);
        classInfo = classInfo.Replace("#��Ա����#", variableStr);
        classInfo = classInfo.Replace("#���ҷ���#", contentStr);
        //����ű����ڣ�����Ӧ�Զ����ɵĴ����滻��
        if (File.Exists(scriptPath))
        {
            string originalCode= File.ReadAllText(scriptPath);
            
            Debug.Log(originalCode);
            classInfo= SubUIBaseScriptsExist(classInfo, originalCode);
            Debug.Log(classInfo);
            Debug.Log("������ͬ���Ƶ���:" + scriptPath + ",�Ѹ���");
            
        }

        //д���ļ�
        FileStream file = new FileStream(scriptPath, FileMode.Create);
        StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
        fileW.Write(classInfo);
        fileW.Flush();
        fileW.Close();
        file.Close();
        //ˢ����Դ�б�
        AssetDatabase.Refresh();


    }
    /// <summary>
    /// ���UIBase�����Ѿ����ڣ�ֻ�޸��Զ����ɵĴ��룬���಻��
    /// </summary>
    static string SubUIBaseScriptsExist(string newCode,string originalCode)
    {
        string originalCodeTemp= StringHelper.MidStrEx(originalCode, "#region AutoUIBase", "#endregion");
        string newCodeTemp= StringHelper.MidStrEx(newCode, "#region AutoUIBase", "#endregion");
        return originalCode.Replace(originalCodeTemp, newCodeTemp);
        
    }


    /// <summary>
    /// ��ȡTransform·��
    /// </summary>
    /// <param name="target">Ŀ������</param>
    /// <param name="root">������</param>
    /// <returns></returns>
    static string GetTransformPath(Transform target,Transform root)
    {
        string pathStr = "";
        if(target.parent==root)
        {
            pathStr = target.name+"/";
        }
        else
        {
            while (target!= root)
            {
                pathStr = target.name + "/" + pathStr;
                target = target.parent;

            }
        }
        //�����һ����/������ȥ��
        pathStr= pathStr.Substring(0,pathStr.Length-1);
        return pathStr;
    }

    
}
