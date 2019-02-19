using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace InvTweaks.UI
{
    internal class CustomInventory : UIState
    {
        private Item innerItem;

        public override void OnInitialize()
        {
            innerItem = new Item();
            innerItem.SetDefaults();
            innerItem.TurnToAir();
        }

        // Using OnDeactivate is useful for clearing out Item slots and returning them to the player, as we do here.
        public override void OnDeactivate()
        {
            if (!innerItem.IsAir)
            {
                // QuickSpawnClonedItem will preserve mod data of the item. QuickSpawnItem will just spawn a fresh version of the item, losing the prefix.
                Main.LocalPlayer.QuickSpawnClonedItem(innerItem, innerItem.stack);
                // Now that we've spawned the item back onto the player, we reset the item by turning it into air.
                innerItem.TurnToAir();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // base.DrawSelf(spriteBatch);
            var p = Main.player[Main.myPlayer];

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                p.mouseInterface = true;
                if (Main.mouseItem.accessory)
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref innerItem, ItemSlot.Context.EquipAccessory);
                }
                else if (!innerItem.IsAir && Main.mouseRight && Main.mouseRightRelease)
                {
                    ItemSlot.SwapEquip(ref innerItem, ItemSlot.Context.EquipAccessory);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            if (Main.playerInventory)
            {
                // TODO
            }
            else if (p.dead && (p.selectedItem >= 10 
                                && (p.selectedItem != 58 || Main.mouseItem.type > 0) ) )
            {
                // TODO
            }
            else
            {
                // TODO
            }
        }
    }
}
