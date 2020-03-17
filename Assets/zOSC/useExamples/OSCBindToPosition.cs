///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class OSCBindToPosition : OSCBindBasic
{

    void setPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }
    protected override void OSCBind()
    {
        zOSC.BindVector3(this,OSCAddress, setPos );
    
    }
    protected override void OSCUnbind()
    {
        zOSC.Unbind(lastAddress);
    }


}
