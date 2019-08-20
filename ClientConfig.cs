using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace InvTweaks
{
    public class ClientConfig : ModConfig
    {
        [DefaultValue(true)]
        [Label("Cursor Refill")]
        [Tooltip("You don't need to worry about running out torches in your hand as long as there's more in your inventory.")]
        public bool CursorFill { get; set; }

        [DefaultValue(true)]
        [Label("Extra Accessory Slot")]
        [Tooltip("This is a slot by the hotbar that allows you to switch accessories/armour during battle.\nItem does not function whilst in slot.")]
        public bool HelmetSlot { get; set; }

        [DefaultValue(true)]
        [Label("Fast Shop Buy")]
        [Tooltip("Shift clicking an item in the shop will open a UI so you can select how much of the item \nyou want to buy next time you click.")]
        public bool ShopClick { get; set; }

        [DefaultValue(true)]
        [Label("Devour Life Crystals")]
        [Tooltip("Super fast life crystal, life fruit, and mana star usage.")]
        public bool LifeCrystalDevour { get; set; }

        [DefaultValue(true)]
        [Label("Sapling Placer")]
        [Tooltip("Chop down a tree and hold down the cursor to place a tree.")]
        public bool SaplingPlacer { get; set; }

        [DefaultValue(false)]
        [Label("Persistent Buffs")]
        [Tooltip("Don't worry about your buffs running out if you have more in your inventory.")]
        public bool AutoBuff { get; set; }

        [DefaultValue(false)]
        [Label("Everlasting Life")]
        [Tooltip("When you hit the Everlasting Life Threshold, a healing potion will be consumed.")]
        public bool AutoHeal { get; set; }

        [DefaultValue(30)]
        [Range(0, 100f)]
        [Label("Everlasting Life Threshold")]
        [Tooltip("Percentage between 1 and 100.")]
        public float AutoHealThreshold { get; set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded()
        {
            InvTweaks.clientConfig = this;
        }
    }
}
