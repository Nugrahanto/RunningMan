﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	private Animator myAnim;
	public float playerJumpForce = 500f;
	private float playerHurtTime = -1;
	private Collider2D myCollider;
	public Text scoreText;
	private float startTime;
	private int jumpsLeft = 2;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		myCollider = GetComponent<Collider2D> ();	

		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (playerHurtTime == -1) {

			if (Input.GetButtonUp ("Jump") && jumpsLeft > 0) {

				if (myRigidBody.velocity.y < 0) {
					myRigidBody.velocity = Vector2.zero;
				}

				if (jumpsLeft == 1) {
					myRigidBody.AddForce (transform.up * playerJumpForce * 0.75f);
				} else {
					myRigidBody.AddForce (transform.up * playerJumpForce);
				}
			
				jumpsLeft--;

			}
			myAnim.SetFloat ("vVelocity", myRigidBody.velocity.y);
			scoreText.text = (Time.time - startTime).ToString ("0.0");
		} 
		else {
			if (Time.time > playerHurtTime + 2) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer ("Enemy")) {

			foreach (PrefabsSpawner spawner in FindObjectsOfType<PrefabsSpawner>()) {
				spawner.enabled = false;
			}

			foreach (MoveLeft moveLefter in FindObjectsOfType<MoveLeft>()) {
				moveLefter.enabled = false;
			}

			playerHurtTime = Time.time;
			myAnim.SetBool ("playerHurt", true);
			myRigidBody.velocity = Vector2.zero;
			myRigidBody.AddForce (transform.up * playerJumpForce);
			myCollider.enabled = false;
		} 
		else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
			jumpsLeft = 2;
		}
	}
}
