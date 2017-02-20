using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GraphItWindow : EditorWindow
{
    const int InValidNum = -1;
    static Vector2 mGraphViewScrollPos;

    static float mWidth;

    static int mMouseOverGraphIndex = InValidNum;
    static float mMouseX = 0;

    static float x_offset = 200.0f;
    static float y_gap = 80.0f;
    static float y_offset = 20;
    static int precision_slider = 3;


    static GUIStyle NameLabel;
    static GUIStyle SmallLabel;
    static GUIStyle HoverText;
    static GUIStyle FracGS;
    static GUIStyle EventInstantButtonStyle;
    static GUIStyle EventDurationButtonStyle;

    static Material mLineMaterial;

    public static float m_winWidth = 0.0f;
    public static float m_winHeight = 0.0f;

    public static float m_controlScreenHeight = 0.0f;
    public static float m_controlScreenPosY = 0.0f;

    public static float m_navigationScreenHeight = 0.0f;
    public static float m_navigationScreenPosY = 0.0f;

    public static int m_variableBarIndex = InValidNum;

    const int variableNumPerLine = 12;
    const int variableLineHight = 20;

    const int variableLineStartY = 25;

    Rect m_popupWindow = new Rect(235, 0, 70, 18);

    static void InitializeStyles()
    {
        if (NameLabel == null)
        {
            NameLabel = new GUIStyle(EditorStyles.whiteBoldLabel);
            NameLabel.normal.textColor = Color.white;
            SmallLabel = new GUIStyle(EditorStyles.whiteLabel);
            SmallLabel.normal.textColor = Color.white;

            HoverText = new GUIStyle(EditorStyles.whiteLabel);
            HoverText.alignment = TextAnchor.UpperRight;
            HoverText.normal.textColor = Color.white;

            FracGS = new GUIStyle(EditorStyles.whiteLabel);
            FracGS.alignment = TextAnchor.LowerLeft;

            EventInstantButtonStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
            EventInstantButtonStyle.normal.background = Resources.Load("instantButton") as Texture2D;
            EventInstantButtonStyle.normal.textColor = Color.white;
            EventInstantButtonStyle.alignment = TextAnchor.MiddleCenter;

            EventDurationButtonStyle = new GUIStyle(EditorStyles.whiteBoldLabel);
            EventDurationButtonStyle.normal.background = Resources.Load("durationButton") as Texture2D;
            EventDurationButtonStyle.normal.textColor = Color.white;
            EventDurationButtonStyle.alignment = TextAnchor.MiddleCenter;
        }
    }

    [MenuItem("Window/PerfAssist" + "/VarTracer")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GraphItWindow window = (GraphItWindow)EditorWindow.GetWindow(typeof(GraphItWindow), false,"GraphItVariable");
        window.minSize = new Vector2(230f, 50f);
        window.Show();
    }

    void OnEnable()
    {
        EditorApplication.update += DrawGraph;
        if (VarTracer.Instance != null)
        {
            if(VarTracer.Instance.Graphs.Count==0)
                VarTracer.AddChannel();

            bool constainsCamera = VarTracer.Instance.VariableBodys.ContainsKey("Camera");
            if (!constainsCamera || VarTracer.Instance.VariableBodys["Camera"].VariableDict.Count==0)
            {
                VarTracerTool.DefineVariable("CameraV_X", "Camera");
                VarTracerTool.DefineVariable("CameraV_Y", "Camera");
                VarTracerTool.DefineVariable("CameraV_Z", "Camera");
                VarTracerTool.DefineVariable("CameraV_T", "Camera");

                VarTracerTool.DefineVariable("NpcV_X", "Npc");
                VarTracerTool.DefineVariable("NpcV_Y", "Npc");
                VarTracerTool.DefineVariable("NpcV_Z", "Npc");
                VarTracerTool.DefineVariable("NpcV_T", "Npc");

                VarTracerTool.DefineVariable("PlayerV_X", "Player");
                VarTracerTool.DefineVariable("PlayerV_Y", "Player");
                VarTracerTool.DefineVariable("PlayerV_Z", "Player");
                VarTracerTool.DefineVariable("PlayerV_T", "Player");
            }

        }
        VarTracer.StartVarTracer();
    }

    void OnDisable()
    {
        EditorApplication.update -= DrawGraph;
        VarTracer.Instance.Graphs.Clear();
        VarTracer.Instance.VariableBodys.Clear();
    }

    void DrawGraph()
    {
        Repaint();
    }


    public void CheckForResizing()
    {
        if (Mathf.Approximately(position.width, m_winWidth) &&
            Mathf.Approximately(position.height, m_winHeight))
            return;

        m_winWidth = position.width;
        m_winHeight = position.height;

        UpdateVariableAreaHight();

        m_controlScreenPosY = 0.0f;

        m_navigationScreenHeight = m_winHeight -  m_controlScreenHeight;
        m_navigationScreenPosY = m_controlScreenHeight;
    }


    void OnGUI()
    {
        CheckForResizing();

        Handles.BeginGUI();
        Handles.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));
        //control窗口内容
        GUILayout.BeginArea(new Rect(0, m_controlScreenPosY, m_winWidth, m_controlScreenHeight));
        {
            DrawVariableBar();
        }
        GUILayout.EndArea();
        ////navigation窗口内容
        GUILayout.BeginArea(new Rect(0, m_navigationScreenPosY, m_winWidth, m_winHeight));
        {
            DrawGraphs(position, this);
        }
        GUILayout.EndArea();
        Handles.EndGUI();
    }


    void UpdateVariableAreaHight()
    {
        var lineNum = CalculateVariableLineNum();
        var ry = variableLineStartY + lineNum * variableLineHight;

        y_offset = ry;
        m_controlScreenHeight = ry;
    }

    int  CalculateVariableLineNum()
    {
        List<GraphItVariable> variableList = new List<GraphItVariable>();
        foreach (var varBody in VarTracer.Instance.VariableBodys.Values)
        {
            foreach (var var in varBody.VariableDict.Values)
            {
                variableList.Add(var);
            }
        }

        int lineNum = variableList.Count / variableNumPerLine;
        int mod = variableList.Count % variableNumPerLine;
        if (mod > 0)
            lineNum += 1;

        return lineNum;
    }

    List<GraphItVariable> GetVariableList()
    {
        List<GraphItVariable> variableList = new List<GraphItVariable>();
        foreach (var varBody in VarTracer.Instance.VariableBodys.Values)
        {
            foreach (var var in varBody.VariableDict.Values)
            {
                variableList.Add(var);
            }
        }
        return variableList;
    }


    void DrawVariableBar()
    {
        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("Add Graph", EditorStyles.toolbarButton, GUILayout.Width(100)))
                VarTracer.AddChannel();

            GUILayout.Space(5);

            if (GUILayout.Button("Remove Graph", EditorStyles.toolbarButton, GUILayout.Width(100)))
                VarTracer.RemoveChannel();

            GUILayout.Space(5);

            if (GUILayout.Button("Clear All", EditorStyles.toolbarButton, GUILayout.Width(100)))
                VarTracer.ClearAll();

            GUILayout.Space(780);


            string buttonName;
            if (EditorApplication.isPaused)
                buttonName = "Resume";
            else
                buttonName = "Pause";
            if (GUILayout.Button(buttonName, EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                EditorApplication.isPaused = !EditorApplication.isPaused;
                if (EditorApplication.isPaused)
                    VarTracer.StopVarTracer();
                else
                    VarTracer.StartVarTracer();
            }

            GUILayout.EndHorizontal();

                var lineNum = CalculateVariableLineNum();
                var varList = GetVariableList();

                for (int i = 0; i < lineNum;i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    int variableNum = 0;
                    for(int j = 0;j < variableNumPerLine;j++)
                    {
                        if (j + i * variableNumPerLine >= varList.Count)
                            continue;
                        var var = varList[j + i * variableNumPerLine];
                        var saveColor = GUI.color;
                        if (VarTracer.IsVariableOnShow(var.VarName))
                            GUI.color = Color.green;

                        if (GUILayout.Button(var.VarName, EditorStyles.toolbarDropDown, GUILayout.Width(100)))
                        {
                            try
                            {
                                m_popupWindow.x = 10 + variableNum * 100;
                                m_popupWindow.y = i * variableLineHight;
                                PopupWindow.Show(m_popupWindow, var.PopupWindow);
                            }
                            catch (ExitGUIException)
                            {
                                // have no idea why Unity throws ExitGUIException() in GUIUtility.ExitGUI()
                                // so we silently ignore the exception 
                            }
                        }                      
                        GUI.color = saveColor;
                        variableNum++;
                    }
                    GUILayout.EndHorizontal();
                }
        GUILayout.EndVertical();
    }

    static void DrawGraphGridLines(float y_pos, float width, float height, bool draw_mouse_line)
    {
        GL.Color(new Color(0.3f, 0.3f, 0.3f));
        float steps = 8;
        float x_step = width / steps;
        float y_step = height / steps;
        for (int i = 0; i < steps + 1; ++i)
        {
            Plot(x_offset + x_step * i, y_pos, x_offset + x_step * i, y_pos + height);
            Plot(x_offset, y_pos + y_step * i, x_offset + width, y_pos + y_step * i);
        }

        GL.Color(new Color(0.4f, 0.4f, 0.4f));
        steps = 4;
        x_step = width / steps;
        y_step = height / steps;
        for (int i = 0; i < steps + 1; ++i)
        {
            Plot(x_offset + x_step * i, y_pos, x_offset + x_step * i, y_pos + height);
            Plot(x_offset, y_pos + y_step * i, x_offset + width, y_pos + y_step * i);
        }

        if (draw_mouse_line)
        {
            GL.Color(new Color(0.8f, 0.8f, 0.8f));
            Plot(mMouseX, y_pos, mMouseX, y_pos + height);
        }
    }

    static void Plot(float x0, float y0, float x1, float y1)
    {
        GL.Vertex3(x0, y0, 0);
        GL.Vertex3(x1, y1, 0);
    }

    static void CreateLineMaterial()
    {
        if (!mLineMaterial)
        {
            mLineMaterial = new Material(Shader.Find("Custom/GraphIt"));
            mLineMaterial.hideFlags = HideFlags.HideAndDontSave;
            mLineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    public static void DrawGraphs(Rect rect, EditorWindow window)
    {
        if (VarTracer.Instance)
        {
            InitializeStyles();
            CreateLineMaterial();

            mLineMaterial.SetPass(0);

            int graph_index = 0;

            //use this to get the starting y position for the GL rendering
            Rect find_y = EditorGUILayout.BeginVertical(GUIStyle.none);
            EditorGUILayout.EndVertical();

            int currentFrameIndex = VarTracer.Instance.GetCurrentFrame();

            float scrolled_y_pos = y_offset - mGraphViewScrollPos.y;
            if (Event.current.type == EventType.Repaint)
            {
                GL.PushMatrix();
                float start_y = find_y.y;
                GL.Viewport(new Rect(0, 0, rect.width, rect.height - start_y));
                GL.LoadPixelMatrix(0, rect.width, rect.height - start_y, 0);

                //Draw grey BG
                GL.Begin(GL.QUADS);
                GL.Color(new Color(0.2f, 0.2f, 0.2f));

                foreach (KeyValuePair<string, GraphItData> kv in VarTracer.Instance.Graphs)
                {
                    float height = kv.Value.GetHeight();

                    GL.Vertex3(x_offset,scrolled_y_pos,0);
                    GL.Vertex3(x_offset + mWidth, scrolled_y_pos,0);
                    GL.Vertex3(x_offset + mWidth, scrolled_y_pos + height,0);
                    GL.Vertex3(x_offset,scrolled_y_pos + height,0);

                    scrolled_y_pos += (height + y_gap);
                }
                GL.End();

                scrolled_y_pos = y_offset - mGraphViewScrollPos.y;
                //Draw Lines
                GL.Begin(GL.LINES);

                foreach (KeyValuePair<string, GraphItData> kv in VarTracer.Instance.Graphs)
                {
                    graph_index++;
                    
                    float height = kv.Value.GetHeight();
                    DrawGraphGridLines(scrolled_y_pos, mWidth, height, graph_index == mMouseOverGraphIndex);

                    foreach (KeyValuePair<string, GraphItDataInternal> entry in kv.Value.mData)
                    {
                        GraphItDataInternal g = entry.Value;

                        float y_min = kv.Value.GetMin(entry.Key);
                        float y_max = kv.Value.GetMax(entry.Key);
                        float y_range = Mathf.Max(y_max - y_min, 0.00001f);

                        //draw the 0 line
                        if (y_min !=0.0f)
                        {
                            GL.Color(Color.white);
                            float y = scrolled_y_pos + height * (1 - (0.0f - y_min) / y_range);
                            Plot(x_offset, y, x_offset + mWidth, y);
                        }

                        GL.Color(g.mColor);

                        float previous_value=0,value = 0;
                        int dataInfoIndex=0,frameIndex = 0;
                        for (int i = 0; i <= currentFrameIndex; i++)
                        {
                            int dataCount = g.mDataInfos.Count;
                            if (dataCount != 0)
                            {
                                int lastFrame = g.mDataInfos[dataCount - 1].FrameIndex;
                                float lastValue = g.mDataInfos[dataCount - 1].Value;

                                if (dataInfoIndex >= 1)
                                    value = g.mDataInfos[dataInfoIndex-1].Value;
                                frameIndex = g.mDataInfos[dataInfoIndex].FrameIndex;
                                if (dataInfoIndex == 0 && i < frameIndex)
                                    value = 0;
                                if (i >= frameIndex && dataInfoIndex < dataCount - 1)
                                    dataInfoIndex++;

                                if (i > lastFrame)
                                    value = lastValue;
                            }
                            else {
                                value = 0;
                            }
                             
                            if (i >= 1)
                            {
                                float x0 = x_offset + (i - 1) * kv.Value.XStep - kv.Value.ScrollPos.x;
                                if (x0 <= x_offset - kv.Value.XStep) continue;
                                if (x0 >= mWidth + x_offset) break;
                                float y0 = scrolled_y_pos + height * (1 - (previous_value - y_min) / y_range);

                                if(i==1)
                                {
                                    x0 = x_offset;
                                    y0 = scrolled_y_pos + height;
                                }

                                float x1 = x_offset + i * kv.Value.XStep - kv.Value.ScrollPos.x;
                                float y1 = scrolled_y_pos + height * (1 - (value - y_min) / y_range);

                                Plot(x0, y0, x1, y1);
                            }
                            previous_value = value;
                        }
                    }

                    foreach (var varBodyName in GetAllVariableBodyFromChannel(kv.Key))
                    {
                        var varBody = VarTracer.Instance.VariableBodys[varBodyName];
                        foreach (var eventInfo in varBody.EventInfos.Values)
                        {
                            foreach (var data in eventInfo)
                            {
                                GL.Color(varBody.EventColors[data.m_eventName]);
                                if (data.m_eventFrameIndex > 0)
                                {
                                    float x = x_offset + data.m_eventFrameIndex * kv.Value.XStep - kv.Value.ScrollPos.x;
                                    if (x <= x_offset - kv.Value.XStep) continue;
                                    if (x >= mWidth + x_offset) break;

                                    float y0 = scrolled_y_pos + height;
                                    float y1 = scrolled_y_pos;

                                    Plot(x, y0, x, y1);
                                }
                            }

                        }
                    }

                    scrolled_y_pos += (height + y_gap);
                }                
                GL.End();

                scrolled_y_pos = y_offset - mGraphViewScrollPos.y;
                scrolled_y_pos = ShowEventLabel(scrolled_y_pos);
                GL.PopMatrix();
            
                GL.Viewport(new Rect(0, 0, rect.width, rect.height));
                GL.LoadPixelMatrix(0, rect.width, rect.height, 0);
            }

            mGraphViewScrollPos = EditorGUILayout.BeginScrollView(mGraphViewScrollPos, GUIStyle.none);
            graph_index = 0;

            foreach (KeyValuePair<string, GraphItData> kv in VarTracer.Instance.Graphs)
            {
                graph_index++;

                mWidth = window.position.width - x_offset;
                float height = kv.Value.GetHeight();
                float width = currentFrameIndex * kv.Value.XStep;
                if (width < mWidth)
                {
                    width = mWidth - x_offset;
                }
                else
                {
                    if (!EditorApplication.isPaused)
                        kv.Value.ScrollPos = new Vector2(width - mWidth,kv.Value.ScrollPos.y);
                }

                GUIStyle s = new GUIStyle();                
                s.fixedHeight = height + y_gap;
                s.stretchWidth = true;
                Rect r = EditorGUILayout.BeginVertical(s);

                string num_format = "###,###,##0.";
                for( int i = 0; i < precision_slider; i++ )
                {
                    num_format += "#";
                }

                //skip subgraph title if only one, and it's the same.
                NameLabel.normal.textColor = Color.white;


                r.height = height+50;
                r.width = width;
                r.x = x_offset-80;
                r.y = (height + y_gap) * (graph_index - 1);

                if (kv.Value.mData.Count > 0)
                {
                    GUILayout.BeginArea(r);
                    GUILayout.BeginVertical();
                    EditorGUILayout.LabelField("Max:" + kv.Value.m_maxValue.ToString(num_format), NameLabel);
                    GUILayout.Space(170);
                    //EditorGUILayout.LabelField("Min:" + VarTracer.Instance.GetCurrentFrame().ToString(), NameLabel);
                    EditorGUILayout.LabelField("Min:" + kv.Value.m_minValue.ToString(num_format), NameLabel);
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(70);
                    kv.Value.ScrollPos = GUILayout.BeginScrollView(kv.Value.ScrollPos, GUILayout.Width(mWidth), GUILayout.Height(0));
                    GUILayout.Label("", GUILayout.Width(width), GUILayout.Height(0));
                    GUILayout.EndScrollView();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndArea();
                }

                EditorGUILayout.LabelField( kv.Key, NameLabel);
                
                foreach (var varBodyName in GetAllVariableBodyFromChannel(kv.Key))
                {
                    NameLabel.normal.textColor = Color.white;
                    EditorGUILayout.LabelField("{" + varBodyName + "}:", NameLabel);

                    foreach (var entry in kv.Value.mData)
                    {
                        var variable = VarTracer.GetGraphItVariableByVariableName(entry.Key);
                        GraphItDataInternal g = entry.Value;
                        if (variable.VarBodyName.Equals(varBodyName))
                        {
                            if (kv.Value.mData.Count >= 1)
                            {
                                NameLabel.normal.textColor = g.mColor;
                            }
                            EditorGUILayout.LabelField("     [" + entry.Key + "]" + "   Value: " + g.mCurrentValue.ToString(num_format), NameLabel);
                        }
                    }

                    var varBody = VarTracer.Instance.VariableBodys[varBodyName];

                    foreach (var eventName in varBody.EventInfos.Keys)
                    {
                        NameLabel.normal.textColor = varBody.EventColors[eventName];
                        EditorGUILayout.LabelField("     <Event>    " + eventName, NameLabel);
                    }

                }

                if (kv.Value.mData.Count >= 1)
                {
                    GUILayout.BeginHorizontal();
                    HoverText.normal.textColor = Color.white;
                    EditorGUILayout.LabelField("STEP SCALE:", HoverText, GUILayout.Width(90));
                    kv.Value.XStep = GUILayout.HorizontalSlider(kv.Value.XStep, 0.1f, 10, GUILayout.Width(100));
                    GUILayout.EndHorizontal();
                }

                ////Respond to mouse input!
                if (Event.current.type == EventType.MouseDrag && r.Contains(Event.current.mousePosition - Event.current.delta))
                {
                    if (Event.current.button == 0)
                    {
                        kv.Value.ScrollPos = new Vector2(kv.Value.ScrollPos.x + Event.current.delta.x, kv.Value.ScrollPos.y);
                    }
                    window.Repaint();
                }

                if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition - Event.current.delta))
                {
                    if (Event.current.button == 1)
                    {
                        //EditorApplication.isPaused = !EditorApplication.isPaused;
                    }
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    static List<string> GetAllVariableBodyFromChannel(string channelName)
    {
        List<string> result = new List<string>();
        foreach (var varBody in VarTracer.Instance.VariableBodys)
        {
            foreach (var var in varBody.Value.VariableDict.Values)
            {
                foreach (var channel in var.ChannelDict.Values)
                {
                    if (channelName.Equals(channel))
                    {
                        if (!result.Contains(var.VarBodyName))
                            result.Add(var.VarBodyName);
                    }
                }
            }
        }
        return result;
    }


    private static float ShowEventLabel(float scrolled_y_pos)
    {
        foreach (KeyValuePair<string, GraphItData> kv in VarTracer.Instance.Graphs)
        {
            float height = kv.Value.GetHeight();
            foreach (var varBodyName in GetAllVariableBodyFromChannel(kv.Key))
            {
                var varBody = VarTracer.Instance.VariableBodys[varBodyName];
                foreach (var eventInfo in varBody.EventInfos.Values)
                {
                    foreach (var data in eventInfo)
                    {
                        GL.Color(varBody.EventColors[data.m_eventName]);
                        if (data.m_eventFrameIndex > 0)
                        {
                            float x = x_offset + data.m_eventFrameIndex * kv.Value.XStep - kv.Value.ScrollPos.x;
                            if (x <= x_offset - kv.Value.XStep) continue;
                            if (x >= mWidth + x_offset) break;
                            float y0 = scrolled_y_pos + height;

                            GUIStyle style=null;
                            int buttonWidth = 0;
                            if(data.m_duration == 0)
                            {
                                style = EventInstantButtonStyle;
                                buttonWidth = 70;
                            }
                            else
                            {
                                style = EventDurationButtonStyle;
                                buttonWidth =(int)(data.m_duration * VarTracerConst.FPSPerSecond );
                            }

                            Rect tooltip_r = new Rect(x - buttonWidth/2, y0 - VarTracerConst.EventStartHigh , buttonWidth, VarTracerConst.EventButtonHeight);
                            style.normal.textColor = varBody.EventColors[data.m_eventName];
                            GUI.Button(tooltip_r, data.m_eventName, style);
                        }
                    }
                }
            }
            scrolled_y_pos += (height + y_gap);
        }

        return scrolled_y_pos;
    }
}