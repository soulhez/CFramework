using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{ }


public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventCenter : Singleton<EventCenter>
{
    //key--�¼�������

    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
    /// <summary>
    /// ���ʱ�����
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="aciton">׼�����������¼���ί�к���</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //�и��¼�
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }
    /// <summary>
    /// ��Ӳ���Ҫ�������¼�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        //�и��¼�
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }
    public void AddEventListener(EventType eventType, UnityAction action)
    {
        AddEventListener(eventType.ToString(), action);
    }
    public void AddEventListener<T>(EventType eventType, UnityAction<T> action)
    {
        AddEventListener(eventType.ToString(), action);
    }


    /// <summary>
    /// �Ƴ��в������¼�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// �¼�����
    /// </summary>
    /// <param name="name">�¼�������</param>
    public void EventTrigger<T>(string name, T info)
    {
        //Debug.Log("�����¼�");
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            //ִ�����е�ί�к���
            //eventDic[name].Invoke(info);    
        }

    }
    /// <summary>
    /// �¼���������Ҫ����
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        //Debug.Log("�����¼�");
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
            //ִ�����е�ί�к���
            //eventDic[name].Invoke(info);    
        }

    }
    /// <summary>
    /// ����¼����ģ����ڳ����л�
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
