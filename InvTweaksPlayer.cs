using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace InvTweaks
{
    public class InvTweaksPlayer : ModPlayer
    {
        public Item extraSlotItem;

        public override void Initialize()
        {
            extraSlotItem = new Item();
            extraSlotItem.SetDefaults();
            extraSlotItem.TurnToAir();
        }

        public override void Load(TagCompound tag)
        {
            extraSlotItem.SetDefaults(tag.GetInt("extraSlotItem_Type"));
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["extraSlotItem_Type"] = extraSlotItem.type
            };
        }

        #region TODO: MAKE THIS SHIT WORK
        /*
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (!inventory[slot].IsAir)
            {
                if (context == ItemSlot.Context.ShopItem)
                {
                    var num = Utils.CoinsCount(out bool flag, player.bank.item);
                    var num2 = Utils.CoinsCount(out flag, player.bank2.item);
                    var num3 = Utils.CoinsCount(out flag, player.bank3.item);
                    var num4 = Utils.CoinsCount(out flag, player.inventory);
                    var totalSavings = Utils.CoinsCombineStacks(out flag, num, num2, num3, num4);
                    if (totalSavings >= inventory[slot].GetStoreValue()
                        && Main.mouseItem.stack <= Main.mouseItem.maxStack)
                    {
                        Main.PlaySound(18);
                        flag = true;
                    }
                    while (totalSavings >= inventory[slot].GetStoreValue()
                        && Main.mouseItem.stack <= Main.mouseItem.maxStack)
                    {
                        Chest chest = Main.instance.shop[Main.npcShop];
                        if (Main.stackSplit <= 1 && inventory[slot].type > 0 
                            && (Main.mouseItem.IsTheSameAs(inventory[slot]) || Main.mouseItem.type == 0))
                        {
                            if ((Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0)
                                && player.BuyItem(inventory[slot].GetStoreValue(),
                                    inventory[slot].shopSpecialCurrency)
                                && inventory[slot].stack > 0)
                                // , PlayerHooks.CanBuyItem())
                            {
                                Main.mouseItem.stack++;
                            }
                        }
                        num = Utils.CoinsCount(out flag, player.bank.item);
                        num2 = Utils.CoinsCount(out flag, player.bank2.item);
                        num3 = Utils.CoinsCount(out flag, player.bank3.item);
                        num4 = Utils.CoinsCount(out flag, player.inventory);
                        totalSavings = Utils.CoinsCombineStacks(out flag, num, num2, num3, num4);
                    }
                    return flag;
                }
            }
            return false;
        }
        */
        #endregion
    }
}
