using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] SpriteRenderer _render;
    [SerializeField] PlayerState _state;
    [SerializeField] float _rayLength;
    [SerializeField] Transform _checkGround;
    [SerializeField] Rigidbody2D _rigid2D;
    [SerializeField] float _jumpForce;
    [SerializeField] BoxCollider2D _hitBox;

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
        //Debug.Log($"Update: {Time.deltaTime}");
        CheckOnGround();

        if (_state == PlayerState.Attack) return;
        if (_state == PlayerState.Jump)
        {
            //if (_isOnGround)
            //{
            //    _state = PlayerState.Idle;
            //    _anim.SetTrigger("idle");
            //}
            //return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            _state = PlayerState.Jump;
            _anim.SetTrigger("jump");
            _rigid2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _state = PlayerState.Attack;
            _anim.SetTrigger("attack");
            Attack();
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _state = PlayerState.Run;
            _render.flipX = false;
            _anim.SetBool("run", true);
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _state = PlayerState.Run;
            _render.flipX = true;
            _anim.SetBool("run", true);
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
        else
        {
            if (_state == PlayerState.Jump && !_isOnGround) return;
            if (_state != PlayerState.Idle)
            {
                _state = PlayerState.Idle;
                _anim.SetBool("run", false);
                _anim.SetTrigger("idle");
            }
        }
    }

    private async void Attack()
    {
        _hitBox.enabled = true;
        await Task.Delay(1000);
        _hitBox.enabled = false;
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        //Debug.Log($"FixedUpdate: {Time.fixedDeltaTime}");
        //if (_state == PlayerState.Jump && _isOnGround)
        //{
        //    _rigid2D.AddForce(Vector2.up * _jumpForce);
        //}
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

        _anim.SetBool("isGround", _isOnGround);
    }

    public void OnAttack_01_End()
    {
        Debug.Log("OnAttack_01_End");
        _state = PlayerState.Idle;
        _anim.SetTrigger("idle");
    }

    [ContextMenu("Test_Force")]
    private void Test_Force()
    {
        _rigid2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Force);
    }

    [ContextMenu("Test_ForceImpulse")]
    private void Test_ForceImpulse()
    {
        _rigid2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
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
