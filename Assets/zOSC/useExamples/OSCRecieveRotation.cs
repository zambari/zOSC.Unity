///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCRecieveRotation : OSCBindBasic
{
    public bool useLocal;
    public bool useNormalised;
    public Vector3 lastRecieved;
    public Vector3 recievedPos;
    public bool useEuler = true;
    public void onOscQ(Quaternion q)
    {
        if (useLocal)
            transform.localRotation = q;
        else
            transform.rotation = q;
    }

    public void onOscEuler(Vector3 q)
    {
        lastRecieved = q;
        if (useNormalised) q = q * 180;
        if (enabled)
            if (useLocal)
                transform.localEulerAngles = q;
            else
                transform.eulerAngles = q;

    }

    public void onOscP(Vector3 q)
    {
        recievedPos = q;
        if (enabled)
            if (useLocal)
                transform.localPosition = q;
            else
                transform.position = q;
    }

    protected override void OSCUnbind()
    {
        zOSC.Unbind(OSCAddress);
    }

    protected override void OSCBind()
    {
        //   if (useEuler)
        // zOSC.bind(this, onOscEuler,OSCAddress);
        //   else
        //    zOSC.bind(this, onOscQ,OSCAddress);
        // if (recievePosition)  zOSC.bind(this, onOscP,positionAddress);
    }
    void Start()
    {
        OSCBind();
    }


}
