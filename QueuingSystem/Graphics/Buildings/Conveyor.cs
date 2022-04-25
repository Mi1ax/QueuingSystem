using System;
using System.Drawing;
using System.Numerics;
using Rectangle = Core.Drawing.Shapes.Rectangle;

namespace QueuingSystem.Graphics.Buildings
{
    public enum ConveyorConnection
    {
        None, 
        WestEast, 
        NorthSouth, 
        SouthEast, 
        SouthWest, 
        NorthEast, 
        NorthWest,
        
        WestNone,
        NorthNone,
        EastNone,
        SouthNone
    }
    
    public class Conveyor : IBuildable
    {
        private readonly Rectangle _conveyorPiece;
        private Vector2 _position;
        
        private Rectangle?[] _rectangles;
        private ConveyorConnection _connection;

        public ConveyorConnection Connection
        {
            get => _connection;
            set 
            {
                _rectangles = new Rectangle?[9];
                _connection = value;
                switch (value) 
                {
                    case ConveyorConnection.None:
                        _rectangles[4] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.WestEast:
                        _rectangles[1] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        _rectangles[7] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.NorthSouth:
                        _rectangles[3] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        _rectangles[5] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.SouthEast:
                        _rectangles[5] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        _rectangles[7] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.SouthWest:
                        _rectangles[1] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        _rectangles[5] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.NorthEast:
                        _rectangles[3] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        _rectangles[7] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.NorthWest:
                        _rectangles[3] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        _rectangles[1] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.WestNone:
                        _rectangles[1] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.NorthNone:
                        _rectangles[3] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.EastNone:
                        _rectangles[7] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        break;
                    case ConveyorConnection.SouthNone:
                        _rectangles[5] = _conveyorPiece.Copy();
                        _rectangles[4] = _conveyorPiece.Copy();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, "Unhandled connection type");
                }
                for (var i = 0; i < _rectangles.Length; i++)
                {
                    if (_rectangles[i] == null) continue;
                    var y = i % 3;
                    var x = i / 3;
                    _rectangles[i]!.Position = new Vector2(
                        PositionOnScreen.X + _conveyorPiece.Size.Width * x,
                        PositionOnScreen.Y + _conveyorPiece.Size.Height * y
                        );
                }
            }
        }
        
        public SizeF Size
        {
            get => _conveyorPiece.Size; 
            set => _conveyorPiece.Size = value;
        }

        public Vector2 PositionOnGrid { get; set; }

        public Vector2 PositionOnScreen { 
            get => _position;
            set
            {
                _position = value;
                for (var i = 0; i < _rectangles.Length; i++)
                {
                    if (_rectangles[i] == null) continue;
                    var y = i % 3;
                    var x = i / 3;
                    _rectangles[i]!.Position = new Vector2(
                        PositionOnScreen.X + _conveyorPiece.Size.Width * x,
                        PositionOnScreen.Y + _conveyorPiece.Size.Height * y
                    );
                }
            }
        }

        public Conveyor(SizeF? size = null)
        {
            size ??= new SizeF(12, 12);
            _rectangles = new Rectangle?[9];
            _conveyorPiece = new Rectangle(Vector2.Zero, size.Value)
            {
                FillColor = Color.White,
                BorderColor = Color.Gray
            };
            Connection = ConveyorConnection.None;
        }

        public void Update(float deltaTime)
        {
            
        }

        public void Draw()
        {
            foreach (var rectangle in _rectangles)
                rectangle?.Draw();
        }
    }
}