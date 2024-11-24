using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFalling : MonoBehaviour
{

    [SerializeField] private float despawnTimerAfterCollision = 2f;
    [SerializeField] private LayerMask ground;
    private bool collided;


    private void Update()
    {
        if (collided)
        {
            updateTimer(despawnTimerAfterCollision);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (((1 << collision.gameObject.layer) & ground) != 0)
        {
            collided = true;
        }
       
    }

    private void updateTimer(float despawnTimer)
    {
        despawnTimer -= Time.deltaTime;

        despawnTimerAfterCollision = despawnTimer;

        if(despawnTimerAfterCollision <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
