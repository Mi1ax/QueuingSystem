using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace QueuingSystem.Graphics.Buildings
{
    public class ConveyorBelt : IBuildable
    {
        private readonly Dictionary<Direction, Conveyor> _conveyors;
        private readonly Grid _grid;
        private int _conveyorCount;
        private Direction _movingDirection;

        private Slot _inputSlot;
        private Slot _outputSlot;

        public SizeF Size { get; set; }
        public Vector2 PositionOnGrid { get; set; }
        public Vector2 PositionOnScreen { get; set; }

        public ConveyorBelt(Grid grid, Direction movingDirection, Vector2? gridPosition = null)
        {
            _grid = grid;
            _conveyorCount = 0;
            _movingDirection = movingDirection;
            
            _conveyors = new Dictionary<Direction, Conveyor>
            {
                {Direction.None, new Conveyor()}
            };
            _grid.Build(_conveyors.First().Value, gridPosition ?? Vector2.Zero);
            _inputSlot = new Slot();
            _outputSlot = new Slot();
        }

        public void AddConveyor(Direction direction, Conveyor conveyor)
        {
            var last = _conveyors.Last().Value;
            var position = new Vector2(
                last.PositionOnGrid.X + direction.X,
                last.PositionOnGrid.Y + direction.Y
            );
            _conveyors.Add(direction, conveyor);
            _grid.Build(conveyor, position);
        }
        
        public void Update(float deltaTime)
        {
            
        }

        public void Draw()
        {
            foreach (var (_, conveyor) in _conveyors)
            {
                conveyor.Draw();
            }
        }
    }
}