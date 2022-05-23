using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Sprites
{
  ///<summary>
  /// Hanterar kollisionen mellan sprites i spelet.
  ///</summary>
  public interface ICollidable
  {
    void OnCollide(Sprite sprite);
  }
}