using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public LayerMask groundLayer;
	public Transform groundCheck;
	public Transform ceilingCheck;
	public Collider2D crouchDisableCollider;
	public Rigidbody2D rigidbody2D;

	const float ceilingRadius = 0.2f;
	const float groundedRadius = 0.2f;
	private float jumpForce = 700f;
	private float crouchSpeed = 0.36f;
	private float movementSmoothing = 0.05f;
	private bool airControl = true;
	private bool grounded = true;
	private bool facingRight = true;
	private bool wasCrouching = false;
	private Vector3 velocity = Vector3.zero;

	public UnityEvent OnLandEvent;
	public UnityEvent OnCrouchEvent;	

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = grounded;
		grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
				break;
			}
		}
	}

	public void Move(float move, bool crouched, bool jumped)
	{
		if (grounded || airControl)
		{
			if (!crouched && Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, groundLayer))
			{
				crouched = true;
			}

			if (crouched != wasCrouching)
            {
				wasCrouching = crouched;
				OnCrouchEvent.Invoke();
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = !crouched;
			}

			Vector3 targetVelocity = new Vector2(move * 10f * ((crouched) ? crouchSpeed : 1), rigidbody2D.velocity.y);
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
			if ((move > 0 && !facingRight) || (move < 0 && facingRight))
			{
				facingRight = !facingRight;
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}

		if (grounded && jumped)
		{
			grounded = false;
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}
	}

}
