using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class CloneButton : MonoBehaviour {
 [HideInInspector]
 [SerializeField]

 Button button;

public GameObject cloneSource;
	void OnValidate()
	{
		button=GetComponent<Button>();
	}
	void Start () {
	button.onClick.AddListener(AddClone);
	}

	void AddClone()
	{
		if (cloneSource==null) return;
		GameObject newObj=Instantiate(cloneSource,cloneSource.transform.parent);
		newObj.SetActive(true);
		newObj.transform.SetSiblingIndex(newObj.transform.parent.childCount-2);
	}
	
}
