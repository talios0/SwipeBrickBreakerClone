using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [Range(100,1000)]
    public int maxBalls;
    public int currentBalls;
    public GameObject Ball;
    private bool fired = false;

    public List<GameObject> ballList;
    public float speed;
    private bool onGround = true;

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
        if (Input.GetButton("Fire1") && !fired)
        {
            fired = true;
            float angle = Vector2.Angle(new Vector2(ballList[0].transform.position.x, ballList[0].transform.position.y), new Vector2(Input.mousePosition.x,Input.mousePosition.y));
            for (int x = 0; x < currentBalls; x++)
            {
                GameObject ball = Instantiate(Ball);
                ball.transform.parent = ballList[0].transform.parent;
                ball.transform.position = ballList[0].transform.position;
                ballList.Add(ball);
            }
            foreach (GameObject g in ballList)
            {
                g.transform.LookAt(Input.mousePosition);
                g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                g.GetComponent<Rigidbody2D>().velocity = new Vector2(0,speed);
            }
        }
    }
}
