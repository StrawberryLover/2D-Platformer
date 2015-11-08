using UnityEngine;
using System.Collections;

public class Camera2D : MonoBehaviour {
	public float dampTime = 0.15f;
	public Vector3 minCameraPos;
	public Vector3 maxCameraPos;

	private Transform target;
	private SpriteRenderer spriteBounds;

	private Vector3 velocity = Vector3.zero;

	private float camVertExtent;
	private float camHorzExtent;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Player").transform;

		Camera camera = Camera.main;

		camVertExtent = camera.orthographicSize;
		camHorzExtent = camera.aspect * camVertExtent;
	}
	
	// Update is called once per frame
	void Update () {
		Camera camera = Camera.main;

		if (target) {
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;

			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

			
			float pixelRatio = (camera.orthographicSize * 2) / camera.pixelHeight;

			// Camera bounds
			float leftBound   = minCameraPos.x;
			float rightBound  = maxCameraPos.x;

			float bottomBound = minCameraPos.y;
			float topBound    = maxCameraPos.y;

			// Camera shrink
			/*if(camera.pixelWidth > 675) {
				leftBound  = leftBound  * (camera.pixelWidth / 675);
				rightBound = rightBound * (camera.pixelWidth / 675);
			}*/

			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, leftBound, rightBound),
				Mathf.Clamp(transform.position.y, bottomBound, topBound),
				transform.position.z
			);
		}
	}
}