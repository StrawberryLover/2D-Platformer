using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public string toLocation = "Home";

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void  OnTriggerStay2D(Collider2D item) {
		if (item.name == "Player") {
			if (Input.GetKeyDown (KeyCode.E)) {
				anim.SetBool("Open", true);
				Application.LoadLevel(toLocation);
			}
		}
	}

	void  OnTriggerExit2D(Collider2D item) {
		if (item.name == "Player") {
			anim.SetBool("Open", false);
		}
	}
}
