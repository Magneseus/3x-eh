 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMeeple : MonoBehaviour {

    public static bool dragging;

	// Use this for initialization
	void Start () {
        dragging = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnBeginDrag() {
        dragging = true;
    }

    public void OnDrag() {
        this.transform.position = Input.mousePosition;
    }

    public void OnEndDrag() {
        dragging = false;
    }
}
