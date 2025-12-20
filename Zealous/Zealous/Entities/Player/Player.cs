using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GameComponents.Managers;
using GameComponents.Entity;
using System;
using GameComponents.Rendering;

namespace Zealous;

public sealed class Player : Entity 
{
    private float moveSpeed = 0f;
    private float maxSpeed = 500f;
    private float acceleration = 1f;
    private float jumpForce = 500f;
    private float gravity = 100f;
    
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = MathHelper.Clamp(value, 0f, maxSpeed) * acceleration; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = MathHelper.Clamp(value, moveSpeed, float.PositiveInfinity); }
    public float Acceleration { get => acceleration; set => acceleration = Math.Abs(value); }
    public float JumpForce { get => jumpForce; set => jumpForce = Math.Abs(value); }
    public float Gravity { get => gravity; set => gravity = Math.Abs(value); }
    public string Name { get; set; } = string.Empty;
    
    public readonly InputManager Input;
    public Sprite Sprite { get; private set; }
    
    public Player() : base(910, 540, 32 * 3, 48 * 3, 100) 
    {
        Input = new InputManager();
    }
    
    // initialization methods
    
    public void LoadContent(GraphicsDevice graphics) 
    {
        Sprite = new Sprite(new Texture2D(graphics, 1, 1), Color.White);
        Sprite.SetData(Color.White);
    }
    
    protected override void MoveAndSlide(GameTime gt) => Position += Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
    
    // main methods
    
    public void Jump() => Velocity_Y -= (int)JumpForce;
    
    public void Update(GameTime gt) 
    {
        MoveAndSlide(gt);
        Input.UpdateInputs();
        
        if (Input.IsKeyPressed(Keys.E)) 
        {
            GameComponents.Helpers.Diagnostics.Write($"hats up dawgs");        
        }
    }
    
    public void DrawPlayer(SpriteBatch batch) 
    {
        Sprite.Draw(batch, Bounds);
    }
    
}