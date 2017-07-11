///zambari codes unity
using UnityEngine;
using System;

[RequireComponent(typeof(zOSC))]
public class zOSCSettings : MonoBehaviour
{
    public int defaultRecievePort = 8899;
    public int defaultSendPort = 8899;
    public bool recieveOnly;
    public string defaultDestination = "127.0.0.1";
    SettingsText destIP;
    SettingsText destPort;
    SettingsText listenPort;
    SettingsText sendTestSliderAddress;
    SettingsText recieveTestSliderAddress;
    string sendSliderAddress = "/test";
    string recieveSliderAddress = "/test";
    SettingsSlider recieveSlider;
    SettingsToggle showLog;
    const string tab = "Network";
    SettingsLabel recieverLabel;
    SettingsLabel destinationLabel;
    bool lastEchoValue;

    void swapPorts()
    {
        try
        {
            int currentListen = Int32.Parse(listenPort.value);
            int currentSend = Int32.Parse(destPort.value);

            destPort.value = currentListen.ToString();
            listenPort.value = currentSend.ToString();
        }
        catch (Exception e)
        {
            Debug.Log("error parsing int when swaping ports " + e.Message);

        }

    }
    void settingsSetup()
    {
        if (zSettings.instance == null) return;

        recieverLabel = zSettings.addLabel("Listener (IP:" + Network.player.ipAddress + ")", tab);
        //recieve port
        listenPort = zSettings.addTextSetting("Listen port", tab);

        listenPort.intOnly = true;


        listenPort.AddPreset("9000");
        listenPort.AddPreset("8000");
        listenPort.AddPreset("9988");
        listenPort.AddPreset("10101");
        SettingsToggle local = null;
        if (!recieveOnly)
        {
            SettingsButton swapButton = zSettings.addButton("▲ ▼ Swap In / Out Ports ▲ ▼ ", tab);
            swapButton.onTrigger += swapPorts;
            // send port
            destinationLabel = zSettings.addLabel("destination", tab);
            destIP = zSettings.addTextSetting("dest IP", tab);
            destPort = zSettings.addTextSetting("dest port", tab);

            local = zSettings.addToggle("Local Echo", tab);


            zSettings.addLabel("Test Send", tab);
            sendTestSliderAddress = zSettings.addTextSetting("Test send addr", tab);
            SettingsSlider sendSlider = zSettings.addSlider("TestFloat", tab);


            sendSlider.valueChanged += sendsliderMoved;

        }
        recieveTestSliderAddress = zSettings.addTextSetting("Test recieve addr", tab);
        recieveSlider = zSettings.addSlider("RECIEVER", tab);
        //log
        if (!recieveOnly)
        {
            showLog = zSettings.addToggle("Show Log", tab);
            showLog.valueChanged += togglezLog;
            SettingsToggle logSend = zSettings.addToggle("Log Sends", tab);
            SettingsToggle logRecieve = zSettings.addToggle("Log recieves", tab);

            logSend.valueChanged += setLogSends;

            logRecieve.valueChanged += setLogRecieve;

            destIP.AddPreset(defaultDestination);
            destIP.AddPreset("127.0.0.1");
            destIP.AddPreset("192.168.0.101");
            destIP.AddPreset("192.168.0.102");
            destIP.AddPreset("192.168.0.2");
            destIP.AddPreset("192.168.1.2");
            destIP.AddPreset("192.168.1.20");
            destIP.AddPreset("192.168.2.20");



            destPort.intOnly = true;

            destPort.AddPreset(defaultSendPort.ToString());
            destPort.AddPreset("9000");
            destPort.AddPreset("8000");
            destPort.AddPreset("9988");
            destPort.AddPreset("10101");
            destPort.AddPreset(defaultSendPort.ToString());
            local.valueChanged += setLocalEcho;
            local.defValue = true;


            destIP.defVal = "127.0.0.1";
            logSend.defValue = true;
            logRecieve.defValue = true;






            sendTestSliderAddress.AddPreset(sendSliderAddress);
            sendTestSliderAddress.AddPreset("/testslider2");
            sendTestSliderAddress.AddPreset("/testslider3");

            destIP.valueChanged += OnNewDestinationIP;
            destPort.valueChanged += OnNewDestinationPort;

            sendTestSliderAddress.defVal = sendSliderAddress;

            destPort.defVal = defaultSendPort.ToString();
        }
        listenPort.defVal = defaultRecievePort.ToString();
        recieveTestSliderAddress.valueChanged += newRecieveSliderAddress;
        recieveTestSliderAddress.AddPreset(recieveSliderAddress);
        recieveTestSliderAddress.AddPreset("/testslider2");
        recieveTestSliderAddress.AddPreset("/testslider3");

        listenPort.valueChanged += OnNewListenPort;

        recieveSlider.disabled = true;

        recieveTestSliderAddress.defVal = recieveSliderAddress;



        zOSC.bind(this, ping, "/ping");
    }

    void ping(float f)
    {//Debug.Log(":")


    }

    void createClient()
    {
        if (String.IsNullOrEmpty(destPort.value)) return;

        zOSC.SetReciever(destIP.value, Int32.Parse(destPort.value));

        destinationLabel.value = "Destination : " + destIP.value + " : " + destPort.value;
    }

    void OnNewDestinationIP(string s)
    {

        createClient();



    }
    void OnNewDestinationPort(string s)
    {
        createClient();

    }
    void OnNewListenPort(string s)
    {

        int newPort;
        try
        {
            newPort = Int32.Parse(s);
        }
        catch
        {
            listenPort.value = zOSC.localPort.ToString();
            return;
        }

        if (newPort == zOSC.localPort)
        {
            Debug.Log("current port"); return;
        }


        if (zOSC.instance.setLocalPort(newPort))
            recieverLabel.value = ("Listener : " + Network.player.ipAddress + " : " + newPort);
        else
            recieverLabel.value = " PORT TAKEN, pick another";
    }
    string oldAddr;
    void newRecieveSliderAddress(string s)
    {
        //  if (String.IsNullOrEmpty(oldAddr))
        zOSC.unbind(oldAddr);
        zOSC.bind(this, recieveSliderMove, s);
        oldAddr = s;
    }

    void recieveSliderMove(float f)
    {
        recieveSlider.value = f;
    }
    void sendsliderMoved(float f)
    {
        zOSC.broadcastOSC(sendTestSliderAddress.value, f);
    }
    void togglezLog(bool b)
    {
        if (b) zLog.show(); else zLog.hide();
    }

    void setLogSends(bool b)
    {
        zOSC.instance.setLogSends(b);
        if (b) showLog.value = true;
    }


    void setLogRecieve(bool b)
    {
        zOSC.instance.setLogRecieve(b);
        if (b) showLog.value = true;
    }
    void setLocalEcho(bool b)
    {
        zOSC.instance.localEcho = b;
    }

    void Start()
    {
        settingsSetup();
        //Invoke("settingsSetup",1);
        zOSC.instance.setLocalPort(defaultRecievePort);
    }

}
