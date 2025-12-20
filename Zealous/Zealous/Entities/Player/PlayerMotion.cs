using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GameComponents;
using GameComponents.Managers;
using GameComponents.Entity;
using System;

namespace Zealous;

public sealed class PlayerMovement 
{
    private float moveSpeed = 50f;
    private float maxSpeed = 500f;
    private float acceleration = 1f;
    private float jumpForce = 500f;
    private float gravity = 10f;
    
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = MathHelper.Clamp(value, 0f, maxSpeed) * acceleration; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = MathHelper.Clamp(value, moveSpeed, float.PositiveInfinity); }
    public float Acceleration { get => acceleration; set => acceleration = Math.Abs(value); }
    public float JumpForce { get => jumpForce; set => jumpForce = Math.Abs(value); }
    public float Gravity { get => gravity; set => gravity = Math.Abs(value); }
    
    public bool CanDash { get; set; } = true;
    public bool IsDashing { get; private set; } = false;
    public bool CanJump { get; set; } = true;
    public bool IsJumping { get; private set; } = false;
    public bool IsControllable { get; set; } = true;
    public bool IsMovementActive { get; set; } = true;
    
    public Motions MotionState = Motions.Idle;
    
    // helpers
    public float NormalizedSpeedProgress => moveSpeed / maxSpeed;
    public void SwitchMotions(Motions newMotionState) => MotionState = newMotionState;
    
    public void Jump() 
    {
        if (!CanJump) return;
        IsJumping = true;
        velocity.Y = (int)-JumpForce;
    }
    // input
    
    private readonly InputManager input = new();
    private Vector2 velocity = Vector2.Zero;
    
    // main Update loop
    
    public void UpdateMotion(GameTime gt, Entity target) 
    {
        if (!IsMovementActive) return;
        input.UpdateInputs();
        
        if (input.IsKeyPressed(Keys.E)) GameComponents.Helpers.Diagnostics.Write($"It's working brother");
    }
}