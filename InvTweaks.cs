using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
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
        public override void Load()
        {
            // if (!Main.dedServ) // Despite the fact that its UI, it's beter to have IL edits on all clients
            {
                // A little unnecessary, but
                IL.Terraria.Main.GUIHotbarDrawInner += il =>
                {
                    var c = il.At(0);

                    int loopIndex = 0;
                    int x = 0;
                    int y = 0;

                    if (c.TryGotoNext(i => i.MatchCall<ItemSlot>("Draw"),
                        // Main.spriteBatch
                        i => i.MatchLdfld<Main>("spriteBatch"),
                        // Main.player[Main.myPlayer].Inventory
                        i => i.MatchLdsfld<Main>("player"),
                        i => i.MatchLdsfld<Main>("myPlayer"),
                        i => i.MatchLdelemRef(),
                        i => i.MatchLdfld<Player>("inventory"),
                        // 13
                        i => i.MatchLdcI4(13),
                        // i
                        i => i.MatchLdloc(out loopIndex),
                        //Load (float)num
                        i => i.MatchLdloc(out x),
                        i => i.MatchConvR4(),
                        //Load (float)num3
                        i => i.MatchLdloc(out y),
                        i => i.MatchConvR4(),
                        // new Vector2((float)num, (float)num3)
                        i => i.MatchNewobj<Vector2>()))
                    {
                        if (Main.player[Main.myPlayer].selectedItem < 10
                            && !Main.player[Main.myPlayer].dead
                             && loopIndex == 9)
                        {
                            c.Emit(OpCodes.Ldstr, "Hey guys, IL editing here, back with another Terraria mod.");
                            c.Emit(OpCodes.Call, typeof(Main).GetMethod("NewText", new Type[] { typeof(string) }));
                        }
                    }
                };
            }
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
