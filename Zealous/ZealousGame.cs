using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Zealous;

public class ZealousGame : Game
{
    private GraphicsDeviceManager device;
    private SpriteBatch spriteBatch;
    
    public ZealousGame(string name) 
    {
        device = new(this);
        IsMouseVisible = true;
        Content.RootDirectory = "Content";
    }
    
    protected override void Initialize() 
    {
        base.Initialize();
        
        device.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
        device.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
        device.ApplyChanges();
    }
    
    protected override void LoadContent() 
    {
        base.LoadContent();
        
        spriteBatch = new(GraphicsDevice);
    }
    
    protected override void Update(GameTime gameTime) 
    {
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime) 
    {
        GraphicsDevice.Clear(Color.Transparent);
        
        spriteBatch.Begin();
        spriteBatch.End();
        
        base.Draw(gameTime);
    }
}
