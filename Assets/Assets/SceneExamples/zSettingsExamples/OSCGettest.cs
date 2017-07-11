///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCGettest : MonoBehaviour {
public bool bindGet;
public bool request;
public bool forcereply;
public bool unbind;
public string[] response;
public bool requestFloat;
public bool addProvider;



void OnValidate()
{
    if (requestFloat)
    {
        requestFloat=false;
       // zOSC.request("/myFloat",reactToFloat);
            zOSC.broadcastOSC("/get/floatValue");
      //  /myFloat
    }
    if (addProvider)
    {
        addProvider=false;
        zOSC.mainRouter.addFloatProvider(this,getSomeFloat,"/myFloat");
    }

    if (request) 
    { 
        request=false;
    //   zOSC.request("/OSCBind",reactToFloatArray);
    }
      if (bindGet) 
    {   bindGet=false;
    //    zOSC.addGet("/OSCBind",getBindHandler);

    zOSC.get.addFloatArrayProvider(this,provideFloats,"/floatValue");


    }
    if (forcereply) 
    {   forcereply=false;
         // zOSC.broadcastOSC("/reply/OSCBind","ala ma kota");
            string[] ss=new string[5];
            for (int i=0;i<5;i++) ss[i]="forcedstring"+i;
            zOSC.broadcastOSC("/reply/OSCBind",ss);
    }
//    if (unbind)
//           zOSC.request("/oscCommands",reactToStringtArray);

           
}
string provideString()
{
   return "aa";

}

void returningFloatValue(float f)
{
    Debug.Log("is returning float "+f);

}
float getSomeFloat()
{
  return 0.777f;
}

void reactToStringtArray(string[] stra)
{
    response=stra;

}
float[] provideFloats()
{
   float[] ss=new float[10];
    for (int i=0;i<10;i++) ss[i]=i;
    return ss;

}

void reactToFloatArray(float[] stra)
{
Debug.Log("recieved aray via osc,");
foreach(float s in stra)
{
    Debug.Log(s);
}

}
void Start()
{
// OSCRouter myRouter=new OSCRouter("/go");
// myRouter.bind(this,"/numchildren",numObjs);

 //zOSC.bind(this,"/strinArrTest",reactToStringArray);



}
/*void getBindHandler()
{
    Debug.Log("getOSCBind handler called");
   /*   string[] ss=new string[5];
    for (int i=0;i<5;i++) ss[i]="reply string"+i;
    zOSC.broadcastOSC("/reply/OSCBind",ss);
     float[] ss=new float[10];
    for (int i=0;i<10;i++) ss[i]=i;
     zOSC.broadcastOSC("/reply/OSCBind",ss);
}
*/
	
}
