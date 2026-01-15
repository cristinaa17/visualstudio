namespace WindowsFormsApp1
{
    public class Pawn
    {
        public int Index { get; }
        public Player Owner { get; set; }

        public bool IsInStart { get; private set; } = true;
        public bool IsInHome { get; private set; } = false;

        // pași de la start-ul jucătorului (0 = pe start)
        public int Position { get; private set; } = 0;

        public Pawn(int index)
        {
            Index = index;
        }

        public void Reset()
        {
            IsInStart = true;
            IsInHome = false;
            Position = 0;
        }

        public void ExitStart()
        {
            if (!IsInStart) return;
            IsInStart = false;
            Position = 0; // fix pe START-ul lui (StartOffset + 0)
        }

        public void Move(int steps)
        {
            if (IsInStart || IsInHome) return;

            // Nu folosim modulo aici dacă vrem să știm când a terminat tura completă
            Position += steps;

            // Verificăm dacă a depășit lungimea traseului pentru a intra în casă (opțional acum)
            if (Position > 51)
            {
                // Logica de intrare în "Home Lane" va veni aici
            }
        }
    }
}
