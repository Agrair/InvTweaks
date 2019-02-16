using System.Collections.Generic;
using InvTweaks.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace InvTweaks
{
    /* TODO LIST
     * Somehow make UI work
     */
    public class InvTweaks : Mod
	{
        internal CustomInventory inventory;
        internal UserInterface inventoryInterface;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                inventory = new CustomInventory();
                inventory.Activate();
                inventoryInterface = new UserInterface();
                inventoryInterface.SetState(inventory);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (!Main.gameMenu)
            {
                inventoryInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "InvTweaks: Extra Slot",
                    delegate {
                        if (!Main.gameMenu)
                        {
                            inventoryInterface?.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
