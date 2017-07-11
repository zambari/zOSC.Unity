///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCBindBasic : MonoBehaviour {
/*


///  example


  protected override void OSCBind()
  {
    zOSC.bind(this,OSCAddress,setPos);
  }
   protected override void OSCUnbind()
   {
        zOSC.unbind(lastAddress,setPos);
   }
  



 */
	
	public string OSCAddress="/oscAddr";
protected    string lastAddress;

protected virtual void OSCUnbind()
{

}

protected virtual void OSCBind()
{

}
protected virtual  void OnValidate()
{
 if (string.IsNullOrEmpty(OSCAddress)) OSCAddress="/my/osc/revieve/address";
    if (!string.IsNullOrEmpty(lastAddress))
    {
    OSCUnbind();
    }

    Invoke("_bind",0.1f);
}

void OnDisable()
{
  OSCUnbind();
}

void _bind()
{ 
    if (!gameObject.activeInHierarchy) return;
    if (zOSC.instance==null) return;
//        Debug.Log("bindin0g "+OSCAddress,gameObject);
        OSCBind();
        lastAddress=OSCAddress;
}

}
