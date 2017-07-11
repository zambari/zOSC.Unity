///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCRecieveRotation : OSCBindBasic
{
  
    public void onOsc(Quaternion q)
    {
        if (enabled)
            transform.localRotation = q;

    }
       protected override void OSCUnbind()
    {
        zOSC.unbind(OSCAddress);
    }

    protected override void OSCBind()
    {
        zOSC.bind(this, onOsc,OSCAddress);
    }
    void Start()
    {
        OSCBind();
    }
 

}
