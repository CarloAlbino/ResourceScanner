using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    private RaycastHit m_hit;
    private Collider m_lastHitCollider;

	void Start ()
    {
		
	}

	void FixedUpdate ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray, out m_hit))
        {
            if(m_lastHitCollider == null)
            {
                m_lastHitCollider = m_hit.collider;
            }

            if(m_lastHitCollider != m_hit.collider)
            {
                // Unhighlight the previous tile
                if (m_lastHitCollider.GetComponent<TileHighlighter>() != null)
                {
                    m_lastHitCollider.GetComponent<TileHighlighter>().UnHighLight();
                }

                // Replace the reference to the last collider hit
                m_lastHitCollider = m_hit.collider;

                // Highligh the new collider that was hit
                if(m_lastHitCollider.GetComponent<TileHighlighter>() != null)
                {
                    m_lastHitCollider.GetComponent<TileHighlighter>().HightLight();
                }
            }
        }
	}

}
