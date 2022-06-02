using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using SpaceShooter.Sprites;
using SpaceShooter.States;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
{
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public static Random Random;

    public static int ScreenWidth = 1280;   //skärmens bredd
    public static int ScreenHeight = 700;   //skrämens höjd

    private State _currentState;      //nuvarande tillstånd
    private State _nextState;         //nästa tillstånd

    Scrolling scrolling1;             //första bilden för skrollande bakgrunden
    Scrolling scrolling2;             //andra bilden för skrollande bakgrunden

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }
    protected override void Initialize()
    {
      Random = new Random();

      graphics.PreferredBackBufferWidth = ScreenWidth;
      graphics.PreferredBackBufferHeight = ScreenHeight;
      graphics.ApplyChanges();

      IsMouseVisible = true;

      base.Initialize();
    }

    protected override void LoadContent()
    {
      
      spriteBatch = new SpriteBatch(GraphicsDevice);

      _currentState = new MenuState(this, Content);
      _currentState.LoadContent();
      _nextState = null;

      scrolling1 = new Scrolling(Content.Load<Texture2D>("sprBg0"), new Rectangle(0,0, 1280, 700));
      scrolling2 = new Scrolling(Content.Load<Texture2D>("sprBg1"), new Rectangle(0,0, 1920, 700));

    }


    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Tillåter spelet att köra logik som att uppdatera världen, checka efter kollisioner, samla in input och spela upp ljud.
    /// </summary>
    protected override void Update(GameTime gameTime)
    {
      if(_nextState != null)
      {
        _currentState = _nextState;
        _currentState.LoadContent();

        _nextState = null;
      }

      _currentState.Update(gameTime);
      _currentState.PostUpdate(gameTime);


      base.Update(gameTime);

       //skrollande bakgrunden

      if(scrolling1.rectangle.X + scrolling1.texture.Width <=0)
        scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
      if(scrolling2.rectangle.X + scrolling2.texture.Width <=0)
        scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling2.texture.Width;

      scrolling1.Update();
      scrolling2.Update();

    }

    public void ChangeState(State state)
    {
      _nextState = state;
    }
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      _currentState.Draw(gameTime, spriteBatch);

      spriteBatch.Begin();
      scrolling1.Draw(spriteBatch);
      scrolling2.Draw(spriteBatch);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}