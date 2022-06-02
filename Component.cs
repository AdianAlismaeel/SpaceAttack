using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
  ///<summary>
  /// En abstrakt klass används här eftersom den är designad för att ärvas av underklasser som antingen implementerar eller "override" dess metoder.
  /// Här änvänds den för spelets huvudkomponenter "Draw" och "Update" metoderna.
  ///</summary>
  public abstract class Component
  {
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch); //Draw metoden

    public abstract void Update(GameTime gameTime); //Update metoden
  }
}