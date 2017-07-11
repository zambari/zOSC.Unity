﻿///zambari codes unity

using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCImageSend : OSCBindBasic
{

    public bool sendNow;
    public Texture2D texture;
    public RenderTexture renderTexture;
    public MeshRenderer mr;
    public RawImage rawimage;
    public int x = 100;
    public int y = 100;
    [Range(1, 100)]
    public int quality = 15;
    public float updateInterval = 1.4f;

    float nextUpdate;
    void OnPostRender()
    {

        if (updateInterval > 0)
            if (Time.time > nextUpdate)
            {
                nextUpdate = Time.time + updateInterval;
                send();

            }

        if (sendNow)
        {
            send();
            sendNow = false;
        }

    }
    void sendn()
    {
        sendNow = true;
    }
    void Start()
    {

        SettingsButton t = zSettings.addButton("send Image", "IMG");
    //    SettingsSlider s = zSettings.addSlider("quality", "IMG");
        t.valueChanged += sendn;
      //  s.valueChanged += setQuality;

        zOSC.bind(this,setResX,"/img/setResX");
        zOSC.bind(this,setResY,"/img/setResY" );
        zOSC.bind(this,setQual,"/img/setQual" );
        zOSC.bind(this,setUpdate,"/img/updateFreq" );
    }
    void setQuality(float f)
    {
        quality = (int)(f);
    }
    void send()
    {

        texture = new Texture2D(renderTexture.width,renderTexture.height);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                 texture.Apply();
   // TextureScale.Bilinear (texture,x,y);
        

        byte[] b = texture.EncodeToJPG(quality);
        if (b!=null)
        {
            zOSC.broadcastOSC(OSCAddress, b);
            rawimage.texture = texture;
        }

    }



    void setResX(float v)
    {
        x = (int)v;
        zLog.Log("x is now " + x);
    }
    void setResY(float v)
    {
        y = (int)v;
        zLog.Log("y is now " + y);
    }

    void setQual(float v)
    {
        quality = (int)v;
        zLog.Log("qual is now " + quality);
    }

    void setUpdate(float v)
    {
        updateInterval = v;
        zLog.Log("upt is now " + updateInterval);
    }


}