
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public class GroundState {
		private GameObject body;
		private float width, height, length;

		public GroundState(GameObject b) {
			body = b;
		}

		public bool isWall() {
			return false;
		}

		public bool isGround() {
			return true;
		}

		public bool isTouching() {
			return true;
		}
			
		// Returns direction of wall	
		public int wallDirection() {
			return -1;
		}
	}

	public float speed    	  = 14f;
	public float accel    	  = 6f;
	public float airAccel 	  = 3f;
	public float jump     	  = 14f;
	public float maxJumpFrame = 25f;
	
	private bool  isJumping = true;
	private float force;

	private GroundState state;
	private Vector2 input;

	public float maxspeed = 10f;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	
	private Rigidbody2D rb2d;
	private Animator anim;
	private SpriteRenderer sprite;

	[HideInInspector]
	public  bool lookingRight = true;

	// Use this for initialization
	void Start () {  
		rb2d   = GetComponent<Rigidbody2D>();
		anim   = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer> ();

		state  = new GroundState (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if      (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  input.x = -1;
		else if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) input.x = 1;
		else                                        						   input.x = 0;

		if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.W)) {
			if (isJumping) {
				input.y += 1;
				force = Mathf.Cos (((input.y * Mathf.PI) / 180));
				isJumping = input.y <= maxJumpFrame;
			} else input.y = 0;

			Debug.Log ("Acceleration Y: " + isJumping + ", force: " + force + ", mY: " + input.y);
		} else input.y = 0;
	}


	void FixedUpdate()
	{
		float move = Input.GetAxis ("Horizontal");

		// Force
		rb2d.AddForce(  new Vector2(((input.x * speed) - rb2d.velocity.x) * (state.isGround() ? accel : airAccel), 0));

		// Velocity
		rb2d.velocity = new Vector2(
			(input.x == 0 && state.isGround())   ? 0                       : rb2d.velocity.x, 
			(input.y >= 1 && state.isTouching()) ? jump + (force / 100f)   : rb2d.velocity.y);

		// Wall Jump
		if (state.isWall () && !state.isGround () && input.y == 1)
			rb2d.velocity = new Vector2 (-state.wallDirection () * speed * 0.75f, 
			                            rb2d.velocity.y);

		//Debug.Log ("vX: " + rb2d.velocity.x + ", vY: " + rb2d.velocity.y);
		//Debug.Log ("mX: " + input.x + ", mY: " + input.y);

		// Flip player
		if ((move > 0 && !lookingRight) || (move < 0 && lookingRight)) Flip();
		else anim.SetBool ("Run", rb2d.velocity.x != 0.0);
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Floor") {
			isJumping = true;
			anim.SetBool ("Jump", false);
		}
	}
	
	public void Flip() {
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}
}