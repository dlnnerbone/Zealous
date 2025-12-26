using Microsoft.Xna.Framework;
using GameComponents.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameComponents.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameComponents.Logic;
using GameComponents.Rendering;
using GameComponents;

namespace Zealous;

public sealed class GameScene : Scene 
{
    public readonly Player Player;
    
    public readonly TileMapVisuals MapVisuals;
    public readonly TileMapLogic MapLogic;
    public TileGrid Grid { get; private set; }
    public Texture2D Texture { get; private set; }
    
    public GameScene() : base("Main Game Scene.") 
    {
        Player = new();
        
        MapVisuals = new(Vector2.Zero, LayoutDirection.Horizontal, 128, new byte[,] 
        {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            
        });
        
        MapLogic = new(LayoutDirection.Horizontal, Vector2.Zero, 128, new byte[,] 
        {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        });
        
        MapVisuals.SetLayerDepth(new HashSet<int> {3}, 0f);
        MapLogic.ToggleCollision(new HashSet<int> {3}, false);
    }
    
    public override void Initialize(Game game) 
    {
        base.Initialize(game);
    }
    
    public void LoadContent(Game game) 
    {
        base.LoadContent(game);
        Texture = Content.Load<Texture2D>("Assets/TextureAtlases/BasicTileSet");
        Grid = new(4, 4, Texture);
        MapVisuals.SetSourceGrid(Grid);
        
        Player.LoadContent(game.GraphicsDevice, Content);
    }
    
    public override void Update(GameTime gt) 
    {
        base.Update(gt);
        Player.Update(gt);
        MapLogic.Update((int i, ref Collider c) => 
        {
            Vector2 colliderCenter;
            Player.Collision(ref c, out colliderCenter);
        });
    }
    
    public override void DrawScene(SpriteBatch batch) 
    {
        base.DrawScene(batch);
        MapVisuals.Draw(batch, Texture, Color.White);
        Player.DrawPlayer(batch);
    }
    
}