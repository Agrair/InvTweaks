using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace InvTweaks
{
    public class InvTweaks : Mod
    {
        public static InvTweaks instance;
        public static ClientConfig clientConfig;

        internal ShopStackUI shopStackState;
        public UserInterface uiInterface;

        public override void Load()
        {
            instance = this;
            Main.OnTick += () => AccUtils.UpdateEquipSwap();
            if (Main.dedServ)
            {
                Logger.Warn("DON'T LOAD INVENTORY TWEAKS ON SERVER, MAY OR MAY NOT CAUSE ISSUES");
            }
            else
            {
                uiInterface = new UserInterface();
                uiInterface.SetState(null);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (ShopStackUI.visible)
            {
                uiInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(x => x.Name == "Vanilla: Hotbar");
            layers.Insert((index == -1) ? layers.Count - 1 : index + 1,
                new LegacyGameInterfaceLayer("InvTweaks: Hotbar", DrawExtraHotbarSlot,
                InterfaceScaleType.UI));
            layers.Add(new LegacyGameInterfaceLayer("InvTweaks: Shop Stack Select", delegate
            {
                if (ShopStackUI.visible)
                {
                    if (Main.LocalPlayer.talkNPC == -1)
                        ShopStackUI.visible = false;
                    uiInterface?.Draw(Main.spriteBatch, new GameTime());
                }
                return true;
            }, InterfaceScaleType.UI));
        }

        private bool DrawExtraHotbarSlot()
        {
            if (!Main.gameMenu && clientConfig.HelmetSlot)
            {
                float oldScale = Main.inventoryScale;
                Main.inventoryScale = .75f;
                if (Main.playerInventory)
                {
                    Main.inventoryScale = .85f;
                    int num = 20 + ((int)(Main.inventoryBackTexture.Width * .85f) + 4) * 10;
                    if (Main.mouseX > num
                        && Main.mouseX < num + Main.inventoryBackTexture.Width * .85f
                        && Main.mouseY > 20
                        && Main.mouseY < 20 + Main.inventoryBackTexture.Width * .85f
                        && AccUtils.IsAccessory(Main.mouseItem))
                    {
                        Main.instance.MouseText(LMPlayer().extraSlotItem.HoverName, LMPlayer().extraSlotItem.rare);

                        // ItemSlot.Handle(ref LMPlayer().extraSlotItem, 0);
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            AccUtils.SwapItem(ref Main.mouseItem, ref LMPlayer().extraSlotItem);
                        }
                        else if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            AccUtils.SwapEquip(ref LMPlayer().extraSlotItem);
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
                        && AccUtils.IsAccessory(Main.mouseItem))
                    {
                        Main.instance.MouseText(LMPlayer().extraSlotItem.HoverName, LMPlayer().extraSlotItem.rare);

                        // ItemSlot.Handle(ref LMPlayer().extraSlotItem, 0);
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            AccUtils.SwapItem(ref Main.mouseItem, ref LMPlayer().extraSlotItem);
                        }
                        else if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            AccUtils.SwapEquip(ref LMPlayer().extraSlotItem);
                        }
                    }
                    ItemSlot.Draw(Main.spriteBatch, ref LMPlayer().extraSlotItem,
                        ItemSlot.Context.EquipAccessory, new Vector2(num, 25));
                }
                Main.inventoryScale = oldScale;
            }
            return true;
        }

        public override void Unload()
        {
            instance = null;
            clientConfig = null;
            Main.OnTick -= () => AccUtils.UpdateEquipSwap();
        }

        private InvTweaksPlayer LMPlayer() => Main.player[Main.myPlayer].GetModPlayer(this, "InvTweaksPlayer") as InvTweaksPlayer;

        public static string GithubUserName { get { return "Agrair"; } }
        public static string GithubProjectName { get { return "InvTweaks"; } }
    }
}
