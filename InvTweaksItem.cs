using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InvTweaks
{
    public class InvTweaksItem : GlobalItem
    {
        public override bool ConsumeItem(Item item, Player player)
        {
            if (item.stack == 1 && Config.cursorFill)
            {
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    Item queryItem = player.inventory[i];
                    if (queryItem.type == item.type && i != player.selectedItem && !queryItem.favorited)
                    {
                        item.stack = queryItem.stack;
                        queryItem.TurnToAir();
                        return false;
                    }
                }
            }
            return true;
        }

        public override void SetDefaults(Item item)
        {
            if ((item.type == ItemID.ManaCrystal ||
                item.type == ItemID.LifeCrystal ||
                item.type == ItemID.LifeFruit) && Config.lifeCrystalDevour)
            {
                item.useTime = 3;
                item.useAnimation = 9;
                item.autoReuse = true;
                item.reuseDelay = 2;
                item.useTurn = true;
            }
        }
    }
}
