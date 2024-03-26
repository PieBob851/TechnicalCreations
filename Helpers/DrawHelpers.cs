using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using Microsoft.Xna.Framework;
using ReLogic.Content;

namespace TechnicalCreations.Helpers
{
    internal class DrawHelpers
    {
        public static Rectangle HighlightTiles(SpriteBatch spriteBatch, Rectangle tiles)
        {
            int leftPos = tiles.X * 16;
            int rightPos = leftPos + tiles.Width * 16;
            int topPos = tiles.Y * 16;
            int bottomPos = topPos + tiles.Height * 16;

            Rectangle mainRectangle = new Rectangle(leftPos, topPos, rightPos - leftPos, bottomPos - topPos);
            Rectangle leftBorder = new Rectangle(leftPos, topPos, 2, bottomPos - topPos);
            Rectangle rightBorder = new Rectangle(rightPos, topPos, 2, bottomPos - topPos);
            Rectangle topBorder = new Rectangle(leftPos, topPos, rightPos - leftPos, 2);
            Rectangle bottomBorder = new Rectangle(leftPos, bottomPos, rightPos - leftPos, 2);

            mainRectangle.Offset((-Main.screenPosition).ToPoint());
            leftBorder.Offset((-Main.screenPosition).ToPoint());
            rightBorder.Offset((-Main.screenPosition).ToPoint());
            topBorder.Offset((-Main.screenPosition).ToPoint());
            bottomBorder.Offset((-Main.screenPosition).ToPoint());

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, mainRectangle, Color.LightBlue * 0.1f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, leftBorder, Color.Blue * 0.3f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rightBorder, Color.Blue * 0.3f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, topBorder, Color.Blue * 0.3f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, bottomBorder, Color.Blue * 0.3f);

            return mainRectangle;
        }
    }
}
