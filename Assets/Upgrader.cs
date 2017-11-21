using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            collision.transform.parent.GetComponent<PlayerManager>().IncreaseBallCount();
            //PARTICLE EFFECT
            Destroy(gameObject);
        }
    }
}
