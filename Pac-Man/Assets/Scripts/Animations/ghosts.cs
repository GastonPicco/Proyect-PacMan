using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghosts : MonoBehaviour
{
    [SerializeField] private int animation_steps, type;
    [SerializeField] private GameObject[] asset;
    [SerializeField] private float timer,timer2, timerlimit, timerlimit2, timerDie;
    [SerializeField] private bool back;
    [SerializeField] private bool blueMode,white,forceBlueMode;
    [SerializeField] float blueModeTimer;
    Vector3 casePosition;
    // Start is called before the first frame update
    void Start()
    {
        casePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ReturnCase();
        EatCheck();
        timer2 += Time.deltaTime;
        if(timer2 > timerlimit2)
        {
            if (white)
            {
                white= false;
            }
            else
            {
                white = true;
            }
            timer2 = 0;
        }
        if (timer < 0)
        {
            back = false;
        }
        if (!back)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (timer > 0 && timer < timerlimit)
        {
            HiddeAll();
            if (!blueMode)
            {
                asset[0].SetActive(true);
            }
            else
            {
                if (!white)
                {
                    asset[2].SetActive(true);
                }
                else if (blueModeTimer > 7)
                {
                    asset[4].SetActive(true);
                }
                else
                {
                    asset[2].SetActive(true);
                }
                
                
            }

        }
        else if (timer >= timerlimit && timer < timerlimit * 2)
        {
            HiddeAll();
            if (!blueMode)
            {
                asset[1].SetActive(true);
            }
            else
            {
                if (!white)
                {
                    asset[3].SetActive(true);
                }
                else if(blueModeTimer > 7)
                {
                    asset[5].SetActive(true);
                }
                else
                {
                    asset[3].SetActive(true);
                }
            }
        }
        else if (timer > timerlimit * 2)
        {
            back = true;
        }
    }


    private void HiddeAll()
    {
        foreach (GameObject assets in asset)
        {
            assets.gameObject.SetActive(false);
        }
    }
    private void EatCheck()
    {
        if(GameManager.data.eatMode == true)
        {
            if(blueMode == false && blueModeTimer == 0)
            {
                blueMode = true;
            }          
            blueModeTimer += Time.deltaTime;
        }
        else
        {
            blueMode = false;
            blueModeTimer = 0;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "AreaSpawn")
        {
            if (blueMode == true)
            {
                blueMode = false;
            }     
        }
        if(other.gameObject.name == "RightPortal")
        {
            transform.position = new Vector3(19.4f, 3, transform.position.z);
        }
        if (other.gameObject.name == "LeftPortal")
        {
            transform.position = new Vector3(-19.4f, 3, transform.position.z);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && blueMode)
        {
            transform.position = casePosition;
            GameManager.data.Score += 200;
        }
        else if(collision.gameObject.tag == "Player" && !blueMode)
        {
            GameManager.data.Die = true;
        }
        
    }
    public void ReturnCase()
    {
        if (GameManager.data.Return1 && type == 1)
        {
            transform.position = casePosition;
            GameManager.data.Return1 = false;
        }
        else if (GameManager.data.Return2 && type == 2)
        {
            transform.position = casePosition;
            GameManager.data.Return2 = false;
        }
        else if (GameManager.data.Return3 && type == 3)
        {
            transform.position = casePosition;
            GameManager.data.Return3 = false;
        }
        else if (GameManager.data.Return4 && type == 4)
        {
            transform.position = casePosition;
            GameManager.data.Return4 = false;
        }
    }
}
