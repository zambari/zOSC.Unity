using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class testh : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
	public void OnPointerEnter(PointerEventData e)
	{
		Debug.Log("3dh");

	}
	public void OnPointerExit(PointerEventData e)
	{
		Debug.Log("exit");

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
