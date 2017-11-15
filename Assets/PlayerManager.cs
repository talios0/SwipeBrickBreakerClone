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
        if (Input.GetButton("Fire1") && !fired)
        {
            fired = true;
            float angle = Vector2.Angle(ballList[0].transform.position, new Vector2(mousePos.x, mousePos.y));
            angle = 180 - angle;
            Debug.Log(angle);
            for (int x = 0; x < currentBalls; x++)
            {
                GameObject ball = Instantiate(Ball);
                ball.transform.parent = ballList[0].transform.parent;
                ball.transform.position = ballList[0].transform.position;
                ballList.Add(ball);
            }
            foreach (GameObject g in ballList)
            {
                g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                g.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sin(angle)*speed,Mathf.Cos(angle)*speed);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ballList[0].transform.position, mousePos);
        Gizmos.DrawRay(ballList[0].transform.position, new Vector3(ballList[0].transform.position.x, ballList[0].transform.position.y + 250, ballList[0].transform.position.z));
    }

}
