///zambari codes unity

using System;
using UnityEngine;

public class TransformCrawlerzOSC : MonoBehaviour {

OSCRouter crawlerHelper;
public bool useLocal=true;
  void Start()
  {
    refreshBindings();
  } 


  void refreshBindings()
  {
      
      //crawlerHelper.kill();
//      crawlerHelper = new OSCRouter(name);  // provides context
      //crawlerHelper.bind ( this,setPosition,"/position");
      //crawlerHelper.bind ( this,setRotation,"/rotation");
      //crawlerHelper.bind ( this,setScale,"/scale");
      zOSC.bind(this,setPosition,"/"+name+"/position");
      zOSC.bind(this,setRotation,"/"+name+"/rotation");
      zOSC.bind(this,setScale,"/"+name+"/scale");


      zOSC.mainRouter.addFloatArrayProvider(this,getPosition,"/"+name+"/position");

  }

public void setTransform(float[] transformArray)
{





}
public float[] getPosition()
{
    float [] newPos= new float[3]; // technically vector3 flies as float array anyway
    newPos[0]=transform.position.x;
    newPos[0]=transform.position.y;
    newPos[0]=transform.position.z;

    return newPos;
   

}

public void setPositionx(float newX)
{
    Debug.Log("new x"+newX);
 ///  if (useLocal) transform.localPosition=newPosition;
  // else transform.position=newPosition;


}

public void setPosition(Vector3 newPosition)
{
   if (useLocal) transform.localPosition=newPosition;
   else transform.position=newPosition;


}

public void setScale(Vector3 newScale)
{
    transform.localScale=newScale;

}
public void setRotation(Quaternion newRot)
{
   if (useLocal) transform.localRotation=newRot;
   else transform.rotation=newRot;


}




}
