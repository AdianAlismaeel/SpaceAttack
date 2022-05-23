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

    public static int ScreenWidth = 1280;
    public static int ScreenHeight = 700;

    private State _currentState;
    private State _nextState;

    Scrolling scrolling1;
    Scrolling scrolling2;

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

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
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

       //scrolling background

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