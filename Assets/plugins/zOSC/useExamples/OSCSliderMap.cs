///zambari codes unity

using UnityEngine;
using UnityEngine.UI;

//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCSliderMap : OSCBindBasic {
Slider slider;
public Slider shadowSlider;

void Awake()
{
    slider=GetComponent<Slider>();
    slider.onValueChanged.AddListener(SendOnSliderMove);

}
void SendOnSliderMove(float f)
{
  zOSC.broadcastOSC(OSCAddress,f);

}

void OnRecieveOsc(float f)
{
 shadowSlider.value=f;

}
protected override void OSCBind()
{
  zOSC.bind(this,OnRecieveOsc,OSCAddress);

}

	
}
