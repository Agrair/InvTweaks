using System.Reflection;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;
using Terraria.UI;

namespace InvTweaks
{
    static class AccUtils
    {
        public static void SwapEquip(ref Item item)
        {
            item = ArmorSwap(item, out bool flag);
            if (flag)
            {
                Main.EquipPageSelected = 0;
                AchievementsHelper.HandleOnEquip(Main.player[Main.myPlayer], item, item.accessory ? 10 : 8);
            }
            Recipe.FindRecipes();
        }

        public static bool IsAccessory(Item item)
        {
            return item.IsAir || (!item.IsAir && (item.vanity
                                || item.accessory
                                || item.headSlot > -1
                                || item.bodySlot > -1
                                || item.legSlot > -1));
        }

        public static void SwapItem(ref Item f, ref Item s)
        {
            var temp = f;
            f = s;
            s = temp;
        }

        private static bool invOpen;
        private static readonly FieldInfo slotCountInfo = typeof(ItemSlot)
            .GetField("accSlotCount", BindingFlags.NonPublic | BindingFlags.Static);
        private static Item ArmorSwap(Item item, out bool success)
        {
            var accSlotCount = (int)slotCountInfo.GetValue(null);
            success = false;
            Player player = Main.player[Main.myPlayer];

            //configure all variables
            int vanityOffset = (item.vanity && !item.accessory) ? 10 : 0;
            item.favorited = false;
            Item result = item;
            
            //decide where the item should go
            if (item.headSlot != -1)
            {
                result = player.armor[vanityOffset].Clone();
                player.armor[vanityOffset] = item.Clone();
            }
            else if (item.bodySlot != -1)
            {
                result = player.armor[vanityOffset + 1].Clone();
                player.armor[vanityOffset + 1] = item.Clone();
            }
            else if (item.legSlot != -1)
            {
                result = player.armor[vanityOffset + 2].Clone();
                player.armor[vanityOffset + 2] = item.Clone();
            }
            else if (item.accessory)
            {
                int totalSlotCount = 5 + Main.player[Main.myPlayer].extraAccessorySlots;
                for (int i = 3; i < 3 + totalSlotCount; i++)
                {
                    if (player.armor[i].type == 0)
                    {
                        accSlotCount = i - 3;
                        break;
                    }
                }
                for (int j = 0; j < player.armor.Length; j++)
                {
                    if (item.IsTheSameAs(player.armor[j])) accSlotCount = j - 3;
                    if (j < 10 && item.wingSlot > 0 && player.armor[j].wingSlot > 0) accSlotCount = j - 3;
                }
                for (int k = 0; k < totalSlotCount; k++)
                {
                    int index = 3 + (accSlotCount + totalSlotCount) % totalSlotCount;
                    if (ItemLoader.CanEquipAccessory(item, index))
                    {
                        accSlotCount = index - 3;
                        break;
                    }
                }
                if (accSlotCount >= totalSlotCount)
                {
                    accSlotCount = 0;
                }
                if (accSlotCount < 0)
                {
                    accSlotCount = totalSlotCount - 1;
                }
                int num3 = 3 + accSlotCount;
                for (int k = 0; k < player.armor.Length; k++)
                {
                    if (item.IsTheSameAs(player.armor[k]))
                    {
                        num3 = k;
                    }
                }

                //finish accessory swap
                if (!ItemLoader.CanEquipAccessory(item, num3)) return item;
                result = player.armor[num3].Clone();
                player.armor[num3] = item.Clone();
                slotCountInfo.SetValue(null, ++accSlotCount);
                if (accSlotCount >= totalSlotCount) slotCountInfo.SetValue(null, 0);
            }

            //finish
            Main.PlaySound(7);
            Recipe.FindRecipes();
            success = true;
            return result;
        }

        public static void UpdateEquipSwap()
        {
            //reset if inventory was closed
            if (invOpen && !Main.playerInventory)
            {
                slotCountInfo.SetValue(null, 0);
            }
            invOpen = Main.playerInventory;
        }
    }
}
