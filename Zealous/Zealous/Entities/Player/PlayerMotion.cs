using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GameComponents;
using GameComponents.Managers;
using GameComponents.Entity;
using System;
using GameComponents.Logic;
using GameComponents.Helpers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GameComponents.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Zealous;

public sealed class PlayerMovement 
{
    private float moveSpeed = 50f;
    private float maxSpeed = 500f;
    private float acceleration = 1f;
    private float jumpForce = 1200f;
    private float gravity = 25f;
    private float dashForce = 2000f;
    private float stamina = 100f;
    private float maxStamina = 100f;
    
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = MathHelper.Clamp(value, 0f, maxSpeed) * acceleration; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = MathHelper.Clamp(value, moveSpeed, float.PositiveInfinity); }
    public float Acceleration { get => acceleration; set => acceleration = Math.Abs(value); }
    public float JumpForce { get => jumpForce; set => jumpForce = Math.Abs(value); }
    public float Gravity { get => gravity; set => gravity = Math.Abs(value); }
    public float DashForce { get => dashForce; set => dashForce = MathHelper.Clamp(value, MaxSpeed, float.PositiveInfinity); }
    public float Stamina { get => stamina; set => stamina = MathHelper.Clamp(value, 0f, maxStamina); }
    public float MaxStamina { get => maxStamina; set => maxStamina = MathHelper.Clamp(value, float.Epsilon, float.PositiveInfinity); }
    
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
    
    public readonly Timer DashDuration, DashCooldown, StaminaRegen;
    public readonly AudioManager Audio;
    public SpriteText Font { get; private set; }
    
    // helpers
    public float NormalizedSpeedProgress => moveSpeed / maxSpeed;
    public float NormalizedStamina => Stamina / MaxStamina;
    
    public void SwitchMotions(Motions newMotionState) => MotionState = newMotionState;
    
    public PlayerMovement() 
    {
        DashDuration = new Timer(0.25f);
        DashCooldown = new Timer(0.65f);
        StaminaRegen = new Timer(0.85f, TimeStates.Down, true);
        
        Audio = new();
    }
    
    public void Load(ContentManager content) 
    {
        Audio.AddSoundEffect("Dash", content.Load<SoundEffect>("Audio/uuhhh_wav"));
        Audio.AddSoundEffect("LowOnStamina", content.Load<SoundEffect>("Audio/HeartbeatWhenLow"));
        
        Font = new(content.Load<SpriteFont>("Fonts/VCR_EAS"));
        Font.Position = new Vector2(50, 100);
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
        staminaRegen();
        
        DashCooldown.TickTock(gt);
        DashDuration.TickTock(gt);
        StaminaRegen.TickTock(gt);
        
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
        
        if (dashIsViable && DashCooldown.TimeHitsFloor() || (dashIsViable && DashCooldown.TimeHitsFloor() && MotionState == Motions.Sliding) && Stamina > 22f) 
        {
            IsControllable = false;
            IsDashing = true;
            DashCooldown.Restart();
            DashDuration.Restart();
            Audio.PlaySound("Dash");
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
    
    private void staminaRegen() 
    {
        var nextRegen = Stamina + 10f;
        if (StaminaRegen.TimeSpan <= 0.1f) 
        {
            Stamina = MathHelper.Lerp(Stamina, nextRegen, 0.2f);
        }
    }
    
    public void DrawMovementStats(SpriteBatch batch) 
    {
        Font.DrawString(batch, $"{Stamina}%");
    }
}