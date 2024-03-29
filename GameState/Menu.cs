using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Sprites;
using SpaceShooter.Controls;
using SpaceShooter.Managers;

namespace SpaceShooter.States
{
  ///<summary> spelets meny </summary>
  public class MenuState : State //": State" eftersom jag behöver använda mig av properties från state klassen
  {
    private List<Component> _components;

    private SpriteFont _Tfont;  //titel typsnitt

    public MenuState(Game1 game, ContentManager content) : base(game, content){
    }

    ///<summary> Laddar in all content som behövs</summary>

    public override void LoadContent(){

      var buttonTexture = _content.Load<Texture2D>("Button");
      var buttonFont = _content.Load<SpriteFont>("Font");

      _Tfont = _content.Load<SpriteFont>("_Tfont");

      ///<summary> Ritar ut knapparna med hjälp av component klassens metoder </summary>
      _components = new List<Component>(){

        new Button(buttonTexture, buttonFont){
          Text = "Start",
          Position = new Vector2(Game1.ScreenWidth / 2, 400),
          Click = new EventHandler(StartButton),
          Layer = 0.1f
        },

        new Button(buttonTexture, buttonFont){
          Text = "Quit",
          Position = new Vector2(Game1.ScreenWidth / 2, 440),
          Click = new EventHandler(QuitButton),
          Layer = 0.1f
        },
      };
    }

  ///<summary> Metod för startknappen som skickar spelaren till spelskärmen och startar spelet</summary>
    private void StartButton(object sender, EventArgs args){
      _game.ChangeState(new GameState(_game, _content){
        PlayerCount = 1,
      });
    }
    ///<summary> Metod för quit knappen som avslutar spelet</summary>
    private void QuitButton(object sender, EventArgs args){
      _game.Exit();
    }

    public override void Update(GameTime gameTime){
      foreach (var component in _components)
        component.Update(gameTime);
    }

    public override void PostUpdate(GameTime gameTime){

    }

    ///<summary> Draw metoden som ritar ut alla behövda sprites/textures etc</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch){

      spriteBatch.Begin();
      spriteBatch.DrawString(_Tfont, "SPACE ATTACK!", new Vector2(360, 270), Color.DimGray);
      spriteBatch.End();

      spriteBatch.Begin(SpriteSortMode.FrontToBack);

      foreach (var component in _components)
        component.Draw(gameTime, spriteBatch);

      spriteBatch.End();


    }
  }
}