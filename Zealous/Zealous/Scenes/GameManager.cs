using Microsoft.Xna.Framework;
using GameComponents.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameComponents.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Zealous;

public sealed class GameScene : Scene 
{
    public readonly Player Player;
    
    public GameScene() : base("Main Game Scene.") 
    {
        Player = new();
    }
    
    public override void Initialize(Game game) 
    {
        base.Initialize(game);
    }
    
    public void LoadContent(Game game) 
    {
        base.LoadContent(game);
        Player.LoadContent(game.GraphicsDevice, Content);
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