using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{
    // Start is called before the first frame update
    public int typeOfScore; //simple = 1, big 2,fruts 3
  
    void Start()
    {
        if (typeOfScore == 1)
        {
            GameManager.data.maxCountOfSimpleScores += 1;
        }
        else if (typeOfScore == 2)
        {
            GameManager.data.maxCountOfBigScores += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (typeOfScore == 1)
            {
                GameManager.data.simpleScoresCount += 1;
                GameManager.data.Score += 10;
            }
            if (typeOfScore == 2)
            {
                GameManager.data.bigScoresCount += 1;
                GameManager.data.Score += 50;
                GameManager.data.eatMode = true;
            }
            if (typeOfScore == 3)
            {
                GameManager.data.frutScoresCount += 1;
            }
            Destroy(gameObject);
        }
    }
}
