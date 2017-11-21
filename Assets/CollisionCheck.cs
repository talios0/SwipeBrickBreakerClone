using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour {

    public bool startGame = true;
    private bool landed = true;

    public int ID;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            landed = true;
            if (ID == 0)
            {
                transform.position = new Vector3(transform.position.x, -4.65f, transform.position.z);
            }
        }
        if (collision.gameObject.tag == "Brick")
        {
            collision.gameObject.GetComponent<BrickStats>().HitBlock();
        }
    }
    


    public void SetLanded(bool _landed)
    {
        landed = _landed;
    }


    public bool GetLanded()
    {
        return landed;
    }


}
