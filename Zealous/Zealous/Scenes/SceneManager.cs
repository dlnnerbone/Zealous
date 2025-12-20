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
    
    public SceneManager(string name) : base(name) 
    {
        Diagnostics.Write($"{name}");
    }
    
    public override void Initialize(Game game) 
    {
        base.Initialize(game);
    }
    
    public override void LoadSceneContent(Game game, string rootDir = "Content") 
    {
        base.LoadSceneContent(game, rootDir);
    }
    
    public override void UpdateScene(GameTime gt) 
    {
        base.UpdateScene(gt);
    }
    
    public void Draw(SpriteBatch batch) 
    {
        base.DrawScene();
        
        batch.Begin(SortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effects, null);
        batch.End();
    }
}