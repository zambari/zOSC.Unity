///zambari codes unity

using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCImageRecieve : OSCBindBasic
{


    RawImage rawImage;
    public Text packetSizeText;
    Texture2D tex;
    SettingsSlider updateFreq;
    SettingsSlider rectx;

    SettingsSlider recty;
    SettingsSlider qual;
    public bool getSettings=true;
    void OnBlob(byte[] blob)
    {

        //int frontremove = 27;
      //  byte[] b = new byte[blob.Length - frontremove - 1];
      //  for (int i = 0; i < blob.Length - frontremove - 1; i++) b[i] = blob[frontremove + i];
     byte[] b=blob;

        Texture2D tex = new Texture2D(10, 10);
        tex.LoadImage(b);

        rawImage.texture = tex;
        rawImage.color=new Color(1,1,1,0.95f);
            if (packetSizeText != null )

            packetSizeText.text = "x " + tex.width + " " + tex.height + " " + b.Length.ToString()+" bytes";
            
        Invoke("restoreImage",0.05f);

    }
    void restoreImage()
    {
         rawImage.color= Color.white;
    }
    protected override void OSCBind()
    {
        zOSC.bind(this, OnBlob,OSCAddress);

    }
    protected override void OSCUnbind()
    {
        zOSC.bind(this,OnBlob,lastAddress );
    }
    void Start()
    {
        OSCBind();
        rawImage = GetComponent<RawImage>();
        if (getSettings)
        {
            
            SettingsSlider updateFreq = zSettings.addSlider("Update Freq", "IMG");
            SettingsSlider rectx = zSettings.addSlider("rect X", "IMG");

            SettingsSlider recty = zSettings.addSlider("rect Y", "IMG");
            SettingsSlider qual = zSettings.addSlider("quality", "IMG");

            qual.setRange(15, 100);
            
            recty.setRange(10, 1920);
            rectx.setRange(10, 1080);
            updateFreq.setRange(0.1f, 1f);
                 updateFreq.defValue=0;
                
                recty.defValue=200;
                recty.defValue=200;

            updateFreq.valueChanged += setUpdate;
            rectx.valueChanged += setResX;
            recty.valueChanged += setResY;
            qual.valueChanged += setQual;
            qual.defValue=25;
        }

    }

    void setResX(float v)
    {
        zOSC.broadcastOSC("/img/setResX", v);
    }
    void setResY(float v)
    {
        zOSC.broadcastOSC("/img/setResY", v);
    }

    void setQual(float v)
    {
        zOSC.broadcastOSC("/img/setQual", v);
    }

    void setUpdate(float v)
    {
        zOSC.broadcastOSC("/img/updateFreq", v);
    }

}
