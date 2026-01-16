using System.Drawing;

namespace WindowsFormsApp1
{
    public abstract class GamePiece
    {
        public Color Color { get; protected set; }

        // poziție pe traseu (0..51), >51 = home lane
        public int Position { get; protected set; }

        public bool IsInStart { get; protected set; }
        public bool IsInHome { get; protected set; }

        protected GamePiece(Color color)
        {
            Color = color;
            Reset();
        }

        // Obligatoriu pentru piese derivate
        public abstract void Move(int steps);

        // Reset general (start / mâncat)
        public virtual void Reset()
        {
            Position = 0;
            IsInStart = true;
            IsInHome = false;
        }
    }
}
