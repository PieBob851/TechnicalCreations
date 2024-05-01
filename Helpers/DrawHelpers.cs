using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.DataStructures;

namespace TechnicalCreations.Helpers
{
    public enum Border
    {
        Top,
        Bottom, 
        Left, 
        Right,
        None
    }
    public class DrawHelpers
    {
        public static void HighlightTiles(SpriteBatch spriteBatch, Rectangle tiles, Border borderHovered = Border.None, bool borderClicked = false)
        {
            int leftPos = tiles.X * 16 - (int) Main.screenPosition.X;
            int rightPos = leftPos + tiles.Width * 16;
            int topPos = tiles.Y * 16 - (int)Main.screenPosition.Y;
            int bottomPos = topPos + tiles.Height * 16;

            Rectangle leftBorder = new Rectangle(leftPos - 2, topPos, 2, bottomPos - topPos);
            Rectangle rightBorder = new Rectangle(rightPos, topPos, 2, bottomPos - topPos);
            Rectangle topBorder = new Rectangle(leftPos, topPos - 2, rightPos - leftPos, 2);
            Rectangle bottomBorder = new Rectangle(leftPos, bottomPos, rightPos - leftPos, 2);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(leftPos, topPos, rightPos - leftPos, bottomPos - topPos), Color.White * 0.25f);
            switch(borderHovered)
            {
                case Border.Right:
                    rightBorder.X -= 2;
                    rightBorder.Width += 4;
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rightPos, topPos - 2, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rightPos, bottomPos, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    break;
                case Border.Top:
                    topBorder.Y -= 2;
                    topBorder.Height += 4;
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(leftPos - 2, topPos - 2, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rightPos, topPos - 2, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    break;
                case Border.Left:
                    leftBorder.X -= 2;
                    leftBorder.Width += 4;
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(leftPos - 2, topPos - 2, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(leftPos - 2, bottomPos, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    break;
                case Border.Bottom:
                    bottomBorder.Y -= 2;
                    bottomBorder.Height += 4;
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(leftPos - 2, bottomPos, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rightPos, bottomPos, 2, 2), borderClicked ? Color.Yellow : Color.White);
                    break;
            }

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, leftBorder, borderHovered == Border.Left ? borderClicked ? Color.Yellow : Color.White : Color.White * 0.75f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rightBorder, borderHovered == Border.Right ? borderClicked ? Color.Yellow : Color.White : Color.White * 0.75f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, topBorder, borderHovered == Border.Top ? borderClicked ? Color.Yellow : Color.White : Color.White * 0.75f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, bottomBorder, borderHovered == Border.Bottom ? borderClicked ? Color.Yellow : Color.White : Color.White * 0.75f);
        }

        public static void ColorTile(SpriteBatch spriteBatch, Point16 tile, Color color)
        {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, 
                            new Rectangle(tile.X * 16 - (int)Main.screenPosition.X, tile.Y * 16 - (int)Main.screenPosition.Y, 16, 16), 
                            color);
        }

        public static void DrawBorder(SpriteBatch spriteBatch, Rectangle tiles)
        {
            int leftPos = tiles.X * 16 - (int)Main.screenPosition.X;
            int rightPos = leftPos + tiles.Width * 16;
            int topPos = tiles.Y * 16 - (int)Main.screenPosition.Y;
            int bottomPos = topPos + tiles.Height * 16;

            Rectangle leftBorder = new Rectangle(leftPos - 2, topPos, 2, bottomPos - topPos);
            Rectangle rightBorder = new Rectangle(rightPos, topPos, 2, bottomPos - topPos);
            Rectangle topBorder = new Rectangle(leftPos, topPos - 2, rightPos - leftPos, 2);
            Rectangle bottomBorder = new Rectangle(leftPos, bottomPos, rightPos - leftPos, 2);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, leftBorder, Color.White * 0.75f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rightBorder, Color.White * 0.75f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, topBorder, Color.White * 0.75f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, bottomBorder, Color.White * 0.75f);
        }
    }
}
