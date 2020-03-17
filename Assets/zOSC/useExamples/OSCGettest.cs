﻿///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCGettest : MonoBehaviour
{
    public bool bindGet;
    public bool request;
    public bool forcereply;
    public bool unbind;
    public string[] response;
    public bool requestFloat;
    public bool addProvider;


    void OnValidate()
    {
        if (requestFloat)
        {
            requestFloat = false;
            // zOSC.request("/myFloat",reactToFloat);
            zOSC.BroadcastOSC("/get/floatValue");
            //  /myFloat
        }
        if (addProvider)
        {
            addProvider = false;
            zOSC.mainRouter.addFloatProvider(this,"/myFloat", GetSomeFloat );
        }

        if (request)
        {
            request = false;
            //   zOSC.request("/OSCBind",reactToFloatArray);
        }
        if (bindGet)
        {
            bindGet = false;
            //    zOSC.addGet("/OSCBind",getBindHandler);
            zOSC.get.addFloatArrayProvider(this, "/floatValue", provideFloats);

        }
        if (forcereply)
        {
            forcereply = false;
            // zOSC.BroadcastOSC("/reply/OSCBind","ala ma kota");
            string[] ss = new string[5];
            for (int i = 0; i < 5; i++) ss[i] = "forcedstring" + i;
            zOSC.BroadcastOSC("/reply/OSCBind", ss);
        }
        //    if (unbind)
        //           zOSC.request("/oscCommands",reactToStringtArray);

    }
    string provideString()
    {
        return "aa";
    }

    void returningFloatValue(float f)
    {
        Debug.Log("is returning float " + f);

    }
    float GetSomeFloat()
    {
        return 0.777f;
    }

    void reactToStringtArray(string[] stra)
    {
        response = stra;

    }
    float[] provideFloats()
    {
        float[] ss = new float[10];
        for (int i = 0; i < 10; i++) ss[i] = i;
        return ss;

    }

    void reactToFloatArray(float[] stra)
    {
        Debug.Log("recieved aray via osc,");
        foreach (float s in stra)
        {
            Debug.Log(s);
        }

    }


}
