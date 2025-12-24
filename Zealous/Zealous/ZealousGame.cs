using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameComponents.Managers;

namespace Zealous;

public class ZealousGame : Game
{
    private GraphicsDeviceManager device;
    private SpriteBatch spriteBatch;
    private Keys keyToExit = Keys.Escape;
    private InputManager input = new();
    
    // object that manages all game stuff
    private SceneManager mainScene = new("Zealous");
    
    public ZealousGame(string name) 
    {
        device = new(this);
        IsMouseVisible = true;
        Content.RootDirectory = "Content";
        Window.Title = name;
    }
    
    protected override void Initialize() 
    {
        base.Initialize();
        
        device.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
        device.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
        device.ApplyChanges();
        
        mainScene.Initialize(this);
    }
    
    protected override void LoadContent() 
    {
        base.LoadContent();
        mainScene.LoadContent(this);
        
        spriteBatch = new(GraphicsDevice);
    }
    
    protected override void Update(GameTime gameTime) 
    {
        MouseManager.UpdateInputs();
        input.UpdateInputs();
        
        if (input.IsKeyPressed(keyToExit)) Exit();
        
        mainScene.Update(gameTime);
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime) 
    {
        GraphicsDevice.Clear(Color.Transparent);
        
        mainScene.Draw(spriteBatch);
        
        base.Draw(gameTime);
    }
}
