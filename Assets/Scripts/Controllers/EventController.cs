using System;
using UnityEngine;

public class EventController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResolveEvent(IEvent e)
    {
        e.Resolve();
    }
}
