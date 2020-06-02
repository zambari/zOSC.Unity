using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z.OSC;

public class zOSCServerListener : MonoBehaviour
{

    List<OSCRouter> routers;

    Queue<OSCPacket> recievePacketQueue;
    void Update()
    {
        if (recievePacketQueue != null)
            lock (recievePacketQueue)
            {
                while (recievePacketQueue.Count > 0)
                    ReactToPacket(recievePacketQueue.Dequeue());
            }

    }
    public int listenPort = 9988;
    public bool autoStart = true;

    public CommStats stats = new CommStats();
    List<ClientLog> OSCRecievers;

    /// <summary>
    /// Contains incoming packet parsing ruleset
    /// </summary>
    /// <param name="packet"></param>

    void ReactToPacket(OSCPacket packet)
    {
        // lastRecieved = packet;
        if (packet == null)
        {
            Debug.LogError("null packet");
            return;
        }
        // if (!detailedLog)
        Debug.Log("incoming " + packet.Address + " " + packet.typeTag);
        // else
        // zOSC.LogReceived("incoming " + packet.Address + "   typetag   " + packet.typeTag + "  " + packet.BinaryData.ByteArrayToStringAsHex());
        stats.TotalBytes += packet.BinaryData.Length;
        stats.TotalPackets++;

        // LIST BIND REQUESTES BEGIN    
        //   instance.listBindAdresses(address);
        // LIST BIND REQUESTES END

        ///// DO NOT REMOVE
        //   if (ParseAcks(packet)) return;
        //   checkIfAckRequired(ref packet);

        bool anyReacted = false;

        int i = 0;

        if (packet.Address[packet.Address.Length - 1] == '?')
        {
            Debug.Log("stopping as there was a request");
            // listBindAdresses(packet.Address.Substring(0, packet.Address.Length - 1));
            return;
        }
        if (routers.Count == 0) Debug.Log("no routers");
        while (i < routers.Count)
        {
            if (packet.Address.StartsWith(routers[i].baseAddress))
            {
                // Debug.Log("potential");
                try
                {
                    if (routers[i].ParsePacket(packet))
                    {
                        //  Debug.Log("router [" + routers[i].baseAddress + "] reacted to " + packet.Address);
                        anyReacted = true;
                    }
                    else
                        Debug.Log("no router reacted to " + packet.Address);
                }
                catch (Exception e)
                {
                    Debug.Log("Exception while parsing router " + i + " [" + routers[i].baseAddress + "]  packet " + packet.Address + " triggered exception " + e.Message);

                }
            }
            i++;
        }
        if (!anyReacted) Debug.Log("Packet with no listeners " + packet.Address + " types: " + packet.typeTag);
    }
    public void AddRouter(OSCRouter router)
    {
        if (routers == null) routers = new List<OSCRouter>();
        routers.Add(router);
    }
    OSCServer server;
    OSCRouter _mainRouter = new OSCRouter("");
    void Awake()
    {

        if (GetComponent<OSCHandler>() == null) gameObject.AddComponent<OSCHandler>();
        _mainRouter = new OSCRouter("");
        AddRouter(_mainRouter);

        //replyRouter = new OSCRouter("/");
        recievePacketQueue = new Queue<OSCPacket>();
    }
    ServerLog localListener;

    [ExposeMethodInEditor]
    void StartServer()
    {
        Debug.Log(RestartLocalServer());
    }
    bool newDataFlag;
    bool RestartLocalServer()
    {
        if (server != null)
        {
            server.Close();
            Debug.Log("closing server");
        }
        server = new OSCServer(listenPort);
        server.PacketReceivedEvent += (server, packet) =>
              {
                  lock (recievePacketQueue)
                  {
                      recievePacketQueue.Enqueue(packet);
                      newDataFlag = true;
                  }
              };
        return true;
    }

    public bool SetLocalPort(int port)
    {
        listenPort = port;

        return RestartLocalServer();
    }

    void Start()
    {
        if (autoStart)
            RestartLocalServer();

        //  Bind(this, OnBeaconDetection, "/beacon");
        //  Bind(this, SetTarget, "/target");
    }
    // SetLocalPort(defaultRecievePort);
    // if (!string.IsNullOrEmpty(targetAddr))
    // {
    //     if (broadcastPresence) PreparePresenceBroadcast(targetAddr);
    //     SetTarget(targetAddr, targetPort);
    // }

    // yield return new WaitForSeconds(0.1f);
    // isSendingBroadcast = false;
    // if (broadcastPresence)
    //     StartCoroutine(SendPings());

}

