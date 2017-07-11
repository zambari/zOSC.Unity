#define SIMPLECONSOLE

///zambari codes unity

//  An extension of
//  UnityOSC -   Copyright (c) 2012 Jorge Garcia Martin
//  base classes slightly modified, some wrappers added by zambari // Stereoko.TV

using UnityEngine;

using System.Collections.Generic;
using System;
using System.Net;
using UnityOSC;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


/*

 syntax :
 /some?

     new meaning :
    please send back a list of matches 
    /_?/some,sss /someone, /somebody, /sometimes

syntax:
        /object/value/set f
shortcut
        /object/value f
other operations
        /object/value/get
        /object/value,sf get/set 0.5


needs syntax for range mapping. Touchosc and altnernatives like to send 0-1 ranged float. so there should be a scaling facielity, syntax to convety means of scaling feedback, requesting at least a scalar, and offset
alternative is to always assume 0-1 and map internally
but reporting on value 'preferred level' for non-linear things like scale vs velocity 

->transform shortcut?

request for 
/object_name/transform/position/x?      

-< /object/transform/position/x,f,0.35




_/Alt_Cursor/transform/?   -
replies with full hierarchy so
/Canvas/Module/Alt_Cursor/

*/


[RequireComponent(typeof(OSCHandler))]
// if youre using zSettingss, otherwise safe to remove
public class zOSC : MonoBehaviour
{
    [Header("Reciever@localhost")]
    public int defaultRecievePort = 8899;
    [Header("Sender")]
    public string targetAddr="127.0.0.1";
    public int targetPort=9988;


    public const string returnAddress = "/_?";
    public const string query = "?";


    public bool localEcho = true;
  //  ClientLog reciever;
    ServerLog localListener;
    int listenPort = 9988;
    List<ClientLog> OSCRecievers;
    static zOSC _instance;
    bool logSends = true;
    public static bool logRecieve = true;
    public static bool started = false;

    
    public bool logToConsole;

    OSCClient client;
    [Header("Stats")]
    public int TotalBytesSent;
    public int TotalBytesRecieved;

    public int TotalPacketsSent;
    public int TotalPacketsRecieved;
    static bool warningDisplated;
        bool recieverIsLocal;
 
    List<OSCPacket> recievePacketQueue;

    // List<AckRequest> sentAckRequests;
    static Dictionary<int, AckRequest> sentAckRequests;
    static List<OSCRouter> routers;
    public static OSCRouter mainRouter;
    public static Action OnOSCRecieve;
    public static Action OnOSCTransmit;
    public static OSCPacket lastRecieved;
    public static OSCMessage lastSent;
  //  static int ackRequestCounter;
    public static OSCRouter get;
   // public static OSCRouter replyRouter;
   // public List<string> bindAddresses;
    public List<string> routerList;
    


   public static zOSC instance
    {
        get
        {
            return _instance;
        }
    }
    public static string sanitizeAddress(string source)
    {
        if (String.IsNullOrEmpty(source)) return "/none";
        string newAddress="";
        
        if (source[0]!='/') newAddress="/";
        for (int i=0;i<source.Length;i++)
        {
            char thisChar=source[i];
            if (thisChar=='/') newAddress+=thisChar;
            if (Char.IsLetterOrDigit(thisChar)) newAddress+=thisChar;

        }
        if (newAddress.Length==1) newAddress+="none";
        return newAddress;

    }


static void log(string s)
{

#if SIMPLECONSOLE
       SimpleConsole.Log(s);
    
#endif


}

    public static string makeQuery(string inp)
    {
        return inp+query;
    }
    bool parseAcks(OSCPacket packet)
    {
        if (packet.Address.Equals("/ack"))
        {
            string typeTag = packet.typeTag;
            // typetag should be iiii, and we
            for (int i = 1; i < typeTag.Length; i++)
            {
                if (typeTag[i] != 'i')
                    Debug.Log(" strange ack packet");
                else
                {
                    try
                    {
                        int tt = Int32.Parse(packet.Data[i - 1].ToString());
                        if (sentAckRequests.ContainsKey(tt))
                        {
                            sentAckRequests.Remove(tt);
                            Debug.Log("removed ack " + tt);
                        }

                    }
                    catch (Exception e) { Debug.Log("Error ack at index " + i + " " + e.Message); }
                }
            }
            return true;
        }
        else return false;
    }


