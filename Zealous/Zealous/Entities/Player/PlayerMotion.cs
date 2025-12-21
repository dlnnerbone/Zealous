using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GameComponents;
using GameComponents.Managers;
using GameComponents.Entity;
using System;
using GameComponents.Helpers;

namespace Zealous;

public sealed class PlayerMovement 
{
    private float moveSpeed = 50f;
    private float maxSpeed = 500f;
    private float acceleration = 1f;
    private float jumpForce = 500f;
    private float gravity = 10f;
    private float dashForce = 2000f;
    
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = MathHelper.Clamp(value, 0f, maxSpeed) * acceleration; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = MathHelper.Clamp(value, moveSpeed, float.PositiveInfinity); }
    public float Acceleration { get => acceleration; set => acceleration = Math.Abs(value); }
    public float JumpForce { get => jumpForce; set => jumpForce = Math.Abs(value); }
    public float Gravity { get => gravity; set => gravity = Math.Abs(value); }
    public float DashForce { get => dashForce; set => dashForce = MathHelper.Clamp(value, MaxSpeed, float.PositiveInfinity); }
    
    public bool CanDash { get; set; } = true;
    public bool IsDashing { get; private set; } = false;
    public bool CanJump { get; set; } = true;
    public bool IsJumping { get; private set; } = false;
    public bool IsControllable { get; set; } = true;
    public bool IsMovementActive { get; set; } = true;
    
    public Motions MotionState = Motions.Idle;
    
    private readonly InputManager input = new();
    private Vector2 velocity = Vector2.Zero;
    
    // helpers
    public float NormalizedSpeedProgress => moveSpeed / maxSpeed;
    public void SwitchMotions(Motions newMotionState) => MotionState = newMotionState;
    
    public void Jump() 
    {
        if (!CanJump) return;
        IsJumping = true;
        velocity.Y = (int)-JumpForce;
    }
    
    // main Update loop
    
    public void UpdateMotion(GameTime gt, Entity target) 
    {
        if (!IsMovementActive) return;
        input.UpdateInputs();
        stateManager();
        
        switch(MotionState) 
        {
            case Motions.Idle: idle(); break; // idle, enables control, turns ashing & jumping to false.
            case Motions.Moving: moving(); break; // Moving,, same with idle.
            case Motions.Dashing: dashing(target); break; // disables control for dashing.
            case Motions.Jumping: jumping(); break; // enables jumping and control.
            case Motions.Sliding: plummeling(); break; // disables control and can be dashed mid-sequence.
        }
        
        target.Velocity = velocity;
    }
    
    // states
    
    private void stateManager() 
    {
        if (input.WASD) SwitchMotions(Motions.Moving);
        else if (MotionState != Motions.Dashing) SwitchMotions(Motions.Idle);
        
    }
    
    private void idle() 
    {
        IsDashing = false;
        IsJumping = false;
        IsControllable = true;
        
        velocity = Vector2.Lerp(velocity, Vector2.Zero, 0.1f);
    }
    
    private void moving() 
    {
        IsDashing = false;
        IsJumping = false;
        IsControllable = true;
        
        var maxVector = new Vector2(MaxSpeed, MaxSpeed);
        var minVector = new Vector2(-MaxSpeed, -MaxSpeed);
        velocity = Vector2.Clamp(velocity, minVector, maxVector);
        
        if (input.IsKeyDown(Keys.A)) velocity.X -= MoveSpeed;
        else if (input.IsKeyDown(Keys.D)) velocity.X += MoveSpeed;
    }
    
    private void dashing(Entity player)
    {
        
    }
    
    private void jumping() {}
    
    private void plummeling() {}
}