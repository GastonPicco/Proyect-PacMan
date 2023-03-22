using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if(player.transform.position.z < 0 && player.transform.position.z > -28.8)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y,startPosition.z) + new Vector3(0, 0, player.transform.position.z * 0.8f);
        }
        transform.position = new Vector3(startPosition.x, transform.position.y, transform.position.z) + new Vector3(player.transform.position.x * 0.6f, 0, 0);
        
    }
}
