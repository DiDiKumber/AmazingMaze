using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRenderer : MonoBehaviour
{
    public SpawnerCell mazeSpawner;
    [SerializeField] LineRenderer line;

    private void OnDestroy()
    {
       
    }
    private void Awake()
    {
        SpawnerCell.CheckLines += DrawPath;
    }

    void DrawPath()
    {
        
        Maze maze = mazeSpawner.maze;
        Vector2Int currPos = maze.finishPosition;
        List<Vector3> positions = new List<Vector3>();

        while (currPos != Vector2Int.zero)
        {
            int x = currPos.x;
            int y = currPos.y;
            positions.Add(new Vector3(x * mazeSpawner.CellSize.x, y * mazeSpawner.CellSize.y, y * mazeSpawner.CellSize.z));

            MazeGeneratorCell currentCell = maze.cells[currPos.x, currPos.y];

            if (currPos.x > 0 &&
                !currentCell.WallLeft &&
                maze.cells[currPos.x - 1, currPos.y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currPos.x--;
            }
            else if (currPos.y > 0 &&
                     !currentCell.WallBottom &&
                     maze.cells[currPos.x, currPos.y - 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currPos.y--;
            }
            else if (currPos.x < maze.cells.GetLength(0) - 1 &&
                    !maze.cells[currPos.x + 1, currPos.y].WallLeft &&
                    maze.cells[currPos.x + 1, currPos.y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currPos.x++;
            }
            else if (currPos.y < maze.cells.GetLength(1) - 1 &&
                    !maze.cells[currPos.x, currPos.y + 1].WallBottom &&
                    maze.cells[currPos.x, currPos.y + 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currPos.y++;
            }
        }

        positions.Add(Vector3.zero);
        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());

        SpawnerCell.CheckLines -= DrawPath;
    }



}
