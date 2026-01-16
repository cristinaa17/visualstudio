using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Player
    {
        public int PlayerId { get; }
        public string Name { get; }
        public Color Color { get; }

        // offset de start pe traseul comun
        public int StartOffset { get; }

        public List<Pawn> Pawns { get; }

        // În Player.cs
        public Player(int id, string name, Color color, int startOffset)
        {
            PlayerId = id;
            Name = name;
            Color = color;
            StartOffset = startOffset;

            Pawns = new List<Pawn>();
            for (int i = 0; i < 4; i++)
            {
                // Trimitem 'i' pentru index și 'color' pentru culoarea pionului
                Pawns.Add(new Pawn(i, color) { Owner = this });
            }
        }

        public bool HasWon()
        {
            // momentan nu ai logica completă de IsInHome,
            // deci asta va rămâne false până implementăm finalul.
            foreach (var p in Pawns)
                if (!p.IsInHome) return false;
            return true;
        }
    }
}
