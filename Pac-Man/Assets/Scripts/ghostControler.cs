using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostControler : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int lastInput; // 1 = down, 2 = up, 3 = right, 4 = left,
    [SerializeField] int movingTo; // same rule
    [SerializeField] int prevMovTo;
    [SerializeField] Vector3 upPosition, downPosition, rightPosition, leftPosition;
    [SerializeField] GameObject anim;
    [SerializeField] bool up, down, right, left;
    [SerializeField] LayerMask walls;
    [SerializeField] GameObject Player;
    [SerializeField] float playerMagnitude, minMagnitude;
    float timer, timer2, speedTimer, turnBlockTimer, timerRandomChase;
    [SerializeField] bool chase, pointingPointFind, randomChase;
    public bool _randomChase;
    int rand;
    float ghostX, ghostZ;
    float playerX, playerZ;
    float absPlayerX, absPlayerZ;
    [SerializeField] bool blockVertical, blockHrizontal;
    float movingSpeed;
    Vector3 posSpeed;
    bool turning,escape;
    GameObject pointingPoint;
    [SerializeField] bool waiting;
    [SerializeField] float waitingTime, waitingTimer;
    float _waitingTime;
    Vector3 outPosition;

    void Start()
    {
        Player = GameManager.data.Player;
        pointingPoint = GameManager.data.pointingPoint;
        posSpeed = Player.transform.position;
        waiting = true;     
        outPosition = new Vector3(0,3,-19.2f);
        _waitingTime = waitingTime;
    }
    void Update()
    {      
        RayCast(); // rayos 
        PositionUpdate();// actualiza posiciones claves
        Rotations(movingTo); // rotacion

        PlayerMagnitude(); //distancias

        if (!turning)
        {
            DirectionUpdate(lastInput); // actualiza movingTo
        }
        
        MoveDirection(); // mueve al personaje
        Antiturn();
        Escape();
        RandomChase();

        if (waiting)
        {
            waitingTimer += Time.deltaTime;
            if(waitingTimer > waitingTime)
            {
                waiting = false;
                waitingTimer = 0;
                transform.position = outPosition;
            }
        }
        if (GameManager.data.Die)
        {
            waitingTimer = 0;
            waitingTime = _waitingTime;
        }
    }
    private void FixedUpdate()
    {
        if (chase == false || GameManager.data.eatMode || _randomChase)
        {
            RandomMoveGetter();//random IA
        }
        else
        {
            ChaseMoveGetter();// Cazar IA
        }
        Speed();
    }
    private void RandomMoveGetter()
    {
        timer2 += Time.deltaTime;
        if(timer2 > 0)
        {
            if (movingTo == 1)
            {
                if (rand == 0 && !down)
                {
                    lastInput = 1;
                }
                else if (rand != 0 && !down && right && left)
                {
                    lastInput = 1;
                }
                else if (rand == 1 && !right)
                {
                    lastInput = 3;
                    timer2 = -0.3f;
                }
                else if (rand == 2 && !left)
                {
                    lastInput = 4;
                    timer2 = -0.3f;
                }
                else if (down && !right && !left)
                {
                    rand = Random.Range(0, 2);
                    if(rand == 0)
                    {
                        lastInput = 3;
                    }
                    else
                    {
                        lastInput = 4;
                    }
                }
                else if(down && right && !left)
                {
                    lastInput = 4;
                }
                else if (down && left && !right)
                {
                    lastInput = 3;
                }
            }
            if (movingTo == 2)
            {
                if (rand == 0 && !up)
                {
                    lastInput = 2;
                }
                else if (rand != 0 && !up && right && left)
                {
                    lastInput = 2;
                }
                else if (rand == 1 && !right || up && !right)
                {
                    lastInput = 3;
                    timer2 = -0.3f;
                }
                else if (rand == 2 && !left || up && !left)
                {
                    lastInput = 4;
                    timer2 = -0.3f;
                }
                else if (up && !right && !left)
                {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        lastInput = 3;
                    }
                    else
                    {
                        lastInput = 4;
                    }
                }
                else if (up && right && !left)
                {
                    lastInput = 4;
                }
                else if (up && left && !right)
                {
                    lastInput = 3;
                }
            }
            if (movingTo == 3)
            {
                if (rand == 0 && !right)
                {
                    lastInput = 3;
                }
                else if (rand != 0 && !right && up && down)
                {
                    lastInput = 3;
                }
                else if (rand == 1 && !up || right && !up)
                {
                    lastInput = 2;
                    timer2 = -0.3f;
                }
                else if (rand == 2 && !down || right && !down)
                {
                    lastInput = 1;
                    timer2 = -0.3f;
                }
                else if (right && !up && !down)
                {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        lastInput = 1;
                    }
                    else
                    {
                        lastInput = 2;
                    }
                }
                else if (right && up && !down)
                {
                    lastInput = 1;
                }
                else if (right && !up && down)
                {
                    lastInput = 2;
                }

            }
            if (movingTo == 4)
            {
                if (rand == 0 && !left)
                {
                    lastInput = 4;
                }
                else if (rand != 0 && !left && up && down)
                {
                    lastInput = 4;
                }
                else if (rand == 1 && !up || left && !up)
                {
                    lastInput = 2;
                    timer2 = -0.3f;
                }
                else if (rand == 2 && !down || left && !down)
                {
                    lastInput = 1;
                    timer2 = -0.3f;
                }
                else if (left && !up && !down)
                {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        lastInput = 1;
                    }
                    else
                    {
                        lastInput = 2;
                    }
                }
                else if (left && up && !down)
                {
                    lastInput = 1;
                }
                else if (left && !up && down)
                {
                    lastInput = 2;
                }

            }
        }
        

    }
    private void ChaseMoveGetter()
    {
        //down
        if (playerZ > ghostZ && absPlayerX < absPlayerZ)
        {
            if (!down && lastInput != 2 && movingTo != 2)
            {
                lastInput = 1;
                blockVertical = true;
                blockHrizontal = false;
                Debug.Log("por down esta block vertical " + blockVertical + ", block horizontal " + blockHrizontal);
            }
            else if(!blockHrizontal)
            {
                if (!right && !left)
                {
                    if (playerX < ghostX)
                    {
                        lastInput = 3;
                        blockHrizontal = true;
                    }
                    else
                    {
                        lastInput = 4;
                        blockHrizontal = true;
                    }
                }
                else if (!right)
                {
                    lastInput = 3;
                    blockHrizontal = true;
                }
                else if (!left)
                {
                    lastInput = 4;
                    blockHrizontal = true;
                }
            }
            else if (blockHrizontal && movingTo == 3 && right && down && !up || blockVertical && movingTo == 4 && left && down && !up)
            {
                lastInput = 2;
            }
        }
        //up
        if (playerZ < ghostZ && absPlayerX < absPlayerZ )
        {
            if (!up && lastInput != 1 && movingTo != 1)
            {
                lastInput = 2;
                blockVertical = true;
                blockHrizontal = false;
                Debug.Log("por up esta block vertical " + blockVertical +", block horizontal " + blockHrizontal);
            }
            else if (!blockHrizontal)
            {
                if (!right && !left)
                {
                    if (playerX < ghostX)
                    {
                        lastInput = 3;
                        blockHrizontal = true;
                    }
                    else
                    {
                        lastInput = 4;
                        blockHrizontal = true;
                    }
                }
                else if (!right)
                {
                    lastInput = 3;
                    blockHrizontal = true;
                }
                else if (!left)
                {
                    lastInput = 4;
                    blockHrizontal = true;
                }
            }
            else if (blockHrizontal && movingTo == 3 && right && up && !down || blockVertical && movingTo == 4 && left && up && !down)
            {
                lastInput = 1;
            }
        }
        //right
        if (playerX < ghostX && absPlayerX > absPlayerZ)
        {
            if (!right && lastInput != 4 && movingTo != 4)
            {
                blockHrizontal = true;
                blockVertical = false;
                lastInput = 3;
                Debug.Log("por right esta block vertical " + blockVertical + ", block horizontal " + blockHrizontal);
            }
            else if (!blockVertical)
            {
                if (!up && !down )
                {
                    if (playerZ < ghostZ)
                    {
                        lastInput = 2;
                        blockVertical = true;
                    }
                    else
                    {
                        lastInput = 1;
                        blockVertical = true;
                    }
                }
                else if (!up)
                {
                    lastInput = 2;
                    blockVertical = true;
                }
                else if (!down)
                {
                    lastInput = 1;
                    blockVertical = true;
                }
            }
            else if(blockVertical && movingTo == 2 && up && right && !left || blockVertical && movingTo == 1 && down && right && !left)
            {
                lastInput = 4;
            }
        }
        //left
        if (playerX>ghostX && absPlayerX > absPlayerZ)
        {
            if (!left && lastInput != 3 && movingTo != 3)
            {
                lastInput = 4;
                blockHrizontal = true;
                blockVertical = false;
                Debug.Log("por left esta block vertical " + blockVertical + ", block horizontal " + blockHrizontal);
            }
            else if (!blockVertical)
            {
                if (!up && !down)
                {
                    if (playerZ < ghostZ)
                    {
                        lastInput = 2;
                        blockVertical = true;

                    }
                    else
                    {
                        lastInput = 1;
                        blockVertical = true;
                    }
                }
                else if (!up)
                {
                    lastInput = 2;
                    blockVertical = true;
                }
                else if (!down)
                {
                    lastInput = 1;
                    blockVertical = true;
                }
            }
            else if (blockVertical && movingTo == 2 && up && left && !right || blockVertical && movingTo == 1 && down && left && !right)
            {
                lastInput = 3;
            }
        }
    }
    private void MoveDirection()
    {
        if (!Physics.Raycast(transform.position, Vector3.right, 1.3f,walls) && movingTo == 4)
        {
            gameObject.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (!Physics.Raycast(transform.position, Vector3.left, 1.3f, walls) && movingTo == 3)
        {
            gameObject.transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (!Physics.Raycast(transform.position, Vector3.back, 1.3f, walls) && movingTo == 2)
        {
            gameObject.transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }
        if (!Physics.Raycast(transform.position, Vector3.forward, 1.3f, walls) && movingTo == 1)
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

        ghostX = transform.position.x;
        ghostZ = transform.position.z;

        if (!pointingPointFind)
        {
            playerX = Player.transform.position.x;
            playerZ = Player.transform.position.z;
        }
        else
        {
            playerX = pointingPoint.transform.position.x;
            playerZ = pointingPoint.transform.position.z;
        }


        absPlayerX = Mathf.Abs(playerX - ghostX);
        absPlayerZ = Mathf.Abs(playerZ - ghostZ);
    }
    private void DirectionUpdate(int _direction)
    {
        if (_direction == 1 && !Physics.Raycast(rightPosition, Vector3.forward, 2) && !Physics.Raycast(leftPosition, Vector3.forward, 2) && movingTo != 2)
        {
            movingTo = 1;
        }
        if (_direction == 2 && !Physics.Raycast(rightPosition, Vector3.back, 2) && !Physics.Raycast(leftPosition, Vector3.back, 2) && movingTo != 1)
        {
            movingTo = 2;
        }
        if (_direction == 3 && !Physics.Raycast(upPosition, Vector3.left, 2) && !Physics.Raycast(downPosition, Vector3.left, 2) && movingTo != 4)
        {
            movingTo = 3;
        }
        if (_direction == 4 && !Physics.Raycast(upPosition, Vector3.right, 2) && !Physics.Raycast(downPosition, Vector3.right, 2) && movingTo != 3)
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

    }
    private void Rotations(int MovingTo)
    {
        if (MovingTo == 1)
        {
            anim.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (MovingTo == 2)
        {
            anim.transform.rotation = Quaternion.Euler(0, 180, 0);
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
    private void RayCast()
    {
        timer += Time.deltaTime;
        if(timer > 0.7f)
        {
            rand = Random.Range(0,3);
            timer = 0;
        }

        if(Physics.Raycast(transform.position, Vector3.forward, 2f,walls))
        {
            down = true;
        }
        else
        {
            down = false;
        }

        if(Physics.Raycast(transform.position, Vector3.back, 2f, walls))
        {
            up = true;
        }
        else
        {
            up = false;
        }
        if(Physics.Raycast(transform.position, Vector3.right, 2f, walls))
        {
            left = true;
        }
        else
        {
            left = false;
        }
        if(Physics.Raycast(transform.position, Vector3.left, 2f, walls))
        {
            right = true;
        }
        else
        {
            right = false;
        }




    }
    private void PlayerMagnitude()
    {
        playerMagnitude = (Player.transform.position - gameObject.transform.position).magnitude;
        if (playerMagnitude < minMagnitude)
        {
            chase = true;           
        }
        else
        {
            chase = false;
        }
        
    }
    private void Speed()
    {
        movingSpeed = (transform.position - posSpeed).magnitude;
        posSpeed = transform.position;
        if (movingSpeed == 0)
        {
            speedTimer += Time.fixedDeltaTime;
            if(speedTimer > 0.1f)
            {
                if (movingTo == 1)
                {
                    if (!right && !left)
                    {
                        lastInput = Random.Range(3, 5);

                    }
                    else if (!right)
                    {
                        lastInput = 3;
                    }
                    else if (!left)
                    {
                        lastInput = 4;
                    }
                }
                if (movingTo == 2)
                {
                    if (!right && !left)
                    {
                        lastInput = Random.Range(3, 5);

                    }
                    else if (!right)
                    {
                        lastInput = 3;
                    }
                    else if (!left)
                    {
                        lastInput = 4;
                    }
                }
                if (movingTo == 3)
                {
                    if (!up && !down)
                    {
                        lastInput = Random.Range(1, 3);

                    }
                    else if (!up)
                    {
                        lastInput = 2;
                    }
                    else if (!down)
                    {
                        lastInput = 1;
                    }
                }
                if (movingTo == 4)
                {
                    if (!up && !down)
                    {
                        lastInput = Random.Range(1, 3);

                    }
                    else if (!up)
                    {
                        lastInput = 2;
                    }
                    else if (!down)
                    {
                        lastInput = 1;
                    }
                }
                speedTimer = 0;
            }
            
        }
    }
    public void Escape()
    {
        if (GameManager.data.eatMode)
        {
            
            if(movingTo == 1 && !escape)
            {
                movingTo = 2;
                escape = true;
            }
            if (movingTo == 2 && !escape)
            {
                movingTo = 1;
                escape = true;
            }
            if (movingTo == 3 && !escape)
            {
                movingTo = 4;
                escape = true;
            }
            if (movingTo == 4 && !escape)
            {
                movingTo = 3;
                escape = true;
            }
        }
        else
        {
            escape = false;
        }
    }
    private void Antiturn()
    {
        if (movingTo != prevMovTo)
        {
            turning = true;
        }
        prevMovTo = movingTo;
        if (turning)
        {
            turnBlockTimer += Time.deltaTime;
            if (turnBlockTimer > 0.3)
            {
                turning = false;
                turnBlockTimer = 0;
            }
        }
    }
    private void RandomChase()
    {
        if (randomChase)
        {
            timerRandomChase += Time.deltaTime;
            if (timerRandomChase > 15 && _randomChase)
            {
                timerRandomChase = 0;
                _randomChase = false;
            }
            if (timerRandomChase > 15 && !_randomChase)
            {
                timerRandomChase = 0;
                _randomChase = true;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AreaSpawn")
        {
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "AreaSpawn")
        {
            waiting = true;
            speed = 4;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "AreaSpawn")
        {
            speed = 7;
        }
    }

}

