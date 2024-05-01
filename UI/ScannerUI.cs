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
using Terraria.ModLoader.IO;

namespace TechnicalCreations.UI
{
    enum ModifyType
    {
        Single,
        Three,
        Area
    }
    public class ScannerUI : BaseScanUI
    {
        //intial scan variables
        private bool scanning, selectFirst, selectSecond, borderClicked;
        private Border borderHovered;
        private Rectangle selectedTiles;

        //modifying area variables
        private bool modifyingArea, selectType, mouseDown;
        private Point16 topLeft;
        private bool[][] includedTiles;
        private Rectangle modifyArea;
        private ModifyType modifyType = ModifyType.Area;

        private Point16 firstPoint;

        private DraggableUIPanel panel;
        private UIPanel scanButton;
        private UIPanel endScanButton;
        public override void OnInitialize()
        {
            panel = new DraggableUIPanel();
            panel.Width.Set(300, 0);
            panel.Height.Set(300, 0);
            panel.VAlign = 0.6f;
            panel.HAlign = 0.4f;
            Append(panel);

            UIText header = new UIText("My UI Header");
            header.HAlign = 0.5f;
            header.Top.Set(15, 0);
            panel.Append(header);

            scanButton = new UIPanel();
            scanButton.Width.Set(100, 0);
            scanButton.Height.Set(50, 0);
            scanButton.HAlign = 0.5f;
            scanButton.Top.Set(25, 0);
            scanButton.OnLeftClick += ScanButtonClick;
            panel.Append(scanButton);

            UIText text = new UIText("Start Scan");
            text.HAlign = text.VAlign = 0.5f;
            scanButton.Append(text);

            endScanButton = new UIPanel();
            endScanButton.Width.Set(100, 0);
            endScanButton.Height.Set(50, 0);
            endScanButton.HAlign = 0.5f;
            endScanButton.Top.Set(125, 0);
            endScanButton.OnLeftClick += EndScan;
            panel.Append(endScanButton);

            UIText text2 = new UIText("Select Area");
            text2.HAlign = text2.VAlign = 0.5f;
            endScanButton.Append(text2);
        }

        private void ScanButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            scanning = !scanning;
            selectFirst = false;
            selectSecond = false;

            modifyingArea = false;
        }

        private void EndScan(UIMouseEvent evt, UIElement listeningElement)
        {
            scanning = false;
            selectFirst = false;
            selectSecond = false;

            modifyingArea = true;
            topLeft = new Point16(selectedTiles.X, selectedTiles.Y);
            includedTiles = new bool[selectedTiles.Width][];

            for (int i = 0; i < selectedTiles.Width; i++)
            {
                includedTiles[i] = new bool[selectedTiles.Height];
                Array.Fill(includedTiles[i], true);
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (scanning)
            {
                if (!selectFirst && !scanButton.IsMouseHovering)
                {
                    selectedTiles = new Rectangle(Player.tileTargetX, Player.tileTargetY, 0, 0);
                    selectFirst = true;
                } else if (selectFirst && !selectSecond)
                {
                    Point16 topLeft = new Point16(Math.Min(Player.tileTargetX, selectedTiles.X), Math.Min(Player.tileTargetY, selectedTiles.Y));
                    Point16 botRight = new Point16(Math.Max(Player.tileTargetX, selectedTiles.X), Math.Max(Player.tileTargetY, selectedTiles.Y));

                    selectedTiles = new Rectangle(topLeft.X, topLeft.Y, botRight.X - topLeft.X + 1, botRight.Y - topLeft.Y + 1);

                    selectSecond = true;
                }
            } else if (modifyingArea && modifyType == ModifyType.Area)
            {
                if (!selectFirst && !panel.IsMouseHovering)
                {
                    firstPoint = new Point16(Player.tileTargetX, Player.tileTargetY);
                    selectFirst = true;
                } else if (selectFirst && !panel.IsMouseHovering)
                {
                    selectFirst = false;

                    int left = Math.Min(firstPoint.X, Player.tileTargetX);
                    int top = Math.Min(firstPoint.Y, Player.tileTargetY);
                    int width = Math.Max(firstPoint.X - left, Player.tileTargetX - left) + 1;
                    int height = Math.Max(firstPoint.Y - top, Player.tileTargetY - top) + 1;
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (selectedTiles.Contains(left + x, top + y))
                            {
                                includedTiles[left + x - topLeft.X][top + y - topLeft.Y] = selectType;
                            }
                        }
                    }
                }
            }
            base.LeftClick(evt);
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            if (selectSecond && borderHovered != Border.None)
            {
                borderClicked = true;
            }

