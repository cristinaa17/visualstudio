using System.Drawing;

namespace WindowsFormsApp1

{
    public abstract class GamePiece
    {
        public int Position { get; protected set; }   // 0 = start, 1..52 = traseu
        public Color Color { get; protected set; }

        public bool IsInStart { get; protected set; }
        public bool IsInHome { get; protected set; }

        protected GamePiece(Color color)
        {
            Color = color;
            Reset();
        }

        public abstract void Move(int steps);

        public virtual void Reset()
        {
            Position = 0;
            IsInStart = true;
            IsInHome = false;
        }
    }
}
