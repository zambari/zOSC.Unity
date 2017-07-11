///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;
using UnityOSC;

public class OSCSliderPanel : MonoBehaviour {
 string[] sliderAddresses;
SettingsSlider[] sliders;
public string tabname="OSCSliders";
public string sliderName="/slider";
public int numSliders=5;
	void Start()
    {   //Debug.Log("sa");
        sliders=new SettingsSlider[numSliders];
      //if (sliders.Length<numSliders)
         sliderAddresses= new string[numSliders];
        for (int i=0;i<numSliders;i++)
        {
            string thisSliderName=sliderName+i;
           // if (sliderAddresses.Length>i)  thisSliderName=sliderAddresses[i];

            sliders[i]=zSettings.addSlider(thisSliderName,tabname);
            sliderAddresses[i]=thisSliderName;
            int t=i;
              sliders[i].valueChanged+=((x)=>onSliderMove(t,x));
              zOSC.bind(this,(x)=>onOSC(t,x),thisSliderName);
        }

    }
     void onOSC(int sliderId,float v)
    {
               if (sliderId>=sliders.Length || sliderId<0) 
               { Debug.Log("invalid slider "+sliderId); 
               }
               else 
               {
                SettingsSlider s=sliders[sliderId];
                s.setLabel(sliderAddresses[sliderId]+" = "+s.value);

               }


  //      Debug.Log("osc  "+sliderAddresses[sliderId]+" moved to "+v);
    }
    void onSliderMove(int sliderId,float v)
    {
//      Debug.Log("slider "+sliders[sliderId].name+" moved to "+v);
       if (sliderId>=sliders.Length) Debug.Log("invalid slider "+sliderId); 
       SettingsSlider s=sliders[sliderId];
//       Debug.Log(sliderAddresses[sliderId]+" = "+s.value);

          zOSC.broadcastOSC(sliderAddresses[sliderId],s.value);
    }
}
