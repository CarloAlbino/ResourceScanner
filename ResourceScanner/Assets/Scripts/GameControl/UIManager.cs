using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text m_messageBox;

    public void UpdateMessage(string message)
    {
        if (m_messageBox)
        {
            m_messageBox.text = message;
        }
    }
}
