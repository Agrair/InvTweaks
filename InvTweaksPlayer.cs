using System.Linq;
using Terraria;
using Terraria.Audio;
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

        public override void LoadData(TagCompound tag)
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
            for (int i = 0; i < Player.inventory.Length; i++)
            {
                Item item = Player.inventory[i];
                if (item.type == ItemID.Acorn)
                {
                    originalSelectedItem = Player.selectedItem;
                    autoRevertSelectedItem = true;
                    Player.selectedItem = i;
                    Player.controlUseItem = true;
                    Player.ItemCheck(Main.myPlayer);
                    return;
                }
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("extraSlotItem", ItemIO.Save(extraSlotItem));
        }

        public override void PostUpdateBuffs()
        {
            if (ClientConfig.Instance.AutoBuff)
            {
                for (int queryBuff = 0; queryBuff < Player.buffTime.Length; queryBuff++)
                {
                    if (Player.buffTime[queryBuff] == 1)
                    {
                        for (int queryItem = 0; queryItem < Player.inventory.Length - 5; queryItem++)
                        {
                            if (Player.inventory[queryItem].buffType == Player.buffType[queryBuff])
                            {
                                ItemLoader.UseItem(Player.inventory[queryItem], Player);
                                Player.buffTime[queryBuff] = Player.inventory[queryItem].buffTime;
                                //the swapping of cursorFill is unconventional,
                                //but prevents items from randomly teleporting thru the inv
                                bool cursorFill = ClientConfig.Instance.CursorFill;
                                ClientConfig.Instance.CursorFill = false;
                                if (ItemLoader.ConsumeItem(Player.inventory[queryItem], Player))
                                {
                                    if (--Player.inventory[queryItem].stack <= 0)
                                        Player.inventory[queryItem].TurnToAir();
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
                if (Player.itemTime == 0 && Player.itemAnimation == 0)
                {
                    Player.selectedItem = originalSelectedItem;
                    autoRevertSelectedItem = false;
                }
            }
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (ClientConfig.Instance.AutoHeal
                && Player.statLife < Player.statLifeMax2 * (ClientConfig.Instance.AutoHealThreshold * .01))
            {
                if (Player.noItems)
                    return;
                if (Player.potionDelay > 0)
                    return;
                if (!(Player.QuickHeal_GetItemToUse() is Item item))
                    return;

                SoundEngine.PlaySound(item.UseSound, Player.position);

                if (item.potion)
                {
                    if (item.type == ItemID.RestorationPotion)
                    {
                        Player.AddBuff(21, Player.potionDelay = Player.restorationDelayTime, true);
                    }
                    else
                    {
                        Player.AddBuff(21, Player.potionDelay = Player.potionDelayTime, true);
                    }
                }
                ItemLoader.UseItem(item, Player);
                int healLife = Player.GetHealLife(item, true);

                Player.statLife += healLife;

                if (Player.statLife > Player.statLifeMax2)
                {
                    Player.statLife = Player.statLifeMax2;
                }
                if (Player.statMana > Player.statManaMax2)
                {
                    Player.statMana = Player.statManaMax2;
                }
                if (healLife > 0 && Main.myPlayer == Player.whoAmI)
                {
                    Player.HealEffect(healLife, true);
                }
                bool cursorFill = ClientConfig.Instance.CursorFill;
                ClientConfig.Instance.CursorFill = false;
                if (ItemLoader.ConsumeItem(item, Player))
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
                && PlayerLoader.CanBuyItem(Player, Main.npc[Player.talkNPC], shopInventory, shopItem)
                && (Main.mouseItem.stack < Main.mouseItem.maxStack)
                && (Main.mouseItem.stack < InvTweaksGuiSystem.Instance.shopStackState.ShopStack)
                && Player.BuyItem(shopItem.GetStoreValue(), shopItem.shopSpecialCurrency))
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
                PlayerLoader.PostBuyItem(Player, Main.npc[Player.talkNPC], shopInventory, Main.mouseItem);
            }
        }

        public override bool CanBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (ClientConfig.Instance.ShopClick && Terraria.UI.ItemSlot.ShiftInUse)
            {
                var gui = InvTweaksGuiSystem.Instance;
                var state = new ShopStackUI();
                state.Activate();
                state.ShopStack = item.maxStack;
                ShopStackUI.visible = true;
                gui.userInterface.SetState(gui.shopStackState = state);
            }
            return true;
        }
    }
}