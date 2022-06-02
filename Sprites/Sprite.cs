using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Managers;
using SpaceShooter.Models;

namespace SpaceShooter.Sprites
{
  ///<summary>
  /// basklassen för att rita ett sprite
  ///</summary>
  public class Sprite : Component
  {
    #region Fields
    protected Dictionary<string, Animation> _animations; //hanterar animation

    protected AnimationManager _animationManager; //hanterar animation

    protected float _layer { get; set; } //lager

    protected Vector2 _origin { get; set; } //Origin är en specifik punkt på spriten, dvs (0,0)


    protected Vector2 _position { get; set; } //spritens position

    protected float _rotation { get; set; } //rotation för sprite


    #endregion

    #region Properties

    protected Texture2D _texture; //hanterar textur för sprite

    public List<Sprite> Secondary { get; set; } //andrahands skott som skjuts av annat än spelaren dvs fiender

    public Color Colour { get; set; } //Hanterar färg

    public bool IsRemoved { get; set; } //hanterar borttagning av sprite vid ex död

    #endregion
    ///<summary> hanterar sprite lager </summary>
    public float Layer{ 
      get { return _layer; }
      set{
        _layer = value;

        if (_animationManager != null)
          _animationManager.Layer = _layer;
      }
    }
    ///<summary> hanterar spritens origin </summary>
    public Vector2 Origin{
      get { return _origin; }
      set{
        _origin = value;

        if (_animationManager != null)
          _animationManager.Origin = _origin;
      }
    }
  ///<summary> hanterar sprite position </summary>
    public Vector2 Position{
      get{
        return _position;
      }

      set{
        _position = value;

        if (_animationManager != null)
          _animationManager.Position = _position;
      }
    }

    ///<summary> hanterar sprites begränsningsrutan.</summary>
    public Rectangle Rectangle{
      
      get{
        if (_texture != null){
          return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
        }

        if (_animationManager != null){
          var animation = _animations.FirstOrDefault().Value;

          return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, animation.FrameWidth, animation.FrameHeight);
        }

        throw new Exception("Unknown sprite");
      }
    }

    ///<summary> läser färg </summary>
    public readonly Color[] TextureData;

    ///<summary>lagrar information om sprites translation, skala och rotation</summary>
    public Matrix Transform{

      get{
        return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
          Matrix.CreateRotationZ(_rotation) *
          Matrix.CreateTranslation(new Vector3(Position, 0));
      }
    }

    /// <summary>Primära kulan  dvs sprite som sköt skottet.</summary>
    public Sprite Primary;


    /// <summary> Arean som en sprite kan potentiellt kollidera med </summary>
    public Rectangle CollisionArea{
      get{
        return new Rectangle(Rectangle.X, Rectangle.Y, MathHelper.Max(Rectangle.Width, Rectangle.Height), MathHelper.Max(Rectangle.Width, Rectangle.Height));
      }
    }

    ///<summary></summary>

    public Sprite(Texture2D texture){
      _texture = texture;

      Secondary = new List<Sprite>();

      Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

      Colour = Color.White;

      TextureData = new Color[_texture.Width * _texture.Height];
      _texture.GetData(TextureData);
    }

    public Sprite(Dictionary<string, Animation> animations){

      _texture = null;
      Secondary = new List<Sprite>();
      Colour = Color.White;
      TextureData = null;
      _animations = animations;
      var animation = _animations.FirstOrDefault().Value;
      _animationManager = new AnimationManager(animation);
      Origin = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);

    }

    #region Methods

    ///<summary>uppdaterar sprite</summary>
    public override void Update(GameTime gameTime){
    }


    ///<summary>Standardritningsmetoden för spriten</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch){
      if (_texture != null)
        spriteBatch.Draw(_texture, Position, null, Colour, _rotation, Origin, 1f, SpriteEffects.None, Layer);
      
      else if (_animationManager != null)
        _animationManager.Draw(spriteBatch);
    }

    ///<summary>Hanterar vad som ska ske om två sprites överlappar/rör varandra</summary>
    public bool Intersects(Sprite sprite){ 

      //Beräkna en matrix (matris?) som förvandlas från A:s lokalrum till världsrum och sedan in i B:s lokalrum
      var transformAToB = this.Transform * Matrix.Invert(sprite.Transform);

      // När en punkt rör sig i A:s lokala rum, rör sig den i B:s lokala rum med en fast riktning och avstånd som är proportionell mot rörelsen i A.
      // Den här algoritmen går igenom A, en pixel i taget längs A:s X- och Y-axlar.
      // Beräkna de analoga stegen i B:
      var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
      var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

      // Beräkna det övre vänstra hörnet av A i B:s lokalrum
      // Denna variabel kommer att återanvändas för att hålla reda på början av varje rad
      var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

      for (int yA = 0; yA < this.Rectangle.Height; yA++)
      {
        // Börja i början av raden
        var posInB = yPosInB;

        for (int xA = 0; xA < this.Rectangle.Width; xA++)
        {
          // Runda till närmaste pixel
          var xB = (int)Math.Round(posInB.X);
          var yB = (int)Math.Round(posInB.Y);

          if (0 <= xB && xB < sprite.Rectangle.Width &&
              0 <= yB && yB < sprite.Rectangle.Height)
          {
            // Få färgerna på de överlappande pixlarna
            var colourA = this.TextureData[xA + yA * this.Rectangle.Width];
            var colourB = sprite.TextureData[xB + yB * sprite.Rectangle.Width];

            if (colourA.A != 0 && colourB.A != 0){
              return true;
            }
          }

          // Flytta till nästa pixel i raden
          posInB += stepX;
        }
        // Flytta till nästa rad
        yPosInB += stepY;
      }
      // Ingen korsning hittades
      return false;
    }
    ///<summary> används för att klona objektet, vilket returnerar ytterligare en kopia av den datan</summary>
    public object Clone()
    {
      var sprite = this.MemberwiseClone() as Sprite;

      if (_animations != null)
      {
        sprite._animations = this._animations.ToDictionary(c => c.Key, v => v.Value.Clone() as Animation);
        sprite._animationManager = sprite._animationManager.Clone() as AnimationManager;
      }

      return sprite;
    }

    #endregion
  }
}