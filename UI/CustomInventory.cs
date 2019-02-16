using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace InvTweaks.UI
{
    internal class CustomInventory : UIState
    {
        private VanillaItemSlotWrapper _vanillaItemSlot;

        public override void OnInitialize()
        {
            _vanillaItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.EquipAccessory, Main.inventoryScale)
            {
                Left = { Pixels = 463 },
                Top = { Pixels = 25 }
            };
            _vanillaItemSlot.OnRightClick += (evt, lst) =>
            {
                if (!_vanillaItemSlot.Item.IsAir)
                {
                    ItemSlot.SwapEquip(ref _vanillaItemSlot.Item,
                        ItemSlot.Context.EquipAccessory);
                }
            };
            Append(_vanillaItemSlot);
        }

        // OnDeactivate is called when the UserInterface switches to a different state. In this mod, we switch between no state (null) and this state (ExamplePersonUI).
        // Using OnDeactivate is useful for clearing out Item slots and returning them to the player, as we do here.
        public override void OnDeactivate()
        {
            if (!_vanillaItemSlot.Item.IsAir)
            {
                // QuickSpawnClonedItem will preserve mod data of the item. QuickSpawnItem will just spawn a fresh version of the item, losing the prefix.
                Main.LocalPlayer.QuickSpawnClonedItem(_vanillaItemSlot.Item, _vanillaItemSlot.Item.stack);
                // Now that we've spawned the item back onto the player, we reset the item by turning it into air.
                _vanillaItemSlot.Item.TurnToAir();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.playerInventory)
            {
                _vanillaItemSlot.Left.Set(495, 0);
                _vanillaItemSlot.Top.Set(20, 0);
            }
            else if (Main.LocalPlayer.dead && Main.LocalPlayer.selectedItem > 9)
            {
                _vanillaItemSlot.Left.Set(505, 0);
            }
            else
            {
                _vanillaItemSlot.Left.Set(463, 0);
                _vanillaItemSlot.Top.Set(25, 0);
            }
        }
    }
}
