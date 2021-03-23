using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSCButtonSync : MonoBehaviour
{
    public bool isReciever;

    public string identifier = "";
    public Button button { get { if (_button == null) _button = GetComponent<Button>(); return _button; } }
    private Button _button;
    void Awake()
    {
#if UNITY_EDITOR || !UNITY_ANDROID
        isReciever = true;
#else
        isReciever=false;
#endif

    }
    void OnValidate()
    {
        if (string.IsNullOrEmpty(identifier)) identifier = zExt.RandomString(6);
    }
    void SendOSC()
    {
        zOSC.BroadcastOSC(identifier);
    }
    void OnOSCRecieved()
    {
        button.onClick.Invoke();
    }

    void Start()
    {
        if (isReciever)
        {
            zOSC.Bind(this, identifier, OnOSCRecieved);

        }
        else
        {
            button.onClick.AddListener(SendOSC);
        }
    }


}
