using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 11f;
    private Rigidbody2D _rigidBody;  

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            _rigidBody.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime, 0f);
        }
        else
        {
            _rigidBody.velocity = new Vector2(-movementSpeed * Time.fixedDeltaTime, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        FlipEnemy();
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_rigidBody.velocity.x)), 1f);
        //movementSpeed = -movementSpeed;
    }
}
