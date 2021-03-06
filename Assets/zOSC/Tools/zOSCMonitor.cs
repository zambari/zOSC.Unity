﻿// https://github.com/zambari/zOSC.Unity

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Z.OSC;
[RequireComponent(typeof(Image))]
public class zOSCMonitor : MonoBehaviour
{
    public float duration = .1f;
    Image image;
    bool isRunning;
    float blinkTime;
    public enum OSCMonitorMode { transmit, recieve } //, loopback, recieveAndLoopback, none 
    public OSCMonitorMode monitorMode;
    IEnumerator Blinker()
    {
        if (isRunning) yield break;

        isRunning = true;
        image.enabled = true;
        while (Time.time < blinkTime)
        {
            yield return null;
        }
        image.enabled = false;
        isRunning = false;
    }
    void Blink()
    {
        blinkTime = Time.time + duration;
        if (gameObject.activeInHierarchy)
            if (!isRunning) StartCoroutine(Blinker());
    }
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        if (monitorMode == OSCMonitorMode.transmit)
            zOSC.OnOSCTransmit += Blink;
        if (monitorMode == OSCMonitorMode.recieve)
            zOSC.OnOSCRecieve += Blink;
        // if (monitorMode == OSCMonitorMode.loopback)
        //     zOSC.OnOSCTransmitLocal += Blink;
        // if (monitorMode == OSCMonitorMode.recieveAndLoopback)
        // {
        //     zOSC.OnOSCRecieve += Blink;
        //     zOSC.OnOSCTransmitLocal += Blink;
        // }
    }

}
