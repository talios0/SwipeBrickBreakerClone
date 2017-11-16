using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //Delete extra balls when they come to a stop
    //Reset startGame bool in the first ball
    //Proceeed to next level
    //Allow the user to launch again
    //Update ball count
    //Number on bricks
    //Color on bricks
    //GameOver animation
    //Touch angle visual

    [Range(100,1000)]
    public int maxBalls;
    public int currentBalls;
    public GameObject Ball;
    private bool fired = false;
    public float waitTime;

    public List<GameObject> ballList;
    public float speed;
    private bool onGround = true;
    Vector3 mousePos;

    private void Start()
    {
        ballList = new List<GameObject>();
        ballList.Add(Instantiate(Ball));
        ballList[0].transform.parent = transform;
        ballList[0].transform.position = new Vector2(0, -4.716731f);
        ballList[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        CheckLaunch();
    }

    void CheckLaunch()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonUp("Fire1") && !fired)
        {
            Vector2 dir = mousePos - ballList[0].transform.position;
            dir.Normalize();

            fired = true;
            for (int x = 0; x < currentBalls; x++)
            {
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
                ballList[x].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                ballList[x].GetComponent<Rigidbody2D>().velocity = dir * speed;
            }
            if (x != 0)
            {
                ballList[x - 1].GetComponent<CollisionCheck>().startGame = false;
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
