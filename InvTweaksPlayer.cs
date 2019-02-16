namespace InvTweaks
{
    /*
    public class InvTweaksPlayer : ModPlayer
    {
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            **************************
            if (!inventory[slot].IsAir
                && player.CanBuyItem(inventory[slot].GetStoreValue())
                && context == ItemSlot.Context.ShopItem)
            {
                var num = Utils.CoinsCount(out var flag, player.bank.item);
                var num2 = Utils.CoinsCount(out flag, player.bank2.item);
                var num3 = Utils.CoinsCount(out flag, player.bank3.item);
                var num4 = Utils.CoinsCount(out flag, player.inventory);
                var totalSavings = Utils.CoinsCombineStacks(out flag, num, num2, num3, num4);
                var canPaySavings = totalSavings -
                    (totalSavings % inventory[slot].GetStoreValue());
                bool flag2 = false;
                for (int i = 0; i < canPaySavings / inventory[slot].GetStoreValue()
                    && i <= inventory[slot].maxStack
                    && BuyItem(inventory[slot].GetStoreValue()); i++)
                {
                    flag2 = true;
                    if (Main.mouseItem.type == 0) Main.mouseItem.SetDefaults(inventory[slot].type);
                    Main.mouseItem.stack++;
                }
                return flag2;
            }
            ***************
            return false;
        }

        public bool BuyItem(int price, int customCurrency = -1)
        {
            var p = Main.player[Main.myPlayer];
            if (customCurrency != -1)
            {
                return CustomCurrencyManager.BuyItem(p, price, customCurrency);
            }
            long num = Utils.CoinsCount(out bool flag, p.inventory, new int[]
                {
                    58,
                    57,
                    56,
                    55,
                    54
                });
            long num2 = Utils.CoinsCount(out flag, p.bank.item, new int[0]);
            long num3 = Utils.CoinsCount(out flag, p.bank2.item, new int[0]);
            long num4 = Utils.CoinsCount(out flag, p.bank3.item, new int[0]);
            long num5 = Utils.CoinsCombineStacks(out flag, new long[]
                {
                    num,
                    num2,
                    num3,
                    num4
                });
            if (num5 < price)
            {
                return false;
            }
            List<Item[]> list = new List<Item[]>();
            Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
            List<Point> list2 = new List<Point>();
            List<Point> list3 = new List<Point>();
            List<Point> list4 = new List<Point>();
            List<Point> list5 = new List<Point>();
            List<Point> list6 = new List<Point>();
            list.Add(p.inventory);
            list.Add(p.bank.item);
            list.Add(p.bank2.item);
            list.Add(p.bank3.item);
            for (int i = 0; i < list.Count; i++)
            {
                dictionary[i] = new List<int>();
            }
            dictionary[0] = new List<int>
            {
                58,
                57,
                56,
                55,
                54
            };
            for (int j = 0; j < list.Count; j++)
            {
                for (int k = 0; k < list[j].Length; k++)
                {
                    if (!dictionary[j].Contains(k) && list[j][k].type >= 71 && list[j][k].type <= 74)
                    {
                        list3.Add(new Point(j, k));
                    }
                }
            }
            int num6 = 0;
            for (int l = list[num6].Length - 1; l >= 0; l--)
            {
                if (!dictionary[num6].Contains(l) && (list[num6][l].type == 0 || list[num6][l].stack == 0))
                {
                    list2.Add(new Point(num6, l));
                }
            }
            num6 = 1;
            for (int m = list[num6].Length - 1; m >= 0; m--)
            {
                if (!dictionary[num6].Contains(m) && (list[num6][m].type == 0 || list[num6][m].stack == 0))
                {
                    list4.Add(new Point(num6, m));
                }
            }
            num6 = 2;
            for (int n = list[num6].Length - 1; n >= 0; n--)
            {
                if (!dictionary[num6].Contains(n) && (list[num6][n].type == 0 || list[num6][n].stack == 0))
                {
                    list5.Add(new Point(num6, n));
                }
            }
            num6 = 3;
            for (int num7 = list[num6].Length - 1; num7 >= 0; num7--)
            {
                if (!dictionary[num6].Contains(num7) && (list[num6][num7].type == 0 || list[num6][num7].stack == 0))
                {
                    list6.Add(new Point(num6, num7));
                }
            }
            bool flag2 = InvTweaksPlayer.TryPurchasing(price, list, list3, list2, list4, list5, list6);
            return !flag2;
        }

        private static bool TryPurchasing(int price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3)
        {
            long num = price;
            Dictionary<Point, Item> dictionary = new Dictionary<Point, Item>();
            bool result = false;
            while (num > 0L)
            {
                long num2 = 1000000L;
                for (int i = 0; i < 4; i++)
                {
                    if (num >= num2)
                    {
                        for (int i1 = 0; i1 < slotCoins.Count; i1++)
                        {
                            Point current = slotCoins[i1];
                            if (inv[current.X][current.Y].type == 74 - i)
                            {
                                long num3 = num / num2;
                                dictionary[current] = inv[current.X][current.Y].Clone();
                                if (num3 < inv[current.X][current.Y].stack)
                                {
                                    inv[current.X][current.Y].stack -= (int)num3;
                                }
                                else
                                {
                                    inv[current.X][current.Y].SetDefaults(0, false);
                                    slotsEmpty.Add(current);
                                }
                                num -= num2 * (dictionary[current].stack - inv[current.X][current.Y].stack);
                            }
                        }
                    }
                    num2 /= 100L;
                }
                if (num > 0L)
                {
                    if (slotsEmpty.Count <= 0)
                    {
                        foreach (KeyValuePair<Point, Item> current2 in dictionary)
                        {
                            inv[current2.Key.X][current2.Key.Y] = current2.Value.Clone();
                        }
                        result = true;
                        break;
                    }
                    slotsEmpty.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
                    Point item = new Point(-1, -1);
                    for (int j = 0; j < inv.Count; j++)
                    {
                        num2 = 10000L;
                        for (int k = 0; k < 3; k++)
                        {
                            if (num >= num2)
                            {
                                for (int i = 0; i < slotCoins.Count; i++)
                                {
                                    Point current3 = slotCoins[i];
                                    if (current3.X == j && inv[current3.X][current3.Y].type == 74 - k && inv[current3.X][current3.Y].stack >= 1)
                                    {
                                        List<Point> list = slotsEmpty;
                                        if (j == 1 && slotEmptyBank.Count > 0)
                                        {
                                            list = slotEmptyBank;
                                        }
                                        if (j == 2 && slotEmptyBank2.Count > 0)
                                        {
                                            list = slotEmptyBank2;
                                        }
                                        if (--inv[current3.X][current3.Y].stack <= 0)
                                        {
                                            inv[current3.X][current3.Y].SetDefaults(0, false);
                                            list.Add(current3);
                                        }
                                        dictionary[list[0]] = inv[list[0].X][list[0].Y].Clone();
                                        inv[list[0].X][list[0].Y].SetDefaults(73 - k, false);
                                        inv[list[0].X][list[0].Y].stack = 100;
                                        item = list[0];
                                        list.RemoveAt(0);
                                        break;
                                    }
                                }
                            }
                            if (item.X != -1 || item.Y != -1)
                            {
                                break;
                            }
                            num2 /= 100L;
                        }
                        for (int l = 0; l < 2; l++)
                        {
                            if (item.X == -1 && item.Y == -1)
                            {
                                for (int i = 0; i < slotCoins.Count; i++)
                                {
                                    Point current4 = slotCoins[i];
                                    if (current4.X == j && inv[current4.X][current4.Y].type == 73 + l && inv[current4.X][current4.Y].stack >= 1)
                                    {
                                        List<Point> list2 = slotsEmpty;
                                        if (j == 1 && slotEmptyBank.Count > 0)
                                        {
                                            list2 = slotEmptyBank;
                                        }
                                        if (j == 2 && slotEmptyBank2.Count > 0)
                                        {
                                            list2 = slotEmptyBank2;
                                        }
                                        if (j == 3 && slotEmptyBank3.Count > 0)
                                        {
                                            list2 = slotEmptyBank3;
                                        }
                                        if (--inv[current4.X][current4.Y].stack <= 0)
                                        {
                                            inv[current4.X][current4.Y].SetDefaults(0, false);
                                            list2.Add(current4);
                                        }
                                        dictionary[list2[0]] = inv[list2[0].X][list2[0].Y].Clone();
                                        inv[list2[0].X][list2[0].Y].SetDefaults(72 + l, false);
                                        inv[list2[0].X][list2[0].Y].stack = 100;
                                        item = list2[0];
                                        list2.RemoveAt(0);
                                        break;
                                    }
                                }
                            }
                        }
                        if (item.X != -1 && item.Y != -1)
                        {
                            slotCoins.Add(item);
                            break;
                        }
                    }
                    slotsEmpty.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
                    slotEmptyBank.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
                    slotEmptyBank2.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
                    slotEmptyBank3.Sort(new Comparison<Point>(DelegateMethods.CompareYReverse));
                }
            }
            return result;
        }
    }
    */
}
