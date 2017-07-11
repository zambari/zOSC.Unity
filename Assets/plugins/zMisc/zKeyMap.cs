﻿//z2k17

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;
using System;

public class zKeyMap : MonoBehaviour
{
    static List<KeyCode> k;
    static List<MonoBehaviour> m;
    static List<Action> onUpList;
    static List<Action> onDownList;
    static zKeyMap _instance;
    void Awake()
    {
        if (_instance != null) Debug.Log("more than one keymap on scene", gameObject);
        _instance = this;
        if (k == null)
        {
            createLists();
        }
    }


    static void createLists()

    {
        if (_instance == null)
        {
            GameObject g = new GameObject("keymap");
            g.AddComponent<zKeyMap>();
        }
        k = new List<KeyCode>();
        m = new List<MonoBehaviour>();
        onUpList = new List<Action>();
        onDownList = new List<Action>();
    }
    public static bool map(MonoBehaviour mono, KeyCode key, Action actionOnDown, Action actionOnUp = null)
    {
        if (k == null)
        {
            createLists();
        }
        if (k.Contains(key))
        {
            Debug.Log("key " + key.ToString() + "is  mapped already, sorry");
            return false;
        }
        k.Add(key);
        m.Add(mono);
        onDownList.Add(actionOnDown);
        onUpList.Add(actionOnUp);
        return true;
    }

    /// Depreciated

    public static bool map(MonoBehaviour mono, Action actionOnDown, KeyCode key)
    {
        return map(mono, actionOnDown, key);
    }

    public static bool map(MonoBehaviour mono, KeyCode key, Action actionOnDown)
    {
        return map(mono, actionOnDown, key);
    }



    void Update()
    {
        for (int i = 0; i < k.Count; i++)
            if (Input.GetKeyDown(k[i]))

            {
                if (m[i].isActiveAndEnabled)
                {
                    if (onDownList[i] != null)
                        onDownList[i]();

                }
            }
            else
           if (Input.GetKeyUp(k[i]))
            {
                if (m[i].isActiveAndEnabled)
                {
                    if (onUpList[i] != null)
                        onUpList[i]();
                }

            }

    }


}
