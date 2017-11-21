using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    //Number on bricks
    //Color on bricks
    //GameOver animation
    //Touch angle visual
    //Particle Effects

    [Range(100,1000)]
    public int maxBalls;
    public int currentBalls;
    public GameObject Ball;
    private bool fired = false;
    public float waitTime;
    public GameObject bricks;
    public int levelCounter;

    public List<GameObject> ballList;
    public float speed;
    private bool onGround = true;
    Vector3 mousePos;

    private bool firstStart = true;

    private bool nextLevel = true;

    private void Start()
    {
        ballList = new List<GameObject>();
        ballList.Add(Instantiate(Ball));
        ballList[0].transform.parent = transform;
        ballList[0].transform.position = new Vector2(0, -4.716731f);
        ballList[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    public void BallLand(int ballIndex)
    {
        Destroy(ballList[ballIndex]);
    }

    public void IncreaseBallCount()
    {
        //Debug.Log("Increased");
        currentBalls++;
        //Debug.Log(currentBalls);
    }

    bool CheckFinished()
    {
        int ballListLength = ballList.Count; //BallList.Count will change if it still has balls in the list other than the first one.
        if (ballList.Count > 1)
        {
            for (int x = 0; x < ballList.Count; x++)
            {
                if (!ballList[x].GetComponent<CollisionCheck>().GetLanded())
                {
                    return false;
                }
                else
                {
                    nextLevel = false;
                    if (x != 0)
                    {
                        Destroy(ballList[x]);
                        ballList.RemoveAt(x);
                        x -= 1;
                    }
                }
            }
        }
        if (ballList[0].GetComponent<Rigidbody2D>().velocity == Vector2.zero && fired && firstStart && levelCounter == 1)
        {
            //firstStart = false;
            Debug.Log("Count = 0");
            nextLevel = false;
            //bricks.GetComponent<CollisionCheck>().SetLanded(true);
            fired = false;
        }
        if (ballList.Count == 1  && fired && ballList[0].GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            Debug.Log("Count = 1 ");
            nextLevel = false;
        }
        //Debug.Log("Ready!");
        return true;
    }


    private void Update()
    {
        CheckLaunch();
        //Debug.Log(nextLevel);
        if (CheckFinished() && !nextLevel)
        {
            fired = false;
            bricks.GetComponent<BrickManager>().NextLevel();
            nextLevel = true;
        }
    }

    void CheckLaunch()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonUp("Fire1") && !fired)
        {
            levelCounter++;
            Vector2 dir = mousePos - ballList[0].transform.position;
            dir.Normalize();

            fired = true;
            //Debug.Log(currentBalls);
            for (int x = 0; x < currentBalls; x++)
            {
                //Debug.Log(x);
                GameObject ball = Instantiate(Ball);
                ball.transform.parent = ballList[0].transform.parent;
                ball.transform.position = ballList[0].transform.position;
                ballList.Add(ball);
            }
            StartCoroutine(LaunchBall(dir));

        }
    }

    IEnumerator LaunchBall(Vector2 dir)
    {
        for (int x= 0; x<ballList.Count+1; x++)
        {
            if (x < ballList.Count)
            {
                //Debug.Log(x);
                ballList[x].GetComponent<CollisionCheck>().SetLanded(false);
                ballList[x].name = "Ball: " + x;
                ballList[x].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                ballList[x].GetComponent<Rigidbody2D>().velocity = dir * speed;
            }
            if (x!= 0)
            {
                ballList[x-1].GetComponent<CollisionCheck>().startGame = false;
            }
            yield return new WaitForSeconds(waitTime);
            
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ballList[0].transform.position, mousePos);
        Gizmos.DrawRay(ballList[0].transform.position, new Vector3(ballList[0].transform.position.x, ballList[0].transform.position.y + 250, ballList[0].transform.position.z));
    }

}
