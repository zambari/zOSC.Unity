using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;
public class zOSCSender : MonoBehaviour
{

    [Header("Sender")]
    public string targetAddr = "127.0.0.1";
    public int targetPort = 9988;
    [ExposeMethodInEditor]
    void SendSomething()
    {
        Send("/something", 0.5f);
    }
    OSCClient client;
    public void Send(string address, float v)
    {

        OSCMessage message = new OSCMessage(address);
        message.Append(v);
        Send(message);
    }


    public CommStats stats = new CommStats();
    public void Send(OSCMessage message)
    {
        if (client != null)
        {

            //   if (!detailedLog)
            Debug.Log(message.Address + " " + message.typeTag);
            //   else
            //  Debug.Log(message.Address + " " + message.typeTag+"    binary: "+message.BinaryData.ByteArrayToStringAsHex());
            client.Send(message);
            stats.TotalBytes += message.BinaryData.Length; // stats
            stats.TotalPackets++;                          // stats

        }
        Debug.Log("sent msg " + message.Address);

    }


    public bool SetTarget(string addr, int portNr)
    {
        if (client != null) client.Close();
        Debug.Log("zOSC target : " + addr + " : " + portNr);

        targetAddr = addr;
        targetPort = portNr;
        client = new OSCClient(IPAddress.Parse(addr), portNr);
        if (client == null)
        {
            Debug.Log("OSC port open failed  : " + addr + " : " + portNr);
            return false;
        }
        return true;

    }


}
