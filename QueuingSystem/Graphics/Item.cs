using System.Drawing;
using System.Numerics;
using Rectangle = Core.Drawing.Shapes.Rectangle;

namespace QueuingSystem.Graphics
{
    public class Item
    {
        private readonly Rectangle _rectangle;

        public Vector2 Position
        {
            get => _rectangle.Position;
            set => _rectangle.Position = value;
        }
        
        public Item()
        {
            _rectangle = new Rectangle(0, 0, 12, 12)
            {
                FillColor = Color.OrangeRed
            };
        }

        public void Draw()
        {
            _rectangle.Draw();
        }
    }
}