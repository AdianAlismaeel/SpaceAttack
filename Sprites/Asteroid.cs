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
    ///<summary>
    /// Hanterar allt relaterat till asteroid fienden
    ///</summary>
    public class Asteroid : Ship // ": Ship" används så att jag kan ha tillgång till vissa porperties som t.ex speed som finns i Ship klassen. 
    {
   
    private float _timer;

    public Asteroid(Texture2D texture) : base(texture){
      Speed = 1.75f; //asteroidens hastighet
    }

    ///<summary> Update metod som hanterar asteroidens position</summary>

    public override void Update(GameTime gameTime){
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      Position += new Vector2(-Speed, 0);

      if (Position.X < -_texture.Width)
        IsRemoved = true;


    }

    ///<summary> Hanterar vad som sker vid kollision mellan spelaren och asteroiden </summary>

    public override void OnCollide(Sprite sprite){

      if (sprite is Player && !((Player)sprite).IsDead){
        ((Player)sprite).Score.Value++; //ökar poäng

        IsRemoved = true; //sprite tas bort
      }
   
      if (sprite is Bullet && ((Bullet)sprite).Primary is Player){
        Health--; //minskar hälsan på fienden

        if (Health <= 0){ //om hälsan är mindre än noll, tas spriten bort
          IsRemoved = true;
          ((Player)sprite.Primary).Score.Value++; //spelarens poäng ökar
        }
      }
    }
  }
}