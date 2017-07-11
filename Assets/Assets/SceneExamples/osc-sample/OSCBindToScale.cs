///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCBindToScale : MonoBehaviour {
    public string address="/slider1";
public  void onOsc(float f)
{
transform.localScale=new Vector3(f,f,f);
}
void Start()
{
    zOSC.bind(this,onOsc,address);
}
	
}
