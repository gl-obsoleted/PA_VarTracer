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
    static float y_gap = 40.0f;
    static float y_offset = 20;
    static int precision_slider = 3;


    static GUIStyle NameLabel;
    static GUIStyle SmallLabel;
    static GUIStyle HoverText;
    static GUIStyle FracGS;

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
        }
    }

    [MenuItem("Window/PerfAssist" + "/GraphItVariable")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GraphItWindow window = (GraphItWindow)EditorWindow.GetWindow(typeof(GraphItWindow), false,"GraphItVariable");
        window.minSize = new Vector2(230f, 50f);
        window.Show();
    }

    void OnEnable()
    {
        EditorApplication.update += MyDelegate;
        if (VarTracer.Instance != null)
        {
            if(VarTracer.Instance.Graphs.Count==0)
                VarTracer.AddChannel();

            bool constainsCamera = VarTracer.Instance.VariableBodys.ContainsKey("Camera");
            if (!constainsCamera || VarTracer.Instance.VariableBodys["Camera"].VariableDict.Count==0)
            {
                VarTracerTool.DefineVariable("CameraV_X", "Camera", Color.green);
                VarTracerTool.DefineVariable("CameraV_Y", "Camera", Color.cyan);
                VarTracerTool.DefineVariable("CameraV_Z", "Camera", Color.yellow);
                VarTracerTool.DefineVariable("CameraV_T", "Camera", Color.magenta);

                VarTracerTool.DefineVariable("NpcV_X", "Npc", Color.green);
                VarTracerTool.DefineVariable("NpcV_Y", "Npc", Color.cyan);
                VarTracerTool.DefineVariable("NpcV_Z", "Npc", Color.yellow);
                VarTracerTool.DefineVariable("NpcV_T", "Npc", Color.magenta);

                VarTracerTool.DefineVariable("PlayerV_X", "Player", Color.green);
                VarTracerTool.DefineVariable("PlayerV_Y", "Player", Color.cyan);
                VarTracerTool.DefineVariable("PlayerV_Z", "Player", Color.yellow);
                VarTracerTool.DefineVariable("PlayerV_T", "Player", Color.magenta);
            }

        }
    }

    void OnDisable()
    {
        EditorApplication.update -= MyDelegate;
        VarTracer.Instance.Graphs.Clear();
        VarTracer.Instance.VariableBodys.Clear();
    }

    void MyDelegate()
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

            GUILayout.Space(800);

            string buttonName;
            if (EditorApplication.isPaused)
                buttonName = "Resume";
            else
                buttonName = "Pause";
            if (GUILayout.Button(buttonName, EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                EditorApplication.isPaused = !EditorApplication.isPaused;
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
                            GUI.color = var.Color;

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

            float scrolled_y_pos = y_offset;
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

                scrolled_y_pos = y_offset;
                //Draw Lines
                GL.Begin(GL.LINES);
                foreach (KeyValuePair<string, GraphItData> kv in VarTracer.Instance.Graphs)
                {
                    graph_index++;

                    float height = kv.Value.GetHeight();
                    DrawGraphGridLines(scrolled_y_pos, mWidth, height, graph_index == mMouseOverGraphIndex);

                    foreach (var eventData in kv.Value.mEventData)
                    {
                        GL.Color(Color.white);
                        if(eventData.m_eventFrameIndex>0)
                        {
                            float x = x_offset + (eventData.m_eventFrameIndex + 1) * kv.Value.XStep - kv.Value.ScrollPos.x;
                            if (x <= x_offset - kv.Value.XStep) continue;
                            if (x >= mWidth + x_offset) break;

                            float y0 = scrolled_y_pos + height;
                            float y1 = scrolled_y_pos;

                            Plot(x, y0, x, y1);
                        }
                    }

                    if (kv.Value.GraphLength() > 0)
                    {
                        foreach (KeyValuePair<string, GraphItDataInternal> entry in kv.Value.mData)
                        {
                            GraphItDataInternal g = entry.Value;

                            float y_min = kv.Value.GetMin(entry.Key);
                            float y_max = kv.Value.GetMax(entry.Key);
                            float y_range = Mathf.Max(y_max - y_min, 0.00001f);

                            //draw the 0 line
                            if (y_max > 0.0f && y_min < 0.0f)
                            {
                                GL.Color(g.mColor * 0.5f);
                                float y = scrolled_y_pos + height * (1 - (0.0f - y_min) / y_range);
                                Plot(x_offset, y, x_offset + mWidth, y);
                            }

                            GL.Color(g.mColor);

                            float previous_value = 0;
                            for (int i = 0; i < kv.Value.GraphLength(); ++i)
                            {
                                float value = 0;
                                if (i >= 1)
                                {
                                    float x0 = x_offset + (i - 1) * kv.Value.XStep - kv.Value.ScrollPos.x;
                                    if (x0 <= x_offset - kv.Value.XStep) continue;
                                    if (x0 >= mWidth + x_offset) break;
                                    value = g.mDataInfos[i].Value;
                                    float y0 = scrolled_y_pos + height * (1 - (previous_value - y_min) / y_range);

                                    float x1 = x_offset + i * kv.Value.XStep - kv.Value.ScrollPos.x;
                                    float y1 = scrolled_y_pos + height * (1 - (value - y_min) / y_range);

                                    Plot(x0, y0, x1, y1);
                                }
                                previous_value = value;
                            }
                        }
                    }

                    scrolled_y_pos += (height + y_gap);
                }                
                GL.End();


                scrolled_y_pos = y_offset;
                foreach (KeyValuePair<string, GraphItData> kv in VarTracer.Instance.Graphs)
                {
                    float height = kv.Value.GetHeight();
                    foreach (var eventData in kv.Value.mEventData)
                    {
                        if (kv.Value.mTotalIndex > 0)
                        {
                            float x = x_offset + (eventData.m_eventFrameIndex + 1) * kv.Value.XStep - kv.Value.ScrollPos.x;
                            if (x <= x_offset - kv.Value.XStep) continue;
                            if (x >= mWidth + x_offset) break;
                            float y0 = scrolled_y_pos + height;

                            Rect tooltip_r = new Rect(x-75, y0, 100, 20);
                            HoverText.normal.textColor = Color.white;
                            GUI.Label(tooltip_r, eventData.m_eventName, HoverText);
                        }
                    }
                    scrolled_y_pos += (height + y_gap);
                }

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
                float width = kv.Value.GraphLength() * kv.Value.XStep;
                if (width < mWidth)
                {
                    width = mWidth - x_offset;
                }
                else
                {
                    if (!EditorApplication.isPaused)
                        kv.Value.ScrollPos = new Vector2(width - mWidth,kv.Value.ScrollPos.y);
                        //kv.Value.ScrollPos.Set(kv.Value.ScrollPos.x + (width - mWidth), kv.Value.ScrollPos.y);
                }

                GUIStyle s = new GUIStyle();                
                s.fixedHeight = height + y_gap;
                s.stretchWidth = true;
                Rect r = EditorGUILayout.BeginVertical(s);
                r.height = height;
                //if (r.width !=0)
                //{
                //    mWidth = r.width - x_offset;
                //}

                string num_format = "###,###,###,##0.";
                for( int i = 0; i < precision_slider; i++ )
                {
                    num_format += "#";
                }

                //skip subgraph title if only one, and it's the same.
                NameLabel.normal.textColor = Color.white;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kv.Key, NameLabel, GUILayout.Width(x_offset-80));
                    if(kv.Value.mData.Count>0)
                        EditorGUILayout.LabelField("Max:" + kv.Value.m_maxValue, NameLabel);
                EditorGUILayout.EndHorizontal();

                foreach (KeyValuePair<string, GraphItDataInternal> entry in kv.Value.mData)
                {
                    GraphItDataInternal g = entry.Value;
                    if (kv.Value.mData.Count > 1 || entry.Key != VarTracer.BASE_GRAPH)
                    {
                        NameLabel.normal.textColor = g.mColor;
                    }
                    //EditorGUILayout.LabelField(entry.Key +"   Avg: " + g.mAvg.ToString(num_format) + " (" + g.mFastAvg.ToString(num_format) + "),Min: " + g.mMin.ToString(num_format) + ",Max: " + g.mMax.ToString(num_format), NameLabel);
                    EditorGUILayout.LabelField(entry.Key + "   Value: " + g.mCurrentValue.ToString(num_format), NameLabel);
                }

                GUILayout.BeginHorizontal();
                HoverText.normal.textColor = Color.grey;
                GUILayout.Label("STEP SCALE:", HoverText, GUILayout.Width(90));
                kv.Value.XStep = GUILayout.HorizontalSlider(kv.Value.XStep, 0.1f, 10, GUILayout.Width(100));
                GUILayout.EndHorizontal();

                ////Respond to mouse input!
                if (Event.current.type == EventType.MouseDrag && r.Contains(Event.current.mousePosition - Event.current.delta))
                {
                    if (Event.current.button == 0)
                    {
                        //kv.Value.DoHeightDelta(Event.current.delta.y);
                        kv.Value.ScrollPos.Set(kv.Value.ScrollPos.x + Event.current.delta.x, kv.Value.ScrollPos.y);
                    }
                    window.Repaint();
                }

                EditorGUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                    GUILayout.Space(x_offset);
                    kv.Value.ScrollPos = GUILayout.BeginScrollView(kv.Value.ScrollPos, GUILayout.Width(mWidth), GUILayout.Height(0));
                    GUILayout.Label("", GUILayout.Width(width), GUILayout.Height(0));
                    GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
                //mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos,GUILayout.Width(mWidth), GUILayout.Height(0));
                //GUILayout.Label("", GUILayout.Width(width), GUILayout.Height(0));
                //EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}