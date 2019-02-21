using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace InvTweaks
{
    /* TODO LIST
     * Somehow make UI work
     */
    public class InvTweaks : Mod
	{
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(x => x.Name == "Vanilla: Hotbar");
            if (index != -1)
            {
                layers.Insert(index + 1,
                    new LegacyGameInterfaceLayer("Inventory Tweaks : Hotbar", 
                    delegate
                    {
                        if (!Main.gameMenu)
                        {
                            float oldScale = Main.inventoryScale;
                            Main.inventoryScale = .75f;
                            if (Main.playerInventory)
                            {
                                Main.inventoryScale = .85f;
                                int num = 20 + ((int)(Main.inventoryBackTexture.Width * Main.inventoryScale) + 4) * 10;
                                if (Main.mouseX > num
                                    && Main.mouseX < num + Main.inventoryBackTexture.Width * Main.inventoryScale
                                    && Main.mouseY > 20
                                    && Main.mouseY < 20 + Main.inventoryBackTexture.Width * Main.inventoryScale
                                    && ValidItem(Main.mouseItem))
                                {
                                    ItemSlot.Handle(ref LMPlayer().extraSlotItem, 0);
                                }
                                ItemSlot.Draw(Main.spriteBatch, ref LMPlayer().extraSlotItem,
                                    ItemSlot.Context.EquipAccessory, new Vector2(num, 20));
                            }
                            else
                            {
                                int num = 20;
                                for (int i = 0; i < Main.hotbarScale.Length; i++)
                                {
                                    num += (int)(Main.inventoryBackTexture.Width * Main.hotbarScale[i]) + 4;
                                }
                                if (Main.mouseX > num
                                    && Main.mouseX < num + (int)(Main.inventoryBackTexture.Width * .75f)
                                    && Main.mouseY > 20
                                    && Main.mouseY < 20 + (int)(Main.inventoryBackTexture.Width * .75f)
                                    && ValidItem(Main.mouseItem))
                                {
                                    ItemSlot.Handle(ref LMPlayer().extraSlotItem, 0);
                                }
                                ItemSlot.Draw(Main.spriteBatch, ref LMPlayer().extraSlotItem,
                                    ItemSlot.Context.EquipAccessory, new Vector2(num, 25));
                            }
                            Main.inventoryScale = oldScale;
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        private InvTweaksPlayer LMPlayer()
        {
            return Main.player[Main.myPlayer].GetModPlayer(this, "InvTweaksPlayer") as InvTweaksPlayer;
        }

        private bool ValidItem(Item item)
        {
            return item.IsAir || (!item.IsAir && (item.vanity 
                                || item.accessory 
                                || item.headSlot > -1
                                || item.bodySlot > -1 
                                || item.legSlot > -1));
        }
    }
}
