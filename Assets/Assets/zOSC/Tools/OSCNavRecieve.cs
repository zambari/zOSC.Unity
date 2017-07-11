///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public interface INavigateKeypad
{
  void OnUp();
  void OnDown();
  void OnLeft();
  void OnRight();
  void OnEnter();
  void OnEscape();
 
  void OnFocus();
  void OnLooseFocus();
}

public class OSCNavRecieve : MonoBehaviour {

public bool recieve;

INavigateKeypad dest;
public bool focusWithEnter=true;
public bool startWithHide=true;
void Start()
{
    dest=GetComponent<INavigateKeypad>();
        if (dest!=null)
        {
                zOSC.bind(this,dest.OnUp,"/keypad/up");
                zOSC.bind(this,dest.OnDown,"/keypad/down");
                zOSC.bind(this,dest.OnLeft,"/keypad/left");
                zOSC.bind(this,dest.OnRight,"/keypad/right");
                zOSC.bind(this,dest.OnEscape,"/keypad/esc");
                zOSC.bind(this,dest.OnEnter,"/keypad/enter");
             if (focusWithEnter)
             {
                zOSC.bind(this,dest.OnFocus,"/keypad/enter");
                zOSC.bind(this,dest.OnLooseFocus,"/keypad/esc");
             }
        }
               /* zOSC.bind(this,"/keypad/up",()=>recievedNav("up"));
                zOSC.bind(this,"/keypad/down",()=>recievedNav("down"));
                zOSC.bind(this,"/keypad/left",()=>recievedNav("left"));
                zOSC.bind(this,"/keypad/right",()=>recievedNav("right"));
                zOSC.bind(this,"/keypad/esc",()=>recievedNav("esc"));
                zOSC.bind(this,"/keypad/enter",()=>recievedNav("enter"));
                }*/
                if (startWithHide) 
                {
                  dest.OnLooseFocus();
                }
}

void recievedNav(string nav)
{
Debug.Log("nav recieved "+nav);



}
	
}
