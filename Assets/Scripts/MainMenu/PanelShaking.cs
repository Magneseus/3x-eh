using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShaking : MonoBehaviour {

	public Vector3 origin;
	Vector3 target;
	Quaternion targetRot;
	public Quaternion originalRot;
	public float ratio;
	public float ratioMultiplier;

	void Start(){
		origin = this.transform.position;
		originalRot = this.transform.rotation;
	}
	void Update(){
		float movement = Mathf.Sin (Time.time);
		//target = new Vector3(origin.x+movement/2,origin.y+movement/2,origin.z+movement/10);
		targetRot = new Quaternion (originalRot.x+movement*Time.deltaTime,originalRot.y+movement*Time.deltaTime,originalRot.z+movement*Time.deltaTime,originalRot.w+movement*Time.deltaTime);
		//Debug.Log (movement);	
		//transform.position = Vector3.Lerp (transform.position,target,ratio);
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation,targetRot,ratio*ratioMultiplier);

	}
}

