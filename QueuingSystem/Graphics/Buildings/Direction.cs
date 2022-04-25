using System.Numerics;

namespace QueuingSystem.Graphics.Buildings
{
    public class Direction
    {
        private readonly Vector2 _vector;
        private Direction(Vector2 value) { _vector = value; }

        public int X => (int) _vector.X;
        public int Y => (int) _vector.Y;

        public static Direction None => new (Vector2.Zero);
        public static Direction West => new (new Vector2(-1, 0));
        public static Direction North => new (new Vector2(0, -1));
        public static Direction East => new (new Vector2(1, 0));
        public static Direction South => new (new Vector2(0, 1));
    }
}