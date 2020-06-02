using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z.OSC;

[RequireComponent(typeof(Slider))]

public class OSCSliderSenderDev : MonoBehaviour
{


    public string OSCAddress;
    Slider slider;
    public bool sendAsInt;
    public bool printDebugs = false;
    // bool antiFeedback;
    // float antiFeedbackTime = 0.1f;
    // float releaseFeedbackAfter;
    float lastSentValue = -1;

    public zOSCSender senderInstance;
    void Reset()
    {
        senderInstance = GameObject.FindObjectOfType<zOSCSender>() as zOSCSender;
    }
    void OnValidate()
    {
        printDebugs = false;
        if (slider == null) slider = GetComponent<Slider>();
        OSCAddress = OSCAddress.SanitizeOSCAddress();
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnNewSliderValue);
        // zOSC.BindInt(this, OSCAddress, Recieveint);
    }

    // void Recieveint(int i)
    // {
    //     slider.value = i;
    //     Debug.Log(name + " " + OSCAddress + "  Recieved value " + i);
    //     SetSliderValue(i);
    // }

    void OnNewSliderValue(float f)
    {
        if (lastSentValue == f)
            return;
        lastSentValue = f;
        if (sendAsInt)
        {
            System.Int32 val = (System.Int32)f;//(f * (negative ? -1 : 1) * (sendmax ? System.Int32.MaxValue : 10000));
            OSCMessage msg = new OSCMessage(OSCAddress);
            // for (int i = 0; i < parameterRepeatCount; i++)
            msg.Append(val);
            Debug.Log("sent " + val);
            senderInstance.Send(msg);
            // zOSC.BroadcastOSC(msg);
        }
        else
        {
             senderInstance.Send(OSCAddress, f);
        }
    }

}
