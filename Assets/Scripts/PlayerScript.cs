using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float speed;
    [Header("Ground")]
    [SerializeField] Transform groundTransform;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundRadius;
    [Header("Attack")]
    [SerializeField] Transform attackTransform;
    [SerializeField] float attackRadius;
    [Header("Sound")]
    [SerializeField] AudioSource footstep;
    [SerializeField] AudioSource attack;

    Rigidbody2D rb;
    Vector2 velocity = Vector2.zero;
    float groundAngle = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // check if on ground
        bool onGround = UpdateGroundCheck() && (velocity.y <= 0);

        // get direction input
        Vector2 direction = Vector2.zero;
        direction.x = Input.GetAxis("Horizontal");

        velocity.x = direction.x * speed;

        // set velocity
        if (onGround)
        {

            if (velocity.y < 0) velocity.y = 0;

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Attack");
                attack.Play();
            }
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;

        // move character
        rb.velocity = velocity;

        // update animator
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
    }

    private bool UpdateGroundCheck()
    {
        // check if the character is on the ground
        Collider2D collider = Physics2D.OverlapCircle(groundTransform.position, groundRadius, groundLayerMask);
        if (collider != null)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(groundTransform.position, Vector2.down, groundRadius, groundLayerMask);
            if (raycastHit.collider != null)
            {
                // get the angle of the ground (angle between up vector and ground normal)
                groundAngle = Vector2.SignedAngle(Vector2.up, raycastHit.normal);
                Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.red);
            }
        }

        return (collider != null);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundTransform.position, groundRadius);
    }

    private void CheackAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackTransform.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;

            if (collider.gameObject.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.Damage(10);
            }
        }
    }

    public void Footstep()
    {
        footstep.Play();
    }
}
