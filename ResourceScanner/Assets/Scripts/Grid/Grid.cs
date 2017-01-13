using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    private GameObject m_gridTile = null;
    private GameObject[,] m_grid;    // Change this later from GameObject to the tile class

    // For testing
    public int m_width = 16;
    public int m_height = 16;
    public bool m_createGrid = false;
    public float m_gridOffset = 0.1f;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        // For testing
		if(m_createGrid)
        {
            CreateGrid(m_width, m_height);
            Camera.main.transform.position = GetGridCenter();
            m_createGrid = false;
        }
	}

    public void CreateGrid(int width, int height)
    {
        if(m_gridTile)  // Make sure there is a reference to a tile object
        {
            m_grid = new GameObject[height, width];
            for(int h = 0; h < height; h++)
            {
                for(int w = 0; w < width; w++)
                {
                    // Spawn and set each grid tile in the grid
                    Vector3 position = new Vector3((m_gridTile.transform.localScale.x + m_gridOffset) * w, 0,
                                                    (m_gridTile.transform.localScale.z + m_gridOffset) * h);
                    m_grid[h, w] = Instantiate(m_gridTile, position,
                                                Quaternion.identity, transform) as GameObject;
                    m_grid[h, w].GetComponent<Tile>().SetTileValues(h, w);
                }
            }
        }
    }

    public void SetGridValues(int numOfFullTiles)
    {
        for (int i = 0; i < numOfFullTiles; i++)
        {
            int randRow;
            int randCol;
            do
            {
                randRow = Random.Range(0, m_grid.GetLength(0));
                randCol = Random.Range(0, m_grid.GetLength(1));
            } while (m_grid[randRow, randCol].GetComponent<Tile>().GetTileValue() < 0.5f);


        }

        
    }

    public Vector3 GetGridCenter()
    {
        // Height
        float z = (m_grid[m_grid.GetLength(0) - 1, 0].transform.position.z - m_grid[0, 0].transform.position.z) * 0.5f;
        // Width
        float x = (m_grid[0, m_grid.GetLength(1) - 1].transform.position.x - m_grid[0, 0].transform.position.x) * 0.5f;

        // Camera distance
        float y = Camera.main.transform.position.y;
    
        Vector3 center = new Vector3(x, y, z);

        // Set camera projection and size
        Camera.main.orthographic = true;
        float gridHeight = Vector3.Distance(m_grid[0, 0].transform.position, m_grid[m_grid.GetLength(0) - 1, 0].transform.position);
        float gridWidth = Vector3.Distance(m_grid[0, 0].transform.position, m_grid[0, m_grid.GetLength(1) - 1].transform.position);

        // If the grid is taller than the width
        if (gridHeight >= gridWidth)
        {
            // Orthorgraphic size is half the height of the grid + 20% of the grid height
            Camera.main.orthographicSize = (gridHeight * 0.5f) + (gridHeight * 0.25f);
        }
        else
        {
            // Orthorgraphic size is 0.3% of the width
            Camera.main.orthographicSize = (gridWidth * 0.3f);
        }

        return center;
    }
}
