﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class GraphItDataInternal
{
    public GraphItDataInternal( int subgraph_index )
    {
        mDataInfos = new List<VarDataInfo>();
        mMin = 0.0f;
        mMax = 0.0f;
        mCurrentValue = 0.0f;
        switch(subgraph_index)
        {
            case 0:
                mColor = new Color( 0, 0.85f, 1, 1);
                break;
            case 1:
                mColor = Color.yellow;
                break;
            case 2:
                mColor = Color.green;
                break;
            case 3:
                mColor = Color.cyan;
                break;
            default:
                mColor = Color.gray;
                break;
        }
    }
    public float mMin;
    public float mMax;
    public float mCurrentValue;
    public Color mColor;
    public List<VarDataInfo> mDataInfos;
}

public class EventData
{
    public string m_eventName;
    public int m_eventFrameIndex;
    public float m_duration;

    public EventData(int eventFrameIndex,string eventName,float duration=0)
    {
        m_eventFrameIndex = eventFrameIndex;
        m_eventName = eventName;
        m_duration  = duration;
    }
}

public class GraphItData
{
    public static int DEFAULT_SAMPLES = 1048;
    public const int RECENT_WINDOW_SIZE = 120;
    
    public Dictionary<string, GraphItDataInternal> mData = new Dictionary<string, GraphItDataInternal>();

    public string mName;

    public bool mInclude0;

    public bool mReadyForUpdate;
    public bool mFixedUpdate;

    public int mWindowSize;

    public float m_maxValue;
    public float m_minValue;

    public bool mSharedYAxis;

    protected bool mHidden;
    protected float mHeight;

    float m_XStep = 4;

    Vector2 m_scrollPos;
    public Vector2 ScrollPos
    {
        get { return m_scrollPos; }
        set { m_scrollPos = value; }
    }

    public float XStep
    {
        get { return m_XStep; }
        set { m_XStep = value; }
    }

    public GraphItData( string name)
    {
        mName = name;

        mData = new Dictionary<string, GraphItDataInternal>();

        mInclude0 = false;

        mReadyForUpdate = true;
        mFixedUpdate = false;

        mWindowSize = DEFAULT_SAMPLES;

        mSharedYAxis = false; 
        mHidden = false;
        mHeight = 175;

        if (PlayerPrefs.HasKey(mName + "_height"))
        {
            SetHeight(PlayerPrefs.GetFloat(mName + "_height"));
        }
    }

    public float GetMin( string subgraph )
    {
        bool min_set = false;
        float min = 0;
        foreach (var entry in mData)
        {
            var g = entry.Value;
            if (!min_set)
            {
                min = g.mMin;
                min_set = true;
            }
            min = Math.Min(min, g.mMin);
        }
        m_minValue = min; 
        return min;
    }

    public float GetMax( string subgraph )
    {
        bool max_set = false;
        float max = 0;
        foreach (KeyValuePair<string, GraphItDataInternal> entry in mData)
        {
            GraphItDataInternal g = entry.Value;
            if (!max_set)
            {
                max = g.mMax;
                max_set = true;
            }
            max = Math.Max(max, g.mMax);
        }
        m_maxValue = max;
        return max;
    }

    public float GetHeight()
    {
        return mHeight;
    }
    public void SetHeight( float height )
    {
        mHeight = height;
    }
    public void DoHeightDelta(float delta)
    {
        SetHeight( Mathf.Max(mHeight + delta, 50) );
        PlayerPrefs.SetFloat( mName+"_height", GetHeight() );
    }
}

public class VarTracer : MonoBehaviour
{
#if UNITY_EDITOR
    public const string VERSION = "1.2.0";
    public Dictionary<string, GraphItData> Graphs = new Dictionary<string, GraphItData>();
    public Dictionary<string, GraphItVariableBody> VariableBodys = new Dictionary<string, GraphItVariableBody>();

    public static VarTracer mInstance = null;

    static bool m_isStart = false;
    static Stopwatch m_timer = new Stopwatch();
#endif

    void Start()
    {
        if (m_isStart)
            StartVarTracer();
    }


    public int GetCurrentFrame()
    {
        int currentFrame = (int)(m_timer.ElapsedMilliseconds / 1000.0f * VarTracerConst.FPSPerSecond);
        return currentFrame;
    }

    public static void StartVarTracer()
    {
        m_isStart = true;
        m_timer.Start();
    }

    public static void StopVarTracer()
    {
        m_isStart = false;
        m_timer.Stop();
    }

    public static bool isVarTracerStart()
    {
        return m_isStart;
    }

    public static VarTracer Instance
    {
        get
        {
#if UNITY_EDITOR
            if( mInstance == null )
            {
                GameObject go = new GameObject("GraphIt");
                go.hideFlags = HideFlags.HideAndDontSave;
                mInstance = go.AddComponent<VarTracer>();
            }
            return mInstance;
#else
            return null;
#endif
        }
    }

