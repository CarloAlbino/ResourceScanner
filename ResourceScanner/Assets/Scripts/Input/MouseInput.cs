using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    [SerializeField]
    private Color m_scanTextColor = new Color(0.64f, 0.22f, 0.22f, 1.0f);
    [SerializeField]
    private Color m_extractTextColor = new Color(0.64f, 0.22f, 0.22f, 1.0f);
    private RaycastHit m_hit;
    private Collider m_lastHitCollider;
    private GameController m_gameController;

	void Start ()
    {
        m_gameController = GameController.Instance;
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

                if (m_gameController.IsScanMode())
                {
                    if (m_gameController.ScansRemaining() > 0)
                    {
                        MessageBox.Instance.QueueUpMessage("Scanning [" + t.GetRow() + ", " + t.GetColumn() + "] area", m_scanTextColor);
                        t.GetGrid().ScanArea(t.GetRow(), t.GetColumn());
                        m_gameController.Scan();
                    }
                }
                else
                {
                    if (m_gameController.ExtractsRemaining() > 0)
                    {
                        MessageBox.Instance.QueueUpMessage("Extracted " + (t.GetTileValue() * 1000) + "kg of element X", m_extractTextColor, false);
                        MessageBox.Instance.QueueUpMessage("Extracting [" + t.GetRow() + ", " + t.GetColumn() + "]", m_extractTextColor);
                        // Extract
                        t.GetGrid().ExtractFromArea(t.GetRow(), t.GetColumn());
                        m_gameController.Extract();
                    }
                }

                m_hit.collider.GetComponent<TileHighlighter>().Click();
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
