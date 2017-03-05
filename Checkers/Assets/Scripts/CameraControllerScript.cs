using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour {

	public Vector3 toPosition = new Vector3();
	public Quaternion toOrientation = new Quaternion();

	private Vector3 fromPosition = new Vector3();
	private Quaternion fromOrientation = new Quaternion();
	public bool isMoving = false;


	private Vector3 lookAt = Vector3.zero;
	private float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {
		fromPosition = transform.position;
		lookAt.x = 4.0f;
		lookAt.y = -2.0f;
		lookAt.z = 4.0f;
		transform.LookAt (lookAt);
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			elapsedTime += Time.deltaTime;

			if (elapsedTime >= 1.0f) {
				isMoving = false;
				transform.position = toPosition;
				fromPosition = transform.position;
				elapsedTime = 0.0f;
			} else {
				transform.position += (toPosition - fromPosition) * Time.deltaTime;
				transform.LookAt (lookAt);
			}
		}
	}
}
