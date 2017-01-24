using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    [SerializeField, Tooltip("Colour of the scanned message")]
    private Color m_scanTextColor = new Color(0.64f, 0.22f, 0.22f, 1.0f);
    [SerializeField, Tooltip("Colour of the extracted message")]
    private Color m_extractTextColor = new Color(0.64f, 0.22f, 0.22f, 1.0f);

    // Holds info on the object hit by the raycast
    private RaycastHit m_hit;
    // The previously hit collider
    private Collider m_lastHitCollider;
    // Reference to the game controller
    private GameController m_gameController;
    // Reference to the pause manager
    private PauseMenuManager m_pauseManager;

	void Start ()
    {
        // Get reference to the game controller
        m_gameController = GameController.Instance;
        // Get reference to the pause menu manager
        m_pauseManager = FindObjectOfType<PauseMenuManager>();
	}

	void FixedUpdate ()
    {
        if (!m_pauseManager.IsGamePaused())
        {
            // Raycast in Higlight()
            Highlight();
            // Check for a click in Click()
            Click();
            // Unclick if necessary
            UnClick();
        }
	}

    /// <summary>
    /// Highlight tile if mouse is over tile 
    /// </summary>
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

    /// <summary>
    /// Click the tile, scan if in scan mode, extract if in extract mode
    /// </summary>
    private void Click()
    {
        if (m_hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0) && m_hit.collider.GetComponent<Tile>())
            {
                // Store the tile clicked on
                Tile t = m_hit.collider.GetComponent<Tile>();

                if (m_gameController.IsScanMode())
                {
                    if (m_gameController.ScansRemaining() > 0)
                    {
                        if (!t.IsScanned())
                        {
                            AudioManager.Instance.Scan();
                            MessageBox.Instance.QueueUpMessage("Scanning [" + t.GetRow() + ", " + t.GetColumn() + "] area", m_scanTextColor);
                            // Scan
                            t.GetGrid().ScanArea(t.GetRow(), t.GetColumn());
                            m_gameController.Scan();
                        }
                    }
                }
                else
                {
                    if (t.CanBeClicked())
                    {
                        if (m_gameController.ExtractsRemaining() > 0)
                        {
                            AudioManager.Instance.Extract();
                            MessageBox.Instance.QueueUpMessage("Extracted " + (t.GetTileValue() * 1000) + "kg of element X", m_extractTextColor, false);
                            MessageBox.Instance.QueueUpMessage("Extracting [" + t.GetRow() + ", " + t.GetColumn() + "]", m_extractTextColor);
                            // Extract
                            t.GetGrid().ExtractFromArea(t.GetRow(), t.GetColumn());
                            m_gameController.Extract();
                        }
                    }
                }

                m_hit.collider.GetComponent<TileHighlighter>().Click();
            }
        }
    }

    /// <summary>
    /// Return the tile to regular highlight
    /// </summary>
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
