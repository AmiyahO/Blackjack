using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class Scoreboard : Form
    {

        public Scoreboard()
        {
            InitializeComponent();
        }

        // Method to update the scoreboard with scores and game statistics
        public void UpdateScores(int playerScore, int dealerScore, int gamesWon, int gamesLost, int gamesTied, int totalGamesPlayed)
        {
            // Update your UI controls with scores and game statistics
            lblPlayerScoreValue.Text = playerScore.ToString();
            lblDealerScoreValue.Text = dealerScore.ToString();
            lblGamesWonValue.Text = gamesWon.ToString();
            lblGamesLostValue.Text = gamesLost.ToString();
            lblGamesTiedValue.Text = gamesTied.ToString();
            lblTotalGamesPlayedValue.Text = totalGamesPlayed.ToString();
        }
    }
}
