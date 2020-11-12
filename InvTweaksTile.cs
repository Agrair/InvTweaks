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
    }
}
