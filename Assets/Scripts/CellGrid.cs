using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    protected Transform _transform_CellGrid;

    public int width = 20;
    public int height = 20;

    public Cell cellPrefab;

    Cell[] cells;

    private void Awake()
    {
        _transform_CellGrid = this.transform;
        _transform_CellGrid.localPosition = new Vector3(-100f, 0, -100f);

        cells = new Cell[height * width];

        for(int z = 0, i = 0; z < height; z++)
        {
            for(int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;

        Cell cell = cells[i] = Instantiate<Cell>(cellPrefab);
        cell.transform.SetParent(_transform_CellGrid, false);
        cell.transform.localPosition = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
