using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GraphItVariable
{
    private string m_varName;
    private string m_varBodyName;
    public string VarBodyName
    {
        get { return m_varBodyName; }
        set { m_varBodyName = value; }
    }
    private List<float> m_valueList = new List<float>();
    Dictionary<string, string> m_channelDict = new Dictionary<string, string>();
    private VariableConfigPopup m_popupWindow;
    private Rect m_popupRect;
    public Rect PopupRect
    {
        get { return m_popupRect; }
        set { m_popupRect = value; }
    }

    public VariableConfigPopup PopupWindow
    {
        get { return m_popupWindow; }
        set { m_popupWindow = value; }
    }

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

    public GraphItVariable(string varName, string varBodyName)
    {
        m_varName = varName;
        m_varBodyName = varBodyName;
        m_popupWindow = new VariableConfigPopup(varName);
    }

    public void InsertValue(float value)
    {
        m_valueList.Add(value);

        foreach (var channel in m_channelDict.Keys)
        {
#if UNITY_EDITOR
            if (VarTracer.Instance.Graphs.ContainsKey(channel))
            {
                GraphItData g = VarTracer.Instance.Graphs[channel];
                if (!g.mData.ContainsKey(m_varName))
                {
                    g.mData[m_varName] = new GraphItDataInternal(g.mData.Count);
                }
                g.mData[m_varName].mColor = m_color;

                g.mData[m_varName].mCurrentValue = value;
                g.mData[m_varName].mCounter += value;
            }
#endif
        }
    }

    public void AttchChannel(string channel)
    {
        if(string.IsNullOrEmpty(channel))
            return ;
        if (!m_channelDict.ContainsKey(channel))
        {
            m_channelDict[channel] = channel;
            if (m_valueList.Count>0)
            {
#if UNITY_EDITOR
                if (VarTracer.Instance.Graphs.ContainsKey(channel))
                {
                    GraphItData g = VarTracer.Instance.Graphs[channel];
                    if (!g.mData.ContainsKey(m_varName))
                    {
                        g.mData[m_varName] = new GraphItDataInternal(g.mData.Count);
                    }

                    g.mData[m_varName].mDataInfos.Clear();
                    foreach (float value in m_valueList)
                    {
                        g.mData[m_varName].mDataInfos.Add(new VarDataInfo(value));                        
                    }

                    g.mData[m_varName].mColor = m_color;
                    g.mCurrentIndex = m_valueList.Count - 1;
                }
#endif
            }
        }
    }

    public void DetachChannel(string channel)
    {
        if (string.IsNullOrEmpty(channel))
            return;
        if (m_channelDict.ContainsKey(channel))
        {
            m_channelDict.Remove(channel);
#if UNITY_EDITOR
            if (VarTracer.Instance.Graphs.ContainsKey(channel))
            {
                GraphItData g = VarTracer.Instance.Graphs[channel];
                if (g.mData.ContainsKey(m_varName))
                {
                    g.mData.Remove(m_varName);
                }
            }
#endif
        }
    }
}
