using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour {

    [SerializeField]
    private Color m_highlightColour = Color.blue;
    private Color m_clickColour = Color.black;
    //private Color m_originalColor = Color.white;
    private MeshRenderer m_meshRenderer = null;
    private Tile m_tile;

	void Start ()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        if(!m_meshRenderer)
        {
            Debug.LogWarning("No Mesh Renderer attached.");
        }
        m_tile = GetComponent<Tile>();
        //m_originalColor = m_meshRenderer.material.color;
	}

	void Update ()
    {
		
	}

    public void HightLight()
    {
        // Combine the current colour with the highlighted colour
        //Color newColor = m_originalColor + m_highlightColour;
        Color newColor = m_tile.GetCurrentColour() + m_highlightColour;
        m_meshRenderer.material.color = newColor;
    }

    public void UnHighLight()
    {
        m_meshRenderer.material.color = m_tile.GetCurrentColour();
    }

    public void Click()
    {
        m_meshRenderer.material.color = m_clickColour;
    }
}
