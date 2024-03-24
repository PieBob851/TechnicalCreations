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
namespace TechnicalCreations.UI
{
    public class ScannerUI : BaseScanUI
    {
        private bool scanning, scanFirst, scanSecond;
        private Rectangle selectTarget;
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
                    selectTarget = new Rectangle(Player.tileTargetX, Player.tileTargetY, 0, 0);
                    scanFirst = true;

                    Main.NewText("First point set");
                } else if (!scanSecond)
                {
                    Point16 topLeft = new Point16(Math.Min(Player.tileTargetX, selectTarget.X), Math.Min(Player.tileTargetY, selectTarget.Y));
                    Point16 botRight = new Point16(Math.Max(Player.tileTargetX, selectTarget.X), Math.Max(Player.tileTargetY, selectTarget.Y));

                    selectTarget = new Rectangle(topLeft.X, topLeft.Y, botRight.X - topLeft.X, botRight.Y - topLeft.Y);

                    scanSecond = true;

                    Main.NewText("Second point set");
                }
            }
            base.LeftClick(evt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (scanSecond)
            {
                for (int x = selectTarget.Left; x < selectTarget.Right; x++) { 
                    for (int y = selectTarget.Top; y <  selectTarget.Bottom; y++) {
                        Vector2 tilePosition = new Vector2(x * 16, y * 16);

                        Dust.QuickDust(new Point(x, y), Color.Red);

                        Main.spriteBatch.Draw(TextureAssets.Extra[2].Value, tilePosition, null,
                            new Color(1, 1, 1, 0.4f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }

            base.Draw(spriteBatch);
        }
    }
}