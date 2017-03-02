using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

[System.Serializable]
public class VarTracerJsonType
{
    public string logicName;
    public string[] variableName;
    public float[] variableValue;
    public string[] eventName;
    public float[] eventDuration;
    public string[] eventDesc;
    public int runingState;
    public long timeStamp;
}
