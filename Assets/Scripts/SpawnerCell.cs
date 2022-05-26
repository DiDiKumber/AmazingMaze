using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnerCell : MonoBehaviour
{
    public static event Action CheckLines;

    [SerializeField] GameObject cellPrefab;
    public Vector3 CellSize = new Vector3(1,1,0);
    [SerializeField] HintRenderer hintRenderer;

    public Maze maze;

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(cellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity).GetComponent<Cell>();

                c.wallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.wallBottom.SetActive(maze.cells[x, y].WallBottom);
                c.floor.enabled = (maze.cells[x, y].Floor);
                c.deadZone.SetActive(maze.cells[x, y].DeadZone);
            }
        }

        CheckLines?.Invoke();
        //hintRenderer.DrawPath();
    }

}