using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GraphItWindow : EditorWindow
{
    const int InValidNum = -1;
    static Vector2 mScrollPos;
    static float mWidth;

    static int mMouseOverGraphIndex = InValidNum;
    static float mMouseX = 0;

    static float x_offset = 150.0f;
    static float y_gap = 40.0f;
    static float y_offset = 55;
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
        GraphItWindow window = (GraphItWindow)EditorWindow.GetWindow(typeof(GraphItWindow), false, "GraphItVariable ");
        window.minSize = new Vector2(230f, 50f);
        window.Show();
    }

    void OnEnable()
    {
        EditorApplication.update += MyDelegate;
        if (GraphItVar.Instance != null && GraphItVar.Instance.Graphs.Count==0)
            GraphItVar.AddChannel();
    }

    void OnDisable()
    {
        EditorApplication.update -= MyDelegate;
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

        m_controlScreenHeight  = m_winHeight / 17;
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


    void DrawVariableBar()
    {
        GUILayout.BeginHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("addGraph", EditorStyles.toolbarButton, GUILayout.Width(100)))
                GraphItVar.AddChannel();

            GUILayout.Space(5);

            if (GUILayout.Button("removeGraph", EditorStyles.toolbarButton, GUILayout.Width(100)))
                GraphItVar.RemoveChannel();

                GUILayout.Space(20);
                
                int variableNum =0;      
                foreach (var varBody in GraphItVar.Instance.VariableBodys.Values)
                {
                    foreach(var var in varBody.VariableDict.Values)
                    {
                        var saveColor = GUI.color;
                        if(GraphItVar.IsVariableOnShow(var.VarName))
                            GUI.color = var.Color;

                        if (GUILayout.Button(var.VarName, EditorStyles.toolbarDropDown,GUILayout.Width(100)))
                        {
                            try
                            {
                                m_popupWindow.x = 235 + variableNum * 100;
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
                }
        GUILayout.EndHorizontal();
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
        if (GraphItVar.Instance)
        {
            InitializeStyles();
            CreateLineMaterial();

            mLineMaterial.SetPass(0);

            int graph_index = 0;

            //use this to get the starting y position for the GL rendering
            Rect find_y = EditorGUILayout.BeginVertical(GUIStyle.none);
            EditorGUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint)
            {
                GL.PushMatrix();
                float start_y = find_y.y;
                GL.Viewport(new Rect(0, 0, rect.width, rect.height - start_y));
                GL.LoadPixelMatrix(0, rect.width, rect.height - start_y, 0);

                //Draw grey BG
                GL.Begin(GL.QUADS);
                GL.Color(new Color(0.2f, 0.2f, 0.2f));

                float scrolled_y_pos = y_offset - mScrollPos.y;
                foreach (KeyValuePair<string, GraphItData> kv in GraphItVar.Instance.Graphs)
                {
                    float height = kv.Value.GetHeight();

                    GL.Vertex3(x_offset,scrolled_y_pos,0);
                    GL.Vertex3(x_offset + mWidth, scrolled_y_pos,0);
                    GL.Vertex3(x_offset + mWidth, scrolled_y_pos + height,0);
                    GL.Vertex3(x_offset,scrolled_y_pos + height,0);

                    scrolled_y_pos += (height + y_gap);
                }
                GL.End();

                //Draw Lines
                GL.Begin(GL.LINES);
                scrolled_y_pos = y_offset - mScrollPos.y;

                foreach (KeyValuePair<string, GraphItData> kv in GraphItVar.Instance.Graphs)
                {
                    graph_index++;

                    float x_step = mWidth / kv.Value.GraphFullLength();

                    float height = kv.Value.GetHeight();
                    DrawGraphGridLines(scrolled_y_pos, mWidth, height, graph_index == mMouseOverGraphIndex);

                    foreach (var eventData in kv.Value.mEventData)
                    {
                        GL.Color(Color.white);
                        int frameIndex = 0;

                        if (kv.Value.mTotalIndex <= kv.Value.GraphFullLength())
                        {
                            frameIndex = eventData.m_eventFrameIndex;
                        }else 
                        if(kv.Value.mTotalIndex - eventData.m_eventFrameIndex <= kv.Value.GraphFullLength())
                            frameIndex = kv.Value.GraphFullLength()-(kv.Value.mTotalIndex - eventData.m_eventFrameIndex);
                        
                        if(frameIndex>0)
                        {
                            float x = x_offset + frameIndex * x_step;
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
                            int start_index = (kv.Value.mCurrentIndex) % kv.Value.GraphLength();
                            for (int i = 0; i < kv.Value.GraphLength(); ++i)
                            {
                                float value = g.mDataPoints[start_index];
                                if (i >= 1)
                                {
                                    float x0 = x_offset + (i - 1) * x_step;
                                    float y0 = scrolled_y_pos + height * (1 - (previous_value - y_min) / y_range);

                                    float x1 = x_offset + i * x_step;
                                    float y1 = scrolled_y_pos + height * (1 - (value - y_min) / y_range);

                                    Plot(x0, y0, x1, y1);
                                }
                                previous_value = value;
                                start_index = (start_index + 1) % kv.Value.GraphFullLength();
                            }
                        }
                    }

                    scrolled_y_pos += (height + y_gap);
                }                
                GL.End();


                scrolled_y_pos = y_offset - mScrollPos.y;
                foreach (KeyValuePair<string, GraphItData> kv in GraphItVar.Instance.Graphs)
                {
                    float x_step = mWidth / kv.Value.GraphFullLength();

                    float height = kv.Value.GetHeight();
                    foreach (var eventData in kv.Value.mEventData)
                    {
                        int frameIndex = 0;

                        if (kv.Value.mTotalIndex <= kv.Value.GraphFullLength())
                        {
                            frameIndex = eventData.m_eventFrameIndex;
                        }
                        else
                            if (kv.Value.mTotalIndex - eventData.m_eventFrameIndex <= kv.Value.GraphFullLength())
                                frameIndex = kv.Value.GraphFullLength() - (kv.Value.mTotalIndex - eventData.m_eventFrameIndex);

                        if (frameIndex > 0)
                        {
                            float x = x_offset + frameIndex * x_step;
                            float y0 = scrolled_y_pos + height;

                            Rect tooltip_r = new Rect(x-60, y0, 80, 20);
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

            mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos, GUIStyle.none);
            graph_index = 0;
            if (Event.current.type == EventType.Repaint)
            {
                mMouseOverGraphIndex = -1; //clear it out every repaint to ensure when the mouse leaves we don't leave the pointer around
            }
            foreach (KeyValuePair<string, GraphItData> kv in GraphItVar.Instance.Graphs)
            {
                graph_index++;
                
                float height = kv.Value.GetHeight();

                GUIStyle s = new GUIStyle();                
                s.fixedHeight = height + y_gap;
                s.stretchWidth = true;
                Rect r = EditorGUILayout.BeginVertical(s);
                if (r.width != -0)
                {
                    mWidth = r.width - x_offset;
                }

                string num_format = "###,###,###,##0.";
                for( int i = 0; i < precision_slider; i++ )
                {
                    num_format += "#";
                }

                string fu_str = " " + (kv.Value.mFixedUpdate ? "(FixedUpdate)" : "");

                //skip subgraph title if only one, and it's the same.
                NameLabel.normal.textColor = Color.white;

                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(kv.Key + fu_str, NameLabel,GUILayout.Width(80));
                    if(kv.Value.mData.Count>0)
                        EditorGUILayout.LabelField("Max:" + kv.Value.m_maxValue, NameLabel);
                EditorGUILayout.EndHorizontal();

                foreach (KeyValuePair<string, GraphItDataInternal> entry in kv.Value.mData)
                {
                    GraphItDataInternal g = entry.Value;
                    if (kv.Value.mData.Count > 1 || entry.Key != GraphItVar.BASE_GRAPH)
                    {
                        NameLabel.normal.textColor = g.mColor;
                    }
                    //EditorGUILayout.LabelField(entry.Key +"   Avg: " + g.mAvg.ToString(num_format) + " (" + g.mFastAvg.ToString(num_format) + "),Min: " + g.mMin.ToString(num_format) + ",Max: " + g.mMax.ToString(num_format), NameLabel);
                    EditorGUILayout.LabelField(entry.Key + "   Value: " + g.mCurrentValue.ToString(num_format), NameLabel);
                }
                
                ////Respond to mouse input!
                if (Event.current.type == EventType.MouseDrag && r.Contains(Event.current.mousePosition - Event.current.delta))
                {
                    if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
                    {
                        kv.Value.DoHeightDelta(Event.current.delta.y);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}