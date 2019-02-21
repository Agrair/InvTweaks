using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(x => x.Name == "Hotbar");
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
                                int num = ((Main.inventoryBackTexture.Width) + 4) * 10;
                                ItemSlot.Draw(Main.spriteBatch, ref LMPlayer().extraSlotItem,
                                    ItemSlot.Context.EquipAccessory, new Vector2(num, 20));
                                if (Main.mouseX > num
                                    && Main.mouseX < num + Main.inventoryBackTexture.Width
                                    && Main.mouseY > 20
                                    && Main.mouseY < 20 + Main.inventoryBackTexture.Width
                                    && Main.mouseItem.accessory)
                                {
                                    ItemSlot.Handle(ref LMPlayer().extraSlotItem, 10);

                                    if (Main.mouseRight && Main.mouseRightRelease)
                                    {
                                        ItemSlot.SwapEquip(ref LMPlayer().extraSlotItem, 10);
                                    }
                                }
                            }
                            else if (Main.player[Main.myPlayer].selectedItem >= 10
                                && Main.player[Main.myPlayer].dead)
                            {
                                int num = (int)(((Main.inventoryBackTexture.Width * .75f) + 4) * 9);
                                num += Main.inventoryBackTexture.Width + 4;
                                ItemSlot.Draw(Main.spriteBatch, ref LMPlayer().extraSlotItem,
                                    ItemSlot.Context.EquipAccessory, new Vector2(num, 20));
                                if (Main.mouseX > num
                                    && Main.mouseX < num + (int)(Main.inventoryBackTexture.Width * .85f)
                                    && Main.mouseY > 20
                                    && Main.mouseY < 20 + (int)(Main.inventoryBackTexture.Width * .85f)
                                    && Main.mouseItem.accessory)
                                {
                                    ItemSlot.Handle(ref LMPlayer().extraSlotItem, 10);

                                    if (Main.mouseRight && Main.mouseRightRelease)
                                    {
                                        ItemSlot.SwapEquip(ref LMPlayer().extraSlotItem, 10);
                                    }
                                }
                            }
                            // else if (!Main.playerInventory)
                            else
                            {
                                int num = ((Main.inventoryBackTexture.Width) + 4) * 10;
                                ItemSlot.Draw(Main.spriteBatch, ref LMPlayer().extraSlotItem,
                                    ItemSlot.Context.EquipAccessory, new Vector2(num, 20));
                                if (Main.mouseX > num
                                    && Main.mouseX < num + (int)(Main.inventoryBackTexture.Width * .85f)
                                    && Main.mouseY > 20
                                    && Main.mouseY < 20 + (int)(Main.inventoryBackTexture.Width * .85f)
                                    && Main.mouseItem.accessory)
                                {
                                    ItemSlot.Handle(ref LMPlayer().extraSlotItem, 10);

                                    if (Main.mouseRight && Main.mouseRightRelease)
                                    {
                                        ItemSlot.SwapEquip(ref LMPlayer().extraSlotItem, 10);
                                    }
                                }
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

        private void DrawExtraSlot(SpriteBatch batch, int i, int j, Color drawColor)
        {
            Item drawItem = Main.player[Main.myPlayer].GetModPlayer<InvTweaksPlayer>().extraSlotItem;
            if (Main.mouseX > i && 
                Main.mouseX < i + (int)(Main.inventoryBackTexture.Width * Main.inventoryScale) &&
                Main.mouseY > j &&
                Main.mouseY < j + (int)(Main.inventoryBackTexture.Height * Main.inventoryScale)
                && Main.mouseItem.accessory)
            {
                ItemSlot.Handle(ref drawItem, 10);

                if (Main.mouseRight && Main.mouseRightRelease)
                {
                    ItemSlot.SwapEquip(ref drawItem, 10);
                }
            }
            // float oldScale = Main.inventoryScale;
            // Main.inventoryScale = 1;
            ItemSlot.Draw(batch, ref drawItem, 
                ItemSlot.Context.EquipAccessory, new Vector2(i, j), drawColor);
            // Main.inventoryScale = oldScale;
        }
    }
}
