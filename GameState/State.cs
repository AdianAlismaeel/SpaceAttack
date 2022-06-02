using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.States
{
  ///<summary>hanterar spelets olika tillstånd dvs meny, gamestate och endmenu</summary>
  public abstract class State //Syftet med att använda en abstrakt klass är att tillhandahålla en gemensam definition av en basklass som flera underklasser kan dela.
  {
    protected Game1 _game;

    protected ContentManager _content;

    public State(Game1 game, ContentManager content){
      
      _game = game;

      _content = content;
    }

    public abstract void LoadContent();

    public abstract void Update(GameTime gameTime);

    public abstract void PostUpdate(GameTime gameTime);

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
  }
}