using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.ProjectWindowCallback;

public class ShaderCreater : Editor
{
    public static string shaderTemplatePath ="Assets/CFramework/Editor/Template/URPTemplate.shader";


    [MenuItem("Assets/Create/Shader/URPTemplate", priority =0)]
    public static void CreateShader()
    {
        //��ȡѡ��·��
        string locationPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            CreateInstance<CreateScriptAssetAction>(),
            locationPath+"/New URPShader.shader",
            null,
            shaderTemplatePath);
    }
    private static string GetSelectedPathOrFallBack()
    {
        string path = "Assets";
        //��������þ��ǻ�ȡ�����ѡ�е��ļ��ж���
        // Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }

        return path;
    }
    class CreateScriptAssetAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            //������Դ
            UnityEngine.Object obj = CreateAssetFromTemplate(pathName, resourceFile);
            //������ʾ����Դ
            ProjectWindowUtil.ShowCreatedAsset(obj);
        }
        internal static UnityEngine.Object CreateAssetFromTemplate(string pathName, string resourceFile)
        {
            //��ȡҪ��������Դ�ľ���·��
            string fullName = Path.GetFullPath(pathName);
            //��ȡ����ģ���ļ�
            StreamReader reader = new StreamReader(resourceFile);
            string content = reader.ReadToEnd();
            reader.Close();

            //��ȡ��Դ���ļ���
            // string fileName = Path.GetFileNameWithoutExtension(pahtName);
            //�滻Ĭ�ϵ��ļ���
            content = content.Replace("#TIME", System.DateTime.Now.ToString("yyyy��MM��dd�� HH:mm:ss dddd"));
            string[] tempStrs = pathName.Split('/');
            string fileName = tempStrs[tempStrs.Length - 1].Split('.')[0];
            content = content.Replace("#URPBaseShader", fileName);
            //д�����ļ�
            StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
            writer.Write(content);
            writer.Close();

            //ˢ�±�����Դ
            AssetDatabase.ImportAsset(pathName);
            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
        }
    }
}


