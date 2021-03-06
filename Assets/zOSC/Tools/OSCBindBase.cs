﻿///zambari codes unity

using UnityEngine;
using Z.OSC;


public class OSCBindBasic : MonoBehaviour
{


    public string OSCAddress = "/oscAddr";
    protected string lastAddress;

    protected virtual void OSCUnbind()
    {

    }

    protected virtual void OSCBind()
    {

    }
    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(OSCAddress)) OSCAddress = "/my/osc/revieve/address";
        if (!string.IsNullOrEmpty(lastAddress))
        {
            OSCUnbind();
        }

        // Invoke("_bind", 0.1f);
    }

    protected virtual void OnDisable()
    {
        OSCUnbind();
    }
    protected virtual void OnEnable()
    {
        if (Time.time > 1)
            OSCBind();
    }
    protected virtual void Start()
    {
        OSCBind();
    }
    void _bind()
    {
        if (!gameObject.activeInHierarchy) return;
        if (zOSC.instance == null) return;
        //        Debug.Log("bindin0g "+OSCAddress,gameObject);
        OSCBind();
        lastAddress = OSCAddress;
    }

}