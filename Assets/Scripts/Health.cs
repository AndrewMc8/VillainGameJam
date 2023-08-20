using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject deathPrefab;
    [SerializeField] bool destroyOnDeath = true;
    [SerializeField] float maxHealth = 100;

    public float health { get; set; }
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void Damage(float damage)
    {
        health -= damage;

        if(!isDead && health <= 0)
        {
            isDead = true;
            if(TryGetComponent<IDestructible>(out IDestructible destructible))
            {
                destructible.Destroyed();
            }

            if(deathPrefab != null)
            {
                Instantiate(deathPrefab, transform.position, transform.rotation);
            }
            if(destroyOnDeath)
            {
                Destroy(gameObject);

            }
        }
    }
}