    void StepGraphInternal(GraphItData graph)
    {
#if UNITY_EDITOR
        foreach (KeyValuePair<string, GraphItDataInternal> entry in graph.mData)
        {
            GraphItDataInternal g = entry.Value;
            if (g.mDataInfos.Count <= 0)
                continue;
                
            float sum = g.mDataInfos[0].Value;
            float min = g.mDataInfos[0].Value;
            float max = g.mDataInfos[0].Value;
            for (int i = 1; i < g.mDataInfos.Count; ++i)
            {
                sum += g.mDataInfos[i].Value;
                min = Mathf.Min(min, g.mDataInfos[i].Value);
                max = Mathf.Max(max, g.mDataInfos[i].Value);
            }
            if (graph.mInclude0)
            {
                min = Mathf.Min(min, 0.0f);
                max = Mathf.Max(max, 0.0f);
            }

            g.mMin = min;
            g.mMax = max;
        }
#endif
    }

    // Update is called once per frame
    void LateUpdate()
    {
#if UNITY_EDITOR
        foreach (KeyValuePair<string, GraphItData> kv in Graphs)
        {
            GraphItData g = kv.Value;
            if (g.mReadyForUpdate && !g.mFixedUpdate)
            {
                StepGraphInternal(g);
            }
        }
#endif
    }

    // Update is called once per fixed frame
    void FixedUpdate()
    {
#if UNITY_EDITOR
        foreach (KeyValuePair<string, GraphItData> kv in Graphs)
        {
            GraphItData g = kv.Value;
            if (g.mReadyForUpdate && g.mFixedUpdate )
            {
                StepGraphInternal(g);
            }
        }
#endif
    }

    /// <summary>
    /// Optional setup function that allows you to specify the initial height of a graph.
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="subgraph"></param>
    /// <param name="height"></param>
    public static void GraphSetupHeight(string graph, float height)
    {
#if UNITY_EDITOR
        if (!Instance.Graphs.ContainsKey(graph))
        {
            Instance.Graphs[graph] = new GraphItData(graph);
        }

        GraphItData g = Instance.Graphs[graph];
        //g.mCurrentIndex = m_wholeFrameIndex;
        g.SetHeight(height);
#endif
    }

    /// <summary>
    /// Allows you to switch between sharing the y-axis on a graph for all subgraphs, or for them to be independent.
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="shared_y_axis"></param>
    public static void ShareYAxis(string graph, bool shared_y_axis)
    {
#if UNITY_EDITOR
        if (!Instance.Graphs.ContainsKey(graph))
        {
            Instance.Graphs[graph] = new GraphItData(graph);
        }

        GraphItData g = Instance.Graphs[graph];
        g.mSharedYAxis = shared_y_axis;
#endif
    }

    public static void AttachVariable(string variableName, string ChannelName)
    {
#if UNITY_EDITOR
        foreach (var VarBody in Instance.VariableBodys.Values)
        {
            if(VarBody.VariableDict.ContainsKey(variableName))
            {
                var var = VarBody.VariableDict[variableName];
                var.AttchChannel(ChannelName);
            }
        }
#endif
    }

    public static void DefineVisualChannel(string channel, float height, bool isShareY = false)
    {
        GraphSetupHeight(channel, height);
        ShareYAxis(channel, isShareY);
    }

    public static GraphItVariable GetGraphItVariableByVariableName(string variableName)
    {
        if (string.IsNullOrEmpty(variableName))
            return null;

#if UNITY_EDITOR
        foreach (var VarBody in Instance.VariableBodys.Values)
        {
            if (VarBody.VariableDict.ContainsKey(variableName))
            {
                var var = VarBody.VariableDict[variableName];
                return var;
            }
        }
#endif    
        return null;
    }

    public static bool IsVariableOnShow(string  variableName)
    {
        if (string.IsNullOrEmpty(variableName))
            return false;

        var graphVar = GetGraphItVariableByVariableName(variableName);
        if (graphVar == null)
            return false;

        if (graphVar.ChannelDict.Count > 0)
            return true;

        return false;
    }

    public static void AddChannel()
    {
        string newChannelName = Instance.Graphs.Count.ToString();
        DefineVisualChannel(newChannelName,200,true);
    }

    public static void RemoveChannel()
    {
        if (Instance.Graphs.Count <= 1)
            return;

        string removeChannelName = (Instance.Graphs.Count-1).ToString();
#if UNITY_EDITOR
        foreach (var VarBody in Instance.VariableBodys.Values)
        {
            foreach(var var in VarBody.VariableDict.Values)
            {
                var.DetachChannel(removeChannelName);
            }
        }
        Instance.Graphs.Remove(removeChannelName);
#endif
    }

    public static void ClearAll()
    {
#if UNITY_EDITOR
        foreach (var VarBody in Instance.VariableBodys.Values)
        {
            foreach (var var in VarBody.VariableDict.Values)
            {
                foreach (var graphName in VarTracer.Instance.Graphs.Keys)
                {
                    if (var.ChannelDict.ContainsKey(graphName))
                        var.DetachChannel(graphName);
                }
            }
        }
#endif
    }
}
