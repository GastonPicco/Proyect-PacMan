using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int animation_steps;
    [SerializeField] private GameObject[] asset;
    [SerializeField] private float timer,timerlimit;
    [SerializeField] private bool back;
    [SerializeField] GameObject pointingPoint;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.data.Player = gameObject;
        GameManager.data.pointingPoint = pointingPoint;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.data.playerStuck == false)
        {
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

            if (timer > 0 && timer < timerlimit && !GameManager.data.Die)
            {
                HiddeAll();
                asset[0].SetActive(true);
            }
            else if (timer >= timerlimit && timer < timerlimit * 2 && !GameManager.data.Die)
            {
                HiddeAll();
                asset[1].SetActive(true);
            }
            else if (timer >= timerlimit * 2 && timer < timerlimit * 3 && !GameManager.data.Die)
            {
                HiddeAll();
                asset[2].SetActive(true);
            }
            else if (timer >= timerlimit * 3 && timer < timerlimit * 4)
            {
                HiddeAll();
                asset[3].SetActive(true);
                if (GameManager.data.Die)
                {
                    timer = timerlimit * 3;
                }
            }
            else if (timer > timerlimit * 4 && !GameManager.data.Die)
            {
                back = true;
            }

        }
        else if(GameManager.data.playerStuck)
        {
            HiddeAll();
            {
                HiddeAll();
                asset[3].SetActive(true);
            }
        }
    }
    private void HiddeAll()
    {
        foreach (GameObject assets in asset)
        {
            assets.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "RightPortal")
        {
            transform.position = new Vector3(19.4f, 3, transform.position.z);
        }
        if (other.gameObject.name == "LeftPortal")
        {
            transform.position = new Vector3(-19.4f, 3, transform.position.z);
        }
    }
}
