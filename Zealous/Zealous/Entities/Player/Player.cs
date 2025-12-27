using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GameComponents.Managers;
using GameComponents.Entity;
using System;
using GameComponents.Rendering;
using GameComponents.Helpers;
using Microsoft.Xna.Framework.Content;
using GameComponents;

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
        Sprite.SetData(Color.Purple);
        Sprite.LayerDepth = 0.85f;
        
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
    }
    
    // for Updating against collision
    
    public void Collision(ref Collider collider, out Vector2 center) 
    {
        var colliderCenter = new Vector2(collider.Bounds.X + collider.Bounds.Width / 2, collider.Bounds.Y + collider.Bounds.Height / 2);
        if (!Intersects(collider.Bounds)) 
        {
            center = colliderCenter;
            return;
        }
        center = colliderCenter;
        
        var distance = Center - colliderCenter;
        
        var minDistanceX = collider.Bounds.Width / 2 + Width / 2;
        var minDistanceY = collider.Bounds.Height / 2 + Height / 2;
        
        var overlapX = Center.X < center.X ? distance.X + minDistanceX : Math.Abs(distance.X - minDistanceX);
        var overlapY = Center.Y < center.Y ? distance.Y + minDistanceY : Math.Abs(distance.Y - minDistanceY);
        
        var isTouching = overlapX < overlapY;
        if (isTouching) 
        {
            if (Center.X < center.X) X -= (int)overlapX;
            else X += (int)overlapX;
        }
        else 
        {
            if (Center.Y < center.Y) 
            {
                Velocity_Y = 0f;
                Movement.Velocity = new(Movement.Velocity.X, -1f);
                Diagnostics.Write($"{Velocity_Y}, {Movement.Velocity.Y}");
                Y -= (int)overlapY;
            }
            else 
            {
                Y += (int)overlapY;
            }
        }
    }
    
}