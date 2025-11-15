using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class BlockEnter : MonoBehaviour
{
    private InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
        inputField.onValidateInput += ValidateInput;
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (addedChar == '\n' || addedChar == '\r')
        {
            return '\0';
        }
        return addedChar;
    }
}
