///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
using System.Collections;
//using System.Collections.Generic;

public class OSCSendRotation : MonoBehaviour
{
public string address="/rot1";
    Quaternion lastSentRotation;
  public float interval=0.1f;
    void Start()
    {
          StartCoroutine(sendRots());
    }

    IEnumerator sendRots()
    {

        while (true)
        {    yield return new WaitForSeconds(interval);
            if (transform.localRotation != lastSentRotation)
            {
                lastSentRotation = transform.localRotation;

                zOSC.broadcastOSC(address, lastSentRotation);

            }
        
        }
    }

}
