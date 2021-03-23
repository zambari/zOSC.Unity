using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSCConfig : SavableJson<OSCConfig>
{
    public override string fileName => "oscConfig.json";
    public string targetIP="127.0.0.1";
    public int targetPort=8899;
}