    public static void reportUnhandled(string msg)
    {
        Debug.Log(" unhadnled " + msg);
    }
    bool checkIfAckRequired(ref OSCPacket packet)
    {
        string typeTag = packet.typeTag;
        if (typeTag[typeTag.Length - 1] == 'i')
        {
            Debug.Log("ack request ");
            return true;
        }
        typeTag = packet.typeTag.Substring(0, typeTag.Length - 1);
        try
        {
            int ackRequest = Int32.Parse(packet.Data[typeTag.Length - 1].ToString());
            Debug.Log("ack nr " + ackRequest);
            OSCMessage ack = new OSCMessage("/ack");
            ack.Append(ackRequest);
            Debug.Log("sent back ack " + ackRequest);
            broadcastOSC(ack);
        }
        catch (Exception e) { Debug.Log("Error ack " + e.Message); }
        return false;

    }
    public void AddRouter(OSCRouter router)
    {
        if (routers == null) routers = new List<OSCRouter>();
        routers.Add(router);
        routerList.Add(router.baseAddress);
    }

/// <summary>
/// Contains incoming packet parsing ruleset
/// </summary>
/// <param name="packet"></param>
    
    void reactToPacket(OSCPacket packet) 
    {
        lastRecieved = packet;
        if (packet == null)
        {
            Debug.LogError("null packet");
            return;
        }

        _instance.TotalBytesRecieved += packet.BinaryData.Length;
        _instance.TotalPacketsRecieved++;
       
        // LIST BIND REQUESTES BEGIN    
            //   instance.listBindAdresses(address);
        // LIST BIND REQUESTES END
        try{
        if (OnOSCRecieve != null) OnOSCRecieve.Invoke();
        } catch (Exception e) {
            Debug.Log("Exception "+e.Message+" when trying to notify local listeners of new packet");
        }
        ///// DO NOT REMOVE
        //   if (parseAcks(packet)) return;
        //   checkIfAckRequired(ref packet);
        //// DO NOT REMOVE

        bool anyReacted = false;

        if (logToConsole) log("RCV:"+packet.Address);
        int i = 0;

        if (packet.Address[packet.Address.Length-1]=='?')
        {
        
            Debug.Log("stopping as there was a request");
  
            listBindAdresses(packet.Address.Substring(0, packet.Address.Length - 1));
            return;
        }
        while (i < routers.Count)
        {
            if (packet.Address.StartsWith(routers[i].baseAddress))
                try
                {
                    if (routers[i].parsePacket(packet))
                    {
                        //   Debug.Log("router [" + routers[i].baseAddress + "] reacted to " + packet.Address);
                        anyReacted = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception while parsing router "+i+" [" + routers[i].baseAddress + "]  packet " + packet.Address + " triggered exception " + e.Message);

                }
            i++;
        }
        if (!anyReacted) Debug.Log("Packet with no listeners " + packet.Address);
    }


    public static byte[] serialize(List<object> list)
    {

        byte[] bytes;
        MemoryStream ms = new MemoryStream();

        new BinaryFormatter().Serialize(ms, list);

        bytes = ms.ToArray();
        ms.Flush();
        ms.Dispose();


        return bytes;

    }
    public static List<object> deserialize(byte[] bytes)
    {
        List<object> list = new List<object>();
        Debug.Log("deserialized len " + bytes.Length);
        using (var ms = new MemoryStream(bytes, 0, bytes.Length))
        {
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            try
            {
                var data = new BinaryFormatter().Deserialize(ms);
                list = (List<object>)data;
            }
            catch (Exception e)
            {
                Debug.Log("deserializing exception " + e.Message);
            }
            Debug.Log("done");
        }
        return list;
    }
    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            byte[] b = ms.ToArray();
            byte[] c = b.Slice(27, b.Length - 1);
            Debug.Log("serialized len " + c.Length);
            return c;
        }
    }

    public static int localPort
    {
        get { return instance.listenPort; }
    }
    static bool checkIfStartedAndStringOk(string s)
    {
        if (String.IsNullOrEmpty(s))
        {
            return false;
        }

        if (s[0] != '/' || (s.Length < 2))
        {
            Debug.Log("osc adresses should start with '/'");
            return false;
        }
        if (_instance == null)
        {
            Debug.Log("zOSC not started !");
            return false;
        }
        return true;
    }
    /* */
    #region bindOverload

    public static void bind(MonoBehaviour requester, Action listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }
    public static void bind(MonoBehaviour requester, Action<float> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }

    public static void bind(MonoBehaviour requester, Action<string> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }

    public static void bind(MonoBehaviour requester, Action<byte[]> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }
    public static void bind(MonoBehaviour requester, Action<float[]> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }

    public static void bind(MonoBehaviour requester, Action<string[]> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }

    public static void bind(MonoBehaviour requester, Action<List<object>> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }
    public static void bind(MonoBehaviour requester, Action<Vector3> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }
    public static void bind(MonoBehaviour requester, Action<Quaternion> listener, string addr)
    {
        mainRouter.bindGeneric(requester, listener,  addr);
    }

    public static void unbind(string addr)
    {
//        Debug.Log("refactor unbind pls "+addr);
      mainRouter.unbind(null,addr);
    }
