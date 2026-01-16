using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private List<Player> players;
        private int currentPlayerIndex;
        private Random random;
        private LudoBoard board;
 


        private int diceValue;
        private bool diceRolled;

        public MainForm()
        {
            InitializeComponent();
            EnableDoubleBuffering(pnlBoard);

            pnlBoard.MouseClick += pnlBoard_MouseClick;

            InitializeGame();
        }

        private void InitializeGame()
        {
            random = new Random();
            diceValue = 0;
            diceRolled = false;
            currentPlayerIndex = 0;

            // IMPORTANT: StartOffset-urile le punem DIRECT (nu FindPathIndex în MainForm)
            // (6,8) = 20, (5,6) = 17, (8,5) = 49, (8,6) = 48 în path-ul tău.
            players = new List<Player>
{
    new Player(0, "Albastru", Color.Blue, 10),   // Acum va ieși la {6, 1}
    new Player(1, "Verde", Color.Green, 49),   // Acum va ieși la {1, 8}
    new Player(2, "Galben", Color.Gold,36 ),   // Acum va ieși la {8, 13}
    new Player(3, "Roșu", Color.Red, 23)       // Acum va ieși la {13, 6}
};


            board = new LudoBoard(players, pnlBoard.ClientSize);

            lblDiceResult.Text = "–";
            lblInstructions.Text = "Aruncă zarul";
            UpdateGameStatus();
            pnlBoard.Invalidate();
        }

        private void btnRollDice_Click(object sender, EventArgs e)
        {
            if (diceRolled)
                return;

            diceValue = random.Next(1, 7);
            diceRolled = true;
            lblDiceResult.Text = diceValue.ToString();

            var player = players[currentPlayerIndex];

            bool hasPawnOutside = player.Pawns.Exists(p => !p.IsInStart && !p.IsInHome);

            if (!hasPawnOutside && diceValue != 6)
            {
                lblInstructions.Text = "Niciun pion afară și nu ai dat 6 → tură pierdută";

                Timer t = new Timer { Interval = 900 };
                t.Tick += (s, ev) =>
                {
                    t.Stop();
                    t.Dispose();
                    EndTurn();
                };
                t.Start();

                return;
            }

            lblInstructions.Text = (diceValue == 6)
                ? "Ai dat 6! Alege un pion (vei mai arunca o dată)"
                : "Alege un pion pentru mutare";
        }

        private void pnlBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (!diceRolled)
                return;

            var player = players[currentPlayerIndex];

            foreach (var pawn in player.Pawns)
            {
                Point pos = board.GetPawnScreenPosition(pawn);
                Rectangle hit = new Rectangle(pos.X - 12, pos.Y - 12, 24, 24);

                if (!hit.Contains(e.Location))
                    continue;

                HandlePawnSelection(pawn);
                return;
            }
        }

        private void HandlePawnSelection(Pawn pawn)
        {
            var player = players[currentPlayerIndex];

            // pion în casă -> iese DOAR la 6
            if (pawn.IsInStart)
            {
                if (diceValue != 6)
                {
                    lblInstructions.Text = "Ai nevoie de 6 ca să scoți pionul din casă.";
                    return;
                }

                pawn.ExitStart(); // pune Position=0 (start-ul lui, prin StartOffset)
            }
            else
            {
                pawn.Move(diceValue);
            }

            // (deocamdată nu ai logica completă de finalizare/casă, deci HasWon va fi false)
            if (player.HasWon())
            {
                lblInstructions.Text = $"{player.Name} a câștigat!";
                InitializeGame();
                return;
            }

            FinishMove();
        }

        private void FinishMove()
        {
            // dacă a dat 6 -> mai aruncă o dată
            if (diceValue == 6)
            {
                diceRolled = false;
                diceValue = 0;
                lblDiceResult.Text = "–";
                lblInstructions.Text = "Mai aruncă o dată!";
            }
            else
            {
                EndTurn();
            }

            pnlBoard.Invalidate();
        }

        private void EndTurn()
        {
            diceRolled = false;
            diceValue = 0;
            lblDiceResult.Text = "–";

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            UpdateGameStatus();
            lblInstructions.Text = "Aruncă zarul";
            pnlBoard.Invalidate();
        }

        private void pnlBoard_Paint(object sender, PaintEventArgs e)
        {
            board?.Draw(e.Graphics);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeGame();
        }

        private void UpdateGameStatus()
        {
            var p = players[currentPlayerIndex];
            lblCurrentPlayer.Text = $"Jucător curent: {p.Name}";
            lblCurrentPlayer.ForeColor = p.Color;
        }

        private void EnableDoubleBuffering(Control c)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(c, true, null);
        }

        private void MainForm_Load(object sender, EventArgs e) { }
    }
}
