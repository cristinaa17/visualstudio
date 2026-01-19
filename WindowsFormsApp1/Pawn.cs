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

            // Verificăm dacă mutarea ar depăși careul final (poziția 57)
            if (Position + steps <= 56)
            {
                Position += steps;

                // Dacă a ajuns exact la 57, pionul este în casă (a terminat)
                if (Position == 56)
                {
                    IsInHome = true;
                }
            }
            // Dacă Position + steps > 57, nu facem nimic (pionul rămâne pe loc)
        }

        // O metodă utilă pentru a verifica în interfață dacă pionul poate fi mutat
        public bool CanMove(int steps)
        {
            if (IsInStart && steps == 6) return true;
            if (IsInStart && steps != 6) return false;
            if (IsInHome) return false;

            return (Position + steps <= 57);
        }



        public void ExitStart()
        {
            if (!IsInStart) return;

            IsInStart = false;
            Position = 0;
        }
    }
}
