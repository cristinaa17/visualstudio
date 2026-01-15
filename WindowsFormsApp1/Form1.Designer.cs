using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlRight;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnRollDice = new System.Windows.Forms.Button();
            this.lblDiceResult = new System.Windows.Forms.Label();
            this.lblCurrentPlayer = new System.Windows.Forms.Label();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBoard
            // 
            this.pnlBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBoard.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBoard.Location = new System.Drawing.Point(0, 0);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(600, 602);
            this.pnlBoard.TabIndex = 1;
            this.pnlBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBoard_Paint);
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlRight.Controls.Add(this.lblInstructions);
            this.pnlRight.Controls.Add(this.btnReset);
            this.pnlRight.Controls.Add(this.btnRollDice);
            this.pnlRight.Controls.Add(this.lblDiceResult);
            this.pnlRight.Controls.Add(this.lblCurrentPlayer);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(600, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20);
            this.pnlRight.Size = new System.Drawing.Size(357, 602);
            this.pnlRight.TabIndex = 0;
            // 
            // lblInstructions
            // 
            this.lblInstructions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Italic);
            this.lblInstructions.Location = new System.Drawing.Point(20, 360);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(260, 80);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "Aruncă zarul";
            this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReset.Location = new System.Drawing.Point(50, 300);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(180, 40);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "🔄 Resetare";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnRollDice
            // 
            this.btnRollDice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRollDice.Location = new System.Drawing.Point(50, 240);
            this.btnRollDice.Name = "btnRollDice";
            this.btnRollDice.Size = new System.Drawing.Size(180, 50);
            this.btnRollDice.TabIndex = 2;
            this.btnRollDice.Text = "🎲 Aruncă zarul";
            this.btnRollDice.Click += new System.EventHandler(this.btnRollDice_Click);
            // 
            // lblDiceResult
            // 
            this.lblDiceResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDiceResult.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Bold);
            this.lblDiceResult.Location = new System.Drawing.Point(60, 60);
            this.lblDiceResult.Name = "lblDiceResult";
            this.lblDiceResult.Size = new System.Drawing.Size(160, 160);
            this.lblDiceResult.TabIndex = 3;
            this.lblDiceResult.Text = "–";
            this.lblDiceResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentPlayer
            // 
            this.lblCurrentPlayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCurrentPlayer.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblCurrentPlayer.Location = new System.Drawing.Point(20, 20);
            this.lblCurrentPlayer.Name = "lblCurrentPlayer";
            this.lblCurrentPlayer.Size = new System.Drawing.Size(317, 40);
            this.lblCurrentPlayer.TabIndex = 4;
            this.lblCurrentPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(957, 602);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlBoard);
            this.Name = "MainForm";
            this.Text = "🎲 Joc de Ludo 🎲";
            this.pnlRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.Panel pnlBoard;
        private System.Windows.Forms.Button btnRollDice;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblDiceResult;
        private System.Windows.Forms.Label lblCurrentPlayer;
        private System.Windows.Forms.Label lblInstructions;
    }
}