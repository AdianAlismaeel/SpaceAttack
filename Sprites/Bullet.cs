using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter.Sprites
{
  /// <summary>
  /// hanterar kulor som skjuts från skeppet dvs spelaren och fienderna
  ///</summary>
  public class Bullet : Sprite, ICollidable
  {
    private float _timer;

    public Explosion Explosion;

    public float LifeSpan { get; set; }

    public Vector2 Velocity { get; set; }
    

    public Bullet(Texture2D texture) : base(texture){
    }

    public override void Update(GameTime gameTime){
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer >= LifeSpan)
        IsRemoved = true;

      Position += Velocity;
    }

    public void OnCollide(Sprite sprite){

      // skott kolliderar ej med varandra
      if (sprite is Bullet)
        return;

      // Fiender kan inte skjuta varandra
      if ((sprite is Enemy && this.Primary is Enemy) || (sprite is Asteroid && this.Primary is Asteroid) )
        return;


      if ((sprite is Enemy && this.Primary is Player) || (sprite is Asteroid && this.Primary is Player)){
        IsRemoved = true;
        AddExplosion();
      }

      if((sprite is Player && this.Primary is Enemy) || (sprite is Player && this.Primary is Asteroid)){
        IsRemoved = true;
        AddExplosion();
      }
    }

    private void AddExplosion(){
      if (Explosion == null)
        return;

      var explosion = Explosion.Clone() as Explosion;
      explosion.Position = this.Position;

      Secondary.Add(explosion);
    }

  }
}