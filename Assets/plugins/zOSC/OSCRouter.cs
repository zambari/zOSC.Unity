///zambari codes unity

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityOSC;
using System;

public class OSCRouter
{
    public string baseAddress;
    protected int baseAddressLen;
    protected bool isRoot;
    public bool oneShot;


    // protected Dictionary<string, object> objectDict; // gdzied dodaje do obejct a nie dodaje do rtp
    // protected Dictionary<string, Type> objectTypeDict;
    protected Dictionary<string, Binding> oscBinding;

    protected List<KeyValuePair<string, Binding>> oscPartialBinding;

    protected class Binding
    {
        //public long addresshash;
        public Type objectType;
        public string address;
        public object bindMethod;
        public MonoBehaviour requester;
        public Binding nextBind;
    }

    #region bindOverloads
    public void bindGeneric<T>(MonoBehaviour requester, T listener, string addr)
    {
        if (String.IsNullOrEmpty(addr)) return;

        //     Debug.Log("bind failed, address "+addr+" is already taken ");
        //  else
        //   {
        if (addr[addr.Length - 1] == '*')
        {
            Debug.Log("binding class");
            bindClass(requester, listener, addr);

        }
        else
        {
            Binding newbinding = new Binding();
            newbinding.address = addr;
            newbinding.bindMethod = listener;
            newbinding.objectType = listener.GetType();
            newbinding.requester = requester;
            if (oscBinding.ContainsKey(addr))
            {
                Binding currentBinding = oscBinding[addr];
                newbinding.nextBind = currentBinding;
                oscBinding.Remove(addr);
                //            Debug.Log("chained bind "+addr+"  " + requester.name + " previous was " + currentBinding.requester.name);
            }
            oscBinding.Add(addr, newbinding);
        }
    }

