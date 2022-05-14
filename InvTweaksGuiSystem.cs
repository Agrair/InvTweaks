using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace InvTweaks {
    public class InvTweaksGuiSystem : ModSystem {
        public static InvTweaksGuiSystem Instance => ModContent.GetInstance<InvTweaksGuiSystem>();

        internal ShopStackUI shopStackState;
        public UserInterface userInterface;

        public override void UpdateUI(GameTime gameTime) {
            if (ShopStackUI.visible) {
                userInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int index = layers.FindIndex(x => x.Name == "Vanilla: Hotbar");
            layers.Insert((index == -1) ? layers.Count - 1 : index + 1,
                new LegacyGameInterfaceLayer("InvTweaks: Hotbar", DrawExtraHotbarSlot,
                InterfaceScaleType.UI));
            layers.Add(new LegacyGameInterfaceLayer("InvTweaks: Shop Stack Select", delegate {
                if (ShopStackUI.visible) {
                    if (Main.LocalPlayer.talkNPC == -1) {
                        ShopStackUI.visible = false;
                        userInterface.SetState(shopStackState = null);
                    }
                    userInterface?.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                }
                return true;
            }, InterfaceScaleType.UI));
        }

        private bool DrawExtraHotbarSlot() {
            if (!Main.gameMenu && ClientConfig.Instance.HelmetSlot) {
                Player player = Main.LocalPlayer;
                InvTweaksPlayer modPlayer = player.GetModPlayer<InvTweaksPlayer>();

                float oldScale = Main.inventoryScale;
                
                if (Main.playerInventory) {
                    Main.inventoryScale = .85f;
                    int x = 88 + ((int)(TextureAssets.InventoryBack.Value.Width * .85f) + 4) * 10;
                    int y = 20;
                    if (Main.mouseX > x
                        && Main.mouseX < x + TextureAssets.InventoryBack.Value.Width * .85f
                        && Main.mouseY > y
                        && Main.mouseY < y + TextureAssets.InventoryBack.Value.Width * .85f) {

                        player.mouseInterface = true;

                        ItemSlot.OverrideHover(ref modPlayer.extraSlotItem);
                        // Checks both the slot item % mouseItem
                        if (isAccessory(modPlayer.extraSlotItem)) {
                            ItemSlot.LeftClick(ref modPlayer.extraSlotItem);
                        }
                        ItemSlot.RightClick(ref modPlayer.extraSlotItem);
                        if (Main.mouseLeftRelease && Main.mouseLeft)
                            Recipe.FindRecipes();

                        ItemSlot.MouseHover(ref modPlayer.extraSlotItem);
                    }
                    ItemSlot.Draw(Main.spriteBatch, ref modPlayer.extraSlotItem,
                        ItemSlot.Context.EquipAccessory, new Vector2(x, y));
                }

                else {
                    Main.inventoryScale = .75f;
                    int x = 20;
                    for (int i = 0; i < Main.hotbarScale.Length; i++) {
                        x += (int)(TextureAssets.InventoryBack.Value.Width * Main.hotbarScale[i]) + 4;
                    }
                    if (Main.LocalPlayer.selectedItem >= Main.hotbarScale.Length) {
                        x += TextureAssets.InventoryBack.Value.Width + 4;
                    }
                    int y = 25;

                    if (Main.mouseX > x
                        && Main.mouseX < x + TextureAssets.InventoryBack.Value.Width * .85f
                        && Main.mouseY > y
                        && Main.mouseY < y + TextureAssets.InventoryBack.Value.Width * .85f) {

                        player.mouseInterface = true;

                        ItemSlot.OverrideHover(ref modPlayer.extraSlotItem, ItemSlot.Context.HotbarItem);
                        ItemSlot.RightClick(ref modPlayer.extraSlotItem);
                        if (Main.mouseLeftRelease && Main.mouseLeft)
                            Recipe.FindRecipes();

                        ItemSlot.MouseHover(ref modPlayer.extraSlotItem, ItemSlot.Context.HotbarItem);
                    }
                    ItemSlot.Draw(Main.spriteBatch, ref modPlayer.extraSlotItem,
                        ItemSlot.Context.EquipAccessory, new Vector2(x, y));
                }

                Main.inventoryScale = oldScale;
            }
            return true;

            bool isAccessory(Item item) {
                Item[] arr = new Item[] { item };

                return checkCtx(ItemSlot.Context.EquipArmor)
                    || checkCtx(ItemSlot.Context.EquipAccessory)
                    || checkCtx(ItemSlot.Context.EquipMount)
                    || checkCtx(ItemSlot.Context.EquipLight)
                    || checkCtx(ItemSlot.Context.EquipPet)
                    || checkCtx(ItemSlot.Context.EquipMinecart)
                    || checkCtx(ItemSlot.Context.EquipAccessoryVanity)
                    || checkCtx(ItemSlot.Context.EquipArmorVanity)
                    || checkCtx(ItemSlot.Context.EquipDye)
                    || checkCtx(ItemSlot.Context.ModdedAccessorySlot)
                    || checkCtx(ItemSlot.Context.ModdedDyeSlot)
                    || checkCtx(ItemSlot.Context.ModdedVanityAccessorySlot);

                bool checkCtx(int ctx) => ItemSlot.PickItemMovementAction(arr, ctx, 0, Main.mouseItem) != -1;
            }
        }
    }
}
