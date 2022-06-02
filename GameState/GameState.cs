using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Sprites;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Managers;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter.States
{
  ///<summary>spelskärmen</summary>
  public class GameState : State //": State " eftersom jag behöver använda mig av vissa properties i state klassen
  {
    private EnemyManager _enemyManager; //behandlar fiender

    private SpriteFont _font;

    private List<Player> _players;  //behandlar spelaren

    private List<Sprite> _sprites;  //behandlar sprites

    public int PlayerCount; //antalet spelare

    Song theme;   //theme låten

    public GameState(Game1 game, ContentManager content): base(game, content){
    }

    #region Methods

    ///<summary> Laddar in all content för spelskärmen </summary>
    public override void LoadContent(){

      theme = _content.Load<Song>("MMC5-track 1"); //laddar in låten samt upprepar den till spelets slut, låten spelas endast i denna skärmen
      MediaPlayer.IsRepeating = true;
      MediaPlayer.Play(theme);

      var playerTexture = _content.Load<Texture2D>("SpaceShip");
      var bulletTexture = _content.Load<Texture2D>("beam");

      _font = _content.Load<SpriteFont>("Font");

      _sprites = new List<Sprite>(){  
      };

      ///<summary> tar hand om explosion effekten </summary>
      var bulletPrefab = new Bullet(bulletTexture){
        Explosion = new Explosion(new Dictionary<string, Models.Animation>(){
              { "Explode", new Models.Animation(_content.Load<Texture2D>("Explosion"), 3) { FrameSpeed = 0.1f, } }
            })
        {
          Layer = 0.5f,
        }
      };
      ///<summary> tar ahnd om allt relaterat till spelaren dvs vad som sker nrä WASD tangenterna trycks och hälsan etc</summary>
      if (PlayerCount >= 1){

        _sprites.Add(new Player(playerTexture){
          Position = new Vector2(100, 50),
          Layer = 0.3f,
          Bullet = bulletPrefab,
          Input = new Models.Input(){
            Up = Keys.W,
            Down = Keys.S,
            Left = Keys.A,
            Right = Keys.D,
            Shoot = Keys.Space,
          },
          Health = 10,
          Score = new Models.Score(){
           
            Value = 0,
          },
        });
      }

      _players = _sprites.Where(c => c is Player).Select(c => (Player)c).ToList();

      _enemyManager = new EnemyManager(_content){
        Bullet = bulletPrefab,
      };
    }

    ///<summary> Lägger till fiender i spelet, hänvisar till maximala antalet i enemymanager klassen</summary>
    public override void Update(GameTime gameTime){


      if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        _game.ChangeState(new MenuState(_game, _content));

      foreach (var sprite in _sprites)
        sprite.Update(gameTime);

      _enemyManager.Update(gameTime);
      if (_enemyManager.CanAdd && _sprites.Where(c => c is Enemy).Count() < _enemyManager.MaxEnemies){
        _sprites.Add(_enemyManager.GetEnemy()); 
        _sprites.Add(_enemyManager.GetAsteroid());
      }
    }

    ///<summary> hanterar vad som sker vid kollision</summary>
    public override void PostUpdate(GameTime gameTime){
      var collidableSprites = _sprites.Where(c => c is ICollidable);

      foreach (var spriteA in collidableSprites){
        foreach (var spriteB in collidableSprites){
         
         //ingenting händer om det är samma sprite som kolliderar
          if (spriteA == spriteB)
            continue;

          if (!spriteA.CollisionArea.Intersects(spriteB.CollisionArea))
            continue;
          
          if (spriteA.Intersects(spriteB))
            ((ICollidable)spriteA).OnCollide(spriteB);
        }
      }

      //lägger till sekundära spirte till listan över sprites (dvs kulorna)
      int spriteCount = _sprites.Count;
      for (int i = 0; i < spriteCount; i++)
      {
        var sprite = _sprites[i];
        foreach (var child in sprite.Secondary)
          _sprites.Add(child);

        sprite.Secondary = new List<Sprite>();
      }

      for (int i = 0; i < _sprites.Count; i++)
      {
        if (_sprites[i].IsRemoved)
        {
          _sprites.RemoveAt(i);
          i--;
        }
      }
      // Om alla spelare är döda så stoppas musiken och spelaren skickas till slutmenyn
      if (_players.All(c => c.IsDead)){
        _game.ChangeState(new Endmenu(_game, _content));
        MediaPlayer.Stop();
      }
    }

    ///<summary> Ritar ut alla textures/sprites samt text som exempelvis health och score</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch){

      spriteBatch.Begin(SpriteSortMode.FrontToBack);

      foreach (var sprite in _sprites)
        sprite.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      spriteBatch.Begin();

      float x = 10f;
      foreach (var player in _players){
        spriteBatch.DrawString(_font, "Health: " + player.Health, new Vector2(x, 10f), Color.White);
        spriteBatch.DrawString(_font, "Score: " + player.Score.Value, new Vector2(x, 30f), Color.White);

        x += 150;
      }
      spriteBatch.End();
    }

    #endregion
  }
}