            mouseDown = true;
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            borderClicked = false;
            mouseDown = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawToGame(SpriteBatch spriteBatch)
        {
            if (scanning && !selectFirst)
            {
                DrawHelpers.HighlightTiles(spriteBatch, new Rectangle(Player.tileTargetX, Player.tileTargetY, 1, 1));
            }
            else if (scanning && selectFirst && !selectSecond)
            {
                Point16 topLeft = new Point16(Math.Min(Player.tileTargetX, selectedTiles.X), Math.Min(Player.tileTargetY, selectedTiles.Y));
                Point16 botRight = new Point16(Math.Max(Player.tileTargetX, selectedTiles.X), Math.Max(Player.tileTargetY, selectedTiles.Y));

                DrawHelpers.HighlightTiles(spriteBatch, new Rectangle(topLeft.X, topLeft.Y, botRight.X - topLeft.X + 1, botRight.Y - topLeft.Y + 1));
            }
            else if (selectSecond)
            {
                DrawHelpers.HighlightTiles(spriteBatch, selectedTiles, borderHovered, borderClicked);
            }

            if (modifyingArea)
            {
                DrawHelpers.DrawBorder(spriteBatch, selectedTiles);
                for (int x = 0; x < includedTiles.Length; x++) { 
                    for (int y = 0; y < includedTiles[x].Length; y++)
                    {
                        DrawHelpers.ColorTile(spriteBatch, new Point16(topLeft.X + x, topLeft.Y + y), includedTiles[x][y] ? Color.White * .25f : Color.Black * .25f);
                    }
                }

                if (modifyType == ModifyType.Single)
                {
                    DrawHelpers.DrawBorder(spriteBatch, new Rectangle(Player.tileTargetX, Player.tileTargetY, 1, 1));
                } else if (modifyType == ModifyType.Three) {
                    DrawHelpers.DrawBorder(spriteBatch, new Rectangle(Player.tileTargetX - 1, Player.tileTargetY - 1, 3, 3));
                } else
                {
                    if (!selectFirst)
                    {
                        DrawHelpers.DrawBorder(spriteBatch, new Rectangle(Player.tileTargetX, Player.tileTargetY, 1, 1));
                    } else
                    {
                        int left = Math.Min(firstPoint.X, Player.tileTargetX);
                        int top = Math.Min(firstPoint.Y, Player.tileTargetY);
                        int width = Math.Max(firstPoint.X - left, Player.tileTargetX - left) + 1;
                        int height = Math.Max(firstPoint.Y - top, Player.tileTargetY - top) + 1;

                        DrawHelpers.DrawBorder(spriteBatch, new Rectangle(left, top, width, height));
                    }
                }
            }

            base.DrawToGame(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (scanning && !selectSecond)
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (borderClicked)
            {
                DraggingBorder();
            } else if (Player.tileTargetX == selectedTiles.X + selectedTiles.Width && Player.tileTargetY >= selectedTiles.Y && Player.tileTargetY <= selectedTiles.Y + selectedTiles.Height)
            {
                borderHovered = Border.Right;
                Main.LocalPlayer.mouseInterface = true;
            } else if (Player.tileTargetY + 1== selectedTiles.Y && Player.tileTargetX >= selectedTiles.X && Player.tileTargetX <= selectedTiles.X + selectedTiles.Width)
            {
                borderHovered = Border.Top;
                Main.LocalPlayer.mouseInterface = true;
            } else if (Player.tileTargetX + 1 == selectedTiles.X && Player.tileTargetY >= selectedTiles.Y && Player.tileTargetY <= selectedTiles.Y + selectedTiles.Height)
            {
                borderHovered = Border.Left;
                Main.LocalPlayer.mouseInterface = true;
            } else if (Player.tileTargetY == selectedTiles.Y + selectedTiles.Height && Player.tileTargetX >= selectedTiles.X && Player.tileTargetX <= selectedTiles.X + selectedTiles.Width)
            {
                borderHovered = Border.Bottom;
                Main.LocalPlayer.mouseInterface = true;
            } else
            {
                borderHovered = Border.None;
            }

            if (modifyingArea && mouseDown) {
                if (modifyType == ModifyType.Single && selectedTiles.Contains(Player.tileTargetX, Player.tileTargetY))
                {
                    includedTiles[Player.tileTargetX - topLeft.X][Player.tileTargetY - topLeft.Y] = selectType;
                }

                if (modifyType == ModifyType.Three)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            if (selectedTiles.Contains(Player.tileTargetX + x, Player.tileTargetY + y)) {
                                includedTiles[Player.tileTargetX + x - topLeft.X][Player.tileTargetY + y - topLeft.Y] = selectType;
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        private void DraggingBorder()
        {
            switch (borderHovered)
            {
                case Border.Right:
                    if (Player.tileTargetX < selectedTiles.X)
                    {
                        borderHovered = Border.Left;
                        selectedTiles.Width = selectedTiles.X - Player.tileTargetX;
                        selectedTiles.X = Player.tileTargetX + 1;
                    }
                    else if (Player.tileTargetX > selectedTiles.X)
                    {
                        selectedTiles.Width = Player.tileTargetX - selectedTiles.X;
                    }
                    break;
                case Border.Top:
                    if (Player.tileTargetY  + 1 > selectedTiles.Y + selectedTiles.Height)
                    {
                        borderHovered = Border.Bottom;
                        selectedTiles.Y = selectedTiles.Y + selectedTiles.Height - 1;
                        selectedTiles.Height = Player.tileTargetY - selectedTiles.Y;
                    }
                    else if (Player.tileTargetY + 1 < selectedTiles.Y + selectedTiles.Height)
                    {
                        selectedTiles.Height += selectedTiles.Y - (Player.tileTargetY + 1);
                        selectedTiles.Y = (Player.tileTargetY + 1);
                    }
                    break;
                case Border.Left:
                    if (Player.tileTargetX + 1 > selectedTiles.X + selectedTiles.Width)
                    {
                        borderHovered = Border.Right;
                        selectedTiles.X = selectedTiles.X + selectedTiles.Width - 1;
                        selectedTiles.Width = Player.tileTargetX - selectedTiles.X;
                    }
                    else if (Player.tileTargetX + 1 < selectedTiles.X + selectedTiles.Width)
                    {
                        selectedTiles.Width += selectedTiles.X - (Player.tileTargetX + 1);
                        selectedTiles.X = (Player.tileTargetX + 1);
                    }
                    break;
                case Border.Bottom:
                    if (Player.tileTargetY < selectedTiles.Y)
                    {
                        borderHovered = Border.Top;
                        selectedTiles.Height = selectedTiles.Y - Player.tileTargetY;
                        selectedTiles.Y = Player.tileTargetY + 1;
                    }
                    else if (Player.tileTargetY > selectedTiles.Y)
                    {
                        selectedTiles.Height = Player.tileTargetY - selectedTiles.Y;
                    }
                    break;
            }
        }
    }

    public class SaveButton : UIPanel
    {
        public void Save(TagCompound scannedArea)
        {

        }
        
        public TagCompound serializeArea(int[][] tileIDs)
        {
            return null;
        }
    }
}