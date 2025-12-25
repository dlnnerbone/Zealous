using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GameComponents.Managers;
using GameComponents.Entity;
using System;
using GameComponents.Rendering;
using GameComponents.Helpers;
using Microsoft.Xna.Framework.Content;

namespace Zealous;

public sealed class Player : Entity 
{
    public string Name { get; set; } = string.Empty;
    public Sprite Sprite { get; private set; }
    public readonly PlayerMovement Movement = new();
    
    public Player() : base(910, 540, 32 * 3, 48 * 3, 100) {}
    
    // initialization methods
    
    public void LoadContent(GraphicsDevice graphics, ContentManager content) 
    {
        Sprite = new Sprite(new Texture2D(graphics, 1, 1), Color.White);
        Sprite.SetData(Color.White);
        
        Movement.Load(content);
    }
    
    protected override void MoveAndSlide(GameTime gt) => Position += Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
    
    // main methods
    
    public void Update(GameTime gt) 
    {
        MoveAndSlide(gt);
        Movement.UpdateMotion(gt, this);
    }
    
    public void DrawPlayer(SpriteBatch batch) 
    {
        Sprite.Draw(batch, Bounds);
        Movement.DrawMovementStats(batch);
    }
    
}