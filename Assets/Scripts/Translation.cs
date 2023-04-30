//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObjectPlacement : MonoBehaviour
//{
//    public LayerMask cellLayer; // set this to the layer of your cells in the Inspector
//    public int houseWidth = 4; // set this to the width of your house object
//    public int houseHeight = 4; // set this to the height of your house object
//    public Cell[,] cells; // set this to a 2D array of your cells

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            RaycastHit hit;
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//            if (Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayer))
//            {
//                // get the clicked cell
//                Cell clickedCell = hit.collider.gameObject.GetComponent<Cell>();

//                // calculate the bottom-left cell of the 4x4 area
//                int bottomLeftX = clickedCell.X - houseWidth / 2;
//                int bottomLeftY = clickedCell.Z - houseHeight / 2;

//                // loop through the cells that the house will occupy
//                for (int x = 0; x < houseWidth; x++)
//                {
//                    for (int y = 0; y < houseHeight; y++)
//                    {
//                        // calculate the position of the current cell
//                        int cellX = bottomLeftX + x;
//                        int cellY = bottomLeftY + y;

//                        // get the current cell
//                        Cell currentCell = GetCellAtPosition(cellX, cellY);

//                        // update its type
//                        currentCell.Type = "house";
//                    }
//                }
//            }
//        }
//    }

//    Cell GetCellAtPosition(int x, int y)
//    {
//        // check if the position is within the bounds of the grid
//        if (x >= 0 && x < cells.GetLength(0) && y >= 0 && y < cells.GetLength(1))
//        {
//            return cells[x, y];
//        }
//        else
//        {
//            return null;
//        }
//    }
//}