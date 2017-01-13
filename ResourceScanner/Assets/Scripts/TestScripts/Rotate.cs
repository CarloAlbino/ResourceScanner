using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    [SerializeField]
    private float m_rotateSpeed = 1.0f;

	// Update is called once per frame
	void Update ()
    {
        // Rotate about the y axis
        transform.Rotate(Vector3.up, m_rotateSpeed * Time.deltaTime);
	}
}
