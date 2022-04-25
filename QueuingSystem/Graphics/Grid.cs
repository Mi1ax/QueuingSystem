using System;
using System.Drawing;
using System.Numerics;
using QueuingSystem.Graphics.Buildings;
using Rectangle = Core.Drawing.Shapes.Rectangle;

namespace QueuingSystem.Graphics
{
    public class Grid
    {
        private readonly IBuildable?[,] _buildings;
        private readonly Rectangle[,] _rectangles;
        private SizeF _cellSize;
        
        public Vector2 Size { get; }

        public Grid(Vector2 size, SizeF? cellSize = null)
        {
            cellSize ??= new SizeF(36, 36);
            Size = size;
            _cellSize = cellSize.Value;
            _buildings = new IBuildable[(int)size.X, (int)size.Y];
            _rectangles = new Rectangle[(int)size.X, (int)size.Y];
            for (var x = 0; x < _rectangles.GetLength(0); x++)
            {
                for (var y = 0; y < _rectangles.GetLength(1); y++)
                {
                    var rectangle = new Rectangle
                    {
                        FillColor = Color.Transparent,
                        BorderColor = Color.FromArgb(128, Color.Red),
                        Size = _cellSize,
                        Position = new Vector2(
                            0 + _cellSize.Width * x, 
                            0 + _cellSize.Height * y
                        )
                    };
                    _rectangles[x, y] = rectangle;
                }
            }
        }

        public void Build(IBuildable buildable, Vector2 gridPosition)
        {
            buildable.PositionOnGrid = gridPosition;
            if (_buildings[(int) gridPosition.X, (int) gridPosition.Y] == null)
                _buildings[(int) gridPosition.X, (int) gridPosition.Y] = buildable;
            else throw new Exception($"There is a building in {gridPosition}");
            SetPosition();
        }

        private void SetPosition()
        {
            for (var x = 0; x < _buildings.GetLength(0); x++)
            {
                for (var y = 0; y < _buildings.GetLength(1); y++)
                {
                    var building = _buildings[x, y];
                    if (building == null) continue;
                    building.PositionOnScreen = new Vector2(
                        0 + _cellSize.Width * x,
                        0 + _cellSize.Height * y
                        );
                    
                    if (building is Conveyor conveyor)
                        ChangeStateByNeighbour(conveyor, x, y);
                }
            }
        }

        // TODO: Simplify this stupid function
        private void ChangeStateByNeighbour(Conveyor conveyor, int X, int Y) 
        {
            var west = false;
            var north = false;
            var east = false;
            var south = false;

            if (Y - 1 >= 0 && _buildings[X, Y - 1] is Conveyor)
                north = true;
            if (Y + 1 < _buildings.GetLength(1) && _buildings[X, Y + 1] is Conveyor)
                south = true;
            if (X - 1 >= 0 && _buildings[X - 1, Y] is Conveyor)
                west = true;
            if (X + 1 < _buildings.GetLength(0) && _buildings[X + 1, Y] is Conveyor)
                east = true;

            if (west && north) conveyor.Connection = ConveyorConnection.NorthWest;
            if (north && east) conveyor.Connection = ConveyorConnection.NorthEast;
            if (west && south) conveyor.Connection = ConveyorConnection.SouthWest;
            if (south && east) conveyor.Connection = ConveyorConnection.SouthEast;
            if (west && east) conveyor.Connection = ConveyorConnection.WestEast;
            if (north && south) conveyor.Connection = ConveyorConnection.NorthSouth;

            if (west && !north && !east && !south)
                conveyor.Connection = ConveyorConnection.WestNone;
            if (north && !west && !east && !south)
                conveyor.Connection = ConveyorConnection.NorthNone;
            if (east && !north && !west && !south)
                conveyor.Connection = ConveyorConnection.EastNone;
            if (south && !north && !east && !west)
                conveyor.Connection = ConveyorConnection.SouthNone;
        }
        
        public void Update(float deltaTime)
        {
            foreach (var buildable in _buildings)
                buildable?.Update(deltaTime);
        }

        public void Draw()
        {
            foreach (var rectangle in _rectangles)
                rectangle.Draw();
            foreach (var buildable in _buildings)
                buildable?.Draw();
        }
    }
}