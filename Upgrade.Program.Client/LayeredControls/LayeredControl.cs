using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public class LayeredControl
    {
        #region Events
        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDoubleClick;
        public event MouseEventHandler MouseDown;
        public event EventHandler MouseEnter;
        public event EventHandler MouseHover;
        public event EventHandler MouseLeave;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseWheel;
        protected virtual void OnMouseClick(MouseEventArgs e) { this.MouseClick?.Invoke(this, e); }
        protected virtual void OnMouseDoubleClick(MouseEventArgs e) { this.MouseDoubleClick?.Invoke(this, e); }
        protected virtual void OnMouseDown(MouseEventArgs e) { this.MouseDown?.Invoke(this, e); }
        protected virtual void OnMouseEnter(EventArgs e) { this.MouseEnter?.Invoke(this, e); }
        protected virtual void OnMouseHover(EventArgs e) { this.MouseHover?.Invoke(this, e); }
        protected virtual void OnMouseLeave(EventArgs e) { this.MouseLeave?.Invoke(this, e); }
        protected virtual void OnMouseMove(MouseEventArgs e) { this.MouseMove?.Invoke(this, e); }
        protected virtual void OnMouseUp(MouseEventArgs e) { this.MouseUp?.Invoke(this, e); }
        protected virtual void OnMouseWheel(MouseEventArgs e) { this.MouseWheel?.Invoke(this, e); }
        #endregion
        #region Bounds
        private int x = 0;
        private int y = 0;
        private int width = 100;
        private int height = 100;
        public int X { get => x; set { x = value; } }
        public int Y { get => y; set { y = value; } }
        public int Width { get => width; set { width = value; } }
        public int Height { get => height; set { height = value; } }
        public Point Location => new Point(this.X, this.Y);
        public Size Size => new Size(this.Width, this.Height);
        public Rectangle Bounds => new Rectangle(this.Location, this.Size);
        public Rectangle ClientBounds => new Rectangle(Point.Empty, this.Size);
        #endregion
        #region Parent
        private Control controlParent = null;
        public Control ControlParent
        {
            get => controlParent;
            set
            {
                controlParent = value;
                if (controlParent != null)
                {
                    controlParent.MouseClick += (s, e) => this.MouseHandler.OnMouseClick(e);
                    controlParent.MouseDoubleClick += (s, e) => this.MouseHandler.OnMouseDoubleClick(e);
                    controlParent.MouseDown += (s, e) => this.MouseHandler.OnMouseDown(e);
                    controlParent.MouseMove += (s, e) => this.MouseHandler.OnMouseMove(e);
                    controlParent.MouseUp += (s, e) => this.MouseHandler.OnMouseUp(e);
                    controlParent.MouseLeave += (s, e) => this.MouseHandler.OnMouseLeave(e);
                    controlParent.MouseWheel += (s, e) => this.MouseHandler.OnMouseWheel(e);

                    controlParent.Paint += (s, e) => this.PaintHandler.OnPaint(e);
                }
            }
        }
        #endregion
        #region  Handler
        public DUIControlMouseHandler MouseHandler { get; }
        public DUIControlPaintHandler PaintHandler { get; }
        #endregion
        #region Properties
        private Color backColor = SystemColors.Control;
        public Color BackColor
        {
            get => backColor;
            set
            {
                backColor = value;
                this.Invalidate();
            }
        }
        #endregion
        public LayeredControl()
        {
            this.MouseHandler = new DUIControlMouseHandler() { Owner = this };
            this.PaintHandler = new DUIControlPaintHandler() { Owner = this };
        }
        public void Invalidate()
        {
            this.ControlParent?.Invalidate();
        }
        protected virtual void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(brush, e.ClipRectangle);
            }
        }
        public class DUIControlMouseHandler
        {
            private bool isMouseEnter = false;
            private bool isMouseDown = false;
            private Point mouseMoveLocation = Point.Empty;
            internal LayeredControl Owner { get; set; }
            public void OnMouseClick(MouseEventArgs e)
            {
                if (this.Owner.Bounds.Contains(e.Location))
                {
                    this.Owner.OnMouseClick(new MouseEventArgs(e.Button, e.Clicks, this.PointToOwner(e.Location).X, this.PointToOwner(e.Location).Y, e.Delta));
                }
            }
            public void OnMouseDoubleClick(MouseEventArgs e)
            {
                if (this.Owner.Bounds.Contains(e.Location))
                {
                    this.Owner.OnMouseDoubleClick(new MouseEventArgs(e.Button, e.Clicks, this.PointToOwner(e.Location).X, this.PointToOwner(e.Location).Y, e.Delta));
                }
            }
            public void OnMouseDown(MouseEventArgs e)
            {
                if (this.Owner.Bounds.Contains(e.Location))
                {
                    this.isMouseDown = true;
                    this.Owner.OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, this.PointToOwner(e.Location).X, this.PointToOwner(e.Location).Y, e.Delta));
                }
            }
            public void OnMouseHover(EventArgs e)
            {
                if (this.Owner.Bounds.Contains(this.mouseMoveLocation))
                {
                    this.Owner.OnMouseHover(EventArgs.Empty);
                }
            }
            public void OnMouseMove(MouseEventArgs e)
            {
                this.mouseMoveLocation = e.Location;
                #region OnMouseEnter OnMouseLeave
                if (this.Owner.Bounds.Contains(e.Location) || this.isMouseDown)
                {
                    if (!this.isMouseEnter)
                    {
                        this.isMouseEnter = true;
                        this.Owner.OnMouseEnter(EventArgs.Empty);
                    }
                }
                else
                {
                    if (this.isMouseEnter)
                    {
                        this.isMouseEnter = false;
                        this.Owner.OnMouseLeave(EventArgs.Empty);
                    }
                }
                #endregion
                this.Owner.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, this.PointToOwner(e.Location).X, this.PointToOwner(e.Location).Y, e.Delta));
            }
            public void OnMouseUp(MouseEventArgs e)
            {
                this.Owner.OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, this.PointToOwner(e.Location).X, this.PointToOwner(e.Location).Y, e.Delta));
                this.isMouseDown = false;
            }
            public void OnMouseWheel(MouseEventArgs e)
            {
                if (this.Owner.Bounds.Contains(e.Location))
                {
                    this.Owner.OnMouseWheel(new MouseEventArgs(e.Button, e.Clicks, this.PointToOwner(e.Location).X, this.PointToOwner(e.Location).Y, e.Delta));
                }
            }
            public void OnMouseLeave(EventArgs e)
            {
                this.isMouseEnter = false;
                this.Owner.OnMouseLeave(EventArgs.Empty);
            }
            private Point PointToOwner(Point point)
            {
                return new Point(point.X - this.Owner.X, point.Y - this.Owner.Y);
            }
        }
        public class DUIControlPaintHandler
        {
            internal LayeredControl Owner { get; set; }
            public void OnPaint(PaintEventArgs e)
            {
                e.Graphics.TranslateTransform(this.Owner.X, this.Owner.Y);
                e.Graphics.SetClip(this.Owner.ClientBounds);
                this.Owner.OnPaint(new PaintEventArgs(e.Graphics, this.Owner.ClientBounds));
                e.Graphics.TranslateTransform(-this.Owner.X, -this.Owner.Y);
            }
        }
    }
}
