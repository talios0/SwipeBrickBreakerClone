using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAssignment : MonoBehaviour {



	// Use this for initialization
	void Start () {
        transform.parent.gameObject.GetComponent<BrickStats>().SetBrickText(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
