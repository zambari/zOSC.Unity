///zambari codes unity

using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class zOscConsole : MonoBehaviour
{
    public static zOscConsole instance;
    public InputField inputField;
    public zLog backLog;
    public ConsoleSuggestions suggestions;

    //string OSCAddress="/console";
    protected virtual void Awake()
    { 
        
    
        if (instance != null) Debug.LogWarning("two consoles !", gameObject);
        instance = this;

        if (inputField == null)
            inputField = GetComponentInChildren<InputField>();
        inputField.onEndEdit.AddListener(onEndEdit);
        inputField.onValueChanged.AddListener(onTextChange);

        zKeyMap.map(this, keyTab, KeyCode.Tab);
        zKeyMap.map(this, keySlash, KeyCode.Slash);
    }

public void setText(string s)
{

     inputField.text=s;
}
void prepareAndSendMessage()
{
        string[] parts = inputField.text.Split(' ');
        OSCMessage message = new OSCMessage(parts[0]);
        for (int i = 1; i < parts.Length; i++)
        {
            float floatval = 0;
            if (Single.TryParse(parts[i], out floatval))
{                message.Append(floatval); Debug.Log("added float");

}
            else
         { message.Append(parts[i]);
          Debug.Log("added as string");
             }
            

            //    zNode bl=backLog.AddNode(parts[i]);
            //    zNode sn=suggestions.AddNode(parts[i]);
        }

        //  enabled=false;
      zOSC.broadcastOSC(message);
}
    public void onEndEdit(string text)
    {
        
        string[] parts = text.Split(' ');
        OSCMessage message = new OSCMessage(parts[0]);
        for (int i = 1; i < parts.Length; i++)

        {
            float floatval = 0;
            if (Single.TryParse(parts[i], out floatval))
                message.Append(floatval);
            else message.Append(parts[i]);

            //    zNode bl=backLog.AddNode(parts[i]);
            //    zNode sn=suggestions.AddNode(parts[i]);
        }

        //  enabled=false;
        //zOSC.broadcastOSC(message);

    }

    void keySlash()
    {

        if (!inputField.isFocused)
        {
            if (inputField.text.Length == 0) inputField.text = "/";
            inputField.Select();
            inputField.ActivateInputField();

            return;
        }
    }

    void focusOnInputField()
    {
     if (inputField.text.Length == 0) inputField.text = "/";
            inputField.Select();
            inputField.ActivateInputField();
            StartCoroutine(MoveTextEnd_NextFrame());


    }
    void keyTab()
    {
        if (!inputField.isFocused)
        
             focusOnInputField();
        
        else

            zOSC.broadcastOSC(inputField.text + "?");
    }


    void recieveSuggestions(string[] newSuggestions)
    {
        Debug.Log("recieved suggestions, SUCCESS " + newSuggestions.Length);
        suggestions.Clear();
        for (int i = 0; i < newSuggestions.Length; i++)
            suggestions.AddNode(newSuggestions[i]);

    }

    IEnumerator MoveTextEnd_NextFrame()
    {
        yield return null; // Skip the first frame in which this is called.
        inputField.MoveTextEnd(false); // Do this during the next frame.
    }

    void Start()
    {
        zOSC.OnOSCRecieve += peeklastRecieve;
        inputField.onValueChanged.AddListener(suggestions.onNewText);


        zOSC.bind(this, recieveSuggestions, "/_?*");
        Invoke("initialsuggestions", 1);
    }

    void initialsuggestions()
    {
        zOSC.broadcastOSC("/?");

    }

    void peeklastRecieve()
    {
        backLog.AddLine(zOSC.lastRecieved.AsString());
    }

    public void onTextChange(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            inputField.text = "/";
            return;
        }
        if (text[0] != '/')
        {
            inputField.text = '/' + text;
            inputField.MoveTextEnd(false);
            return;
        }
        enabled = true;


    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //if (inputField.isFocused && 
            if (inputField.text.Length > 1)
              prepareAndSendMessage();//  zOSC.broadcastOSC(inputField.text);
           else
            {
                //Debug.Log("not focused?");
                suggestions.activeNodeClicked();
                focusOnInputField();
             }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            suggestions.hideAll();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            suggestions.OnDown();

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            suggestions.OnUp();
        }


    }


}