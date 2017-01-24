using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceLevel
{
    Empty,
    Low,
    Medium, 
    High, 
    Full
}

public class Tile : MonoBehaviour {

    [SerializeField, Tooltip("Unscanned colour")]
    private Color m_normalColour = Color.white;
    [SerializeField, Tooltip("Lowest amount resource colour")]
    private Color m_emptyColour = Color.grey;
    [SerializeField, Tooltip("Low amount resource colour")]
    private Color m_lowColour = Color.green;
    [SerializeField, Tooltip("Medium amount resource colour")]
    private Color m_mediumColour = Color.yellow;
    [SerializeField, Tooltip("High amount resource colour")]
    private Color m_highColour = Color.red + Color.yellow;
    [SerializeField, Tooltip("Full amount resource colour")]
    private Color m_fullColour = Color.red;

    // Click delay variables
    [SerializeField, Tooltip("Delay between clicks")]
    private float m_clickDelay = 0.5f;
    private float m_currentClickTime = 0.0f;
    private bool m_canClick = true;

    // Tile data
    private int m_row = 0;
    private int m_column = 0;
    private float m_value = 1.0f / 8.0f;
    private ResourceLevel m_currentLevel = ResourceLevel.Low;
    private bool m_hasBeenScanned = false;
    private bool m_hasBeenExtracted = false;

    // Reference to the renderer and colour
    private MeshRenderer m_meshRenderer;
    private Color m_currentColour;


    void Start()
    {
        // Set the starting colour
        m_currentColour = m_normalColour;
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.material.color = m_currentColour;
    }

    void Update()
    {
        // Count the click timer after a click
        if(!m_canClick)
        {
            if(m_currentClickTime < m_clickDelay)
            {
                m_currentClickTime += Time.deltaTime;
            }
            else
            {
                m_currentClickTime = 0;
                m_canClick = true;
            }
        }
    }

    /// <summary>
    /// Can the tile be clicked?
    /// </summary>
    /// <returns>True if the tile can be clicked</returns>
    public bool CanBeClicked()
    {
        return m_canClick;
    }

    /// <summary>
    /// Call this function when the tile is clicked
    /// </summary>
    public void Click()
    {
        m_canClick = true;
    }

    /// <summary>
    /// Set the value of the tile
    /// </summary>
    /// <param name="row">X coordinate</param>
    /// <param name="col">Y coordinate</param>
    public void SetPositionValues(int row, int col)
    {
        m_row = row;
        m_column = col;
    }

    /// <summary>
    /// Get row of the tile
    /// </summary>
    /// <returns>X coordinate</returns>
    public int GetRow()
    {
        return m_row;
    }

    /// <summary>
    /// Get column of the tile
    /// </summary>
    /// <returns>Y coordinate</returns>
    public int GetColumn()
    {
        return m_column;
    }

    /// <summary>
    /// Get the current colour of the tile
    /// </summary>
    /// <returns>Current colour</returns>
    public Color GetCurrentColour()
    {
        return m_currentColour;
    }

    /// <summary>
    /// Set a new value for the tile
    /// </summary>
    /// <param name="newValue">The new value</param>
    public void SetNewTileValue(float newValue)
    {
        m_value = newValue;
        if (m_value <= (1.0f / 16.0f))
        {
            m_currentLevel = ResourceLevel.Empty;
        }
        else if (m_value <= (1.0f / 8.0f))
        {
            m_currentLevel = ResourceLevel.Low;
        }
        else if (m_value <= (1.0f / 4.0f))
        {
            m_currentLevel = ResourceLevel.Medium;
        }
        else if (m_value <= (1.0f / 2.0f))
        {
            m_currentLevel = ResourceLevel.High;
        }
        else if (m_value <= 1.0f)
        {
            m_currentLevel = ResourceLevel.Full;
        }
        else
        {
            Debug.LogWarning("Value not set properly");
        }
    }

    /// <summary>
    /// Get the tile's current value
    /// </summary>
    /// <returns></returns>
    public float GetTileValue()
    {
        return m_value;
    }

    /// <summary>
    /// Has the tile been scanned
    /// </summary>
    /// <returns></returns>
    public bool IsScanned()
    {
        return m_hasBeenScanned;
    }

    /// <summary>
    /// Scan the tile
    /// </summary>
    public void Scan()
    {
        m_hasBeenScanned = true;
        switch (m_currentLevel)
        {
            case ResourceLevel.Empty:
                m_currentColour = m_emptyColour;
                break;
            case ResourceLevel.Low:
                m_currentColour = m_lowColour;
                break;
            case ResourceLevel.Medium:
                m_currentColour = m_mediumColour;
                break;
            case ResourceLevel.High:
                m_currentColour = m_highColour;
                break;
            case ResourceLevel.Full:
                m_currentColour = m_fullColour;// (m_normalColour + m_fullColour) / 2.0f;
                break;
            default:
                m_currentColour = m_normalColour;
                break;
        }
        m_meshRenderer.material.color = m_currentColour;
    }

    public bool IsExtracted()
    {
        return m_hasBeenExtracted;
    }

    public void Extract()
    {
        m_hasBeenExtracted = true;
    }

    /// <summary>
    /// Get the grid the tile is part of
    /// </summary>
    /// <returns>Main grid if it exists</returns>
    public Grid GetGrid()
    {
        if(GetComponentInParent<Grid>() != null)
        {
            return GetComponentInParent<Grid>();
        }
        else
        {
            Debug.LogWarning("No grid attached.");
            return null;
        }

    }
}
