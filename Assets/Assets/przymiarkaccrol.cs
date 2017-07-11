//z2k17

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class przymiarkaccrol : MonoBehaviour
{
    public RectTransform content;
    public RectTransform mask;
    public Scrollbar scrollbar;
          bool enableScroll;
   public bool scrollStateDirty;
   public  float scrollAmount;
  public  bool scrollReversed ;

   public  void  OnTransformChildrenChanged()
    {
        Debug.Log("chchc");
    }
public bool test;
void CanvasUpdate()
{
    Debug.Log("postlayuout");
}

void OnValidate()
{  checkReferecnes();
    if (test)
    {
        test=false;
        setScrollStateDirty();
        handleScrollStuff();
    }
     /* if (scrollbar!=null)
  if (scrollReversed) 
    
        scrollbar.direction=Scrollbar.Direction.BottomToTop;
        else 
      scrollbar.direction=Scrollbar.Direction.TopToBottom;*/

    }
    protected virtual void scrollContentMouse(float f)
    {
        if (scrollbar.gameObject.activeInHierarchy)
            if (scrollReversed) f *= -1;
        scrollbar.value = (scrollbar.value - 200 * f / scrollAmount);
    }
    public void setScrollStateDirty()
    {
        scrollStateDirty = true;
    }
    public virtual void scrollContentSlider(float f)
    {
        if (content == null) Debug.Log("no content?", gameObject);
        else
           if (!scrollReversed)
            content.anchoredPosition = new Vector2(0, f * scrollAmount);
        else
            content.anchoredPosition = new Vector2(0, -f * scrollAmount);
    }
  
   protected virtual void handleScrollStuff()
    {
        if (scrollbar == null) return;
        Canvas.ForceUpdateCanvases();
        scrollStateDirty = false;
        bool isOne = scrollbar.value == 1;
        //    if (isOne) Debug.Log("scrolbar is one"); else Debug.Log("scrollbar is not one");

        float contentHeight = content.rect.height;
        float maskHeight = mask.rect.height;
         if (contentHeight < maskHeight)
        {
            scrollAmount = 0;
            scrollbar.gameObject.SetActive(false);
        }
        else
        {
            scrollbar.gameObject.SetActive(true);
            scrollbar.size = 1 - (contentHeight - maskHeight) / maskHeight;
            scrollAmount = (contentHeight - maskHeight);
            if (scrollAmount == 0) scrollbar.value = 1;
            else
                scrollbar.value = (content.anchoredPosition.y) / scrollAmount;
        }
        if (isOne)
        {
            scrollbar.value = 1;
            scrollContentSlider(1); // hackish

        }
    }


    void checkReferecnes()
    {
        if (scrollbar==null) scrollbar=GetComponentInChildren<Scrollbar>();
           if (scrollbar != null)
            scrollbar.onValueChanged.AddListener(scrollContentSlider);
        if (content == null)
        {
            var t = transform.Find("CONTENT");

            if (t != null)
            {
                content = t.gameObject.GetComponent<RectTransform>();
            }
            else Debug.Log("no CONTENT");

        }
        if (mask == null)
        {
            var m = GetComponentInChildren<Mask>();
            if (m != null) mask = m.GetComponent<RectTransform>();
            {
               
            }
        }
 if (mask != null && content == null)
                {
                    if (mask.transform.childCount > 0)
                        content = mask.transform.GetChild(0).GetComponent<RectTransform>();

                }
    }

    void Start()
    {

        //transform.chi
    }

}
