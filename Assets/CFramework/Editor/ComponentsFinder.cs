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
        //以下代码都是通过脚本自动生成的
    public class #类名# :UIBase
    {
        #region AutoUIBase
        #成员变量#
        [ContextMenu(""Generate"")]
        public void Generate()
        {
            #查找方法#
        }
        #endregion
            
   
    } ";
    public static string TransformFindStr = "#目标成员变量#=transform.Find(\"#路径#\").GetComponent<#组件名称#>();";
    /// <summary>
    /// UI组件查找工具命名标准：_xxx，如：_Img，_Sli，_Tog
    /// </summary>
    [MenuItem("Tools/CreateUIBaseScript")]
    public static void UIComponentFinder()
    {
        GameObject item = Selection.activeGameObject;
        string className = item.name;
        string scriptPath = Application.dataPath + "/AutoUIScripts";

        //创建自动生成的脚本文件
        if (!Directory.Exists(scriptPath))
        {
            Directory.CreateDirectory(scriptPath);
        }
        scriptPath = scriptPath + "/" + className + ".cs";
        //判断文件是否已经存在
        
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
           
           

            //获取变量名
            variableStrList.Add(trans[i].name);
            if(componentName!="")
            {
                //获取变量类型名称
                variableTypeList.Add(componentName);
            }

            //获取Transform路径
            transPathStrList.Add(GetTransformPath(trans[i], item.transform));

            
        }
        for(int i=0;i<variableStrList.Count;i++)
        {
            variableStr += "public "+variableTypeList[i] +" "+ variableStrList[i]+";\n";

        }
        for(int i=0;i<transPathStrList.Count;i++)
        {
            string transformFindStr= TransformFindStr.Replace("#目标成员变量#", variableStrList[i]);
            transformFindStr=transformFindStr.Replace("#路径#", transPathStrList[i]);
            transformFindStr = transformFindStr.Replace("#组件名称#", variableTypeList[i]);
            contentStr += transformFindStr+"\n";
        }
        string classInfo = objBaseClassStr;
        classInfo = classInfo.Replace("#类名#", className);
        classInfo = classInfo.Replace("#成员变量#", variableStr);
        classInfo = classInfo.Replace("#查找方法#",contentStr);
        if (File.Exists(scriptPath))
        {
            string originalCode= File.ReadAllText(scriptPath);
            
            Debug.Log(originalCode);
            classInfo= SubUIBaseScriptsExist(classInfo, originalCode);
            Debug.Log(classInfo);
            Debug.Log("存在相同名称的类:" + scriptPath + ",已覆盖");
            
        }


        FileStream file = new FileStream(scriptPath, FileMode.Create);
        StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
        fileW.Write(classInfo);
        fileW.Flush();
        fileW.Close();
        file.Close();

        AssetDatabase.Refresh();


    }
    /// <summary>
    /// 如果UIBase子类已经存在，只修改自动生成的代码，其余不变
    /// </summary>
    static string SubUIBaseScriptsExist(string newCode,string originalCode)
    {
        string originalCodeTemp= StringHelper.MidStrEx(originalCode, "#region AutoUIBase", "#endregion");
        string newCodeTemp= StringHelper.MidStrEx(newCode, "#region AutoUIBase", "#endregion");
        return originalCode.Replace(originalCodeTemp, newCodeTemp);
        
    }


    /// <summary>
    /// 获取Transform路径
    /// </summary>
    /// <param name="target"></param>
    /// <param name="root"></param>
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
        
        pathStr= pathStr.Substring(0,pathStr.Length-1);
        return pathStr;
    }

    
}
