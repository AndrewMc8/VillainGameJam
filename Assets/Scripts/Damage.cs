using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] float damage = 0;
    [SerializeField] bool oneTime = true;
    [SerializeField] string[] avoidTags;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (avoidTags.Length != 0)
        {
            foreach (string tag in avoidTags)
            {
                if (tag == other.tag) return;
            }
        }

        if (!oneTime) return;

        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.Damage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (avoidTags.Length != 0)
        {
            foreach (string tag in avoidTags)
            {
                if (tag == other.tag) return;
            }
        }

        if (oneTime) return;

        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.Damage(damage * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
            Debug.Log("attacking");
        }
    }
}
