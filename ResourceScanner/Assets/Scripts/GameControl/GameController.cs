using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    [SerializeField, Tooltip("Grid Prefab")]
    private Grid m_grid;

    [SerializeField, Tooltip("Reference to the buttons")]
    private Button m_scanButton, m_extractButton;
    [SerializeField, Tooltip("Button colours when active or disabled")]
    private Color m_activeColor, m_disabledColor;
    // Is the game in scan mode?
    private bool m_isScanMode = true;   // False is extract mode

    [SerializeField, Tooltip("Max amount of scans in the game")]
    private int m_maxScans = 3;
    private int m_scansRemaining = 3;
    [SerializeField, Tooltip("Max amount of extractions in the game")]
    private int m_maxExtracts = 3;
    private int m_extractsRemaining = 3;

    [SerializeField, Tooltip("Text field to display score")]
    private Text m_scoreText;
    [SerializeField, Tooltip("The score that a full tile contains")]
    private int m_fullScore = 1000;
    // The current score
    private int m_score = 0;

    void Start()
    {
        // For testing
        //StartGame(m_maxScans, m_maxExtracts, 16, 8);
    }

    /// <summary>
    /// Start a game
    /// </summary>
    /// <param name="maxScans"></param>
    /// <param name="maxExtracts"></param>
    public void StartGame(int maxScans, int maxExtracts, int gridLength, int numFullTiles)
    {
        // Create and populate grid
        Grid grid = Instantiate(m_grid, Vector3.zero, Quaternion.identity) as Grid;
        grid.CreateGrid(Mathf.Clamp(Mathf.Abs(gridLength), 0, 128));
        int fullTiles = Mathf.Abs(numFullTiles);
        if (fullTiles > (Mathf.Abs(gridLength * gridLength)) / 4)
            fullTiles = (Mathf.Abs(gridLength * gridLength)) / 4;
        //fullTiles = Mathf.Clamp(fullTiles, 0, (gridLength * gridLength) / 2);
        grid.SetResources(fullTiles);

        // Start on scan mode
        ToggleMode(m_scanButton, m_extractButton);
        m_isScanMode = true;

        // Update the score display
        UpdateScore(0);

        // Set defaults
        m_maxScans = Mathf.Abs(maxScans);
        m_maxExtracts = Mathf.Abs(maxExtracts);
        m_scansRemaining = m_maxScans;
        m_extractsRemaining = m_maxExtracts;

        // Display start message
        MessageBox.Instance.QueueUpMessage(m_extractsRemaining + " extractions remaining", Color.green, false);
        MessageBox.Instance.QueueUpMessage(m_scansRemaining + " scans remaining", Color.green, false);
        MessageBox.Instance.QueueUpMessage("Collect as much element X as you can", Color.green);
    }

    // Toggle mode and change all the colours
    private void ToggleMode(Button activeButton, Button inactiveButton)
    {
        m_isScanMode = !m_isScanMode;
        ColorBlock tempColors = m_scanButton.colors;
        tempColors.normalColor = m_activeColor;
        tempColors.highlightedColor = m_activeColor;
        activeButton.colors = tempColors;

        tempColors = m_extractButton.colors;
        tempColors.normalColor = m_disabledColor;
        tempColors.highlightedColor = m_disabledColor;
        inactiveButton.colors = tempColors;
    }

    // Toggle scan mode on
    public void ToggleScanMode()
    {
        ToggleMode(m_scanButton, m_extractButton);
    }

    // Toggle extract mode on
    public void ToggleExtractMode()
    {
        ToggleMode(m_extractButton, m_scanButton);
    }

    /// <summary>
    /// Is game in Scan Mode?
    /// </summary>
    public bool IsScanMode()
    {
        return m_isScanMode;
    }

    /// <summary>
    /// Scan when clicking a tile
    /// </summary>
    public void Scan()
    {
        m_scansRemaining--;
        if (m_scansRemaining <= 0)
        {
            MessageBox.Instance.QueueUpMessage("No more scans remaining - Please switch to extract mode", Color.green);
            //ToggleExtractMode();
        }
        else
        {
            MessageBox.Instance.QueueUpMessage(m_scansRemaining + " scans remaining", Color.green);
        }
    }

    /// <summary>
    /// How many scans are remaining?
    /// </summary>
    /// <returns>Scans remaining</returns>
    public int ScansRemaining()
    {
        return m_scansRemaining;
    }

    /// <summary>
    /// Extract when clicking a tile
    /// </summary>
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

    /// <summary>
    /// How many extracts remaining?
    /// </summary>
    public int ExtractsRemaining()
    {
        return m_extractsRemaining;
    }

    /// <summary>
    /// Update the score field
    /// </summary>
    /// <param name="pointMultiplier">Multiplier that the max score will be multiplied by</param>
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
