using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Managers;
using SpaceShooter.Models;

namespace SpaceShooter.Sprites
{
    public class Asteroid : Ship
    {
   
    private float _timer;

    public Asteroid(Texture2D texture) : base(texture){
      Speed = 1.75f;
    }

    public override void Update(GameTime gameTime){
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      Position += new Vector2(-Speed, 0);

      if (Position.X < -_texture.Width)
        IsRemoved = true;


    }

    public override void OnCollide(Sprite sprite){

      if (sprite is Player && !((Player)sprite).IsDead){
        ((Player)sprite).Score.Value++;

        IsRemoved = true;
      }
   
      if (sprite is Bullet && ((Bullet)sprite).Primary is Player){
        Health--;

        if (Health <= 0){
          IsRemoved = true;
          ((Player)sprite.Primary).Score.Value++;
        }
      }
    }
  }
}