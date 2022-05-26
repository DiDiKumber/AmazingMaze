using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public SpawnerCell mazeSpawner;
    [SerializeField] UIManager uiManager;
    [SerializeField] Sounds sound;

    Vector3[] nextPos;
    int startPosNumber;
    int defaultPosNumber;

    [SerializeField] int speed;
    Vector3 startPos = new Vector3(0, 1.5f, 0);
    bool canMove;
    bool ShieldActive;

    Color mainColor = new Color(255, 255, 0, 255);
    Color shieldColor = new Color(173, 255, 47, 255);
    Renderer myRenderer;

    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] ParticleSystem winEffect;

    IEnumerator startlvl;
    private void OnDestroy()
    {
        UIManager.ShieldActive -= ActivateShied;
        SpawnerCell.CheckLines -= CheckRoad;
    }

    private void Awake()
    {
        UIManager.ShieldActive += ActivateShied;
        SpawnerCell.CheckLines += CheckRoad;
        myRenderer = GetComponent<Renderer>();
    }

    private void CheckRoad()
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

        nextPos = positions.ToArray();
        startPosNumber = positions.Count;
        defaultPosNumber = startPosNumber;

        StartGame();
    }

    Vector3 GetNextPos()
    {
        Vector3 nextPosition = nextPos[startPosNumber - 1];
        nextPosition.y = 1.5f;

        return nextPosition;
    }

    public void StartGame()
    {
        startlvl = StartLevel();

        StartCoroutine(startlvl);
        
        IEnumerator StartLevel()
        {
            yield return new WaitForSeconds(1f);
            startPosNumber = defaultPosNumber;
            myRenderer.enabled = true;
            myRenderer.material.color = mainColor;
            yield return new WaitForSeconds(2f);
            
            canMove = true;
        }
    }



    private void FixedUpdate()
    {
        if(canMove)
            transform.position = Vector3.MoveTowards(transform.position, GetNextPos(), speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, GetNextPos()) < 1 ) {
            startPosNumber--;
            GetNextPos();
        }
    }

    public void ActivateShied(bool active)
    {
        if (active)
        {
            myRenderer.material.color = Color.green;
            ShieldActive = true;
        }
        else
        {
            myRenderer.material.color = mainColor;
            ShieldActive = false;
        }
    }

    void GoToNextLevel()
    {
        uiManager.FinishLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishZone"))
        {
            canMove = false;
            Instantiate(winEffect, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.Euler(-90, 0, 0));
            sound.AudioWin();
            Invoke("GoToNextLevel", 2f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("DeadZone") && !ShieldActive)
        {
            canMove = false;
            sound.AudioLose();
            Instantiate(deathEffect, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.Euler(-90, 0,0));
            myRenderer.enabled = false;
            transform.position = startPos;
            
            StopCoroutine(startlvl);
            StartGame();
        }

    }
}
