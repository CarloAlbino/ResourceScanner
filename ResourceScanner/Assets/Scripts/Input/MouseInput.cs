using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    private RaycastHit m_hit;
    private Collider m_lastHitCollider;

	void Start ()
    {
        	
	}

    void Update()
    {

    }

	void FixedUpdate ()
    {
        Highlight();
        Click();
        UnClick();
	}

    private void Highlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out m_hit))
        {
            if (m_lastHitCollider == null)
            {
                m_lastHitCollider = m_hit.collider;
            }

            if (m_lastHitCollider != m_hit.collider)
            {
                // Unhighlight the previous tile
                if (m_lastHitCollider.GetComponent<TileHighlighter>() != null)
                {
                    m_lastHitCollider.GetComponent<TileHighlighter>().UnHighLight();
                }

                // Replace the reference to the last collider hit
                m_lastHitCollider = m_hit.collider;

                // Highligh the new collider that was hit
                if (m_lastHitCollider.GetComponent<TileHighlighter>() != null)
                {
                    m_lastHitCollider.GetComponent<TileHighlighter>().HightLight();
                }
            }
        }
    }

    private void Click()
    {
        if (m_hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0) && m_hit.collider.GetComponent<Tile>())
            {
                Tile t = m_hit.collider.GetComponent<Tile>();
                MessageBox.Instance.QueueUpMessage("Scanning [" + t.GetRow() + ", " + t.GetColumn() + "] area", Color.red);
                m_hit.collider.GetComponent<TileHighlighter>().Click();
                t.GetGrid().ScanArea(t.GetRow(), t.GetColumn());
            }
        }
    }

    private void UnClick()
    {
        if (m_hit.collider != null)
        {
            if (Input.GetMouseButtonUp(0) && m_hit.collider.GetComponent<TileHighlighter>())
            {
                m_hit.collider.GetComponent<TileHighlighter>().HightLight();
            }
        }
    }

}
