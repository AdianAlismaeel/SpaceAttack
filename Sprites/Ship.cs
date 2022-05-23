using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter.Sprites
{
  ///<summary>
  ///ett skepp i spelet
  ///</summary>
  public class Ship : Sprite, ICollidable
  {
    public int Health { get; set; } //rymdraketens hälsa

    public Bullet Bullet { get; set; } //kulan som skjuts från rymdraketen

    public float Speed; //rymdraketen fart


    public Ship(Texture2D texture) : base(texture){ //rymdraketens textur
    }

    ///<summary>Metoden som gör att rymdraketen kan skjuta.</summary>
    protected void Shoot(float speed){
      var bullet = Bullet.Clone() as Bullet;
      bullet.Position = this.Position;
      bullet.Colour = this.Colour;
      bullet.Layer = 0.1f;
      bullet.LifeSpan = 5f;
      bullet.Velocity = new Vector2(speed, 0f);
      bullet.Primary = this;

      Secondary.Add(bullet);
    }
    ///<summary> hanterar kollision</summary>
    public virtual void OnCollide(Sprite sprite){

      throw new NotImplementedException();
    }
  }
}