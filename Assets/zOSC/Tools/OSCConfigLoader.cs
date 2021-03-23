using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSCConfigLoader : MonoBehaviour
{
    [ExposeMethodInEditor]
    void Start()
    {
#if !UNITY_EDITOR
        OSCConfig config = OSCConfig.instance;
        zOSC.SetTarget(config.targetIP, config.targetPort);
        Debug.Log($"set target {config.targetIP} {config.targetPort}");
#else
        Debug.Log("unity editor- not loading congig");
#endif
    }


}
