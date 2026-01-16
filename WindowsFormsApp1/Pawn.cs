using System.Drawing;
using WindowsFormsApp1;

public class Pawn : GamePiece
{
    public int Index { get; }
    public Player Owner { get; set; }

    // Constructorul primește acum și culoarea
    public Pawn(int index, Color color) : base(color)
    {
        Index = index;
    }

    public override void Move(int steps)
    {
        if (IsInStart || IsInHome) return;
        Position += steps;
    }

    public void ExitStart()
    {
        if (IsInStart)
        {
            IsInStart = false;
            Position = 0;
        }
    }
}