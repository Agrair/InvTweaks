using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace InvTweaks.Gui
{
    //https://raw.githubusercontent.com/JavidPack/ModdersToolkit/master/UIElements/NewUITextBox.cs
    internal class NewUITextBox : UITextPanel<string>
    {
        private readonly string hintText;
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

        public NewUITextBox(string hint) : base("", 1, false)
        {
            hintText = hint;
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
            if (text.Any(x => !numbers.Contains(x)))
                return;
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
            if (_cursor == 0)
                return;
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
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, hitbox, BackgroundColor);

            // Draw panel -- Panel draws odd when too small
            // base.DrawSelf(spriteBatch);

            if (focused)
            {
                Terraria.GameInput.PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                // This might work.....assuming chat isn't open
                WriteAll(Main.GetInputText(Text));

                if (JustPressed(Keys.Left))
                    CursorLeft();
                if (JustPressed(Keys.Right))
                    CursorRight();
                if (JustPressed(Keys.Back))
                    Backspace();

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
            DynamicSpriteFont spriteFont = IsLarge ? FontAssets.DeathText.Value : FontAssets.MouseText.Value;
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

            if (!focused)
                return;

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
}
