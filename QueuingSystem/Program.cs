using System.Drawing;
using System.Numerics;
using Core;
using QueuingSystem.Graphics;
using QueuingSystem.Graphics.Buildings;

namespace QueuingSystem
{
    public static class Program
    {
        private class TempApp : Window
        {
            private readonly Grid _grid;
            private ConveyorBelt _belt;
            
            public TempApp()
            {
                Settings.Title = "Queuing System";
                Settings.Color = Color.Black;
                Settings.VWidth = Settings.Width;
                Settings.VHeight = Settings.Height;

                _grid = new Grid(new Vector2(8));
                _belt = new ConveyorBelt(_grid, Direction.East);
                _belt.AddConveyor(Direction.East, new Conveyor());
                _belt.AddConveyor(Direction.East, new Conveyor());
                _belt.AddConveyor(Direction.East, new Conveyor());
            }
            
            protected override void Update(float deltaTime)
            {
                _grid.Update(deltaTime);
            }

            protected override void Draw()
            {
                _grid.Draw();
            }
        }
        
        private static void Main(string[] args)
        {
            var app = new TempApp();
            app.Run();
            /*var app = new Application();
            app.Run();*/
        }
    }
}