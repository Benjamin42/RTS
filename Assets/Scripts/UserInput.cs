using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour {

	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		MoveCamera();
		RotateCamera();

		if (Input.GetKey (KeyCode.Escape)) {
			Debug.Log ("Button clicked !");
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
	}

	private void MoveCamera() {
		Vector3 movement = new Vector3(0,0,0);

		//horizontal camera movement
		if(Input.GetKey(KeyCode.Q)) {
			movement.x -= ResourceManager.ScrollSpeed;
		} else if (Input.GetKey(KeyCode.D)) {
			movement.x += ResourceManager.ScrollSpeed;
		}

		//vertical camera movement
		if (Input.GetKey(KeyCode.S)) {
			movement.z -= ResourceManager.ScrollSpeed;
		} else if (Input.GetKey(KeyCode.Z)) {
			movement.z += ResourceManager.ScrollSpeed;
		}

		//make sure movement is in the direction the camera is pointing
		//but ignore the vertical tilt of the camera to get sensible scrolling
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;

		//away from ground movement
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;

		//limit away from ground movement to be between a minimum and maximum distance
		if (destination.y > ResourceManager.MaxCameraHeight) {
			destination.y = ResourceManager.MaxCameraHeight;
		} else if (destination.y < ResourceManager.MinCameraHeight) {
			destination.y = ResourceManager.MinCameraHeight;
		}

		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}
	}

	private void RotateCamera() {
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;

		// TODO : pivoter la caméra par rapport à l'objet pointé (prendre en compte l'angle)
		if(Input.GetKey(KeyCode.A)) {
			destination.y -= ResourceManager.ScrollSpeed;
		} else if (Input.GetKey(KeyCode.E)) {
			destination.y += ResourceManager.ScrollSpeed;
		}

		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		}
	}
}