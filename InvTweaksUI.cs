using InvTweaks.Gui;
using System;
using Terraria.UI;

namespace InvTweaks
{
    internal class InvTweaksUI : UIState
    {
        public static bool visible = false;
        private DragableUIPanel panel;
        private NewUITextBox textBox;

        public override void OnInitialize()
        {
            panel = new DragableUIPanel();
            panel.SetPadding(0);
            panel.HAlign = panel.VAlign = .5f;
            panel.Width.Set(180, 0);
            panel.Height.Set(70, 0);

            textBox = new NewUITextBox("Enter stack");
            textBox.SetPadding(0);
            textBox.Left.Set(20, 0);
            textBox.Top.Set(10, 0);
            textBox.Width.Set(140, 0);
            textBox.Height.Set(20, 0);
            panel.Append(textBox);

            Append(panel);
        }

        public int ShopStack
        {
            get => Convert.ToInt32(textBox.Text);
            set => textBox.SetText(value.ToString(), 1, false);
        }
    }
}
