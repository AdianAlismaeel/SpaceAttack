using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class Scrolling : Backgrounds //": Backrgounds" eftersom jag behöver använda mig av vissa properties i backgrounds klassen
        {
            public Scrolling(Texture2D newTexture, Rectangle newRectangle){
                
                texture = newTexture;
                rectangle = newRectangle;
            }

            public void Update(){
                rectangle.X -= 3;
            }
        }

    public class Backgrounds
    {
        public Texture2D texture;
        public Rectangle rectangle;

        public void Draw(SpriteBatch spriteBatch){

            spriteBatch.Draw(texture, rectangle, Color.White);

        }

    }
}