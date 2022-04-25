using QueuingSystem.Graphics.Buildings;

namespace QueuingSystem.Graphics
{
    public class Slot
    {
        public Item? Item
        {
            get;
            set;
        }
        
        public Slot()
        {
            Item = null;
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}