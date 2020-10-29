using Mono.CompilerServices.SymbolWriter;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InvTweaks
{
    public class InvTweaksItem : GlobalItem
    {
        public override bool ConsumeItem(Item item, Player player)
        {
            if (item.stack == 1 && InvTweaks.clientConfig.CursorFill)
            {
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    Item queryItem = player.inventory[i];
                    if (queryItem.type == item.type && i != player.selectedItem && !queryItem.favorited)
                    {
                        item.stack = queryItem.stack;
                        queryItem.TurnToAir();
                        Recipe.FindRecipes();
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
                item.type == ItemID.LifeFruit) && InvTweaks.clientConfig.LifeCrystalDevour)
            {
                item.useTime = 3;
                item.useAnimation = 9;
                item.autoReuse = true;
                item.reuseDelay = 2;
                item.useTurn = true;
            }
        }

        public override bool AltFunctionUse(Item item, Player player) => item.hammer != 0;

        public override bool CanUseItem(Item item, Player player)
        {
            if (item.hammer != 0 && player.altFunctionUse == 2)
            {

            }

            return true;
        }
    }
}
