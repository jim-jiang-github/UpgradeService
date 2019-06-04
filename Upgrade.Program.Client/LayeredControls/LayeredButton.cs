using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public class LayeredButton : LayeredControl
    {
        private bool isMouseEnter = false;
        private bool isMouseDown = false;
        private Image texture = null;
        public Image Texture
        {
            get => this.texture;
            set
            {
                this.texture = value;
                this.Width = this.Texture.Width;
                this.Height = this.Texture.Height / 3;
                this.Invalidate();
            }
        }

        public LayeredButton()
        {
            this.BackColor = Color.Transparent;
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.isMouseEnter = true;
            this.Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.isMouseEnter = false;
            this.Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.isMouseDown = true;
            this.Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.isMouseDown = false;
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Texture == null) { return; }
            if (this.isMouseDown)
            {
                e.Graphics.DrawImage(this.Texture
                    , new Rectangle(0, 0, this.Texture.Width, this.Texture.Height / 3)
                    , new Rectangle(0, this.Texture.Height / 3 * 2, this.Texture.Width, this.Texture.Height / 3)
                    , GraphicsUnit.Pixel);
            }
            else
            {
                if (this.isMouseEnter)
                {
                    e.Graphics.DrawImage(this.Texture
                        , new Rectangle(0, 0, this.Texture.Width, this.Texture.Height / 3)
                        , new Rectangle(0, this.Texture.Height / 3, this.Texture.Width, this.Texture.Height / 3)
                        , GraphicsUnit.Pixel);
                }
                else
                {
                    e.Graphics.DrawImage(this.Texture
                        , new Rectangle(0, 0, this.Texture.Width, this.Texture.Height / 3)
                        , new Rectangle(0, 0, this.Texture.Width, this.Texture.Height / 3)
                        , GraphicsUnit.Pixel);
                }
            }
        }
    }
}
