using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    [SerializeField] int lastInput; // 1 = down, 2 = up, 3 = right, 4 = left,
    [SerializeField] int movingTo; // same rule
    [SerializeField] Vector3 upPosition, downPosition, rightPosition, leftPosition;
    [SerializeField] GameObject anim;
    [SerializeField] LayerMask wall;
    [SerializeField] bool canGetKey;
    bool die;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(movingTo == 0 || GameManager.data.pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        PositionUpdate();

        Rotations(movingTo);
        if (Input.anyKey)
        {
            KeyGetter();
        }

        DirectionUpdate(lastInput);

        if (!GameManager.data.Die)
        {
            MoveDirection();
        }
        

        WallDetection();

        Die();

        if (GameManager.data.win)
        {
            movingTo = 0;
            canGetKey = false;
        }
    }
    private void KeyGetter()
    {
        if (canGetKey)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                lastInput = 4;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                lastInput = 3;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                lastInput = 2;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                lastInput = 1;
            }
        }
        
    }

    private void MoveDirection()
    {
        if (!Physics.Raycast(transform.position, Vector3.right, 1.3f, wall) && movingTo == 4)
        {
            gameObject.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (!Physics.Raycast(transform.position, Vector3.left, 1.3f, wall) && movingTo == 3)
        {
            gameObject.transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (!Physics.Raycast(transform.position, Vector3.back, 1.3f, wall) && movingTo == 2)
        {
            gameObject.transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }
        if (!Physics.Raycast(transform.position, Vector3.forward, 1.3f, wall) && movingTo == 1)
        {
            gameObject.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
    }

    private void PositionUpdate()
    {
        upPosition = transform.position + new Vector3(0, 0, -1.1f);
        downPosition = transform.position + new Vector3(0, 0, 1.1f);
        rightPosition = transform.position + new Vector3(-1.1f, 0, 0);
        leftPosition = transform.position + new Vector3(1.1f, 0, 0);
    }


    private void DirectionUpdate(int _direction)
    {
        if (_direction == 1 && !Physics.Raycast(rightPosition, Vector3.forward, 2) && !Physics.Raycast(leftPosition, Vector3.forward, 2))
        {
            movingTo = 1;
        }
        if (_direction == 2 && !Physics.Raycast(rightPosition, Vector3.back, 2) && !Physics.Raycast(leftPosition, Vector3.back, 2))
        {
            movingTo = 2;
        }
        if (_direction == 3 && !Physics.Raycast(upPosition, Vector3.left, 2) && !Physics.Raycast(downPosition, Vector3.left, 2))
        {
            movingTo = 3;
        }
        if (_direction == 4 && !Physics.Raycast(upPosition, Vector3.right, 2) && !Physics.Raycast(downPosition, Vector3.right, 2))
        {
            movingTo = 4;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (lastInput == 1)
        {
            Gizmos.DrawRay(rightPosition, Vector3.forward);
            Gizmos.DrawRay(leftPosition, Vector3.forward);
        }
        if (lastInput == 2)
        {
            Gizmos.DrawRay(rightPosition, Vector3.back);
            Gizmos.DrawRay(leftPosition, Vector3.back);
        }
        if (lastInput == 3)
        {
            Gizmos.DrawRay(upPosition, Vector3.left);
            Gizmos.DrawRay(downPosition, Vector3.left);
        }
        if (lastInput == 4)
        {
            Gizmos.DrawRay(upPosition, Vector3.right);
            Gizmos.DrawRay(downPosition, Vector3.right);
        }
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, Vector3.down * 1.4f);
        Gizmos.DrawRay(transform.position, Vector3.back * 1.4f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.right * 1.4f);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, Vector3.left * 1.4f);

    }
    private void Rotations(int MovingTo)
    {
        if(MovingTo == 1)
        {
            anim.transform.rotation = Quaternion.Euler(0,0,0);
        }
        if (MovingTo == 2)
        {
            anim.transform.rotation = Quaternion.Euler(0,180,0);
        }
        if (MovingTo == 3)
        {
            anim.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        if (MovingTo == 4)
        {
            anim.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    private void WallDetection()
    {
        if (movingTo == 1 && lastInput == 1 && Physics.Raycast(transform.position, Vector3.forward, 1.4f)|| movingTo == 2 && lastInput == 2 && Physics.Raycast(transform.position, Vector3.back, 1.4f)|| movingTo == 3 && lastInput == 3 && Physics.Raycast(transform.position, Vector3.left, 1.4f)|| movingTo == 4 && lastInput == 4 && Physics.Raycast(transform.position, Vector3.right, 1.4f))
        {
            GameManager.data.playerStuck = true;
        }
        else
        {
            GameManager.data.playerStuck = false;
        }
    }
    private void Die()
    {
        if (GameManager.data.Die)
        {
            gameObject.transform.localScale = transform.localScale - new Vector3(1f, 1f, 1f)*Time.deltaTime;
            if (transform.localScale.x < 0)
            {
                GameManager.data.lifes -= 1;
                transform.position = new Vector3 (0, 3, 0);
                transform.localScale = Vector3.one;
                lastInput = 0;
                movingTo = 0;
                GameManager.data.Die = false;
                GameManager.data.Return1 = true;
                GameManager.data.Return2 = true;
                GameManager.data.Return3 = true;
                GameManager.data.Return4 = true;
            }       
        }
    }

}
