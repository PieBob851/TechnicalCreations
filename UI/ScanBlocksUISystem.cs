using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TechnicalCreations.UI
{
    [Autoload(Side = ModSide.Client)] 
    public class ScanBlocksUISystem : ModSystem
    {
        private UserInterface baseUI;
        internal BaseScanUI painterUIState, scannerUIState;

        private GameTime _lastUpdateUIGameTime;

        public void OpenPainterUI()
        {
            baseUI?.SetState(painterUIState);
        }

        public void OpenScannerUI()
        {
            baseUI?.SetState(scannerUIState);
        }

        public void CloseUI()
        {
            baseUI?.SetState(null);
        }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            baseUI = new UserInterface();
            painterUIState = new PainterUI();
            scannerUIState = new ScannerUI();

            painterUIState.Activate();
            scannerUIState.Activate();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUIGameTime = gameTime;
            if (baseUI?.CurrentState != null)
            {
                baseUI.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TechnicalCreations: UI Panel Logic",
                    delegate {
                        if (_lastUpdateUIGameTime != null && baseUI?.CurrentState != null)
                        {
                            baseUI.Draw(Main.spriteBatch, _lastUpdateUIGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TechnicalCreations: GameInterfaceLayer",
                    delegate {
                        if (baseUI?.CurrentState != null)
                        {
                            ((BaseScanUI)baseUI.CurrentState).DrawToGame(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.Game)
                );
            }
        }
    }
}