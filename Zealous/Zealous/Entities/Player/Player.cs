using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GameComponents.Managers;
using GameComponents.Entity;
using System;
using GameComponents.Rendering;
using GameComponents.Helpers;

namespace Zealous;

public sealed class Player : Entity 
{
    public string Name { get; set; } = string.Empty;
    public Sprite Sprite { get; private set; }
    public readonly PlayerMovement movement = new();
    
    public Player() : base(910, 540, 32 * 3, 48 * 3, 100) {}
    
    // initialization methods
    
    public void LoadContent(GraphicsDevice graphics) 
    {
        Sprite = new Sprite(new Texture2D(graphics, 1, 1), Color.White);
        Sprite.SetData(Color.White);
    }
    
    protected override void MoveAndSlide(GameTime gt) => Position += Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
    
    // main methods
    
    public void Update(GameTime gt) 
    {
        MoveAndSlide(gt);
        movement.UpdateMotion(gt, this);
    }
    
    public void DrawPlayer(SpriteBatch batch) 
    {
        Sprite.Draw(batch, Bounds);
    }
    
}