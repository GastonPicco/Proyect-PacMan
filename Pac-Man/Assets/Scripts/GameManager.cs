using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager data;
    public GameObject pointingPoint;
    public GameObject Player;
    public int maxCountOfSimpleScores, maxCountOfBigScores;
    public int simpleScoresCount, bigScoresCount, frutScoresCount;
    public int TotalScore;
    public bool playerStuck;
    public bool eatMode;
    public float eatModeTimer;
    public int Score, lifes;
    public bool Die, Return1, Return2, Return3, Return4;
    public bool pause;
    public bool win;

    private void Awake()
    {       
        if(data==null)
        {
            data = this;
        }
        else
        {
            if(data == this)
            {
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        lifes = 3;
        TotalScore = 300;
    }

    // Update is called once per frame
    void Update()
    {
        TotalScore = maxCountOfBigScores + maxCountOfSimpleScores;
        if (eatMode == true)
        {
            eatModeTimer += Time.deltaTime;
            {
                if(eatModeTimer > 10)
                {
                    eatMode = false;
                    eatModeTimer = 0;
                }
            }
        }
        if (lifes == 0)
        {
            Time.timeScale = 0;
        }
        if(TotalScore == simpleScoresCount + bigScoresCount)
        {
            win = true;
        }
    }
}
