using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InvTweaks
{
    public class InvTweaksTile : GlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            if (ClientConfig.Instance.SaplingPlacer)
            {
                int underneath = Main.tile[i, j + 1].type;
                if (type == TileID.Trees
                    && (TileID.Sets.Conversion.Grass[underneath]
                    || underneath == TileID.Sand
                    || underneath == TileID.SnowBlock))
                {
                    var mouse = Main.MouseWorld.ToTileCoordinates16();
                    //Main.NewText("i: " + i);
                    //Main.NewText("j: " + j);
                    //Main.NewText("X: " + mouse.X);
                    //Main.NewText("Y: " + mouse.Y);
                    if (mouse.X == i && mouse.Y == j)
                    {
                        Main.LocalPlayer.GetModPlayer<InvTweaksPlayer>().PlaceTree();
                    }
                }
            }
            return true;
        }

        public override void RightClick(int i, int j, int type)
        {
            Main.NewText("HI");
            var player = Main.LocalPlayer;
            var item = player.HeldItem;
            if (item.hammer != 0)
            {
/*                SlopeHammerUI.visible = true;
                var state = new SlopeHammerUI();
                state.Activate();
                state.Tile = (i, j);
                InvTweaks.instance.uiInterface.SetState(state);*/

                Main.NewText(Main.tile[i, j].slope());
            }
        }
    }
}
