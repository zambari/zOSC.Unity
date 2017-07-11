using UnityEngine;

public class testKeyboardMap : MonoBehaviour
{
    public KeyCode mappedKey;
    public bool myToggle;
    void onKey()
    {
        myToggle = !myToggle;
    }

    void Start()
    {
      //  zKeyMap.map(this, onKey, mappedKey);
    }

    void Update()
    {
        if (Input.GetKeyDown(mappedKey)) onKey();

    }

}
