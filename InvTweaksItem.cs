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
    }
}
