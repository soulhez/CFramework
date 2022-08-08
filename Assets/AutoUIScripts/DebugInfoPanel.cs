using UnityEngine;
        using UnityEngine.UI;
        using UnityEngine.EventSystems;
        using System;
//以下代码都是通过脚本自动生成的
public class DebugInfoPanel : UIBase
{
    #region AutoUIBase
        public Text CPU_Txt;
public Text Memory_Txt;
public Text FPS_Txt;
public Text System_Txt;

        [ContextMenu("Generate")]
        public void Generate()
        {
            CPU_Txt=transform.Find("CPU_Txt").GetComponent<Text>();
Memory_Txt=transform.Find("Memory_Txt").GetComponent<Text>();
FPS_Txt=transform.Find("FPS_Txt").GetComponent<Text>();
System_Txt=transform.Find("System_Txt").GetComponent<Text>();

        }
        #endregion

    public void Test()
    {

    }
}