using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
            if (type != 0)
                extraSlotItem.SetDefaults(type);
            if (tag.GetCompound("extraSlotItem") is TagCompound item
                && item != new TagCompound())
            {
                extraSlotItem = ItemIO.Load(item);
            }
        }

        private int originalSelectedItem;
        private bool autoRevertSelectedItem;

        public void PlaceTree()
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if (item.type == ItemID.Acorn)
                {
                    originalSelectedItem = player.selectedItem;
                    autoRevertSelectedItem = true;
                    player.selectedItem = i;
                    player.controlUseItem = true;
                    player.ItemCheck(Main.myPlayer);
                    return;
                }
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["extraSlotItem"] = ItemIO.Save(extraSlotItem)
            };
        }

        public override void PostUpdateBuffs()
        {
            if (ClientConfig.Instance.AutoBuff)
            {
                for (int queryBuff = 0; queryBuff < player.buffTime.Length; queryBuff++)
                {
                    if (player.buffTime[queryBuff] == 1)
                    {
                        for (int queryItem = 0; queryItem < player.inventory.Length - 5; queryItem++)
                        {
                            if (player.inventory[queryItem].buffType == player.buffType[queryBuff])
                            {
                                ItemLoader.UseItem(player.inventory[queryItem], player);
                                player.buffTime[queryBuff] = player.inventory[queryItem].buffTime;
                                //the swapping of cursorFill is unconventional,
                                //but prevents items from randomly teleporting thru the inv
                                bool cursorFill = ClientConfig.Instance.CursorFill;
                                ClientConfig.Instance.CursorFill = false;
                                if (ItemLoader.ConsumeItem(player.inventory[queryItem], player))
                                {
                                    if (--player.inventory[queryItem].stack <= 0)
                                        player.inventory[queryItem].TurnToAir();
                                }
                                ClientConfig.Instance.CursorFill = cursorFill;
                                Recipe.FindRecipes();
                            }
                        }
                    }
                }
            }
        }

        public override void PostUpdate()
        {
            if (autoRevertSelectedItem)
            {
                if (player.itemTime == 0 && player.itemAnimation == 0)
                {
                    player.selectedItem = originalSelectedItem;
                    autoRevertSelectedItem = false;
                }
            }
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (ClientConfig.Instance.AutoHeal
                && player.statLife < player.statLifeMax2 * (ClientConfig.Instance.AutoHealThreshold * .01))
            {
                if (player.noItems)
                    return;
                if (player.potionDelay > 0)
                    return;
                if (!(player.QuickHeal_GetItemToUse() is Item item))
                    return;

                Main.PlaySound(item.UseSound, player.position);

                if (item.potion)
                {
                    if (item.type == ItemID.RestorationPotion)
                    {
                        player.AddBuff(21, player.potionDelay = player.restorationDelayTime, true);
                    }
                    else
                    {
                        player.AddBuff(21, player.potionDelay = player.potionDelayTime, true);
                    }
                }
                ItemLoader.UseItem(item, player);
                int healLife = player.GetHealLife(item, true);

                player.statLife += healLife;

                if (player.statLife > player.statLifeMax2)
                {
                    player.statLife = player.statLifeMax2;
                }
                if (player.statMana > player.statManaMax2)
                {
                    player.statMana = player.statManaMax2;
                }
                if (healLife > 0 && Main.myPlayer == player.whoAmI)
                {
                    player.HealEffect(healLife, true);
                }
                bool cursorFill = ClientConfig.Instance.CursorFill;
                ClientConfig.Instance.CursorFill = false;
                if (ItemLoader.ConsumeItem(item, player))
                {
                    if (--item.stack <= 0)
                        item.TurnToAir();
                }
                ClientConfig.Instance.CursorFill = cursorFill;
                Recipe.FindRecipes();
            }
        }

        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            Item shopItem = shopInventory.FirstOrDefault(x => x.type == item.type);
            if (ShopStackUI.visible
                && !Terraria.UI.ItemSlot.ShiftInUse
                && PlayerHooks.CanBuyItem(player, Main.npc[player.talkNPC], shopInventory, shopItem)
                && (Main.mouseItem.stack < Main.mouseItem.maxStack)
                && (Main.mouseItem.stack < ShopStackUI.Instance.ShopStack)
                && player.BuyItem(shopItem.GetStoreValue(), shopItem.shopSpecialCurrency))
            {
                Main.mouseItem.stack++;
                if (Main.stackSplit == 0)
                {
                    Main.stackSplit = 15;
                }
                else
                {
                    Main.stackSplit = Main.stackDelay;
                }
                if (shopItem.buyOnce && --shopItem.stack <= 0)
                {
                    shopItem.SetDefaults(0, false);
                }
                //PlayerHooks.PostBuyItem(player, Main.npc[player.talkNPC], shopInventory, Main.mouseItem);
            }
        }

        public override bool CanBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (ClientConfig.Instance.ShopClick && Terraria.UI.ItemSlot.ShiftInUse)
            {
                var mod = ModContent.GetInstance<InvTweaks>();
                var state = new ShopStackUI();
                state.Activate();
                state.ShopStack = item.maxStack;
                mod.userInterface.SetState(mod.shopStackState = state);
            }
            return true;
        }
    }
}