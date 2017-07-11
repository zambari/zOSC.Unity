///zambari codes unity

using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class zOSCMonitor : MonoBehaviour {
public Image transmitLED;
public Image recieveLED;
public float duration=.1f;

void OnValidate()
{

 if (    transmitLED==null || recieveLED ==null)
 {

  Image[] images=GetComponentsInChildren<Image>();
  if (images.Length<2) 
  {
       Debug.Log(" osc monitor does not have led refrecnes");

  }
  else
  {
     transmitLED=images[images.Length-1];
     recieveLED=images[images.Length-2];
  }


 }

}
void Start()
{

    transmitLED.enabled=false;
    recieveLED.enabled=false;

    zOSC.OnOSCTransmit+=OnTransmit;
    zOSC.OnOSCRecieve+=OnRecieve;
}


void OnDisable()
{
 // zOSC.OnOSCTransmit-=OnTransmit;
 // zOSC.OnOSCRecieve-=OnRecieve;
}


void OnTransmit()
{    transmitLED.enabled=true;
    Invoke("hideTransmit",duration);
}

void hideTransmit()
{
    transmitLED.enabled=false;

}
void OnRecieve()
{
    recieveLED.enabled=true;
    CancelInvoke("hideRecieve");
    Invoke("hideRecieve",duration);

}

void hideRecieve()
{
     recieveLED.enabled=false;

}
	
}
