///zambari codes unity

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class ConsoleSuggestions : zNodeController
{
    zOscConsole console;

    public void onNewText(string s)
    {try {
        
       // scrollBar.value=0;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].nodeName.Length>=s.Length && nodes[i].nodeName.StartsWith(s)) nodes[i].gameObject.SetActive(true);
            else nodes[i].gameObject.SetActive(false);

        }
        if (activeNodePresent())
        if (activeNodeIndex>=0 && activeNodeIndex<nodes.Count)
            if (!nodes[activeNodeIndex].gameObject.activeInHierarchy) activeNodeIndex=-1;
    }catch {
        Debug.Log("wrong ",gameObject);
    }
     //   setScrollStateDirty();

    }
     public void hideAll()
    {
     //   for (int i = 0; i < nodes.Count; i++)
      
    //         nodes[i].gameObject.SetActive(false);
    }
   public override void OnUp()
    {
       base.OnUp();
     //if (activeNodePresent())
//       console.setText(nodes[activeNodeIndex].nodeName);
    }
    public override void OnDown()
    {
        base.OnDown();
        //if (activeNodePresent())
       //console.setText(nodes[activeNodeIndex].nodeName);
    }
protected override void Start()
{
  base.Start();
 console=GetComponentInParent<zOscConsole>();
}
  public override void NodeClicked(zNode node)
    {
        Debug.Log("lcick "+node.nodeName);
        console.inputField.text=node.nodeName;
        
    }
}
