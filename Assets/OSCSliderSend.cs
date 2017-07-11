using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// zOSC use example

public class OSCSliderSend : MonoBehaviour
{

    public string OSCName;
    public Slider slider;
	public Text valueDisplayText;
    void OnValidate()
    {
        if (slider == null) slider = GetComponent<Slider>();
        OSCName = zOSC.sanitizeAddress(OSCName);
		if (valueDisplayText==null) valueDisplayText=GetComponentInChildren<Text>();
    }

    void Start()
    {
        slider.onValueChanged.AddListener(newSliderValue);
    }

    void newSliderValue(float f)
    {
        zOSC.broadcastOSC(OSCName, f);
		if (valueDisplayText!=null) valueDisplayText.text=(Mathf.Round(f*10)/10).ToString();
    }

}
