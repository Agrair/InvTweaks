using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace InvTweaks
{
    public class InvTweaks : Mod
    {
        public override void Load()
        {
            Config.Load();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(x => x.Name == "Vanilla: Hotbar");
            if (index != -1)
            {
                layers.Insert(index + 1,
                    new LegacyGameInterfaceLayer("InvTweaks: Hotbar", DrawExtraHotbarSlot,
                    InterfaceScaleType.UI));
            }
        }

        private bool DrawExtraHotbarSlot()
        {
            if (!Main.gameMenu && Config.helmetSlot)
            {
                float oldScale = Main.inventoryScale;
                Main.inventoryScale = .75f;
                // FixMouse();
                if (Main.playerInventory)
                {
                    Main.inventoryScale = .85f;
                    int num = 20 + ((int)(Main.inventoryBackTexture.Width * .85f) + 4) * 10;
                    if (Main.mouseX > num
                        && Main.mouseX < num + Main.inventoryBackTexture.Width * .85f
                        && Main.mouseY > 20
                        && Main.mouseY < 20 + Main.inventoryBackTexture.Width * .85f
                        && IsAccessory(Main.mouseItem))
                    {
                        Main.instance.MouseText(LMPlayer().extraSlotItem.HoverName, LMPlayer().extraSlotItem.rare);

                        // ItemSlot.Handle(ref LMPlayer().extraSlotItem, 0);
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            Swap(ref Main.mouseItem, ref LMPlayer().extraSlotItem);
                        }
                        else if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            ItemSlot.SwapEquip(ref LMPlayer().extraSlotItem);
                        }
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
                    if (Main.LocalPlayer.selectedItem >= Main.hotbarScale.Length)
                    {
                        num += Main.inventoryBackTexture.Width + 4;
                    }
                    if (Main.mouseX > num
                        && Main.mouseX < num + (int)(Main.inventoryBackTexture.Width * .75f)
                        && Main.mouseY > 20
                        && Main.mouseY < 20 + (int)(Main.inventoryBackTexture.Width * .75f)
                        && IsAccessory(Main.mouseItem))
                    {
                        Main.instance.MouseText(LMPlayer().extraSlotItem.HoverName, LMPlayer().extraSlotItem.rare);

                        // ItemSlot.Handle(ref LMPlayer().extraSlotItem, 0);
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            Swap(ref Main.mouseItem, ref LMPlayer().extraSlotItem);
                        }
                        else if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            ItemSlot.SwapEquip(ref LMPlayer().extraSlotItem);
                        }
                    }
                    ItemSlot.Draw(Main.spriteBatch, ref LMPlayer().extraSlotItem,
                        ItemSlot.Context.EquipAccessory, new Vector2(num, 25));
                }
                // FixMouse(false);
                Main.inventoryScale = oldScale;
            }
            return true;
        }

        #region Code given by direwolf, doesn't work in this situation anymore though
        /*
        int lastMouseXbak;
        int lastMouseYbak;
        int mouseXbak;
        int mouseYbak;
        int lastscreenWidthbak;
        int lastscreenHeightbak;
        private void FixMouse(bool fix = true)
        {
            if (fix)
            {
                lastMouseXbak = Main.lastMouseX;
                lastMouseYbak = Main.lastMouseY;
                mouseXbak = Main.mouseX;
                mouseYbak = Main.mouseY;
                lastscreenWidthbak = Main.screenWidth;
                lastscreenHeightbak = Main.screenHeight;

                PlayerInput.SetZoom_Unscaled();
                PlayerInput.SetZoom_MouseInWorld();
            }
            else
            {
                Main.lastMouseX = lastMouseXbak;
                Main.lastMouseY = lastMouseYbak;
                Main.mouseX = mouseXbak;
                Main.mouseY = mouseYbak;
                Main.screenWidth = lastscreenWidthbak;
                Main.screenHeight = lastscreenHeightbak;
            }
        }
        */
        #endregion

        private void Swap(ref Item f, ref Item s)
        {
            var temp = f;
            f = s;
            s = temp;
        }

        private InvTweaksPlayer LMPlayer()
        {
            return Main.player[Main.myPlayer].GetModPlayer(this, "InvTweaksPlayer") as InvTweaksPlayer;
        }

        private bool IsAccessory(Item item)
        {
            return item.IsAir || (!item.IsAir && (item.vanity 
                                || item.accessory 
                                || item.headSlot > -1
                                || item.bodySlot > -1 
                                || item.legSlot > -1));
        }
        
        public static string GithubUserName { get { return "Agrair"; } }
        public static string GithubProjectName { get { return "InvTweaks"; } }
    }
}
