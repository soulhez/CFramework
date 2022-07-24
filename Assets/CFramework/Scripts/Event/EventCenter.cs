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
    //key--事件的名字

    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
    /// <summary>
    /// 添加时间监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="aciton">准备用来处理事件的委托函数</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //有该事件
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
    /// 添加不需要参数的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        //有该事件
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
    /// 移除有参数的事件
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
    /// 事件触发
    /// </summary>
    /// <param name="name">事件的名字</param>
    public void EventTrigger<T>(string name, T info)
    {
        //Debug.Log("触发事件");
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            //执行所有的委托函数
            //eventDic[name].Invoke(info);    
        }

    }
    /// <summary>
    /// 事件触发不需要参数
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        //Debug.Log("触发事件");
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
            //执行所有的委托函数
            //eventDic[name].Invoke(info);    
        }

    }
    /// <summary>
    /// 清空事件中心，用于场景切换
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
