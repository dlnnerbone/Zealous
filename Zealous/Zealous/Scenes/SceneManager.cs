using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameComponents.Managers;
using GameComponents.Helpers;

namespace Zealous;

public sealed class SceneManager : Scene 
{
    public SamplerState SamplerState {get; private set; } = SamplerState.PointClamp;
    public DepthStencilState DepthStencilState { get; private set; } = null;
    public BlendState BlendState { get; private set; } = null;
    public SpriteSortMode SortMode { get; private set; } = SpriteSortMode.FrontToBack;
    public RasterizerState RasterizerState { get; private set; } = null;
    public Effect Effects { get; private set; } = null;
    
    public enum GameStates { Zealous, UI, GameOver } GameStates CurrentGameState = GameStates.Zealous;
    
    // Managers
    
    public readonly GameScene GameManager = new();
    
    public SceneManager(string name) : base(name) {}
    
    public override void Initialize(Game game) 
    {
        base.Initialize(game);
        GameManager.Initialize(game);
    }
    
    public override void LoadSceneContent(Game game, string rootDir = "Content") 
    {
        GameManager.LoadSceneContent(game);
    }
    
    public override void UpdateScene(GameTime gt) 
    {
        base.UpdateScene(gt);
        GameManager.UpdateScene(gt);
    }
    
    public void Draw(SpriteBatch batch) 
    {
        DrawScene();
        
        batch.Begin(SortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effects, null);
        GameManager.DrawScene(batch);
        batch.End();
    }
}