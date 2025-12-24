using Microsoft.Xna.Framework;
using GameComponents.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameComponents.Entity;
using System.Collections.Generic;

namespace Zealous;

public sealed class GameScene : Scene 
{
    public readonly Player Player;
    public readonly List<Entity> Entities, InactiveEntities;
    
    public GameScene() : base("Main Game Scene.") 
    {
        Player = new();
        Entities = new List<Entity>();
        InactiveEntities = new List<Entity>();
    }
    
    public override void Initialize(Game game) 
    {
        base.Initialize(game);
    }
    
    public void LoadContent(Game game) 
    {
        Player.LoadContent(game.GraphicsDevice);
    }
    
    public override void Update(GameTime gt) 
    {
        base.Update(gt);
        Player.Update(gt);
    }
    
    public override void DrawScene(SpriteBatch batch) 
    {
        base.DrawScene(batch);
        
        Player.DrawPlayer(batch);
    }
    
}