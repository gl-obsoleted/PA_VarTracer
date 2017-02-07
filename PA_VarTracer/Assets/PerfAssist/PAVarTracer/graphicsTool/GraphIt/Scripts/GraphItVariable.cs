using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GraphItVariable
{
    private string m_varName;
    private List<float> m_valueList = new List<float>();
    const int saveValueNum = GraphItData.DEFAULT_SAMPLES;
    Dictionary<string, string> m_channelDict = new Dictionary<string, string>();

    public Dictionary<string, string> ChannelDict
    {
        get { return m_channelDict; }
        set { m_channelDict = value; }
    }
    private Color m_color;
    public Color Color
    {
        get { return m_color; }
        set { m_color = value; }
    }

    public string VarName
    {
        get { return m_varName; }
        set { m_varName = value; }
    }

    public GraphItVariable(string varName)
    {
        m_varName = varName;
    }

    public void InsertValue(float value)
    {
        if (m_valueList.Count ==saveValueNum)
        {
            m_valueList.RemoveAt(0);
        }
        m_valueList.Add(value);

        foreach (var channel in m_channelDict.Keys)
        {
#if UNITY_EDITOR
            if (GraphItVar.Instance.Graphs.ContainsKey(channel))
            {
                GraphItData g = GraphItVar.Instance.Graphs[channel];
                if (!g.mData.ContainsKey(m_varName))
                {
                    g.mData[m_varName] = new GraphItDataInternal(g.mData.Count);
                }
                g.mData[m_varName].mColor = m_color;

                g.mData[m_varName].mCounter += value;
                g.mReadyForUpdate = true;
            }
#endif
        }
    }

    public void  AttchChannel(string channel)
    {
        if(string.IsNullOrEmpty(channel))
            return ;
        if (!m_channelDict.ContainsKey(channel))
            m_channelDict[channel] = channel;
    }

}
