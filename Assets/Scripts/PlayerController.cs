
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float maxspeed = 10f;
	public float jumpForce = 0.05f;
	public float jumpX = 0;
	public float doubleJumpModifier = 1.2f;

	private float groundRadius = 1f;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	
	private Rigidbody2D rb2d;
	private Animator anim;
	private SpriteRenderer sprite;

	[HideInInspector]
	public  bool lookingRight = true;
	private bool doubleJump = false;
	private bool standing = true;

	// Use this for initialization
	void Start () {  
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Jump") && standing) {
			//rb2d.velocity = new Vector2(rb2d.velocity.x, -jumpForce);
			//jumpX = 0.2f;

			standing = false;
			rb2d.AddForce(transform.up * 30000f * Time.deltaTime, ForceMode2D.Force);
		}

		if (Input.GetButtonDown ("Jump")) {
			//rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce * 0.1f * jumpX);
			//jumpX = ((jumpX > 0.1f) ? jumpX * 0.95f : 0f);
		}

		Debug.Log (rb2d.velocity.y);
	}
	
	void FixedUpdate()
	{
		// grounded = setStatus ();

		float move = Input.GetAxis ("Horizontal");
	
		// Move
		rb2d.velocity = new Vector2(move * maxspeed, rb2d.velocity.y);

		// Flip player
		if ((move > 0 && !lookingRight) || (move < 0 && lookingRight)) {
			Flip ();
		} else {
			anim.SetBool ("Run", rb2d.velocity.x != 0.0);
		}
	}

	private bool setStatus()
	{
		float yVelocity = rb2d.velocity.y;


		anim.SetBool("Jump",   yVelocity >= 0);
		anim.SetBool("Falling", yVelocity < 0);

		Debug.Log ("Vertical: " + rb2d.velocity.y);

		return false;
	}	

	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Floor") {
			anim.SetBool ("Jump", false);
			standing = true;
		}
	}
	
	public void Flip()
	{
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}
}