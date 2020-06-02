///zambari codes unity

using System;
using System.Text;
using UnityEngine;
using UnityOSC;


// extensions of UnityOSC's packet class by zambari/stereoko.tv 2017
// v.0.1
// v.0.1.1 vector3 bug removed


public static class OSCPacketExtensions
{


    public static float getFloat(this OSCPacket packet, int index)
    {
        float value = 0;
        if (packet.typeTag[index + 1] == 'f' && Single.TryParse(packet.Data[index].ToString(), out value))
            return value;
        else
            return -2.99f;
    }

    public static bool getString(this OSCPacket packet, out string value)
    {
        value = "";
        if (packet.typeTag[1] != 's') return false;
        value = packet.Data[0].ToString();
        return true;

    }
    public static bool getStringArray(this OSCPacket packet, out string[] value)
    {
        value = new string[0];
        if (packet.typeTag.Length < 3) return false;
        for (int i = 1; i < packet.typeTag.Length; i++)
            if (packet.typeTag[i] != 's') return false;
        value = new string[packet.typeTag.Length - 1];
        for (int i = 0; i < packet.typeTag.Length - 1; i++)
            value[i] = packet.Data[i].ToString();
        return true;
    }
    public static bool getFloat(this OSCPacket packet, int index, out float value)
    {
        value = 0;
        if (packet.typeTag[index + 1] == 'f' && Single.TryParse(packet.Data[index].ToString(), out value))
            return true;
        else
            return false;
    }


    public static bool getFloat(this OSCPacket packet, out float value)
    {
        value = 0;
        if (packet.typeTag[1] == 'f' && Single.TryParse(packet.Data[0].ToString(), out value))
            return true;
        else
            return false;
    }


    public static string AsString(this OSCPacket packet)
    {
        string typeTag = packet.typeTag;
        StringBuilder sb = new StringBuilder();
        sb.Append("<b>" + packet.Address + "</b>");
        if (typeTag.Length > 1)
        {
            sb.Append(" [");
            sb.Append(typeTag);
            sb.Append("] ");
            for (int i = 0; i < typeTag.Length - 1; i++)
            {

                switch (typeTag[i + 1])
                {
                    case 's':
                        sb.Append(packet.Data[i].ToString());
                        break;
                    case 'f':
                        float f = -99;
                        if (Single.TryParse(packet.Data[i].ToString(), out f))
                            sb.Append((Mathf.Round(f * 100) / 100).ToString());
                        else sb.Append("[X]");
                        break;
                    case 'i':
                        int ii;
                        if (Int32.TryParse(packet.Data[i].ToString(), out ii))
                            sb.Append(ii.ToString());
                        else sb.Append("[X]");

                        break;
                    case 'b':
                        sb.Append("[BLOB]");

                        break;

                }
                sb.Append(" ");
            }
        }
        //        Debug.Log(sb.ToString());
        return sb.ToString();


    }
    public static bool getQuaternion(this OSCPacket packet, out Quaternion q)
    {
        q = Quaternion.identity;
        if (packet.typeTag[1] != 'f' ||
           packet.typeTag[2] != 'f' ||
           packet.typeTag[3] != 'f' ||
           packet.typeTag[4] != 'f') return false;
        float f1, f2, f3, f4;
        if (Single.TryParse(packet.Data[0].ToString(), out f1))
            if (Single.TryParse(packet.Data[2].ToString(), out f2))
                if (Single.TryParse(packet.Data[3].ToString(), out f3))
                    if (Single.TryParse(packet.Data[4].ToString(), out f4))
                    {
                        q = new Quaternion(f1, f2, f3, f4);
                        return true;
                    }

        return false;
    }


    public static bool getVector3(this OSCPacket packet, out Vector3 v)
    {
        v = Vector3.zero;
        float f1, f2, f3;
        if (packet.typeTag[1] != 'f' ||
            packet.typeTag[2] != 'f' ||
            packet.typeTag[3] != 'f') return false;
        if (Single.TryParse(packet.Data[0].ToString(), out f1))
            if (Single.TryParse(packet.Data[1].ToString(), out f2))
                if (Single.TryParse(packet.Data[2].ToString(), out f3))
                {
                    v = new Vector3(f1, f2, f3);
                    return true;
                }

        return false;
    }


}

// and a snippet from the stack overflow

public static class Extensions
{
    /// <summary>
    /// Get the array slice between the two indexes.
    /// ... Inclusive for start index, exclusive for end index.
    /// </summary>
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
        // Handles negative ends.
        if (end < 0)
        {
            end = source.Length + end;
        }
        int len = end - start;

        // Return new array.
        T[] res = new T[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = source[i + start];
        }
        return res;
    }
}
