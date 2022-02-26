using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] SpriteRenderer _render;
    [SerializeField] PlayerState _state;
    [SerializeField] float _rayLength;
    [SerializeField] Transform _checkGround;
    [SerializeField] Rigidbody2D _rigid2d;
    [SerializeField] float _jumpForce;
    public float _speed;

    [SerializeField] private bool _isOnGround;
    
    // Start is called before the first frame update
    void Start()
    {
        _state = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnGround();

        if (_state == PlayerState.Attack) return;
        if (_state == PlayerState.Jump)
        {
            // TODO: What should I do?
            if (_isOnGround)
            {
                _state = PlayerState.Idle;
                _anim.SetTrigger("idle");
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            _state = PlayerState.Jump;
            _anim.SetTrigger("jump");
            _rigid2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _state = PlayerState.Attack;
            _anim.SetTrigger("attack");
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _state = PlayerState.Run;
            _render.flipX = false;
            _anim.SetBool("run", true);
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
            //_rigid2d.velocity += Vector2.left * _speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _state = PlayerState.Run;
            _render.flipX = true;
            _anim.SetBool("run", true);
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
            //_rigid2d.velocity -= Vector2.left * _speed * Time.deltaTime;
        }
        else
        {
            if (_state != PlayerState.Idle)
            {
                _state = PlayerState.Idle;
                _anim.SetBool("run", false);
                _anim.SetTrigger("idle");
            }
        }
    }

    private void CheckOnGround()
    {
        Debug.DrawRay(_checkGround.position, Vector2.down * _rayLength, Color.red);
        var hit = Physics2D.Raycast(_checkGround.position, Vector2.down, _rayLength);
        if (hit.collider && hit.collider.CompareTag("Ground"))
            _isOnGround = true;
        else
            _isOnGround = false;
        // Ngắn hơn.
        //_isOnGround = hit.collider && hit.collider.CompareTag("Ground");
    }

    public void OnAttack_01_End()
    {
        Debug.Log("OnAttack_01_End");
        _state = PlayerState.Idle;
        _anim.SetTrigger("idle");
    }
}

public enum PlayerState
{
    None = 0,
    Idle = 1,
    Run = 2,
    Attack = 3,
    Jump = 4,
    Dead = 5,
}
