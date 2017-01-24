using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour {

    [SerializeField, Tooltip("The colour to highlight to")]
    private Color m_highlightColour = Color.blue;
    // The colour the tile will be when clicked on
    private Color m_clickColour = Color.black;
    // References to the renderer and tile
    private MeshRenderer m_meshRenderer = null;
    private Tile m_tile;

	void Start ()
    {
        // Grab references
        m_meshRenderer = GetComponent<MeshRenderer>();
        if(!m_meshRenderer)
        {
            Debug.LogWarning("No Mesh Renderer attached.");
        }
        m_tile = GetComponent<Tile>();
	}

    /// <summary>
    /// Highlight the tile
    /// </summary>
    public void HightLight()
    {
        // Combine the current colour with the highlighted colour
        Color newColor = m_tile.GetCurrentColour() + m_highlightColour;
        m_meshRenderer.material.color = newColor;
    }

    /// <summary>
    /// Unhighlight the tile
    /// </summary>
    public void UnHighLight()
    {
        m_meshRenderer.material.color = m_tile.GetCurrentColour();
    }

    /// <summary>
    /// Click the tile
    /// </summary>
    public void Click()
    {
        m_meshRenderer.material.color = m_clickColour;
        m_tile.Click();
    }
}
