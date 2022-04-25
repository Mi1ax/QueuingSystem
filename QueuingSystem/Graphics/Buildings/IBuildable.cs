using System.Drawing;
using System.Numerics;

namespace QueuingSystem.Graphics.Buildings
{
    public interface IBuildable
    {
        public SizeF Size { get; set; }
        public Vector2 PositionOnGrid { get; set; }
        public Vector2 PositionOnScreen { get; set; }

        public void Update(float deltaTime);
        public void Draw();
    }
}