    public void bind(MonoBehaviour requester, Action listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<float> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<string> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<byte[]> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<float[]> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<string[]> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<List<object>> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<Vector3> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void bind(MonoBehaviour requester, Action<Quaternion> listener, string addr)
    { bindGeneric(requester, listener, addr); }
    public void addStringArrayProvider(MonoBehaviour monoSource, Func<string[]> provider, string addr)
    { bindGeneric(monoSource, provider, addr); }
    public void addFloatArrayProvider(MonoBehaviour monoSource, Func<float[]> provider, string addr)
    { bindGeneric(monoSource, provider, addr); }
    public void addStringProvider(MonoBehaviour monoSource, Func<string> provider, string addr)
    { bindGeneric(monoSource, provider, addr); }
    public void addFloatProvider(MonoBehaviour monoSource, Func<float> provider, string addr)
    { bindGeneric(monoSource, provider, addr); }

    public virtual void bind<T>(MonoBehaviour monoSource, Func<T> provider, string addr)
    { bindGeneric(monoSource, provider, addr); }




    public void bindClass<T>(MonoBehaviour requester, T listener, string addr)
    {

        if (String.IsNullOrEmpty(addr)) return;

        if (addr[addr.Length - 1] == '*') addr = addr.Substring(0, addr.Length - 1);
        Debug.Log(addr + " by " + requester.name);

        Binding newbinding = new Binding();
        newbinding.address = addr;
        newbinding.bindMethod = listener;
        newbinding.objectType = listener.GetType();
        newbinding.requester = requester;
        Debug.Log("adding partial binding " + addr + "  " + requester.name);
        oscPartialBinding.Add(new KeyValuePair<string, Binding>(addr, newbinding));

        //  List<Binding> thisList;

        /*
        if (oscPartialBinding.ContainsKey(addr))
         {
             thisList=oscPartialBinding[add]
             Binding currentBinding = oscBinding[addr];
             newbinding.nextBind = currentBinding;
             oscBinding.Remove(addr);
             Debug.Log("chained bind "+addr+"  " + requester.name + " previous was " + currentBinding.requester.name);
         }
         oscBinding.Add(addr, newbinding);*/


    }


    #endregion bindOverloads

    #region useBind

    bool use(Action listener, OSCPacket packet)
    {
        //        Debug.Log("trying void");
        if (listener == null) Debug.Log("no listener");
        else { listener.Invoke(); }
        return true;

    }


    bool use(Action<float> listener, OSCPacket packet)
    {
        float value;
        if (packet.getFloat(out value))
        {
            listener.Invoke(value);
            return true;
        }
        return false;

    }
    bool use(Action<string> listener, OSCPacket packet)
    {
        string value;
        if (packet.getString(out value))
        {
            listener.Invoke(value);
            return true;
        }
        return false;

    }
    bool use(Action<string[]> listener, OSCPacket packet)
    {
        string[] value;
        if (packet.getStringArray(out value))
        {
            listener.Invoke(value);
            return true;
        }
        return false;

    }
    bool useBinding(Binding listener, OSCPacket packet)
    {

        bool returned = false;

        Type savedType = listener.objectType;

        if (savedType == typeof(Action))
            returned = use(((Action)listener.bindMethod), packet);
        else
        if (savedType == typeof(Action<float>))
            returned = use(((Action<float>)listener.bindMethod), packet);
        else
         if (savedType == typeof(Action<string>))
        {
            returned = use((Action<string>)listener.bindMethod, packet);
        }
        else
          if (savedType == typeof(Action<string[]>))
            returned = use((Action<string[]>)listener.bindMethod, packet);



        object provider = listener.bindMethod;


        if (savedType == typeof(Func<string>))
        {
            var returnVal = ((Func<string>)provider)();
            zOSC.broadcastOSC(zOSC.returnAddress + packet.Address, returnVal);
            returned = true;

        }
        if (savedType == typeof(Func<float>))
        {
            var returnVal = ((Func<float>)provider)();
            Debug.Log("broadcasting return float " + returnVal);
            zOSC.broadcastOSC(zOSC.returnAddress + packet.Address, returnVal);

            returned = true;
        }
        if (savedType == typeof(Func<float[]>))
        {
            var returnVal = ((Func<float[]>)provider)();
            zOSC.broadcastOSC(zOSC.returnAddress + packet.Address, returnVal);
            returned = true;
        }
        if (savedType == typeof(Func<string[]>))
        {
            var returnVal = ((Func<string[]>)provider)();
            zOSC.broadcastOSC(zOSC.returnAddress + packet.Address, returnVal);
            returned = true;
        }

        if (oneShot) // typically this means this is a request, and before we reply we remove it from the bind list
        {
            oscBinding.Remove(listener.address);
        }
        if (listener.nextBind != null)

        {
            //     Debug.Log("going for reccuence");
            useBinding(listener.nextBind, packet);
            //    Debug.Log("aand we're back");

        }


        return returned;
    }

    public virtual bool parsePacket(OSCPacket packet)
    {
        if (String.IsNullOrEmpty(packet.Address))
        {
            Debug.Log("null packet address"); return false;

        }
        //        Debug.Log(baseAddressLen + " " + packet.Address);
        try
        {

            string address = packet.Address.Substring(baseAddressLen);

            for (int i = 0; i < oscPartialBinding.Count; i++)
            {
                string thisString = oscPartialBinding[i].Key;
                if (address.StartsWith(thisString))
                    useBinding(oscPartialBinding[i].Value, packet);
                else Debug.Log("not matched");

            }



            Binding listener;
            if (oscBinding.TryGetValue(address, out listener))
            {

                try
                {

                    return useBinding(listener, packet);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error using binding " + e.Message);
                    if (listener.requester != null)
                        Debug.LogError("Binding info: " + address + " Monobehaviour: " + listener.requester.GetType().ToString() + " on gameobject " + listener.requester.name + " " + listener.objectType.ToString() + " " + listener.bindMethod.GetType().ToString());
                }

            }


        }
        catch (Exception e) { Debug.LogError("invalid string " + e.Message); }

        //string address;
        return true;
    }

    public OSCRouter(string s)
    {

        baseAddress = s;
        baseAddressLen = baseAddress.Length;

        oscBinding = new Dictionary<string, Binding>();
        oscPartialBinding = new List<KeyValuePair<string, Binding>>();

        zOSC.instance.AddRouter(this);


    }
    #endregion useBind


    public void listBindAdresses(string startStrting, ref List<string> list)
    {
        foreach (string s in oscBinding.Keys)
            if (!list.Contains(s))
                list.Add(s);
    }

    bool checkIfFloatArray(string address, OSCPacket packet)
    {
        string typeTag = packet.typeTag;

        if (typeTag.Length <= 1) return false;
        //     if (!floatArrayListeners.ContainsKey(address)) return false;
        for (int i = 1; i < typeTag.Length; i++)
            if (typeTag[i] != 'f')
                return false;
        //        Action<float[]> floatArrAction;
        //     if (floatArrayListeners.TryGetValue(address, out floatArrAction))
        {
            float[] floatArr = new float[typeTag.Length - 1];
            float nextFloat = 0;
            for (int i = 0; i < typeTag.Length - 1; i++)
            {
                if (Single.TryParse(packet.Data[i].ToString(), out nextFloat))
                    floatArr[i] = nextFloat;
                else
                {
                    Debug.Log("parsing float failed" + address + "   " + packet.Data[i].ToString());
                    return false;
                }
            }
            //    floatArrAction(floatArr);
            //
        }
        { /*Debug.Log("t");*/ return true; }

    }
    protected virtual void reportUnBind<T>(string addr)
    {
        //   zOSC.reportUnBind(addr);
    }
    protected virtual void reportBind<T>(string addr)
    {
        //  zOSC.reportBind(addr);
    }

    public virtual void unbind(MonoBehaviour requester, string addr)
    {
        if (String.IsNullOrEmpty(addr)) return;
        if (oscBinding.ContainsKey(addr))
        {
            Binding thisBinding = oscBinding[addr];
            if (thisBinding.requester != requester)
                while (thisBinding.nextBind != null)
                {
                    thisBinding = thisBinding.nextBind;

                }

            oscBinding.Remove(addr);
        }

    }

    protected bool stringWrong(string s)
    {
        if (String.IsNullOrEmpty(s))
        { /*Debug.Log("t");*/ return true; }
        if (s[0] != '/' || (s.Length < 2))
        {
            Debug.Log("osc adresses should start with '/'");
            return true;
        }

        return false;
    }
}
