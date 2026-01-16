using System.Drawing;

namespace WindowsFormsApp1
{
    public class Pawn : GamePiece
    {
        public int Index { get; }
        public Player Owner { get; set; }

        public Pawn(int index, Color color) : base(color)
        {
            Index = index;
        }

        public override void Move(int steps)
        {
            if (IsInStart || IsInHome)
                return;

            Position += steps;
        }

        public void ExitStart()
        {
            if (!IsInStart) return;

            IsInStart = false;
            Position = 0;
        }
    }
}
