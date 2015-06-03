using UnityEngine;
using System.Collections;

public class Simplemovement : MonoBehaviour {
    //before start put in this
    Vector3 moveVector = Vector3.zero;
    float playerSpeed;
    float gravity;
    float jumpSpeed;
    CharacterController controller;

    void Awake()
    {
        playerSpeed = 6;
        gravity = 20;
        jumpSpeed = 16;
        controller = GetComponent<CharacterController>();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (controller.isGrounded)
        {
            moveVector = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            moveVector *= jumpSpeed;
            if(Input.GetKey(KeyCode.Space))
            {
                moveVector.y = jumpSpeed;
            }
        }

        moveVector.y -= gravity * Time.deltaTime;
        controller.Move(moveVector * Time.deltaTime);

	}
}
