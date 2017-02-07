using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GraphItVariableBody
{
    private string channelName;
    public string ChannelName
    {
        get { return channelName; }
        set { channelName = value; }
    }

    private string m_variableBodyName;
    public string VariableBodyName
    {
        get { return m_variableBodyName; }
        set { m_variableBodyName = value; }
    }

    public GraphItVariableBody(string varBodyName)
    {
        m_variableBodyName = varBodyName;
    }

    private Dictionary<string, GraphItVariable> m_variableDict = new Dictionary<string, GraphItVariable>();
    public Dictionary<string, GraphItVariable> VariableDict
    {
        get { return m_variableDict; }
        set { m_variableDict = value; }
    }

    private Dictionary<string, string> m_registEventList = new Dictionary<string, string>();

    public Dictionary<string, string> RegistEventList
    {
        get { return m_registEventList; }
        set { m_registEventList = value; }
    }

    public void registEvent(string eventName)
    {
        if(string.IsNullOrEmpty(eventName))
            return ;
        if (!m_registEventList.ContainsKey(eventName))
            m_registEventList[eventName] = eventName;
    }
}
