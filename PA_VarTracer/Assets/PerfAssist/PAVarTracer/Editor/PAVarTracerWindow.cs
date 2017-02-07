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


         void Update()
         {  
             //UpdateVisualVarByGroup("Camera Rate", new List<string> { "X", "Y", "Z", "Total" }, new List<float>{ 0.3f +UnityEngine.Random.value * 0.1f
             //    ,0.5f +UnityEngine.Random.value * 0.1f,0.7f+UnityEngine.Random.value * 0.1f,0.9f+UnityEngine.Random.value * 0.1f});
             //UpdateVisualVarByGroup("Player Rate", new List<string> { "X", "Y", "Z", "Total" }, new List<float>{0.3f+UnityEngine.Random.value * 0.1f
             //    ,0.5f+UnityEngine.Random.value * 0.1f,0.7f +UnityEngine.Random.value * 0.1f,0.9f+UnityEngine.Random.value * 0.1f});
             //UpdateVisualVarByGroup("NPC Rate", new List<string> { "X", "Y", "Z", "Total" }, new List<float>{NpcObj.transform.position.x
             //    ,NpcObj.transform.position.y,NpcObj.transform.position.z,0.9f+UnityEngine.Random.value * 0.1f});
             //UpdateVisualVarByGroup("Test1 Rate", new List<string> { "X", "Y", "Z", "Total" }, new List<float>{0.3f+UnityEngine.Random.value * 0.1f
             //    ,0.5f+UnityEngine.Random.value * 0.1f,0.7f+UnityEngine.Random.value * 0.1f,0.9f+UnityEngine.Random.value * 0.1f});
             //UpdateVisualVarByGroup("Test2 Rate", new List<string> { "X", "Y", "Z", "Total" }, new List<float>{0.3f+UnityEngine.Random.value * 0.1f
             //    ,0.5f+UnityEngine.Random.value * 0.1f,0.7f+UnityEngine.Random.value * 0.1f,0.9f+UnityEngine.Random.value * 0.1f});
             Repaint();
         }

         void OnDestroy() {
         
         }

         public PAVarTracerWindow()
         {
         }

         public void OnGUI()
         {
             CheckForInput();
             CheckForResizing();

             Handles.BeginGUI();
             Handles.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, 1));
             //control窗口内容
             GUILayout.BeginArea(new Rect(0, m_controlScreenPosY, m_winWidth, m_controlScreenHeight));
             {
                 drawGUIElement();
             }
             GUILayout.EndArea();
             ////navigation窗口内容
             GUILayout.BeginArea(new Rect(0, m_navigationScreenPosY, m_winWidth, m_winHeight));
             {
                 GraphItWindow.DrawGraphs(this.position, this);
             }
             GUILayout.EndArea();
             Handles.EndGUI();
         }

         private void drawGUIElement()
         {
             GUILayout.BeginHorizontal();
             {
                 //select = GUI.Toolbar(Rect(10, 10, barResource.length * 80, 30), select, barResource);  

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

             //DefineVisualChannel("Camera Rate", m_navigationScreenHeight / 2, true, true);
             //DefineVisualChannel("Player Rate", m_navigationScreenHeight / 2, true, true);
             //DefineVisualChannel("NPC Rate", m_navigationScreenHeight / 2, true, false);
             //DefineVisualChannel("Test1 Rate", m_navigationScreenHeight / 2, true, true);
             //DefineVisualChannel("Test2 Rate", m_navigationScreenHeight / 2, true, true);

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
                     //SendVisualEvent("NPC Rate", "move");   
                     break;
                 case EventType.mouseMove:
                     {
                     }
                     break;
                 case EventType.MouseDrag:
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
 