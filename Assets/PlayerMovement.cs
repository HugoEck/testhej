using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : CharacterController2D
{
    [SerializeField] protected float _runSpeed;
    [SerializeField] private float _knockbackForce;    
    [SerializeField] private float _knockbackTotalTime;
    
    private Wall_Jumping _wallJumping;

    protected float _horizontalMove;
    private float _knockbackCounter;

    public Vector2 inputMovement = Vector2.zero;
    private bool jumped;
    private bool moveLeft;
    private bool moveRight;
    private bool spaceJump;
    public bool SpaceJump { get { return spaceJump; } } 
        
    private bool knockbackFromRight;
    public float KnockbackForce { get { return _knockbackForce; } set { _knockbackForce = value; } }  
    public float KnockbackCounter { get { return _knockbackCounter; } set { _knockbackCounter = value; } }  
    public float KnockbackTotalTime { get { return _knockbackTotalTime; } }    
    public bool KnockbackFromRight { get { return knockbackFromRight; } set { knockbackFromRight = value; } }


    protected override void Awake()
    {        
        base.Awake();
    }

    // Update is called once per frame
    protected virtual void Update()
    {              
        _wallJumping = Object.FindObjectOfType<Wall_Jumping>();
        
        _horizontalMove = /*Input.GetAxisRaw("Horizontal")*/inputMovement.x * _runSpeed;
        Vector2 move = new Vector2(inputMovement.x, inputMovement.y) * _runSpeed;
        //Debug.Log(jumped);
        if (/*Input.GetButtonDown("Jump")*/  jumped || spaceJump)
        {
            
            
            //Debug.Log(_jump);
        }        
    }
    public void Jump(InputAction.CallbackContext value)
    {
        //Debug.Log(value.phase);
        
    }
    protected override void FixedUpdate()
    {
        //Move our character
        
        base.FixedUpdate();
        
        if (_knockbackCounter <= 0)
        {
            if (!spaceJump)
            {
                Move(_horizontalMove * Time.fixedDeltaTime, false, jumped);
            }
            else
            {
                Move(_horizontalMove * Time.fixedDeltaTime, false, spaceJump);
            }

            //Debug.Log(jumped);

        }        
        else
        {
            if (knockbackFromRight == true)
            {
                _rigidBody2D.velocity = new Vector2(-_knockbackForce, _knockbackForce);
            }
            if (knockbackFromRight == false)
            {
                _rigidBody2D.velocity = new Vector2(_knockbackForce, _knockbackForce);
            }
            _knockbackCounter -= Time.deltaTime;
            
        }
    }
    protected void ChangeMovementValues(float newMaxSpeed, float newAccelerationForce, float newJumpForce, float newDeaccelerationForce)
    {
        this.transform.parent.GetComponent<PlayerMovement>().M_MaxSpeed = newMaxSpeed;
        this.transform.parent.GetComponent<PlayerMovement>().AccelerationForce = newAccelerationForce;
        this.transform.parent.GetComponent<PlayerMovement>().JumpForce = newJumpForce;
        this.transform.parent.GetComponent<PlayerMovement>().DeaccelerationForce = newDeaccelerationForce;
    }

    //Metoder till nya input systemets inputActions
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        moveLeft = context.action.triggered;
        if (moveLeft)
        {
            inputMovement.x = -1;
        }
        else if (moveRight)
        {
            inputMovement.x = 1;
        }
        else
        {
            inputMovement.x = 0;
        }
    }
    public void OnRight(InputAction.CallbackContext context)
    {
        moveRight = context.action.triggered;
        if (moveRight)
        {
            inputMovement.x = 1;
        }
        else if (moveLeft)
        {
            inputMovement.x = -1;
        }
        else
        {
            inputMovement.x = 0;
        }
    }

    public virtual void OnJump(InputAction.CallbackContext context)
    {
        
        jumped = context.action.triggered;
        _wallJumping.HasJumped = context.action.triggered;
    }
    
    public void OnSpaceJump(InputAction.CallbackContext context)
    {
        spaceJump = context.action.triggered;
        _wallJumping.HasJumped = context.action.triggered;
    }
   
}
