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

    public GraphItVariable(string varName)
    {
        m_varName = varName;

        m_popupWindow = new VariableConfigPopup(varName);
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

                    g.mData[m_varName].mDataPoints = new float[GraphItData.DEFAULT_SAMPLES];
                    for (int i = 0; i < m_valueList.Count; i++)
                    {
                        g.mData[m_varName].mDataPoints[i] = m_valueList[i];
                    }

                    g.mData[m_varName].mColor = m_color;
                    g.mCurrentIndex = m_valueList.Count-1;
                    if (g.mCurrentIndex == GraphItData.DEFAULT_SAMPLES-1)
                        g.mFullArray = true; 
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
