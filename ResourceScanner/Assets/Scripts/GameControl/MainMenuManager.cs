using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    [SerializeField, Tooltip("Placeholder Input Fields")]
    private Text m_placeholderRows, m_placeholderFull, m_placeholderScans, m_placeholderExtracts;
    [SerializeField, Tooltip("Input Fields")]
    private Text m_textRows, m_textFull, m_textScans, m_textExtracts;
    [SerializeField, Tooltip("The main menu panel")]
    private GameObject m_panel = null;

    public void StartGame()
    {
        int rows;
        int.TryParse(m_textRows.text, out rows);
        int fullTiles;
        int.TryParse(m_textRows.text, out fullTiles);
        int scans;
        int.TryParse(m_textRows.text, out scans);
        int extracts;
        int.TryParse(m_textRows.text, out extracts);

        if (int.TryParse(m_textRows.text, out rows) == false)
        {
            int.TryParse(m_placeholderRows.text, out rows);
        }
        if (int.TryParse(m_textRows.text, out fullTiles) == false)
        {
            int.TryParse(m_placeholderFull.text, out fullTiles);
        }
        if (int.TryParse(m_textRows.text, out scans) == false)
        {
            int.TryParse(m_placeholderScans.text, out scans);
        }
        if (int.TryParse(m_textRows.text, out extracts) == false)
        {
            int.TryParse(m_placeholderExtracts.text, out extracts);
        }

        GameController.Instance.StartGame(scans, extracts, rows, fullTiles);
        m_panel.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
