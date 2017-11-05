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

	private Vector3 mouseOrigin;
	private bool isPanning;

	private void RotateCamera() {
		
		if (Input.GetMouseButtonDown (1)) 
		{
			//right click was pressed    
			isPanning = true;
			mouseOrigin = Input.mousePosition;
		}


		// cancel on button release
		if (!Input.GetMouseButton (1)) 
		{
			isPanning = false;
		}

		//move camera on X & Y
		if (isPanning) 
		{
			Vector3 pos     = Camera.main.ScreenToViewportPoint (Input.mousePosition - mouseOrigin);

			// update x and y but not z
			Vector3 move     = new Vector3 (pos.y * ResourceManager.RotateSpeed, pos.x * ResourceManager.RotateSpeed, 0);

			//Camera.main.transform.Translate (move, Space.Self);

			Camera.main.transform.Rotate (move, Space.World);

			//Vector3 origin = Camera.main.transform.eulerAngles;
			//Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, move, Time.deltaTime * ResourceManager.RotateSpeed);
		}
	}
}