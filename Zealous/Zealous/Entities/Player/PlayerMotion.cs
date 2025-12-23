using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GameComponents;
using GameComponents.Managers;
using GameComponents.Entity;
using System;
using GameComponents.Logic;
using GameComponents.Helpers;

namespace Zealous;

public sealed class PlayerMovement 
{
    private float moveSpeed = 50f;
    private float maxSpeed = 500f;
    private float acceleration = 1f;
    private float jumpForce = 1200f;
    private float gravity = 25f;
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
    public bool IsJumping => velocity.Y < 1f;
    public bool IsControllable { get; set; } = true;
    public bool IsMovementActive { get; set; } = true;
    public bool IsPlummeting { get; private set; } = false;
    
    public Motions MotionState = Motions.Idle;
    
    private readonly InputManager input = new();
    private Vector2 velocity = Vector2.Zero;
    
    public readonly Timer DashDuration, DashCooldown;
    
    // helpers
    public float NormalizedSpeedProgress => moveSpeed / maxSpeed;
    public void SwitchMotions(Motions newMotionState) => MotionState = newMotionState;
    
    public PlayerMovement() 
    {
        DashDuration = new Timer(0.25f);
        DashCooldown = new Timer(0.65f);
    }
    
    public void Jump() 
    {
        if (!CanJump) return;
        velocity.Y = (int)-JumpForce;
    }
    
    // main Update loop
    
    public void UpdateMotion(GameTime gt, Entity target) 
    {
        if (!IsMovementActive) return;
        input.UpdateInputs();
        stateManager();
        DashCooldown.TickTock(gt);
        DashDuration.TickTock(gt);
        
        switch(MotionState) 
        {
            case Motions.Idle: idle(); break; // idle, enables control, turns ashing & jumping to false.
            case Motions.Moving: moving(); break; // Moving,, same with idle.
            case Motions.Dashing: dashing(target); break; // disables control for dashing.
            case Motions.Sliding: plummeting(); break; // disables control and can be dashed mid-sequence.
        }
        
        target.Velocity = velocity;
    }
    
    // states
    
    private void stateManager() 
    {
        if (input.WASD && IsControllable && MotionState != Motions.Sliding) 
        {
            Gravity = 0;
            SwitchMotions(Motions.Moving);
        }
        else if (MotionState != Motions.Dashing && MotionState != Motions.Sliding) 
        {
            Gravity = 0;
            SwitchMotions(Motions.Idle);
        }
        
        bool dashIsViable = CanDash && IsControllable && !IsDashing && input.IsKeyPressed(Keys.LeftShift) && velocity != Vector2.Zero;
        if (dashIsViable && DashCooldown.TimeHitsFloor() || (dashIsViable && DashCooldown.TimeHitsFloor() && MotionState == Motions.Sliding)) 
        {
            IsControllable = false;
            IsDashing = true;
            DashCooldown.Restart();
            DashDuration.Restart();
            SwitchMotions(Motions.Dashing);
        }
        
        if (input.IsKeyPressed(Keys.E) && IsJumping) 
        {
            IsDashing = false;
            SwitchMotions(Motions.Sliding);
        }
        else if (input.IsKeyPressed(Keys.Space)) 
        {
            Jump();
            Gravity = 0;
        }
        else if (MotionState != Motions.Sliding) 
        {
            Gravity = 25;
            velocity.Y += Gravity;
        }
        
        Diagnostics.Write($"CurrentState: {MotionState}, {velocity}");
    }
    
    private void idle() 
    {
        IsDashing = false;
        IsControllable = true;
        IsPlummeting = false;
        
        velocity.X = MathHelper.Lerp(velocity.X, 0f, 0.1f);
    }
    
    private void moving() 
    {
        IsDashing = false;
        IsControllable = true;
        IsPlummeting = false;
        
        velocity.X = MathHelper.Clamp(velocity.X, -MaxSpeed, MaxSpeed);
        
        if (input.IsKeyDown(Keys.A)) velocity.X -= MoveSpeed;
        else if (input.IsKeyDown(Keys.D)) velocity.X += MoveSpeed;
    }
    
    private void dashing(Entity player)
    {
        velocity.Y = 0;
        IsPlummeting = false;
        
        velocity.X = player.Direction.X * DashForce;
        if (DashDuration.TimeHitsFloor() || IsPlummeting) 
        {
            IsControllable = true;
            IsDashing = false;
            SwitchMotions(Motions.Moving);
        }
    }
    
    private void plummeting() 
    {
        IsPlummeting = true;
        velocity.Y = 1500f;
        velocity.X = input.IsKeyDown(Keys.A) ? -1f : input.IsKeyDown(Keys.D) ? 1f : 1f;
    }
}