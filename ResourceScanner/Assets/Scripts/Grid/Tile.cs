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

    [SerializeField]
    private Color m_normalColour = Color.white;
    [SerializeField]
    private Color m_emptyColour = Color.grey;
    [SerializeField]
    private Color m_lowColour = Color.green;
    [SerializeField]
    private Color m_mediumColour = Color.yellow;
    [SerializeField]
    private Color m_highColour = Color.red + Color.yellow;
    [SerializeField]
    private Color m_fullColour = Color.red;

    private int m_row = 0;
    private int m_column = 0;
    [SerializeField]
    private float m_value = 1.0f / 8.0f;
    [SerializeField]
    private ResourceLevel m_currentLevel = ResourceLevel.Low;
    private bool m_hasBeenScanned = false;
    private Color m_currentColour;
    private MeshRenderer m_meshRenderer;

    void Start()
    {
        m_currentColour = m_normalColour;
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.material.color = m_currentColour;
    }

    public void SetPositionValues(int row, int col)
    {
        m_row = row;
        m_column = col;
    }

    public int GetRow()
    {
        return m_row;
    }

    public int GetColumn()
    {
        return m_column;
    }

    public Color GetCurrentColour()
    {
        return m_currentColour;
    }

    public void SetNewTileValue(float newValue)
    {
        m_value = newValue;
        //Debug.Log("Value: " + m_value);
        //Debug.Log("Condition: " + (1.0f / 16.0f));
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

    public float GetTileValue() // Change this to extract
    {
        return m_value;
    }

    public void Scan()
    {
        if (!m_hasBeenScanned)
        {
            m_hasBeenScanned = true;
            switch (m_currentLevel)
            {
                case ResourceLevel.Empty:
                    m_currentColour = (m_normalColour + m_emptyColour) / 2.0f;
                    break;
                case ResourceLevel.Low:
                    m_currentColour = (m_normalColour + m_lowColour) / 2.0f;
                    break;
                case ResourceLevel.Medium:
                    m_currentColour = (m_normalColour + m_mediumColour) / 2.0f;
                    break;
                case ResourceLevel.High:
                    m_currentColour = (m_normalColour + m_highColour) / 2.0f;
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
    }

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
