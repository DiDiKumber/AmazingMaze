using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject wallLeft;
    public GameObject wallBottom;
    public MeshRenderer floor;
    public GameObject deadZone;

    Vector3 startPos = new Vector3(0, 0, 0);
    Vector3 finishPos = new Vector3(80, 0, 80);
    private void Start()
    {
        int luckyNumber = 5;
        int randomNumber = Random.Range(0, 10);


        if (randomNumber == luckyNumber)
        {
            if (transform.position == startPos || transform.position == finishPos || transform.position.x > 85 || transform.position.z > 85)
            {
                deadZone.SetActive(false);
            }
            else
            {
                deadZone.SetActive(true);
            }
        }
 
                
        
    }


}
