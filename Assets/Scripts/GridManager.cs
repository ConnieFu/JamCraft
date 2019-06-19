using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private class sNode
    {
        public sNode parentNode = null;
        public Vector3Int cell;

        public bool isWalkable = true;
        public int price = 0;
        public int gValue = 0;
        public int hValue = 0;

        public int fValue
        {
            get
            {
                return gValue + hValue;
            }
        }
    }

    private const string GROUND_TILEMAP_NAME = "Ground";
    private const string INTERACTABLE_TILEMAP_NAME = "Interactable";
    private const string COLLIDABLE_TILEMAP_NAME = "Collidable";

    [SerializeField] private int m_InteractableNodePrice = 50;
    [SerializeField] private RectInt m_GridSize;
    [SerializeField] private InteractableTilemap m_InteractableTilemap;
    public InteractableTilemap InteractableTilemap
    {
        get
        {
            return m_InteractableTilemap;
        }
    }

    private Grid m_Grid;
    protected List<Tilemap> m_TileMaps = new List<Tilemap>();

    private Dictionary<Vector3Int, sNode> m_Nodes = new Dictionary<Vector3Int, sNode>();

    public void Initialize()
    {
        m_Grid = GetComponent<Grid>();
        m_InteractableTilemap.Initialize();

        Tilemap[] tilemaps = m_Grid.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            m_TileMaps.Add(tilemap);
        }

        InitializeNodes();
    }

    #region A*Pathfinding
    private void InitializeNodes()
    {
        Vector3Int cell = new Vector3Int(m_GridSize.x, m_GridSize.y, 0);
        for (int x = m_GridSize.x; x < m_GridSize.x + m_GridSize.width - 1; x++)
        {
            cell.x = x;
            for (int y = m_GridSize.y; y < m_GridSize.y + m_GridSize.height - 1; y++)
            {
                cell.y = y;
                UpdateNodeAtCell(cell);
            }
        }
    }

    public void UpdateNodeAtCell(Vector3Int cell)
    {
        if (!IsCellEmpty(cell, false))
        {
            if (!m_Nodes.ContainsKey(cell))
            {
                m_Nodes.Add(cell, new sNode());
            }
            sNode node = m_Nodes[cell];

            node.cell = cell;

            if (m_TileMaps != null && m_TileMaps.Count > 0)
            {
                bool foundTile = false;
                for (int i = 0; i < m_TileMaps.Count; i++)
                {
                    foundTile = false;
                    switch (m_TileMaps[i].name)
                    {
                        case COLLIDABLE_TILEMAP_NAME:
                            if (m_TileMaps[i].HasTile(cell))
                            {
                                node.isWalkable = false;
                                foundTile = true;
                            }
                            break;
                        case INTERACTABLE_TILEMAP_NAME:
                            if (m_InteractableTilemap.IsTileOccupied(cell))
                            {
                                node.isWalkable = true;
                                node.price = m_InteractableNodePrice;
                                if (cell == m_InteractableTilemap.HomeBaseCell)
                                {
                                    node.price = 1;
                                }
                                foundTile = true;
                            }
                            break;
                        case GROUND_TILEMAP_NAME:
                            if (m_TileMaps[i].HasTile(cell))
                            {
                                node.isWalkable = true;
                                node.price = 1;
                                foundTile = true;
                            }
                            break;
                    }
                    if (foundTile)
                    {
                        break;
                    }
                }
            }
        }
    }

    private sNode GetNodeAtCell(Vector3Int cell)
    {
        if (m_Nodes.ContainsKey(cell))
        {
            return m_Nodes[cell];
        }
        
        return null;
    }

    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int endPos)
    {
        sNode startNode = GetNodeAtCell(startPos);
        sNode endNode = GetNodeAtCell(endPos);

        List<sNode> openSet = new List<sNode>();
        HashSet<sNode> closedSet = new HashSet<sNode>();

        sNode currentNode;

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // set current node to the first node in the open set
            currentNode = openSet[0];
            
            // loop through open nodes and find set the currentNode to the node with the smallest distances
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fValue <= currentNode.fValue)
                {
                    currentNode = openSet[i];
                }
            }

            // since we are looking at this node, remove it from the open set and add it to the closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // if our current node is the same as our end node, we've reached our target and can return the path
            if (currentNode.cell == endNode.cell)
            {
                return RetracePath(startNode, endNode);
            }

            // get a list of the neighbours to check
            List<sNode> neighbourNodes = GetNeighbourNodes(currentNode);
            for (int i = 0; i < neighbourNodes.Count; i++)
            {
                // if the neighbour was already checked, don't check it again
                if (closedSet.Contains(neighbourNodes[i]))
                {
                    continue;
                }

                
                neighbourNodes[i].gValue = currentNode.gValue + neighbourNodes[i].price;
                neighbourNodes[i].hValue = GetDistanceBetweenNodes(neighbourNodes[i], endNode);
                neighbourNodes[i].parentNode = currentNode;

                // keeping this for now for debugging SINCE I CANT ATTACH VISUAL STUDIO TO UNITY FOR SOME GODDAMN REASON ASLJFIOA JGJSF
                //Debug.LogError("neighbour cell: " + neighbourNodes[i].cell + "\nneighbour g: " + neighbourNodes[i].gValue + "\nneighbour h: " + neighbourNodes[i].hValue + "\nneighbour price: " + neighbourNodes[i].price);

                if (openSet.Contains(neighbourNodes[i]))
                {
                    continue;
                }

                openSet.Add(neighbourNodes[i]);
            }
        }

        return null;
    }

    private static List<Vector3Int> RetracePath(sNode startNode, sNode endNode)
    {
        List<sNode> path = new List<sNode>();
        sNode currentNode = endNode;

        while (currentNode.cell != startNode.cell)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        List<Vector3Int> cellPath = new List<Vector3Int>();
        for (int i = path.Count - 1; i >= 0; i--)
        {
            cellPath.Add(path[i].cell);
        }
        return cellPath;
    }

    private List<sNode> GetNeighbourNodes(sNode node)
    {
        List<sNode> nodes = new List<sNode>();

        foreach(Vector3Int direction in GameConstants.FOUR_DIRECTIONS)
        {
            Vector3Int cell = node.cell + direction;
            
            if (m_Nodes.ContainsKey(cell) && m_Nodes[cell].isWalkable)
            {
                nodes.Add(m_Nodes[cell]);
            }
        }
        return nodes;
    }

    private static int GetDistanceBetweenNodes(sNode nodeA, sNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.cell.x - nodeB.cell.x);
        int dstY = Mathf.Abs(nodeA.cell.y - nodeB.cell.y);

        return (dstX * dstX) + (dstY * dstY);
    }
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
                switch (m_TileMaps[i].name)
                {
                    case COLLIDABLE_TILEMAP_NAME:
                        if (m_TileMaps[i].HasTile(cell))
                        {
                            return false;
                        }
                        break;
                    case INTERACTABLE_TILEMAP_NAME:
                        if (m_InteractableTilemap.IsTileOccupied(cell))
                        {
                            return false;
                        }
                        break;
                    case GROUND_TILEMAP_NAME:
                        if (m_TileMaps[i].HasTile(cell) && !ignoreGround)
                        {
                            return false;
                        }
                        break;
                }
            }
        }
        return true;
    }
    #endregion
}
