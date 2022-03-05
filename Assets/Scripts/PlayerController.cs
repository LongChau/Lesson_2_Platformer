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
    private bool _isFlip;

    [SerializeField] private float _attackTime = 0.5f;
    private float _time;
    
    // Start is called before the first frame update
    void Start()
    {
        _state = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //_time -= Time.deltaTime;
        //if (_time <= 0)
        //{
        //    _hitBox.enabled = false;
        //    _time = 0;
        //}

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
            //AttackAsync();
            //_hitBox.enabled = true;
            //_time = _attackTime;
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _state = PlayerState.Run;
            //_render.flipX = false;
            if (_isFlip)
                Flip();
            _anim.SetBool("run", true);
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _state = PlayerState.Run;
            //_render.flipX = true;
            if (!_isFlip)
                Flip();
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

    private void Flip()
    {
        _isFlip = !_isFlip;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private async void AttackAsync()
    {
        _hitBox.enabled = true;
        await Task.Delay(100);
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
        //Debug.Log("OnAttack_01_End");
        _state = PlayerState.Idle;
        _anim.SetTrigger("idle");
        _hitBox.enabled = false;
    }

    public void OnAttack_01()
    {
        _hitBox.enabled = true;
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
