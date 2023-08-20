using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FireSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] fires;
    [SerializeField] float delay;

    Bounds bounds;
    float timer;

    void Start()
    {
        timer = delay;
        bounds = GetComponent<BoxCollider2D>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
        float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);

        if(timer <= 0)
        {
            GameObject newFire = GameObject.Instantiate(fires[Random.Range(0, fires.Length)]);
            newFire.transform.position = bounds.center + new Vector3(offsetX, offsetY, 0);

            timer = delay;
        }

        timer -= Time.deltaTime;
    }
}
