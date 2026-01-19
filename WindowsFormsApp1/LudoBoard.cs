using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp1
{
    public class LudoBoard
    {
        private int Cell;
        private int Margin;

        private readonly List<Player> players;

        private readonly Point[,] startSlots = new Point[4, 4];
        private readonly Point[,] homeSlots = new Point[4, 4];
        private readonly int[,] path = new int[,]
{
    // 1. De la Stânga la Dreapta (Sus) - rândul 6
    {0,6}, {1,6}, {2,6}, {3,6}, {4,6}, {5,6},
    {6,5}, {6,4}, {6,3}, {6,2}, {6,1}, {6,0},
    {7,0}, // Mijloc Sus
    // 2. De Sus în Jos (Dreapta) - coloana 8
    {8,0}, {8,1}, {8,2}, {8,3}, {8,4}, {8,5},
    {9,6}, {10,6}, {11,6}, {12,6}, {13,6}, {14,6},
    {14,7}, // Mijloc Dreapta
    // 3. De la Dreapta la Stânga (Jos) - rândul 8
    {14,8}, {13,8}, {12,8}, {11,8}, {10,8}, {9,8},
    {8,9}, {8,10}, {8,11}, {8,12}, {8,13}, {8,14},
    {7,14}, // Mijloc Jos
    // 4. De Jos în Sus (Stânga) - coloana 6
    {6,14}, {6,13}, {6,12}, {6,11}, {6,10}, {6,9},
    {5,8}, {4,8}, {3,8}, {2,8}, {1,8}, {0,8},
    {0,7} // Mijloc Stânga
};

        private readonly int[] safePositions = new int[]
{
    1,   // Albastru
    14,  // Verde
    27,  // Galben
    40   // Roșu
};
 
        public LudoBoard(List<Player> players, Size boardSize)
        {
            this.players = players;
        }
      

    private Rectangle R(int c, int r)
            => new Rectangle(Margin + c * Cell, Margin + r * Cell, Cell, Cell);

        private Point C(int c, int r)
        {
            var rc = R(c, r);
            return new Point(rc.X + Cell / 2, rc.Y + Cell / 2);
        }

        private void BuildSlots()
        {
            // Index 0: ALBASTRU (Stânga Sus - coloane 2,3 rânduri 2,3)
            startSlots[0, 0] = C(2, 2); startSlots[0, 1] = C(3, 2);
            startSlots[0, 2] = C(2, 3); startSlots[0, 3] = C(3, 3);

            // Index 1: VERDE (Dreapta Sus - coloane 11,12 rânduri 2,3)
            startSlots[1, 0] = C(11, 2); startSlots[1, 1] = C(12, 2);
            startSlots[1, 2] = C(11, 3); startSlots[1, 3] = C(12, 3);

            // Index 2: GALBEN (Dreapta Jos - coloane 11,12 rânduri 11,12)
            startSlots[2, 0] = C(11, 11); startSlots[2, 1] = C(12, 11);
            startSlots[2, 2] = C(11, 12); startSlots[2, 3] = C(12, 12);

            // Index 3: ROȘU (Stânga Jos - coloane 2,3 rânduri 11,12)
            startSlots[3, 0] = C(2, 11); startSlots[3, 1] = C(3, 11);
            startSlots[3, 2] = C(2, 12); startSlots[3, 3] = C(3, 12);
        }

        public void Draw(Graphics g)
        {
            RectangleF bounds = g.VisibleClipBounds;

            int usableSize = (int)Math.Min(bounds.Width, bounds.Height);

            Cell = usableSize / 15;
            Margin = (usableSize - Cell * 15) / 2;

            BuildSlots(); // ⚠️ IMPORTANT: după ce știm Cell & Margin

            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawStartZones(g);
            DrawPathGrid(g);
            DrawHomeLanes(g);
            DrawFinalHomeCells(g);
            DrawCenter(g);
            DrawPawns(g);
        }


        private void DrawStartZones(Graphics g)
{
    DrawHouse(g, 0, 0, Color.Blue);   // Index 0 - Albastru
    DrawHouse(g, 9, 0, Color.Green);  // Index 1 - Verde
    DrawHouse(g, 9, 9, Color.Gold);   // Index 2 - Galben
    DrawHouse(g, 0, 9, Color.Red);    // Index 3 - Roșu
}

        private void DrawHouse(Graphics g, int c, int r, Color color)
        {
            Rectangle outer = new Rectangle(Margin + c * Cell, Margin + r * Cell, Cell * 6, Cell * 6);
            using (var br = new SolidBrush(color))
                g.FillRectangle(br, outer);
            g.DrawRectangle(Pens.Black, outer);

            Rectangle inner = new Rectangle(outer.X + Cell + 2, outer.Y + Cell + 2, Cell * 4 - 4, Cell * 4 - 4);
            g.FillRectangle(Brushes.White, inner);
            g.DrawRectangle(Pens.Black, inner);
        }

        private void DrawPathGrid(Graphics g)
        {
            for (int r = 0; r < 15; r++)
            {
                for (int c = 0; c < 15; c++)
                {
                    // Verificăm dacă celula aparține traseului (crucea centrală)
                    bool isPath = (c >= 6 && c <= 8) || (r >= 6 && r <= 8);
                    // Excludem centrul (3x3)
                    bool inCenter = (c >= 6 && c <= 8 && r >= 6 && r <= 8);
                    // Excludem zonele mari de start
                    bool inHouse =
                        (c < 6 && r < 6) ||
                        (c > 8 && r < 6) ||
                        (c < 6 && r > 8) ||
                        (c > 8 && r > 8);

                    if (isPath && !inCenter && !inHouse)
                    {
                        Rectangle rc = R(c, r);
                        Brush fillBrush = Brushes.White; // Culoarea implicită

                        // Colorarea pătratelor de START (ieșirea din bază)
                        // ALBASTRU: Rândul 6, Coloana 1
                        if (r == 6 && c == 1) fillBrush = Brushes.Blue;
                        // VERDE: Rândul 1, Coloana 8
                        else if (r == 1 && c == 8) fillBrush = Brushes.Green;
                        // GALBEN: Rândul 8, Coloana 13
                        else if (r == 8 && c == 13) fillBrush = Brushes.Gold;
                        // ROȘU: Rândul 13, Coloana 6
                        else if (r == 13 && c == 6) fillBrush = Brushes.Red;

                        g.FillRectangle(fillBrush, rc);
                        g.DrawRectangle(Pens.Black, rc);
                    }
                }
            }
        }

        private void DrawHomeLanes(Graphics g)
        {
            // VERDE – SUS
            DrawLane(g, 7, 1, 1, 5, Color.Green);

            // GALBEN – DREAPTA
            DrawLane(g, 9, 7, 5, 1, Color.Gold);

            // ROȘU – JOS
            DrawLane(g, 7, 9, 1, 5, Color.Red);

            // ALBASTRU – STÂNGA
            DrawLane(g, 1, 7, 5, 1, Color.Blue);
        }

        private void DrawLane(Graphics g, int c, int r, int w, int h, Color color)
        {
            int count = w > h ? w : h;
            using (var br = new SolidBrush(color))
            {
                for (int i = 0; i < count; i++)
                {
                    Rectangle rc = R(c + (w > h ? i : 0), r + (h > w ? i : 0));
                    g.FillRectangle(br, rc);
                    g.DrawRectangle(Pens.Black, rc);
                }
            }
        }

        // Aici sunt doar cele 4 celule finale (nu 5)
        private void DrawFinalHomeCells(Graphics g)
        {
            // VERDE sus: r=2..5
            DrawFinalLine(g, 7, 2, 1, 4, Color.Green);

            // ALBASTRU stânga: c=2..5
            DrawFinalLine(g, 2, 7, 4, 1, Color.Blue);

            // GALBEN dreapta: c=9..12
            DrawFinalLine(g, 9, 7, 4, 1, Color.Gold);

            // ROȘU jos: r=9..12
            DrawFinalLine(g, 7, 9, 1, 4, Color.Red);
        }

        private void DrawFinalLine(Graphics g, int c, int r, int w, int h, Color color)
        {
            int count = w > h ? w : h;
            using (var br = new SolidBrush(color))
            {
                for (int i = 0; i < count; i++)
                {
                    Rectangle rc = R(
                        c + (w > h ? i : 0),
                        r + (h > w ? i : 0)
                    );

                    g.FillRectangle(br, rc);
                    g.DrawRectangle(Pens.Black, rc);
                }
            }
        }

        private void DrawCenter(Graphics g)
        {
            Rectangle r = new Rectangle(Margin + 6 * Cell, Margin + 6 * Cell, Cell * 3, Cell * 3);
            Point m = new Point(r.X + r.Width / 2, r.Y + r.Height / 2);

            // SUS – VERDE
            g.FillPolygon(Brushes.Green, new[] { new Point(r.Left, r.Top), new Point(r.Right, r.Top), m });

            // DREAPTA – GALBEN
            g.FillPolygon(Brushes.Gold, new[] { new Point(r.Right, r.Top), new Point(r.Right, r.Bottom), m });

            // JOS – ROȘU
            g.FillPolygon(Brushes.Red, new[] { new Point(r.Right, r.Bottom), new Point(r.Left, r.Bottom), m });

            // STÂNGA – ALBASTRU
            g.FillPolygon(Brushes.Blue, new[] { new Point(r.Left, r.Bottom), new Point(r.Left, r.Top), m });

            g.DrawRectangle(Pens.Black, r);
        }

        private void DrawPawns(Graphics g)
        {
            for (int p = 0; p < players.Count; p++)
            {
                for (int i = 0; i < 4; i++)
                {
                    var pawn = players[p].Pawns[i];

                    Point pos;
                    if (pawn.IsInStart)
                        pos = startSlots[p, i];
                    else if (pawn.IsInHome)
                        pos = GetPathPosition(pawn);
                    else
                        pos = GetPathPosition(pawn);

                    DrawPawn(g, pawn.Owner.Color, pos, i + 1);
                }
            }
        }

        // În LudoBoard.cs
        private Point GetPathPosition(Pawn pawn)
        {
            // Verificăm dacă a depășit drumul comun (51 pași max pe alb)
            if (pawn.Position > 51)
            {
                int stepInHome = pawn.Position - 50;
                return GetHomeLaneCoordinate(pawn.Owner.PlayerId, stepInHome);
            }

            // Această formulă este esențială:
            int index = (pawn.Owner.StartOffset + pawn.Position) % 52;

            // Returnăm coordonatele din matricea path
            return C(path[index, 0], path[index, 1]);
        }
        private Point GetHomeLaneCoordinate(int playerId, int step)
        {
            switch (playerId)
            {
                case 0: // Albastru (Stânga) -> merge la dreapta pe rândul 7
                    return C(step, 7);
                case 1: // Verde (Sus) -> coboară pe coloana 7
                    return C(7, step);
                case 2: // Galben (Dreapta) -> merge la stânga pe rândul 7
                    return C(14 - step, 7);
                case 3: // Roșu (Jos) -> urcă pe coloana 7
                    return C(7, 14 - step);
                default: return new Point(0, 0);
            }
        }

        private void DrawPawn(Graphics g, Color color, Point p, int nr)
        {
            int s = (int)(Cell * 0.7);
            using (var br = new SolidBrush(color))
                g.FillEllipse(br, p.X - s / 2, p.Y - s / 2, s, s);

            g.DrawEllipse(Pens.Black, p.X - s / 2, p.Y - s / 2, s, s);

            g.DrawString(
                nr.ToString(),
                SystemFonts.DefaultFont,
                Brushes.White,
                new RectangleF(p.X - s / 2, p.Y - s / 2, s, s),
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
            );
        }

        public Point GetPawnScreenPosition(Pawn pawn)
        {
            int p = players.IndexOf(pawn.Owner);

            if (pawn.IsInStart)
                return startSlots[p, pawn.Index];

            if (pawn.IsInHome)
                return homeSlots[p, pawn.Index];

            return GetPathPosition(pawn);
        }

        public void CheckCapture(Pawn movingPawn)
        {
            if (movingPawn.Position > 51)
                return; // nu se mănâncă în drumul final

            int targetGlobalPos =
                (movingPawn.Owner.StartOffset + movingPawn.Position) % 52;

            // SAFE ZONE?
            foreach (int safe in safePositions)
                if (targetGlobalPos == safe)
                    return;

            foreach (var player in players)
            {
                if (player == movingPawn.Owner) continue;

                foreach (var p in player.Pawns)
                {
                    if (p.IsInStart || p.IsInHome) continue;
                    if (p.Position > 51) continue;

                    int otherGlobalPos =
                        (p.Owner.StartOffset + p.Position) % 52;

                    if (targetGlobalPos == otherGlobalPos)
                    {
                        p.Reset(); // trimite pionul înapoi în casă
                    }
                }
            }
        }

    }
}
