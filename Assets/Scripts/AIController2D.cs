using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIController2D : MonoBehaviour, IDamagable
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
	[SerializeField] AudioSource attackSound;
    [Header("AI")]
	[SerializeField] Transform[] waypoints;
	[SerializeField] float rayDistance = 1;
	[SerializeField] string enemyTag;
	[SerializeField] LayerMask raycastLayerMask;

	public float health = 100;

	Rigidbody2D rb;

	Vector2 velocity = Vector2.zero;
	bool faceRight = true;
	Transform targetWaypoint = null;
	GameObject playerGameObject = null;

	enum State
	{
		IDLE,
		PATROL,
		CHASE,
		ATTACK
	}

	State state = State.IDLE;
	float stateTimer = 1;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// update AI
		CheckEnemySeen();

		Vector2 direction = Vector2.zero;
		switch (state)
		{
			case State.IDLE:
				{
					if (playerGameObject != null) state = State.CHASE;
					stateTimer -= Time.deltaTime;
					if (stateTimer <= 0)
					{
						SetNewWaypointTarget();
						state = State.PATROL;
					}
					break;
				}
			case State.PATROL:
				{
					if (playerGameObject != null) state = State.CHASE;
					direction.x = Mathf.Sign(targetWaypoint.position.x - transform.position.x);
					float dx = Mathf.Abs(transform.position.x - targetWaypoint.position.x);
					if (dx <= 0.25f)
					{
						state = State.IDLE;
						stateTimer = 1;
					}
					break;
				}
			case State.CHASE:
                {
                    if (playerGameObject == null)
                    {
                        state = State.IDLE;
                        stateTimer = 1;
                        break;
                    }
                    float dx = Mathf.Abs(playerGameObject.transform.position.x - transform.position.x);
                    if (dx <= 1f)
                    {
                        state = State.ATTACK;
                        animator.SetTrigger("Attack");
                    }
                    else
                    {
                        direction.x = Mathf.Sign(playerGameObject.transform.position.x - transform.position.x);
                    }
					break;
                }
            case State.ATTACK:
				{
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                    {
                        state = State.CHASE;
                    }
					break;
                }
			default:
				break;
		}

		// check if on ground
		bool onGround = Physics2D.OverlapCircle(groundTransform.position, groundRadius, groundLayerMask) != null;

		velocity.x = direction.x * speed;
		
		// set velocity
		if (onGround)
		{

			if(velocity.y < 0) velocity.y = 0;
			if(Input.GetButtonDown("Jump"))
			{
				//velocity.y += Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
				//StartCoroutine(DoubleJump());
				//animator.SetTrigger("Jump");
			}
		}

		velocity.y += Physics.gravity.y * Time.deltaTime;

		// move character
		rb.velocity = velocity;

		//rotate character to face direction of movement
		if (velocity.x > 0 && !faceRight) Flip();
		if (velocity.x < 0 && faceRight) Flip();

		// update animator
		animator.SetFloat("Speed", Mathf.Abs(velocity.x));
		
		if (health <= 0)
		{
			animator.SetTrigger("Death");
			//velocity = Vector2.zero;
			GameManager.Instance.SetNumLeft();
			Destroy(gameObject);
		}
	}

	private void Flip()
	{
		faceRight = !faceRight;
		spriteRenderer.flipX = !faceRight;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(groundTransform.position, groundRadius);
	}

	private void SetNewWaypointTarget()
	{
		Transform waypoint = null;
		while(waypoint == targetWaypoint || !waypoint)
		{
			waypoint = waypoints[Random.Range(0, waypoints.Length)];
		}
		targetWaypoint = waypoint;
	}

    private void CheckEnemySeen()
    {
        playerGameObject = null;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, ((faceRight) ? Vector2.right : Vector2.left), rayDistance, raycastLayerMask);
        Debug.DrawRay(transform.position, ((faceRight) ? Vector2.right : Vector2.left) * rayDistance, Color.red);
        if (raycastHit.collider != null && raycastHit.transform.gameObject.CompareTag(enemyTag))
        {
            playerGameObject = raycastHit.collider.gameObject;
            Debug.DrawRay(transform.position, ((faceRight) ? Vector2.right : Vector2.left) * rayDistance, Color.green);
        }
    }

    private void CheackAttack()
    {
		attackSound.Play();
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

    public void Damage(int damage)
	{
		health -= damage;
		print(health);
	}
}
