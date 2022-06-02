using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceShooter.Sprites;

namespace SpaceShooter.Managers
{
  ///<summary>hanterar allt relaterat till spelets fiender</summary>
  public class EnemyManager
  {
    private float _timer;

    private List<Texture2D> _textures;

    private List<Texture2D> _asteroid;

    public bool CanAdd { get; set; }

    public Bullet Bullet { get; set; }

    public int MaxEnemies { get; set; }

    public float SpawnTimer { get; set; }

    ///<summary> Laddar in fiende content filerna </summary>
    public EnemyManager(ContentManager content){

      _textures = new List<Texture2D>(){
        content.Load<Texture2D>("Enemy_1"),
        content.Load<Texture2D>("Enemy_2"),
      };

      _asteroid = new List<Texture2D>(){

        content.Load<Texture2D>("Asteroid Brown")

      };


      MaxEnemies = 4;   //maximala antalet fiender
      SpawnTimer = 3.85f; 
    }

    public void Update(GameTime gameTime){

      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      CanAdd = false;

      if (_timer > SpawnTimer){
        CanAdd = true;

        _timer = 0;
      }
    }

    ///<summary> Metod för fienden som ritar ut allt relaterat till skepp fienden, nedanstående mtod tar hand om asteroiden på samma vis</summary>
    public Enemy GetEnemy(){

      var texture = _textures[Game1.Random.Next(0, _textures.Count)];

      return new Enemy(texture){
        Bullet = Bullet,
        Health = 5,
        Layer = 0.2f,
        Position = new Vector2(Game1.ScreenWidth + texture.Width, Game1.Random.Next(0, Game1.ScreenHeight)),
        Speed = 2 + (float)Game1.Random.NextDouble(),
        ShootingTimer = 1.75f + (float)Game1.Random.NextDouble(),
      };
    }

     public Enemy GetAsteroid(){

       var asteroid =  _asteroid[Game1.Random.Next(0, _asteroid.Count)];

      return new Enemy(asteroid){
        Bullet = Bullet,
        Health = 7,
        Layer = 0.2f,
        Position = new Vector2(Game1.ScreenWidth + asteroid.Width, Game1.Random.Next(0, Game1.ScreenHeight)),
        Speed = 2 + (float)Game1.Random.NextDouble(),
        ShootingTimer = 2000f + (float)Game1.Random.NextDouble(),
      };
    }
      
    
  }
}