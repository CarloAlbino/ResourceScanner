using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    private GameObject m_gridTile = null;
    private GameObject[,] m_grid;    // Change this later from GameObject to the tile class

    // For testing
    public int m_side = 16;    // Should always be a square
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
            CreateGrid(m_side);
            Camera.main.transform.position = GetGridCenter();
            m_createGrid = false;
        }
	}

    public void CreateGrid(int side)
    {
        if(m_gridTile)  // Make sure there is a reference to a tile object
        {
            m_grid = new GameObject[side, side];
            for(int h = 0; h < side; h++)
            {
                for(int w = 0; w < side; w++)
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

        // Set camera projection and size
        Camera.main.orthographic = true;
        Vector3 bottomPos = m_grid[0, 0].transform.position;
        bottomPos.z -= (m_grid[0, 0].transform.localScale.z * 0.5f);
        Vector3 topPos = m_grid[m_grid.GetLength(0) - 1, 0].transform.position;
        topPos.z += (m_grid[m_grid.GetLength(0) - 1, 0].transform.localScale.z * 0.5f);
        float gridHeight = Vector3.Distance(bottomPos, topPos);

        // Orthorgraphic size is half the height of the grid + 5% of the grid height
        // This will fill the screen verticaly with a bit of a border
        Camera.main.orthographicSize = (gridHeight * 0.5f) + (gridHeight * 0.05f);

        // Offset the camera so that grid is at the right of screen
        float screenWidth = gridHeight * ((float)Screen.width / (float)Screen.height);
        // Formula fits a square to the right of the screen || TODO: Only seems to work with 16:9 aspect ratio, fix this in the future
        float newX = x - (screenWidth * ((((gridHeight * 0.5f) - ((screenWidth * 0.5f) - (screenWidth - gridHeight))))/screenWidth));

        // Set centre
        Vector3 center = new Vector3(newX, y, z);

        return center;
    }
}
