using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ComponentsFinder : Editor
{
    public static string objBaseClassStr =
        @"using UnityEngine;
        using UnityEngine.UI;
        using UnityEngine.EventSystems;
        using System;
        //���´��붼��ͨ���ű��Զ����ɵ�
        public class #����# :UIBase
        {
            #��Ա����#
            [ContextMenu(""Generate"")]
            public void Generate()
            {
                #���ҷ���#
            }
   
        } ";
    public static string TransformFindStr = "#Ŀ���Ա����#=transform.Find(\"#·��#\").GetComponent<#�������#>();";
    /// <summary>
    /// UI������ҹ���������׼��_xxx���磺_Img��_Sli��_Tog
    /// </summary>
    [MenuItem("Tools/CreateUIBaseScript")]
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
        if(File.Exists(scriptPath))
        {
            File.Delete(scriptPath);
            Debug.Log("������ͬ���Ƶ���:" + scriptPath+",�Ѹ���");
            
        }
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
        for(int i=0;i<variableStrList.Count;i++)
        {
            variableStr += "public "+variableTypeList[i] +" "+ variableStrList[i]+";\n";

        }
        for(int i=0;i<transPathStrList.Count;i++)
        {
            string transformFindStr= TransformFindStr.Replace("#Ŀ���Ա����#", variableStrList[i]);
            transformFindStr=transformFindStr.Replace("#·��#", transPathStrList[i]);
            transformFindStr = transformFindStr.Replace("#�������#", variableTypeList[i]);
            contentStr += transformFindStr+"\n";
        }
        string classInfo = objBaseClassStr;
        classInfo = classInfo.Replace("#����#", className);
        classInfo = classInfo.Replace("#��Ա����#", variableStr);
        classInfo = classInfo.Replace("#���ҷ���#",contentStr);
        

        FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
        StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
        fileW.Write(classInfo);
        fileW.Flush();
        fileW.Close();
        file.Close();

        AssetDatabase.Refresh();


    }
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
        
        pathStr= pathStr.Substring(0,pathStr.Length-1);
        return pathStr;
    }
}