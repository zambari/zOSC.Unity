///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCRemoteCamControlSetting : MonoBehaviour {
SettingsSlider x;
SettingsSlider y;
SettingsSlider fov;
const string tab="Camera";

	void Start()
    {
        x=zSettings.addSlider("Cam X",tab);
        y=zSettings.addSlider("Cam Y",tab);
        x.setRange(-30,30);
          y.setRange(-120,120);

        fov=zSettings.addSlider("Cam Fov",tab);
        fov.setRange(30,100);
        x.valueChanged+=sendCamRot;
        y.valueChanged+=sendCamRot;
        fov.valueChanged+=sendCamFov;
        fov.defValue=65;
        x.defValue=0;
        y.defValue=0;
    }



void sendCamFov(float f)
{

zOSC.broadcastOSC("/cam/fov",f);


}

void sendCamRot(float f)
{
Vector3 rot=new Vector3(x.value,y.value,0);

zOSC.broadcastOSC("/cam/rot",Quaternion.Euler(rot));

}


}

