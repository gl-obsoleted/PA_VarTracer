using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
     public class PAVarTracerWindow : EditorWindow
     {
         public enum MouseInArea
         {
             none,
             ControlScreen,
             NavigationScreen ,
             DetailScreen ,
         };

         [SerializeField]
         internal Vector2 m_Scale = new Vector2(1.0f, 1.0f);
         [SerializeField]
         internal Vector2 m_Translation = new Vector2(0, 0);

         public static float m_winWidth = 0.0f;
         public static float m_winHeight = 0.0f;

         public static float m_controlScreenHeight = 0.0f;
         public static float m_controlScreenPosY = 0.0f;

         public static float m_navigationScreenHeight = 0.0f;
         public static float m_navigationScreenPosY = 0.0f;

         public static float m_detailScreenHeight = 0.0f;
         public static float m_detailScreenPosY = 0.0f;

         public bool m_isTestCallLua = false;

         private static PAVarTracerWindow m_window;
         public PAVarTracerWindow Window
         {
             get { return m_window; }
             set { m_window = value; }
         }

         [MenuItem(PAEditorConst.MenuPath + "/PAVarTracerWindow")]
         static void Create()
         {
             //// Get existing open window or if none, make a new one:
                 m_window = (PAVarTracerWindow)EditorWindow.GetWindow(typeof(PAVarTracerWindow));
                 m_window.Show();
                 m_window.wantsMouseMove = true;
                 m_window.CheckForResizing();
         }


         public static void DefineVisualVar(string channel,string varName)
         {

         }

         public static void UpdateVisualVar(string channel, string varName,float value)
         {

         }

         public static void SendVisualEvent(string channel,string EventFlag)
         {

         }


         void OnEnable()
         {
             EditorApplication.update += MyDelegate;
             GraphIt.GraphSetupColour("TestData", "X", Color.red);
             GraphIt.GraphSetupColour("TestData", "Y", Color.green);
             GraphIt.GraphSetupColour("TestData", "Z", Color.blue);

             GraphIt.GraphSetupColour("TestData2", "X", Color.red);
             GraphIt.GraphSetupColour("TestData2", "Y", Color.green);
             GraphIt.GraphSetupColour("TestData2", "Z", Color.blue);
         }

         void OnDisable()
         {
             EditorApplication.update -= MyDelegate;
         }

         void MyDelegate()
         {
             Repaint();
         }

         void Update()
         {
             GraphIt.Log("TestData", "X", UnityEngine.Random.value * 5.0f);
             GraphIt.Log("TestData", "Y", UnityEngine.Random.value * 5.0f + 10f);
             GraphIt.Log("TestData", "Z", UnityEngine.Random.value * 5.0f + 20f);

             GraphIt.StepGraph("TestData");

             GraphIt.Log("TestData2", "X", UnityEngine.Random.value * 5.0f);
             GraphIt.Log("TestData2", "Y", UnityEngine.Random.value * 5.0f + 10f);
             GraphIt.Log("TestData2", "Z", UnityEngine.Random.value * 5.0f + 20f);

             GraphIt.StepGraph("TestData2");

             //var dataInfoMap = result.NavigateResult;
                //GraphItLuaPro.Log(HanoiData.GRAPH_TIMECONSUMING, HanoiData.SUBGRAPH_LUA_TIMECONSUMING_INCLUSIVE, dataInfoMap[HanoiData.SUBGRAPH_LUA_TIMECONSUMING_INCLUSIVE]);
                //GraphItLuaPro.Log(HanoiData.GRAPH_TIMECONSUMING, HanoiData.SUBGRAPH_LUA_TIMECONSUMING_EXCLUSIVE, dataInfoMap[HanoiData.SUBGRAPH_LUA_TIMECONSUMING_EXCLUSIVE]);
                //GraphItLuaPro.Log(HanoiData.GRAPH_TIME_PERCENT, HanoiData.SUBGRAPH_LUA_PERCENT_INCLUSIVE, dataInfoMap[HanoiData.SUBGRAPH_LUA_PERCENT_INCLUSIVE]);
                //GraphItLuaPro.Log(HanoiData.GRAPH_TIME_PERCENT, HanoiData.SUBGRAPH_LUA_PERCENT_EXCLUSIVE, dataInfoMap[HanoiData.SUBGRAPH_LUA_PERCENT_EXCLUSIVE]);
             Repaint();
         }

         void OnDestroy() {
         
         }

         public PAVarTracerWindow()
         {
         }

         public void OnGUI()
         {
             GraphItWindow.DrawGraphs(this.position, this);
             CheckForResizing();
             Handles.BeginGUI();
             Handles.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));
             //control窗口内容
             GUILayout.BeginArea(new Rect(0, m_controlScreenPosY, m_winWidth, m_controlScreenHeight));
             {
                 drawGUIElement();
             }
             GUILayout.EndArea();

             //navigation窗口内容
             GUILayout.BeginArea(new Rect(0, m_navigationScreenPosY, m_winWidth, m_navigationScreenHeight));
             {
                 //if (m_data.isHanoiDataHasContent())
             }
             GUILayout.EndArea();

             if (EditorWindow.focusedWindow == this)
             {
                 CheckForInput();
             }
             Handles.EndGUI();
         }

         private void ClearHanoiRoot() {
             //GraphItLuaPro.Clear();
             //GraphItWindowLuaPro.MouseXOnPause = -1;
         }

         private void drawGUIElement()
         {
             GUILayout.BeginHorizontal();
             {
                 if (GUILayout.Button("Loadfile", GUILayout.Width(70)))
                 {

                 }

                 if (GUILayout.Button("Open Dir", GUILayout.MaxWidth(80)))
                 {

                 }
                  //GraphItWindowLuaPro._TimeLimitSelectIndex = GUI.SelectionGrid(new Rect(600, 0, 350, 20), GraphItWindowLuaPro._TimeLimitSelectIndex, GraphItWindowLuaPro._TimeLimitStrOption, GraphItWindowLuaPro._TimeLimitStrOption.Length);
                  //GraphItWindowLuaPro._PercentLimitSelectIndex = GUI.SelectionGrid(new Rect(1000, 0, 300, 20), GraphItWindowLuaPro._PercentLimitSelectIndex, GraphItWindowLuaPro._PercentLimitStrOption, GraphItWindowLuaPro._PercentLimitStrOption.Length);
             }
             GUILayout.EndHorizontal();
         }


         public void CheckForResizing()
         {
             if (Mathf.Approximately(position.width, m_winWidth) && 
                 Mathf.Approximately(position.height, m_winHeight))
                 return; 

             m_winWidth = position.width;
             m_winHeight = position.height;

             m_detailScreenHeight = m_winHeight / 1.65f;
             m_detailScreenPosY = m_winHeight / 2.5f;

             m_navigationScreenHeight = (m_winHeight - m_detailScreenHeight)/1.1f;
             m_navigationScreenPosY = m_detailScreenPosY/10.0f;

             m_controlScreenHeight = m_winHeight - m_detailScreenHeight - m_navigationScreenHeight;
             m_controlScreenPosY = 0.0f;


             GraphIt.GraphSetupHeight("TestData", m_navigationScreenHeight / 2);
             GraphIt.GraphSetupHeight("TestData2", m_navigationScreenHeight / 2);
             //GraphItLuaPro.GraphSetupHeight(HanoiData.GRAPH_TIME_PERCENT, m_navigationScreenHeight / 2);
             GraphIt.ShareYAxis("TestData", true);
             GraphIt.ShareYAxis("TestData2",true);
             //GraphItLuaPro.ShareYAxis(HanoiData.GRAPH_TIME_PERCENT, true);

             //GraphItLuaPro.GraphSetupHeight(HanoiData.GRAPH_TIMECONSUMING, m_navigationScreenHeight / 2 - GraphItWindowLuaPro.y_gap);
             //GraphItLuaPro.GraphSetupHeight(HanoiData.GRAPH_TIME_PERCENT, m_navigationScreenHeight / 2 - GraphItWindowLuaPro.y_gap);
             //GraphItLuaPro.ShareYAxis(HanoiData.GRAPH_TIMECONSUMING,true);
             //GraphItLuaPro.ShareYAxis(HanoiData.GRAPH_TIME_PERCENT,true);
         }

         public Vector2 ViewToDrawingTransformPoint(Vector2 lhs)
         {return new Vector2((lhs.x - m_Translation.x) / m_Scale.x, (lhs.y - m_Translation.y) / m_Scale.y);}
         public Vector3 ViewToDrawingTransformPoint(Vector3 lhs)
         { return new Vector3((lhs.x - m_Translation.x) / m_Scale.x, (lhs.y - m_Translation.y) / m_Scale.y,0); }

         public Vector2 DrawingToViewTransformVector(Vector2 lhs)
         { return new Vector2(lhs.x * m_Scale.x, lhs.y * m_Scale.y); }
         public Vector3 DrawingToViewTransformVector(Vector3 lhs)
         { return new Vector3(lhs.x * m_Scale.x, lhs.y * m_Scale.y, 0); }

         public Vector2 ViewToDrawingTransformVector(Vector2 lhs)
         { return new Vector2(lhs.x / m_Scale.x, lhs.y / m_Scale.y); }
         public Vector3 ViewToDrawingTransformVector(Vector3 lhs)
         { return new Vector3(lhs.x / m_Scale.x, lhs.y / m_Scale.y, 0); }

         public float ViewToDrawingTransformValue(float x)
         { return (x - m_Translation.x) / m_Scale.x;}

         public Vector2 mousePositionInDrawing
         {
             get { return ViewToDrawingTransformPoint(Event.current.mousePosition);}
         }

         public Vector2 mousePositionInDetailScreen
         {
             get { return mousePositionInDrawing - new Vector2(0, m_detailScreenPosY); }
         }

         public float GetDrawingLengthByPanelPixels(int pixels)
         {
             return Mathf.Abs(ViewToDrawingTransformPoint(new Vector2(pixels, 0)).x - ViewToDrawingTransformPoint(new Vector2(0, 0)).x); 
         }

         private void CheckForInput()
         {
             switch (Event.current.type)
             {
                 case EventType.KeyUp:
                     m_isTestCallLua = !m_isTestCallLua;
                     EditorApplication.isPaused = false;
                     break;
                 case EventType.mouseMove:
                     {
                     }
                     break;
                 case EventType.MouseDrag:
                     if (Event.current.button == 1)
                     {
                     }
                     break;
                 case EventType.ScrollWheel:
                     {
                     }
                     break;
                 default:
                     break;
             }
         }
     }
 