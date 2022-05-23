using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShooter.Models;

namespace SpaceShooter.Sprites
{
  ///<summary> hanterar explosion effekten</summary>
  public class Explosion : Sprite
  {
    private float _timer = 0f;

    public Explosion(Dictionary<string, Animation> animations) : base(animations){
      
    }

    public override void Update(GameTime gameTime){
      
      _animationManager.Update(gameTime);

      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer > _animationManager.CurrentAnimation.FrameCount * _animationManager.CurrentAnimation.FrameSpeed)
        IsRemoved = true;
    }
  }
}