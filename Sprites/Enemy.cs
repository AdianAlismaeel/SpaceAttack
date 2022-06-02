using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Sprites
{
  public class Enemy : Ship
  {
    ///<summary> spelets fiender </summary>
    private float _timer; //tid

    public float ShootingTimer = 1f; //tid för skott

    public Enemy(Texture2D texture) : base(texture){  
      Speed = 3f; //fiendens hastighet
    }

    public override void Update(GameTime gameTime){
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer >= ShootingTimer){ //hanterar skotten som skjuts
        Shoot(-5f); //skottets hastighet
        _timer = 0;
      }

      Position += new Vector2(-Speed, 0);

      if (Position.X < -_texture.Width)
        IsRemoved = true;
    }

    ///<summary> Hanterar vad som sker vid kollision mellan spelaren och skeppfienderna </summary>

    public override void OnCollide(Sprite sprite){

      if (sprite is Player && !((Player)sprite).IsDead){
        ((Player)sprite).Score.Value++; //ökar poäng

        IsRemoved = true; //sprite tas bort
      }
   
      if (sprite is Bullet && ((Bullet)sprite).Primary is Player){
        Health--;

        if (Health <= 0){ //om hälsan är mindre än 0 hos fienden så tas den bort
          IsRemoved = true;
          ((Player)sprite.Primary).Score.Value++; //poäng ökar vid fiendens död
        }
      }
    }
  }
}