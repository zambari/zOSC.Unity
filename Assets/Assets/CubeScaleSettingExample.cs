///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class CubeScaleSettingExample : MonoBehaviour {

void Start()
{
    
  SettingsSlider myRot=zSettings.addSlider(name+" rotation","Rots");
  myRot.setRange(0,360);
  myRot.valueChanged+=setRotation;

  
  SettingsSlider myScaleY=zSettings.addSlider(name+" height","Height");
  myScaleY.valueChanged+=setScaleY;

  SettingsSlider myScaleXZ=zSettings.addSlider(name+" fat","Fat");
  myScaleXZ.setRange(0.1f,0.4f);
  myScaleXZ.valueChanged+=setScaleXZ;

  



}
void setRotation(float f)
{

transform.localRotation=Quaternion.Euler(0,f,0);

}

void setScaleY(float f)
{
    transform.localScale=new Vector3(transform.localScale.x,f,transform.localScale.z);
    transform.localPosition=new Vector3(transform.localPosition.x,f/2,transform.localPosition.z);

}

void setScaleXZ(float f)
{
    transform.localScale=new Vector3(f,transform.localScale.y,f);


}
	
}
