using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickStats : MonoBehaviour {

    private GameObject PlayerManager;
    public GameObject BrickManager;
    public int HP;
    public GameObject BrickText;

    private void Start()
    {
        PlayerManager = GameObject.Find("PlayerManager");
        BrickManager = GameObject.Find("Brick Manager");
        SetHP(BrickManager.GetComponent<BrickManager>().GetScore());
    }

    public void SetBrickText(GameObject text)
    {
        BrickText = text;
        BrickText.GetComponent<TextMesh>().text = HP.ToString();
    }

    public void SetHP(int amount)
    {
        HP = amount;
        if (BrickText != null)
        {
            BrickText.GetComponent<TextMesh>().text = HP.ToString();
        }
    }

    public void HitBlock()
    {
        HP -= 1;
        BrickText.GetComponent<TextMesh>().text = HP.ToString();
        if (HP <= 0)
        {
            //PlayerManager.GetComponent<PlayerManager>().IncreaseBallCount();
            //Particle Effect
            Destroy(gameObject);
        }
    }

}
