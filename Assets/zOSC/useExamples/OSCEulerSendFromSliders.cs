using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSCEulerSendFromSliders : MonoBehaviour
{
    public Slider xSlider;
    public Slider ySlider;
    public Slider zSlider;
    public string oscAddres = "/rot";
    public bool sendAsEuler = true;
    void Start()
    {
        xSlider.onValueChanged.AddListener(OnSliderMoved);
        ySlider.onValueChanged.AddListener(OnSliderMoved);
        zSlider.onValueChanged.AddListener(OnSliderMoved);
    }
    void OnSliderMoved(float ignored)
    {
        Vector3 euler = new Vector3(xSlider.value*360, ySlider.value*360, zSlider.value*360);
        Quaternion rot = Quaternion.Euler(euler);
        if (sendAsEuler)
            zOSC.BroadcastOSC(oscAddres, euler);
        else
            zOSC.BroadcastOSC(oscAddres, rot);
    }


}