#endregion bindOverload
    #region broadcastOverload

    public static bool broadcastOSC(OSCMessage message, bool requireAck = false)
    {  

        lastSent = message;
        /*    if (requireAck)
            {
                _instance.pendingAcks.Add(ackRequestCounter);
                AckRequest ack = new AckRequest();
                ack.message = message;
                ack.time = Time.time;
                ack.requested = ackRequestCounter;
                if (sentAckRequests == null) sentAckRequests = new Dictionary<int, AckRequest>();
                sentAckRequests.Add(ack.requested, ack);
                message.Append(ackRequestCounter);
                ackRequestCounter++;
            }*/

        if (_instance == null&&!warningDisplated) { warningDisplated=true; Debug.LogWarning("Please add zOSC to your scene first"); return false; }
      
        if (_instance.client != null)
        {
        if (_instance.logToConsole) log("SND:"+message.Address);
            _instance.client.Send(message);
            _instance.TotalBytesSent += message.BinaryData.Length; // stats
            _instance.TotalPacketsSent++;                          // stats
                if (OnOSCTransmit != null) OnOSCTransmit();
        }
        else Debug.Log("no client " + message.Address);
         try{
           if (_instance.localEcho)
            {
              if (!_instance.recieverIsLocal)
                   _instance.reactToPacket(message);                   // dogfeeding our message
             }
             if (_instance.logSends && zLog.instance!=null ) log("- > " + message.AsString());
                   return true;
        } catch (Exception e) {
             Debug.Log("Error broadcasting OSC to local listenerd (not through network!) "+e.Message);
             return false;
        }
     
    }

    public static void broadcastOSC(string address, bool requireAck = false)
    {
        broadcastOSC(new OSCMessage(address), requireAck);

    }
    public static void broadcastOSC(string address, float v, bool requireAck = false)
    {

        OSCMessage message = new OSCMessage(address);
        message.Append(v);
        broadcastOSC(message, requireAck);


    }

    public static void broadcastOSC(string address, float[] v, bool requireAck = false)
    {
        OSCMessage message = new OSCMessage(address);
        for (int i = 0; i < v.Length; i++)
            message.Append(v[i]);
        broadcastOSC(message, requireAck);
    }

    public static void broadcastOSC(string address, string s, bool requireAck = false)
    {

        OSCMessage message = new OSCMessage(address);
        message.Append(s);
        broadcastOSC(message, requireAck);
    }
    public static void broadcastOSC(string address, string[] s, bool requireAck = false)
    {
        OSCMessage message = new OSCMessage(address);
        for (int i = 0; i < s.Length; i++)
            message.Append(s[i]);
        broadcastOSC(message, requireAck);
    }


    public static void broadcastOSC(string address, byte[] b, bool requireAck = false)
    {

        OSCMessage message = new OSCMessage(address);
        message.Append(b);
        broadcastOSC(message, requireAck);
    }

    public static void broadcastOSC(string address, List<object> o, bool requireAck = false) //, string format
    {
        OSCMessage message = new OSCMessage(address);
        byte[] b = serialize(o);
        message.Append(b);
        broadcastOSC(message, requireAck);
    }

    public static void broadcastOSC(string address, Quaternion q, bool requireAck = false)
    {
        OSCMessage message = new OSCMessage(address);

        message.Append(q.x);
        message.Append(q.y);
        message.Append(q.z);
        message.Append(q.w);

        broadcastOSC(message, requireAck);

    }

    public static void broadcastOSC(string address, Vector3 v, bool requireAck = false)
    {

        OSCMessage message = new OSCMessage(address);
        message.Append(v.x);
        message.Append(v.y);
        message.Append(v.z);
        broadcastOSC(message, requireAck);

    }


    #endregion broadcastOverload

    public static bool SetReciever(string addr, int portNr)
    { 
        if (_instance.client != null) _instance.client.Close();
        _instance.client = new OSCClient(IPAddress.Parse(addr), portNr);
        if (_instance.client == null)
        {
            log("OSC port open failed  : " + addr + " : " + portNr);
            return false;
        }
        if (addr == "127.0.0.1" && portNr == instance.listenPort)

            _instance.recieverIsLocal = true;
        else

            _instance.recieverIsLocal = false;

        if (logRecieve) log("OSC recieving : " + addr + " : " + portNr);
      
      _instance.targetAddr=addr;
      _instance.targetPort=portNr;
        return true;

    }

    void newOScPacket(OSCServer s, OSCPacket packet)
    {
        recievePacketQueue.Add(packet);
    }

    #region misc


    public void setLogSends(bool b)
    {
        logSends = b;
        zLog.log("Displaying sent messages :" + b);
    }

    public void setLogRecieve(bool b)
    {
        logRecieve = b;
        zLog.log("Displaying recieved messages :" + b);
    }

    public bool setLocalPort(int port)
    {
        listenPort = port;
        return restartLocalServer();
    }

    bool restartLocalServer()
    {
        OSCHandler.Instance.closeAllListeners();
        if (OSCHandler.Instance.Servers.ContainsKey("LocalHost"))
            OSCHandler.Instance.Servers.Remove("LocalHost");
        try
        {
            localListener = OSCHandler.Instance.CreateServer("LocalHost", listenPort);
            localListener.server.PacketReceivedEvent += newOScPacket;

            started = true;
            Debug.Log("started listenning at port "+listenPort);
        }
        catch (Exception e)
        {
            Debug.Log("local port "+listenPort+" failed " + e.Message);
            return false;
        }

        if (OSCHandler.Instance.Servers.ContainsKey("LocalHost"))
        {
            if (logRecieve) log("localhost open running on port : " + OSCHandler.Instance.Servers["LocalHost"].server.LocalPort);
            Debug.Log("localhost open running on port : " + OSCHandler.Instance.Servers["LocalHost"].server.LocalPort);
        }

        return true;
    }

    public static string printableBlob(byte[] b, int length, int start = 0)
    {

        string s = " ->: ";
        if (length > b.Length) length = b.Length;
        for (int i = 0; i < b.Length; i++)
            s += "[" + (b[i] > 65 ? ((char)b[i]).ToString() : " ") + "] ";

        return s;



    }

