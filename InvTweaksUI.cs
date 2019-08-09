using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
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
            panel.Left.Set(500, 0);
            panel.Top.Set(258, 0);
            panel.Width.Set(180, 0);
            panel.Height.Set(70, 0);

            textBox = new NewUITextBox();
            textBox.SetPadding(0);
            textBox.Left.Set(20, 0);
            textBox.Top.Set(10, 0);
            textBox.Width.Set(140, 0);
            textBox.Height.Set(20, 0);
            panel.Append(textBox);

            Append(panel);
        }

        public void SetStack(int stack)
        {
            textBox.SetText(stack.ToString(), 1, false);
        }

        public int GetStack()
        {
            return Convert.ToInt32(textBox.Text);
        }
    }

    //https://raw.githubusercontent.com/JavidPack/ModdersToolkit/master/UIElements/NewUITextBox.cs
    internal class NewUITextBox : UITextPanel<string>
    {
        private const string hintText = "Enter stack";
        private static readonly char[] numbers = new char[]
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '7',
            '8',
            '9'
        };

        private const int _maxLength = 10;

        internal bool focused = false;
        private int _cursor;
        private int _frameCount;

        public NewUITextBox() : base("", 1, false)
        {
            //SetPadding(4);
        }

        public override void Click(UIMouseEvent evt)
        {
            Focus();
            base.Click(evt);
        }

        public void Unfocus()
        {
            if (focused)
            {
                focused = false;
                Main.blockInput = false;
            }
        }

        public void Focus()
        {
            if (!focused)
            {
                Main.clrInput();
                focused = true;
                Main.blockInput = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 MousePosition = new Vector2(Main.mouseX, Main.mouseY);
            if (!ContainsPoint(MousePosition) && Main.mouseLeft)
            {
                // TODO, figure out how to refocus without triggering unfocus while clicking enable button.
                Unfocus();
            }
            base.Update(gameTime);
        }

        /*
        public void Write(string text)
        {
            if (text.Any(x => !numbers.Contains(x))) return;
            SetText(Text.Insert(_cursor, text));
            _cursor += text.Length;
            _cursor = Math.Min(Text.Length, _cursor);
            Recalculate();

            OnTextChanged?.Invoke();
        }
        */

        public void WriteAll(string text)
        {
            if (text.Any(x => !numbers.Contains(x))) return;
            bool changed = text != Text;
            SetText(text);
            _cursor = text.Length;
            //_cursor = Math.Min(Text.Length, _cursor);
            Recalculate();
        }

        public override void SetText(string text, float textScale, bool large)
        {
            if (text.ToString().Length > _maxLength)
            {
                text = text.ToString().Substring(0, _maxLength);
            }
            base.SetText(text, textScale, large);

            //this.MinWidth.Set(120, 0f);

            _cursor = Math.Min(Text.Length, _cursor);
        }

        public void Backspace()
        {
            if (_cursor == 0) return;
            SetText(Text.Substring(0, Text.Length));
            Recalculate();
        }

        public void CursorLeft()
        {
            if (_cursor == 0)
            {
                return;
            }
            _cursor--;
        }

        public void CursorRight()
        {
            if (_cursor < Text.Length)
            {
                _cursor++;
            }
        }

        static bool JustPressed(Keys key)
        {
            return Main.inputText.IsKeyDown(key) && !Main.oldInputText.IsKeyDown(key);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Rectangle hitbox = GetDimensions().ToRectangle();
            //hitbox.Inflate(4, 4);
            Main.spriteBatch.Draw(Main.magicPixel, hitbox, Color.White);

            // Draw panel -- Panel draws odd when too small
            // base.DrawSelf(spriteBatch);

            if (focused)
            {
                Terraria.GameInput.PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                // This might work.....assuming chat isn't open
                WriteAll(Main.GetInputText(Text));

                if (JustPressed(Keys.Left)) CursorLeft();
                if (JustPressed(Keys.Right)) CursorRight();
                if (JustPressed(Keys.Back)) Backspace();

            }
            CalculatedStyle innerDimensions2 = GetInnerDimensions();
            Vector2 pos2 = innerDimensions2.Position();
            if (IsLarge)
            {
                pos2.Y -= 10f * TextScale * TextScale;
            }
            else
            {
                pos2.Y -= 2f * TextScale;
            }
            //pos2.X += (innerDimensions2.Width - TextSize.X) * 0.5f;
            if (IsLarge)
            {
                Utils.DrawBorderStringBig(spriteBatch, Text, pos2, TextColor, TextScale, 0f, 0f, -1);
                return;
            }
            Utils.DrawBorderString(spriteBatch, Text, pos2, TextColor, TextScale, 0f, 0f, -1);

            _frameCount++;

            CalculatedStyle innerDimensions = GetInnerDimensions();
            Vector2 pos = innerDimensions.Position();
            DynamicSpriteFont spriteFont = IsLarge ? Main.fontDeathText : Main.fontMouseText;
            Vector2 vector = new Vector2(spriteFont.MeasureString(Text.Substring(0, _cursor)).X, IsLarge ? 32f : 16f) * TextScale;
            if (IsLarge)
            {
                pos.Y -= 8f * TextScale;
            }
            else
            {
                pos.Y -= 1f * TextScale;
            }
            if (Text.Length == 0)
            {
                //Vector2 hintTextSize = new Vector2(spriteFont.MeasureString(hintText.ToString()).X, IsLarge ? 32f : 16f) * TextScale;
                pos.X += 5;//(hintTextSize.X);
                if (base.IsLarge)
                {
                    Utils.DrawBorderStringBig(spriteBatch, hintText, pos, Color.Gray, TextScale, 0f, 0f, -1);
                    return;
                }
                Utils.DrawBorderString(spriteBatch, hintText, pos, Color.Gray, TextScale, 0f, 0f, -1);
                pos.X -= 5;
                //pos.X -= (innerDimensions.Width - hintTextSize.X) * 0.5f;
            }

            if (!focused) return;

            pos.X += /*(innerDimensions.Width - base.TextSize.X) * 0.5f*/ +vector.X - (IsLarge ? 8f : 4f) * TextScale + 6f;
            if ((_frameCount %= 40) > 20)
            {
                return;
            }
            if (IsLarge)
            {
                Utils.DrawBorderStringBig(spriteBatch, "|", pos, TextColor, TextScale, 0f, 0f, -1);
                return;
            }
            Utils.DrawBorderString(spriteBatch, "|", pos, TextColor, TextScale, 0f, 0f, -1);
        }
    }

    //https://raw.githubusercontent.com/tModLoader/tModLoader/master/ExampleMod/UI/DragableUIPanel.cs
    internal class DragableUIPanel : UIPanel
    {
        // Stores the offset from the top left of the UIPanel while dragging.
        private Vector2 offset;
        public bool dragging;

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            DragStart(evt);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            DragEnd(evt);
        }

        private void DragStart(UIMouseEvent evt)
        {
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            Left.Set(end.X - offset.X, 0f);
            Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.

            // Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same.
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            // Here we check if the DragableUIPanel is outside the Parent UIElement rectangle. 
            // (In our example, the parent would be ExampleUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
            // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution.
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                // Recalculate forces the UI system to do the positioning math again.
                Recalculate();
            }
        }
    }
}
