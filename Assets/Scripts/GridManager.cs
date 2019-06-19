using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private struct sNode
    {

    }

    private const string GROUND_TILEMAP_NAME = "Ground";

    private Grid m_Grid;
    protected List<Tilemap> m_TileMaps = new List<Tilemap>();

    private Dictionary<Vector3Int, sNode> m_Nodes = new Dictionary<Vector3Int, sNode>();

    public void Initialize()
    {
        m_Grid = GetComponent<Grid>();

        Tilemap[] tilemaps = m_Grid.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            m_TileMaps.Add(tilemap);
        }
    }

    #region A*Pathfinding
    #endregion

    #region GridUtilities
    public Vector3 GetCellWorldPos(Vector3Int cell)
    {
        return m_Grid.GetCellCenterWorld(cell);
    }

    public Vector3Int WorldPosToCell(Vector3 pos)
    {
        return m_Grid.WorldToCell(pos);
    }

    public Vector3Int GetRandomCell(RectInt cellRect)
    {
        return new Vector3Int(cellRect.x + Random.Range(0, cellRect.width), cellRect.y + Random.Range(0, cellRect.height), 0);
    }

    public bool IsCellEmpty(Vector3Int cell, bool ignoreGround = true)
    {
        if (m_TileMaps != null && m_TileMaps.Count > 0)
        {
            for (int i = 0; i < m_TileMaps.Count; i++)
            {
                if (ignoreGround && string.Compare(m_TileMaps[i].name, GROUND_TILEMAP_NAME) == 0)
                {
                    continue;
                }

                if (m_TileMaps[i].GetTile(cell) != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
    #endregion
}
