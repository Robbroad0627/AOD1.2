using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxInput : MonoBehaviour
{
    public InputField nameInputField;

    private string lastTypedString = "";

    private bool canType = true;
    
    public void LimitString()
    {
        var str = nameInputField.text;
        
        if(canType)
            lastTypedString = str;
        
        if (str.Length > 32)
        {
            nameInputField.text = lastTypedString;
            canType = false;
        }
        else
        {
            canType = true;
        }
        
    }
    
}
