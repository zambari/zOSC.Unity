///zambari codes unity

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class panelResizer : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{

	public bool changeCursor;
    public Cursor horizontalCursor;
 Image image;
 public Color hoverColor=Color.red;
 void Awake()
 {

     image=GetComponent<Image>();
 }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        image.color =hoverColor;
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
         image.color = new Color(0,0,0,0);
        if (changeCursor) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
    }


}
