using UnityEngine;
using System.Collections;

public class mover : MonoBehaviour {

    bool flagMove = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.x < -6.5f || transform.position.x > 6.5){
            flagMove = !flagMove;
        }

        if(flagMove){
            transform.Translate(Vector3.right/4f);    
        }
        else
        {
            transform.Translate(Vector3.left/4f);
        }

	}
}
