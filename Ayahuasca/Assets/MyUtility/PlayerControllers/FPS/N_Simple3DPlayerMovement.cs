using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class N_Simple3DPlayerMovement : MonoBehaviour
{
    [SerializeField] float movespeed = 6;
    [SerializeField] float jumpheight = 2;
    public float gravity;

    [Range(0, 10), SerializeField] float airControl = 5;

    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    Vector3 input;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input *= movespeed;
        input = transform.TransformDirection(input);

        if (controller.isGrounded)
        {
            moveDirection = input;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(8 * gravity * jumpheight);
            }
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }
        input = transform.TransformDirection(input);
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }


}
