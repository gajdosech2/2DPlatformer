using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jumped = false;
	bool crouched = false;

	void Update() 
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Crouch"))
		{
			crouched = true;
		} 
		else if (Input.GetButtonUp("Crouch"))
		{
			crouched = false;
		}
		else if (Input.GetButtonDown("Jump") && !animator.GetBool("IsCrouching"))
		{
			jumped = true;
			animator.SetBool("IsJumping", true);
		}
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	public void OnCrouching()
	{
		animator.SetBool("IsCrouching", crouched);
	}

	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouched, jumped);
		jumped = false;
	}
}
