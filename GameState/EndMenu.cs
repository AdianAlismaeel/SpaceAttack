using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Controls;
using SpaceShooter.Managers;
using SpaceShooter.Models;

namespace SpaceShooter.States
{
  ///<summary>slutmeny</summary>
  public class Endmenu : State
  {
    private List<Component> _components;

    private SpriteFont _font;

    public Endmenu(Game1 game, ContentManager content) : base(game, content){
    }

    public override void LoadContent(){

      _font = _content.Load<SpriteFont>("Font");

      var buttonTexture = _content.Load<Texture2D>("Button");
      var buttonFont = _content.Load<SpriteFont>("Font");

      _components = new List<Component>(){

        new Button(buttonTexture, buttonFont){

          Text = "Return",
          Position = new Vector2(Game1.ScreenWidth / 2, 400),
          Click = new EventHandler(MainMenuButton),
          Layer = 0.1f

        },
        
      };

       
    }

    private void MainMenuButton(object sender, EventArgs args){
      _game.ChangeState(new MenuState(_game, _content));
    }

    public override void Update(GameTime gameTime){
      if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        MainMenuButton(this, new EventArgs());

      foreach (var component in _components)
        component.Update(gameTime);
        
    }

    public override void PostUpdate(GameTime gameTime){

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch){

      spriteBatch.Begin();
        foreach (var component in _components)
        component.Draw(gameTime, spriteBatch);
      spriteBatch.End();

    }
  }
}