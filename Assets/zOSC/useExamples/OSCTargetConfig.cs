using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSCTargetConfig : MonoBehaviour
{
    public zOSC zOSC;
    public InputField address;
    public InputField port;
    public InputField recievePort;
    public void Apply()
    {
        zOSC.SetTarget(address.text, System.Int32.Parse(port.text));
        zOSC.SetRecievelPort(System.Int32.Parse(recievePort.text));
    }

}
