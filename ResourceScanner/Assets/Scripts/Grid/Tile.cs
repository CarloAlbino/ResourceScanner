using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int m_row = 0;
    private int m_column = 0;
    private float m_value = 1 / 16;

    public void SetTileValues(int row, int col)
    {
        m_row = row;
        m_column = col;
    }

    public int GetRow()
    {
        return m_row;
    }

    public int GetColumn()
    {
        return m_column;
    }

    public void SetNewTileValue(float newValue)
    {
        m_value = newValue;
    }

    public float GetTileValue()
    {
        return m_value;
    }
}
