using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class OSCSliderConfigurable : MonoBehaviour
{
    [Header("Where to send")]
    public string OSCAddress;
    [Header("Ui references")]
    LayoutElement layoutElement;
    public OSCSliderSend slider;
    public InputField addressInput;
    public InputField minValue;
    public InputField maxValue;
    public Text addressLabel;
    [Header("Playerprefs id")]
    public string myID;
    void loadPreference()
    {
        if (PlayerPrefs.HasKey(myID))
        {

            string test = PlayerPrefs.GetString(myID);
            OSCAddress = zOSC.sanitizeAddress(test);
//            Debug.Log("loaded " + test);
            PlayerPrefs.DeleteKey(myID);

        }

    }
    void OnValidate()
    {
        myID = getSliderId();
        if (!gameObject.activeInHierarchy) return;
		
        loadPreference();
        if (!gameObject.activeInHierarchy) return;
        if (slider == null) slider = GetComponentInChildren<OSCSliderSend>();
        if (slider == null)
        {
            var k = GetComponentInChildren<Slider>();
            if (k != null) k.gameObject.AddComponent<OSCSliderSend>();
            if (slider != null) slider.OSCName = OSCAddress;
        }
        setOSCAddress(OSCAddress);
		
        if (addressInput != null) addressInput.text = OSCAddress;
        if (addressLabel != null) addressLabel.text = OSCAddress;
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetString(myID, OSCAddress);
//        Debug.Log("saved" + myID + " b" + OSCAddress);
    }

    string getSliderId()
    {
 if (transform.parent==null) return "/none";
        for (int i = 0; i < transform.parent.childCount; i++)
            if (transform.parent.GetChild(i) == transform)
                return "oscSlider" + i;
        return "/none";
    }
    void setOSCAddress(string newOSCAddress)
    {
      // Debug.Log("setting osc addres " + newOSCAddress);
        OSCAddress = zOSC.sanitizeAddress(newOSCAddress);
	     if (newOSCAddress.Equals(OSCAddress)) return;
        if (slider != null)
            slider.OSCName = OSCAddress;
        if (addressLabel != null) addressLabel.text = OSCAddress;
        if (addressInput != null) addressInput.text = OSCAddress;

    }
    void Start()
    {  if (OSCAddress.Equals("/none")) setOSCAddress(getSliderId()); else
        setOSCAddress(OSCAddress);
        if (minValue != null) minValue.onValueChanged.AddListener(setMinValue);
        if (maxValue != null) maxValue.onValueChanged.AddListener(setMaxValue);
        if (addressInput != null) addressInput.onValueChanged.AddListener(setOSCAddress);
    }
    void setMaxValue(string s)
    {
        Debug.Log("max" + s);
        float max = 0;
        if (float.TryParse(s, out max))

            slider.slider.maxValue = max;
        else
          if (maxValue != null) maxValue.text = slider.slider.maxValue.ToString();
    }

    void setMinValue(string s)
    {
        Debug.Log("min " + s);
        float min = 0;
        if (float.TryParse(s, out min))
            slider.slider.maxValue = min;
        else
          if (minValue != null) minValue.text = slider.slider.minValue.ToString();
    }

}
