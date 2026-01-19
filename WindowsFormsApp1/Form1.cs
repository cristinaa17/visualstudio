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
            // Actualizează în MainForm.cs
            players = new List<Player>
{
    new Player(0, "Albastru", Color.Blue, 1),
    new Player(1, "Verde", Color.Green, 14),
    new Player(2, "Galben", Color.Gold, 27),
    new Player(3, "Roșu", Color.Red, 40)
};


            board = new LudoBoard(players, pnlBoard.ClientSize);

            lblDiceResult.Text = "–";
            lblInstructions.Text = "Aruncă zarul";
            UpdateGameStatus();
            pnlBoard.Invalidate();
        }

        private void btnRollDice_Click(object sender, EventArgs e)
        {
            if (diceRolled) return;

            diceValue = random.Next(1, 7);
            diceRolled = true;
            lblDiceResult.Text = diceValue.ToString();

            var player = players[currentPlayerIndex];

            // VERIFICARE: Poate face vreo mutare?
            if (!HasAnyValidMove(player, diceValue))
            {
                lblInstructions.Text = "Nu ai mutări posibile! Tura trece...";

                // Așteptăm puțin ca jucătorul să vadă mesajul, apoi trecem tura
                Timer t = new Timer { Interval = 1500 };
                t.Tick += (s, ev) =>
                {
                    t.Stop();
                    t.Dispose();
                    EndTurn();
                };
                t.Start();
                return;
            }

            // Dacă are mutări, jocul continuă normal
            lblInstructions.Text = (diceValue == 6)
                ? "Ai dat 6! Alege un pion."
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

            if (pawn.IsInStart)
            {
                if (diceValue != 6)
                {
                    lblInstructions.Text = "Ai nevoie de 6 ca să scoți pionul din casă.";
                    return;
                }

                pawn.ExitStart();
            }
            else
            {
                // VERIFICARE: Dacă pionul depășește casa
                if (pawn.Position + diceValue > 56)
                {
                    lblInstructions.Text = "Zar prea mare! Ai nevoie de valoare exactă.";
                    return; // Nu apelăm FinishMove, îi dăm voie să aleagă alt pion
                }

                pawn.Move(diceValue);
            }

            // 🔴 AICI mâncăm pionii
            board.CheckCapture(pawn);

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
        private bool HasAnyValidMove(Player player, int dice)
        {
            foreach (var pawn in player.Pawns)
            {
                // 1. Dacă e în casă (a terminat jocul), nu mai poate fi mutat
                if (pawn.IsInHome) continue;

                // 2. Dacă e în start, poate ieși doar cu 6
                if (pawn.IsInStart)
                {
                    if (dice == 6) return true;
                    else continue;
                }

                // 3. Dacă e pe traseu, verificăm să nu depășească fix căsuța 57 (sau 56, depinde de indexare)
                // În codul tău de Move, limita este 57.
                if (pawn.Position + dice <= 56)
                {
                    return true;
                }
            }
            return false;
        }

        private void MainForm_Load(object sender, EventArgs e) { }
    }
}
