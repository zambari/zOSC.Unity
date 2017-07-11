///zambari codes unity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class zButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    public Color nonHoveredColor = Color.gray;
    public Color hoveredColor = Color.black;
    public Color pressedColor = Color.black;
    public KeyCode keyBind;
    public bool muteOSC = true;
    public bool localPressSignal;


    protected virtual void OSCBind()
    {
//Debug.Log("binding "+OSCAddress);
      //  zOSC.bind(this, SignalClick,OSCAddress);
        
    }
    protected virtual void OSCUnbind()
    {
        //check;
      //  zOSC.unbind(lastAddress);
    }

    void Start()
    {
     
        image = GetComponent<Image>();


        OSCBind();
    }

    public virtual void OnClick()
    {
      //  if (!muteOSC)
      //      zOSC.broadcastOSC(OSCAddress);
        if (localPressSignal) SignalClick();
    }
    public virtual void SignalClick()
    {
        image.color = pressedColor;
        Invoke("restoreColor", 0.1f);
    }
    void restoreColor()
    {
        image.color = nonHoveredColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
   protected  void OnValidate()
    {
        image = GetComponent<Image>();
        image.color = nonHoveredColor;
        //base.OnValidate();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoveredColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = nonHoveredColor;

    }
}