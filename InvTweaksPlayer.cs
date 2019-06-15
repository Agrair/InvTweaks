using System;
using Terraria;
using Terraria.ID;
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
            // legacy load
            int type = tag.GetInt("extraSlotItem_Type");
            if (type != 0) extraSlotItem.SetDefaults(type);
            if (tag.GetCompound("extraSlotItem") is TagCompound item
                && item != new TagCompound())
            {
                extraSlotItem = ItemIO.Load(item);
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["extraSlotItem"] = ItemIO.Save(extraSlotItem)
            };
        }
        
        /*

        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (!inventory[slot].IsAir)
            {
                if (context == ItemSlot.Context.ShopItem)
                {
                    var num = Utils.CoinsCount(out _, player.bank.item);
                    var num2 = Utils.CoinsCount(out _, player.bank2.item);
                    var num3 = Utils.CoinsCount(out _, player.bank3.item);
                    var num4 = Utils.CoinsCount(out _, player.inventory, new int[]
                    {
                        58,
                        57,
                        56,
                        55,
                        54
                    });
                    var change = Utils.CoinsCombineStacks(out _, num, num2, num3, num4);
                    if (change < inventory[slot].GetStoreValue()) return false;
                    if ((Main.mouseItem.type == inventory[slot].type || Main.mouseItem.type == 0)
                        && Main.mouseItem.stack < Main.mouseItem.maxStack)
                    {
                        do
                        {
                            if (Main.mouseItem.type == 0)
                            {
                                Main.mouseItem.SetDefaults(inventory[slot].type);
                                Main.mouseItem.stack = 0;
                            }
                            Main.mouseItem.stack++;
                            purchase();
                            change -= inventory[slot].GetStoreValue();
                        }
                        while (Main.mouseItem.stack < Main.mouseItem.maxStack
                            && change >= inventory[slot].GetStoreValue());
                        return true;
                    }
                }
            }
            return false;

            void purchase()
            {
                int remainingCost = inventory[slot].GetStoreValue();
                for (int i = 0; i < player.bank.item.Length; i++)
                {
                    if (consumeCoin(player.bank.item[i])) return;
                }
                for (int i = 0; i < player.bank2.item.Length; i++)
                {
                    if (consumeCoin(player.bank2.item[i])) return;
                }
                for (int i = 0; i < player.bank3.item.Length; i++)
                {
                    if (consumeCoin(player.bank3.item[i])) return;
                }
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    if (consumeCoin(player.inventory[i])) return;
                }

                bool consumeCoin(Item item)
                {
                    if (item.type == ItemID.PlatinumCoin)
                    {
                        var consumeCount = Utils.Clamp((int)Math.Floor(remainingCost / 100000000.0), 0, item.stack);
                        remainingCost -= 100000000 * consumeCount;
                        if ((item.stack -= consumeCount) == 0) item.TurnToAir();
                        if (remainingCost == 0) return true;
                        if (remainingCost < 0)
                        {
                            remainingCost = Math.Abs(remainingCost);
                            // if `remainingCost` was negative, that means it was
                            // less than one platinum
                            player.QuickSpawnItem(ItemID.GoldCoin, remainingCost);
                        }
                    }
                    else if (item.type == ItemID.GoldCoin)
                    {
                        var consumeCount = Utils.Clamp((int)Math.Floor(remainingCost / 100000000.0), 0, item.stack);
                        remainingCost -= 100000000 * consumeCount;
                        if ((item.stack -= consumeCount) == 0) item.TurnToAir();
                        if (remainingCost == 0) return true;
                        if (remainingCost < 0)
                        {
                            remainingCost = Math.Abs(remainingCost);
                            player.QuickSpawnItem(ItemID.GoldCoin, remainingCost);
                        }
                    }
                    else if (item.type == ItemID.SilverCoin)
                    {
                        var consumeCount = Utils.Clamp((int)Math.Floor(remainingCost / 100000000.0), 0, item.stack);
                        remainingCost -= 100000000 * consumeCount;
                        if ((item.stack -= consumeCount) == 0) item.TurnToAir();
                        if (remainingCost == 0) return true;
                        if (remainingCost < 0)
                        {
                            remainingCost = Math.Abs(remainingCost);
                            player.QuickSpawnItem(ItemID.CopperCoin, remainingCost);
                        }
                    }
                    else if (item.type == ItemID.CopperCoin)
                    {
                        var consumeCount = Utils.Clamp(remainingCost, 0, item.stack);
                        remainingCost -= consumeCount;
                        if ((item.stack -= consumeCount) == 0) item.TurnToAir();
                        if (remainingCost == 0) return true;
                    }
                    return false;
                }
            }
        }

    */
    }
}
