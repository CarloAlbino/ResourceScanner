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
    public int m_numOfFUllTiles = 6;

	void Start ()
    {
        m_numOfFUllTiles = Mathf.Clamp(m_numOfFUllTiles, 0, (m_side * m_side) / 2);
	}
	
	void Update ()
    {
        // For testing
		if(m_createGrid)
        {
            CreateGrid(m_side);
            Camera.main.transform.position = GetGridCenter();
            SetResources(m_numOfFUllTiles);
            m_createGrid = false;
        }
	}

    public void Scan()
    {
        for (int x = 0; x < m_grid.GetLength(0); x++)
        {
            for (int y = 0; y < m_grid.GetLength(1); y++)
            {
                m_grid[x, y].GetComponent<Tile>().Scan();
            }
        }
    }

    public void ScanArea(int x, int y)
    {
        for (int _x = x - 2; _x < x + 2 + 1; _x++)
        {
            for (int _y = y - 2; _y < y + 2 + 1; _y++)
            {
                if (_x > -1 && _x < m_grid.GetLength(0) && _y > -1 && _y < m_grid.GetLength(1))
                {
                    m_grid[_x, _y].GetComponent<Tile>().Scan();
                }
            }
        }
    }

    public void ExtractFromArea(int x, int y)
    {
        GameController.Instance.UpdateScore(GetTile(x, y).GetTileValue());
        // Extract tiles
        for (int _x = x - 2; _x < x + 2 + 1; _x++)
        {
            for (int _y = y - 2; _y < y + 2 + 1; _y++)
            {
                if (GetTile(_x, _y) != null)
                {
                    // Extract Full resource
                    if (_x == x && _y == y)
                    {
                        GetTile(_x, _y).SetNewTileValue(1.0f/16.0f);
                    }
                    else
                    {
                        GetTile(_x, _y).SetNewTileValue(GetTile(_x, _y).GetTileValue() / 2);
                    }
                }
            }
        }
        ScanArea(x, y);
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
                    m_grid[h, w].GetComponent<Tile>().SetPositionValues(h, w);
                }
            }
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

    private void SetResources(int numOfFullTiles)
    {
        for (int fullTiles = 0; fullTiles < numOfFullTiles; fullTiles++)
        {
            // For each full tile populate the tiles around it
            int randX = 0;
            int randY = 0;

            do
            {
                randX = Random.Range(0, m_side);
                randY = Random.Range(0, m_side);
            } while (GetTile(randX, randY).GetTileValue() >= (1.0f / 2.0f));

            // Fill tiles
            for(int x = randX - 2; x < randX + 2 + 1; x++)
            {
                for(int y = randY - 2; y < randY + 2 + 1; y++)
                {
                    //Debug.Log("[" + x + ", " + y + "]");
                    if(GetTile(x, y) != null)
                    {
                        // Set 1/4 resource
                        if(y == randY - 2 || y == randY + 2 ||
                           x == randX - 2 || x == randX + 2)
                        {
                            //Debug.Log("Hit 1/4");
                            if(GetTile(x, y).GetTileValue() < 1.0f / 4.0f)
                                GetTile(x, y).SetNewTileValue(1.0f / 4.0f);
                        }
                        // Set 1/2 resource
                        else if(y == randY - 1 || y == randY + 1||
                                x == randX - 1 || x == randX +1)
                        {
                            //Debug.Log("Hit 1/2");
                            if (GetTile(x, y).GetTileValue() < 1.0f / 2.0f)
                                GetTile(x, y).SetNewTileValue(1.0f / 2.0f);
                        }
                        // Set Full resource
                        else if (x == randX && y == randY)
                        {
                            //Debug.Log("Hit Full");
                            if (GetTile(x, y).GetTileValue() < 1.0f)
                                GetTile(x, y).SetNewTileValue(1.0f);
                        }
                    }
                }
            }
        }
    }

    private Tile GetTile(int x, int y)
    {
        if ((x > -1 && x < m_side) && (y > -1 && y < m_side))
        {
            if (m_grid[x, y] != null)
            {
                return m_grid[x, y].GetComponent<Tile>();
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