#pragma warning disable 414
    List<int> pendingAcks;
#pragma warning restore 414

#pragma warning disable 649
    class AckRequest
    {
        public OSCPacket message;
        public float time;
        public int requested;

    }
#pragma warning restore 649

    void Start()
    {
          if (!string.IsNullOrEmpty(targetAddr)) SetReciever(targetAddr,targetPort);
       // SetReciever("127.0.0.1", listenPort);
    }

    void Update()
    {
        while (recievePacketQueue.Count > 0)
        {
            reactToPacket(recievePacketQueue[0]);
            recievePacketQueue.RemoveAt(0);
        }

    }
    void Awake()
    {
        pendingAcks = new List<int>();
        _instance = this;

        mainRouter = new OSCRouter("");
        
        //replyRouter = new OSCRouter("/");

        recievePacketQueue = new List<OSCPacket>();
        setLocalPort(defaultRecievePort);
      
    }


    #endregion
/*

    public static void request(string address, Action<float> parseReply)
    {
        //   if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
        replyRouter.bind(null, parseReply,address );
        broadcastOSC("/get" + address);
    }
    public static void request(string address, Action<string[]> parseReply)
    {
        if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
        replyRouter.bind(null, parseReply,address + returnAddress);
        instance.listBindAdresses(address);
        //broadcastOSC(address+query);

    }

    public static void bindReply(MonoBehaviour source, Action<string[]> parseReply, string address)
    {
        if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
        replyRouter.bind(source, parseReply,returnAddress+address);
        
        Debug.Log("reply bind "+"returnAddress+address");
        
        instance.listBindAdresses(address);
        broadcastOSC(address);

    }
*/

    void listBindAdresses(string addressSoFar)
    {
        List<string> availableAddresses = new List<string>();

        for (int i = 0; i < routers.Count; i++)
            routers[i].listBindAdresses(addressSoFar, ref availableAddresses);

        OSCMessage m = new OSCMessage(returnAddress+addressSoFar);
        foreach (string s in availableAddresses)
            m.Append(s);
        broadcastOSC(m);


    }


/*

    public static void request(string address, Action<float[]> parseReply)
    {
        if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
        replyRouter.bind(this,address, parseReply);
        broadcastOSC("/get" + address);
    }

    public static void request(string address, Action<byte[]> parseReply)
    {
        if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
        replyRouter.bind(this,address, parseReply);
        broadcastOSC("/get" + address);
    }


    public static void request(string address, Action<List<object>> parseReply)
    {
        if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
        replyRouter.bind(this,address, parseReply);
        broadcastOSC("/get" + address);
    } 
    
  */
/*
    string[] listOSCCommands()
    {
        return bindAddresses.ToArray();
    } */
    
    public static void console(string s)
    {

        broadcastOSC("/console/status", s);
    }


}


