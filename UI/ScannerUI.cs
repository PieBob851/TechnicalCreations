using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TechnicalCreations.Helpers;

namespace TechnicalCreations.UI
{
    public class ScannerUI : BaseScanUI
    {
        private bool scanning, scanFirst, scanSecond, borderClicked;
        private Border borderHovered;
        private Rectangle selectedTiles;
        public override void OnInitialize()
        {
            DraggableUIPanel panel = new DraggableUIPanel();
            panel.Width.Set(300, 0);
            panel.Height.Set(300, 0);
            Append(panel);

            UIText header = new UIText("My UI Header");
            header.HAlign = 0.5f;
            header.Top.Set(15, 0);
            panel.Append(header);

            UIPanel scanButton = new UIPanel();
            scanButton.Width.Set(100, 0);
            scanButton.Height.Set(50, 0);
            scanButton.HAlign = 0.5f;
            scanButton.Top.Set(25, 0);
            scanButton.OnLeftClick += ScanButtonClick;
            panel.Append(scanButton);

            UIText text = new UIText("Click me!");
            text.HAlign = text.VAlign = 0.5f;
            scanButton.Append(text);
        }

        private void ScanButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.NewText(scanning);
            scanning = !scanning;
            scanFirst = false;
            scanSecond = false;
            Main.LocalPlayer.mouseInterface = scanning;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            Main.NewText($"Targeting {Player.tileTargetX}, {Player.tileTargetY}");
            if (scanning)
            {
                if (!scanFirst)
                {
                    selectedTiles = new Rectangle(Player.tileTargetX, Player.tileTargetY, 0, 0);
                    scanFirst = true;

                    Main.NewText("First point set");
                } else if (!scanSecond)
                {
                    Point16 topLeft = new Point16(Math.Min(Player.tileTargetX, selectedTiles.X), Math.Min(Player.tileTargetY, selectedTiles.Y));
                    Point16 botRight = new Point16(Math.Max(Player.tileTargetX, selectedTiles.X), Math.Max(Player.tileTargetY, selectedTiles.Y));

                    selectedTiles = new Rectangle(topLeft.X, topLeft.Y, botRight.X - topLeft.X + 1, botRight.Y - topLeft.Y + 1);

                    scanSecond = true;

                    Main.NewText("Second point set");
                }
            }
            base.LeftClick(evt);
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            if (scanSecond && borderHovered != Border.None)
            {
                borderClicked = true;
            }
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            borderClicked = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (scanning && !scanFirst)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.TransformationMatrix);

                DrawHelpers.HighlightTiles(spriteBatch, new Rectangle(Player.tileTargetX, Player.tileTargetY, 1, 1)) ;

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);
            } else if (scanFirst && !scanSecond) 
            {
                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.TransformationMatrix);

                Point16 topLeft = new Point16(Math.Min(Player.tileTargetX, selectedTiles.X), Math.Min(Player.tileTargetY, selectedTiles.Y));
                Point16 botRight = new Point16(Math.Max(Player.tileTargetX, selectedTiles.X), Math.Max(Player.tileTargetY, selectedTiles.Y));

                DrawHelpers.HighlightTiles(spriteBatch, new Rectangle(topLeft.X, topLeft.Y, botRight.X - topLeft.X + 1, botRight.Y - topLeft.Y + 1));

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);
            } else if (scanSecond)
            {
                spriteBatch.End();
				spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.TransformationMatrix);

                DrawHelpers.HighlightTiles(spriteBatch, selectedTiles, borderHovered, borderClicked);

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);
            }

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (borderClicked)
            {
                switch (borderHovered)
                {
                    case Border.Right:
                        selectedTiles.Width = Player.tileTargetX - selectedTiles.X;
                        break;
                    case Border.Top:
                        selectedTiles.Height += selectedTiles.Y - Player.tileTargetY;
                        selectedTiles.Y = Player.tileTargetY;
                        break;
                    case Border.Left:
                        selectedTiles.Width += selectedTiles.X - Player.tileTargetX;
                        selectedTiles.X = Player.tileTargetX;
                        break;
                    case Border.Bottom:
                        selectedTiles.Height = Player.tileTargetY - selectedTiles.Y;
                        break;
                }
            } else if (Player.tileTargetX == selectedTiles.X + selectedTiles.Width && Player.tileTargetY >= selectedTiles.Y && Player.tileTargetY <= selectedTiles.Y + selectedTiles.Height)
            {
                borderHovered = Border.Right;
            } else if (Player.tileTargetY == selectedTiles.Y && Player.tileTargetX >= selectedTiles.X && Player.tileTargetX <= selectedTiles.X + selectedTiles.Width)
            {
                borderHovered = Border.Top;
            } else if (Player.tileTargetX == selectedTiles.X && Player.tileTargetY >= selectedTiles.Y && Player.tileTargetY <= selectedTiles.Y + selectedTiles.Height)
            {
                borderHovered = Border.Left;
            } else if (Player.tileTargetY == selectedTiles.Y + selectedTiles.Height && Player.tileTargetX >= selectedTiles.X && Player.tileTargetX <= selectedTiles.X + selectedTiles.Width)
            {
                borderHovered = Border.Bottom;
            } else
            {
                borderHovered = Border.None;
            }
            base.Update(gameTime);
        }
    }
}