/* 
 * Filename: Scoreboard.cs
 * Author: Amirah Yahaya
 * Date created: 26 December 2023
 * Description: This file contains the implementation of the Scoreboard form
 *              used to display scores and game statistics in the Blackjack game.
 */
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
            // Update player's score label
            lblPlayerScoreValue.Text = playerScore.ToString();

            // Update dealer's score label
            lblDealerScoreValue.Text = dealerScore.ToString();

            // Update games won label
            lblGamesWonValue.Text = gamesWon.ToString();

            // Update games lost label
            lblGamesLostValue.Text = gamesLost.ToString();

            // Update games tied label
            lblGamesTiedValue.Text = gamesTied.ToString();

            // Update total games played label
            lblTotalGamesPlayedValue.Text = totalGamesPlayed.ToString();
        }
    }
}
