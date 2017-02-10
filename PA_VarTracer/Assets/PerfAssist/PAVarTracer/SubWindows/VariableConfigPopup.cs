using UnityEngine;
using System.Collections;
using UnityEditor;

public class VariableConfigPopup : PopupWindowContent
{
    bool m_isOff=false;
    string m_variableName;
    public bool IsOff
    {
      get { return m_isOff; }
      set { m_isOff = value; }
    }


    public VariableConfigPopup(string variableName)
    {
        m_variableName = variableName;
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(100, 40 + 20 * VarTracer.Instance.Graphs.Count);
    }

    public override void OnGUI(Rect rect)
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Space(5);

        m_isOff =GUILayout.Toggle(m_isOff,"off");
        GUILayout.Space(15);        
        if(m_isOff)
        {
            foreach (var graphName in VarTracer.Instance.Graphs.Keys)
            {
                var graphItVar = VarTracer.GetGraphItVariableByVariableName(m_variableName);
                if (graphItVar != null)
                {
                    if (graphItVar.ChannelDict.ContainsKey(graphName))
                        graphItVar.DetachChannel(graphName);
                }
            }
        }

        foreach (var graphName in VarTracer.Instance.Graphs.Keys)
        {
            var graphItVar = VarTracer.GetGraphItVariableByVariableName(m_variableName);
            if (graphItVar != null)
            {
                if (GUILayout.Toggle(graphItVar.ChannelDict.ContainsKey(graphName),"graph "+graphName))
                    graphItVar.AttchChannel(graphName);
                else
                    graphItVar.DetachChannel(graphName);
            }
        }

        EditorGUILayout.EndVertical();
    }

}

