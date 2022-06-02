using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Controls
{
  ///<summary>hanterar menyns knappar</summary>
  public class Button : Component
  {
    #region Fields

    private MouseState _currentMouse; //nuvarande staten för musen

    private SpriteFont _font; //typsnitten

    private bool _isHovering; // Om musen svävar 

    private MouseState _previousMouse; //föregående staten för musen

    private Texture2D _texture; //textil

    #endregion

    #region Properties

    public EventHandler Click; //händelse när musen klickar

    public float Layer { get; set; } //lager

    public Vector2 Origin{    //knappens origin psoition
      get{
        return new Vector2(_texture.Width / 2, _texture.Height / 2);
      }
    }

    public Color PenColour { get; set; } //färg

    public Vector2 Position { get; set; } //position

    public Rectangle Rectangle{   //rektangeln
      get{
        return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
      }
    }

    public string Text; //text

    #endregion

    #region Methods
    ///<summary> hanterar knappens sprite och typsnitt samt färg </summary>
    public Button(Texture2D texture, SpriteFont font){ 
      _texture = texture;

      _font = font;

      PenColour = Color.DarkSlateGray;
    }

     ///<summary> ritar ut knappen samt hanterar färg och vad som ska ske om musen svävar över knappen </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch){ 

      var colour = Color.White;

      if (_isHovering)
        colour = Color.Gray;

      spriteBatch.Draw(_texture, Position, null, colour, 0f, Origin, 1f, SpriteEffects.None, Layer);
     
      if (!string.IsNullOrEmpty(Text)){

        var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
        var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

        spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer + 0.01f);
      }

    }

    ///<summary> uppdaterar musens stat samt vad som sker när musen svävar över knappen</summary>
    public override void Update(GameTime gameTime){ 
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();

      var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

      _isHovering = false;

      if (mouseRectangle.Intersects(Rectangle)){

        _isHovering = true;

        if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed){

          Click.Invoke(this, new EventArgs());

        }
      }
    }

    #endregion
  }
}