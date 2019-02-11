using Terraria;
using Terraria.ModLoader;

namespace InvTweaks
{
    public class InvTweaksItem : GlobalItem
    {
        public override bool ConsumeItem(Item item, Player player)
        {
            if (item.stack == 1)
            {
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    Item curItem = player.inventory[i];
                    if (curItem.type == item.type && i != player.selectedItem)
                    {
                        item.stack = curItem.stack;
                        curItem.TurnToAir();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
