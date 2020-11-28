using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    // Config
    [SerializeField]
    [Range(20, 1000)]
    int spood = 100;

    [SerializeField]
    [Range(20, 1000)]
    int climbSpood = 100;

    [SerializeField]
    [Range(5, 1000)]
    float jumpSpood = 525;

    [SerializeField] Vector2 deathAnimation = new Vector2(25f, 25f);
    [SerializeField] float deathRotation = 10f;

    // Cached component references
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private BoxCollider2D _bottomCollider; 
    private float _gravityScaleAtStart;

    // State
    private float _movementHorizontal;
    private float _movementVertical;
    private bool _isJumping;
    private bool _isTouchingLadder;
    private bool _isAlive = true;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
        _bottomCollider = GetComponent<BoxCollider2D>();

        _gravityScaleAtStart = _rigidBody.gravityScale;
    }

    private void Update()
    {
        GetPlayerInput();
    }

    private void FixedUpdate()
    {
        if (!_isAlive) return;
        Run();
        Jump();
        FlipMountainDew();
        ClimbLadder();
        Die();
    }

    private void GetPlayerInput()
    {
        // Checks User input a/d keys or </> keys 
        _movementHorizontal = Input.GetAxis("Horizontal");

        // Checks User input w/s keys or ^/v keys 
        _movementVertical = Input.GetAxis("Vertical"); 

        if (Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
        }

    }

    private void Run()
    {
        // If movement not equal to zero means user pressed movement key, then 
        _animator.SetBool("Running", _movementHorizontal != 0);

        // Set velocity 
        _rigidBody.velocity = new Vector2(_movementHorizontal * spood * Time.fixedDeltaTime, _rigidBody.velocity.y);
    }

    private void ClimbLadder()
    {
        if (!_bottomCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            _rigidBody.gravityScale = _gravityScaleAtStart;
            _animator.SetBool("Climbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(_rigidBody.velocity.x, _movementVertical * climbSpood * Time.fixedDeltaTime);
        _rigidBody.velocity = climbVelocity;

        _rigidBody.gravityScale = 0f;

        bool _isClimbing = Mathf.Abs(_rigidBody.velocity.y) > Mathf.Epsilon;
        _animator.SetBool("Climbing", _isClimbing);

        print("climbiiiiing hehehhehe te nandayo");
    }

    private void Jump()
    {
        if (!_bottomCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            _isJumping = false;
            return;
        }; // prevents double jumps 

        if (_isJumping)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpood * Time.deltaTime);
            _rigidBody.velocity += jumpVelocityToAdd;
            _isJumping = false;
        }
    }

    private void FlipMountainDew()
    {
        bool playerHasHorizontalSpood = Mathf.Abs(_rigidBody.velocity.x) > Mathf.Epsilon;

        // If player has spood, player goes flip floop
        if (playerHasHorizontalSpood)
        {
            transform.localScale = new Vector3(Mathf.Sign(_rigidBody.velocity.x), 1f);
        }

    }

    private void Die()
    {
        if(_collider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            _animator.SetTrigger("Die");
            _rigidBody.velocity = deathAnimation;
            _isAlive = false;
        }
    }

}
