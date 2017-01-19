using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    [SerializeField]
    private Button m_scanButton, m_extractButton;
    [SerializeField]
    private Color m_activeColor, m_disabledColor;
    private bool m_isScanMode = true;   // False is extract mode

    [SerializeField]
    private int m_maxScans = 3;
    private int m_scansRemaining = 3;
    [SerializeField]
    private int m_maxExtracts = 3;
    private int m_extractsRemaining = 3;

    [SerializeField]
    private Text m_scoreText;
    [SerializeField]
    private int m_fullScore = 1000;
    private int m_score = 0;

    void Start()
    {
        ToggleScanMode();
        UpdateScore(0);
        m_scansRemaining = m_maxScans;
        m_extractsRemaining = m_maxExtracts;
        MessageBox.Instance.QueueUpMessage(m_extractsRemaining + " extractions remaining", Color.green, false);
        MessageBox.Instance.QueueUpMessage(m_scansRemaining + " scans remaining", Color.green, false);
        MessageBox.Instance.QueueUpMessage("Collect as much element X as you can", Color.green);
    }

    public void ToggleScanMode()
    {
        m_isScanMode = true;
        ColorBlock tempColors = m_scanButton.colors;
        tempColors.normalColor = m_activeColor;
        tempColors.highlightedColor = m_activeColor;
        m_scanButton.colors = tempColors;

        tempColors = m_extractButton.colors;
        tempColors.normalColor = m_disabledColor;
        tempColors.highlightedColor = m_disabledColor;
        m_extractButton.colors = tempColors;
    }

    public void ToggleExtractMode()
    {
        m_isScanMode = false;
        ColorBlock tempColors = m_scanButton.colors;
        tempColors.normalColor = m_disabledColor;
        tempColors.highlightedColor = m_disabledColor;
        m_scanButton.colors = tempColors;

        tempColors = m_extractButton.colors;
        tempColors.normalColor = m_activeColor;
        tempColors.highlightedColor = m_activeColor;
        m_extractButton.colors = tempColors;
    }

    public bool IsScanMode()
    {
        return m_isScanMode;
    }

    public void Scan()
    {
        m_scansRemaining--;
        if (m_scansRemaining <= 0)
        {
            MessageBox.Instance.QueueUpMessage("No more scans remaining - switching to extract mode", Color.green);
            ToggleExtractMode();
        }
        else
        {
            MessageBox.Instance.QueueUpMessage(m_scansRemaining + " scans remaining", Color.green);
        }
    }

    public int ScansRemaining()
    {
        return m_scansRemaining;
    }

    public void Extract()
    {
        m_extractsRemaining--;
        if(m_extractsRemaining <= 0)
        {
            MessageBox.Instance.QueueUpMessage("GAME OVER! " + m_score + "kgs of element X extracted", Color.green);
        }
        else
        {
            MessageBox.Instance.QueueUpMessage(m_extractsRemaining + " extractions remaining", Color.green);
        }
    }

    public int ExtractsRemaining()
    {
        return m_extractsRemaining;
    }

    public void UpdateScore(float pointMultiplier)
    {
        m_score += (int)((float)m_fullScore * pointMultiplier);
        if(m_score < 0)
        {
            m_score = 0;
        }

        m_scoreText.text = "Element X: " + m_score.ToString("000000") + " kg";
    }
}
