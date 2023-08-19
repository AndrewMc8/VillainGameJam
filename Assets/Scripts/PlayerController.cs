using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController2D controller;
    [SerializeField] Animator animator;
    [SerializeField] float speed;

    Vector2 direction = Vector2.zero;
    bool jump = false;
    bool crouch = false;

    void Update()
    {
        direction.x = Input.GetAxis("Horizontal") * speed;
        animator.SetFloat("Speed", Mathf.Abs(direction.x));

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jumping", true);
            jump = true;
        }

        controller.Move(direction.x, false, jump);
    }

    private void FixedUpdate()
    {
        controller.Move(direction.x * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void OnLand()
    {
        animator.SetBool("Jumping", false);
    }
}
