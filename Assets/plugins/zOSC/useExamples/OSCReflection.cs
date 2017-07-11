///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;
using UnityOSC;
public class OSCReflection : MonoBehaviour {

  public GameObject go;
OSCRouter reflectionRouter;
void Start()
{
 
 reflectionRouter=new OSCRouter("/ref");
 reflectionRouter.bind(this,findGO,"/find");
  reflectionRouter.bind(this,findGO,"/ddaared");
 reflectionRouter.bind(this,findGO2,"/find2");
 reflectionRouter.bind(this,findGO2,"/bbdd2");
 reflectionRouter.bind(this,findGO,"/froward");
  reflectionRouter.bind(this,findGO2,"/frrend2");
   reflectionRouter.bind(this,findGO2,"/fxxffnd2");
 reflectionRouter.bind(this,findGO,"/aabind");
  reflectionRouter.bind(this,findGO,"/ddaard");

 reflectionRouter.bind(this,findGO,"/ddaared/ldpa");
 reflectionRouter.bind(this,findGO,"/ddaa/dffrd");

}


void setActive(float f)
{
    if (go==null)
     zOSC.console("no gameObject selected");
     else
     {
     go.transform.Translate(go.transform.forward*f);
     zOSC.console("OK moved "+go.name+" by "+f);
     }

}

void forward(float f)
{
    if (go==null)
     zOSC.console("no gameObject selected");
     else
     {
     go.transform.Translate(go.transform.forward*f);
     zOSC.console("OK moved "+go.name+" by "+f);
     }

}


void findGO2(string[] s)
{
Debug.Log("go2 array ");

}
void findGO(string s)
{
    Debug.Log("looking for gameobject "+s);
    go=GameObject.Find(s);
    if (go!=null) 
         zOSC.console("found gameObject "+gameObject.name);
     else
      zOSC.console("not found");
      //zOSC.console()
}

	
}
