using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {

    public Transform startMenuMount;
    public Transform mainMenuMount;
    public float distance = 10f;

    public Transform targetMount;
    public float speed = 3.0f;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.SqrMagnitude(transform.position-startMenuMount.transform.position) < distance && Input.anyKey)
        {
            targetMount = mainMenuMount;
        }
		transform.position = Vector3.Lerp(transform.position, targetMount.position, speed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetMount.rotation, speed * Time.deltaTime);
		
	}

    public void setMount(Transform newMount)
    {
        targetMount = newMount;
    }


}
