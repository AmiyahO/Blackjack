/* 
 * Filename: Blackjack.cs
 * Author: Amirah Yahaya
 * Date created: 26 December 2023
 * Description: Blackjack game implementation
 */
using Blackjack.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Blackjack
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack = 10,
        Queen = 10,
        King = 10,
        Ace = 11
    }

    public partial class Blackjack : Form
    {
        private BlackjackGame blackjackGame;

        // PictureBox controls for displaying player and dealer cards
        private PictureBox[] playerPictureBoxes;
        private PictureBox[] dealerPictureBoxes;

        private const int MinBet = 2;  // Minimum betting amount
        private const int MaxBet = 100;  // Maximum betting amount
        private int playerBalance;  // Player's balance
        private int bankBalance;  // Bank's balance
        private int splitsRemaining;  // Variable to track the number of splits remaining
        public int gamesWon;
        public int gamesLost;
        public int gamesTied;
        public int totalGamesPlayed;

        public Blackjack()
        {
            InitializeComponent();

            blackjackGame = new BlackjackGame(this); // Initialize the Blackjack game

            playerBalance = 1000;  // Initial player balance
            bankBalance = 1000000;  // Initial bank balance
            gamesWon = 0;
            gamesLost = 0;
            gamesTied = 0;
            totalGamesPlayed = 0;
            splitsRemaining = 2; // Initialize the number of splits for the new hand

            // Array of PictureBox controls for player and dealer cards
            playerPictureBoxes = new PictureBox[] { pictureBox1Player, pictureBox2Player, pictureBox3Player, pictureBox4Player, pictureBox5Player, pictureBox6Player, pictureBox7Player, pictureBox8Player, pictureBox9Player, pictureBox10Player, pictureBox11Player };
            dealerPictureBoxes = new PictureBox[] { pictureBox1Dealer, pictureBox2Dealer, pictureBox3Dealer, pictureBox4Dealer, pictureBox5Dealer, pictureBox6Dealer, pictureBox7Dealer, pictureBox8Dealer, pictureBox9Dealer, pictureBox10Dealer, pictureBox11Dealer };

            // Disable buttons
            btnStart.Enabled = false;
            btnTwist.Enabled = false;
            btnStick.Enabled = false;
            btnSplit.Enabled = false;
            btnDoubleDown.Enabled = false;
            btnContinue.Enabled = false;
            btnReset.Enabled = false;

            // Set initial balance
            lblBalance.Text = $"Balance: £{playerBalance}";
            lblBank.Text = $"Bank: £{bankBalance}";
        }

        private void StartGame()
        {
            blackjackGame.StartGame();

            // Check if the player can double down
            if (blackjackGame.PlayerHand.CalculateScore() >= 9 && blackjackGame.PlayerHand.CalculateScore() <= 11)
            {
                btnDoubleDown.Enabled = true;
            }

            // Check if the player has a pair for splitting
            if (blackjackGame.PlayerHand.Cards.Count == 2 && blackjackGame.PlayerHand.Cards[0].Rank == blackjackGame.PlayerHand.Cards[1].Rank)
            {
                // Check if the pair is eligible for splitting
                if (IsEligibleForSplit(blackjackGame.PlayerHand.Cards[0]))
                {
                    btnSplit.Enabled = true;
                }
            }

            // Update PictureBoxes and labels
            UpdatePictureBoxes();
            UpdateScores();

            // Determine the winner after dealing initial cards
            if (blackjackGame.PlayerHand.CalculateScore() == 21)
            {
                blackjackGame.DetermineWinner();
                UpdateBalances();
                DisablePlayerButtons();
            }
        }

        // Method to update player and dealer scores, and display player and dealer scores and player and bank balance on the form.
        private void UpdateScores()
        {
            // Display player and dealer scores
            lblPlayerScore.Text = ((int)blackjackGame.PlayerHand.CalculateScore()).ToString();
            lblDealerScore.Text = ((int)blackjackGame.DealerHand.CalculateScore()).ToString();
            lblBalance.Text = $"Balance: £{playerBalance}";
            lblBank.Text = $"Bank: £{bankBalance}";
        }

        // Method to update PictureBoxes with player and dealer card images
        private void UpdatePictureBoxes()
        {
            UpdatePlayerPictureBoxes();
            UpdateDealerPictureBoxes();
        }

        // Method to update player PictureBoxes with card images
        private void UpdatePlayerPictureBoxes()
        {
            // Display player's cards using PictureBox controls
            for (int i = 0; i < blackjackGame.PlayerHand.Cards.Count; i++)
            {
                if (i < playerPictureBoxes.Length)
                {
                    playerPictureBoxes[i].Image = blackjackGame.PlayerHand.Cards[i].CardImage;
                    playerPictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        // Method to update dealer PictureBoxes with card images
        private void UpdateDealerPictureBoxes()
        {
            // Display dealer's cards using PictureBox controls
            for (int i = 0; i < blackjackGame.DealerHand.Cards.Count; i++)
            {
                if (i < dealerPictureBoxes.Length)
                {
                    dealerPictureBoxes[i].Image = blackjackGame.DealerHand.Cards[i].CardImage;
                    dealerPictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;

                }
            }
        }

        // Method to clear card images in PictureBoxes
        private void ClearPictureBoxes()
        {
            // Clear images in player PictureBoxes
            foreach (PictureBox pictureBox in playerPictureBoxes)
            {
                pictureBox.Image = Resources.back;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            // Clear images in dealer PictureBoxes
            foreach (PictureBox pictureBox in dealerPictureBoxes)
            {
                pictureBox.Image = Resources.back;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        // Method to update player and bank balances on the form
        private void UpdateBalances()
        {
            playerBalance += blackjackGame.PlayerBalance;
            bankBalance += blackjackGame.BankBalance;

            lblBalance.Text = $"Balance: £{playerBalance}";
            lblBank.Text = $"Bank: £{bankBalance}";
        }

        // Method to disable player action buttons
        private void DisablePlayerButtons()
        {
            // Disable buttons
            btnTwist.Enabled = false;
            btnStick.Enabled = false;
            btnDoubleDown.Enabled = false;
            btnSplit.Enabled = false;
            btnContinue.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnTwist.Enabled = true;
            btnStick.Enabled = true;
            btnReset.Enabled = true;

            StartGame();
        }

        private void btnTwist_Click(object sender, EventArgs e)
        {
            // Player takes another card
            blackjackGame.PlayerTwist(blackjackGame.PlayerHand);

            // Update PictureBoxes and scores
            UpdatePictureBoxes();
            UpdateScores();

            // Determine the winner
            // Check if the player has busted
            if (blackjackGame.PlayerHand.CalculateScore() >= 21)
            {
                // Player has busted, determine the winner
                blackjackGame.DetermineWinner();
                UpdateBalances();
                DisablePlayerButtons();
            }
        }

        private void btnStick_Click(object sender, EventArgs e)
        {
            // Dealer takes cards until score is at least 17
            blackjackGame.DealerTwist();

            UpdatePictureBoxes();
            UpdateScores();

            // Determine the winner after the dealer has completed their turn
            blackjackGame.DetermineWinner();
            UpdateBalances();
            DisablePlayerButtons();
        }

        private void btnDoubleDown_Click(object sender, EventArgs e)
        {
            // Double the player's bet
            blackjackGame.PlayerBet *= 2;

            // update players balance
            playerBalance -= blackjackGame.PlayerBet / 2;

            // Player takes one more card
            blackjackGame.PlayerTwist(blackjackGame.PlayerHand);

            // Dealer takes cards until score is at least 17
            blackjackGame.DealerTwist();

            // Update PictureBoxes and scores
            UpdatePictureBoxes();
            UpdateScores();

            // Update the bet textbox
            txtBet.Text = blackjackGame.PlayerBet.ToString();

            // Determine the winner
            blackjackGame.DetermineWinner();
            UpdateBalances();
            DisablePlayerButtons();
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            if (splitsRemaining > 0)
            {
                // Deduct the second bet from the player's balance
                playerBalance -= blackjackGame.PlayerBet;

                // Update the balances
                UpdateBalances();

                // Disable the Split button after splitting twice
                if (--splitsRemaining == 0)
                {
                    btnSplit.Enabled = false;
                }

                // Create a new hand for the split cards
                Hand splitHand = new Hand();

                // Add one card from the original hand to the new hand
                splitHand.AddCard(blackjackGame.PlayerHand.Cards[1]);
                blackjackGame.PlayerHand.Cards.RemoveAt(1);

                // Update PictureBoxes and scores
                UpdatePictureBoxes();
                UpdateScores();

                // Determine the winner for the original hand
                blackjackGame.DetermineWinner();

                // Continue the game with the new split hand
                blackjackGame.PlayerHand = splitHand;

                // Start the game for the split hand
                StartGame();
            }
        }

        // Method to check if a card is eligible for splitting
        private bool IsEligibleForSplit(Card card)
        {
            // Check if the card is not a 5 or 10
            return card.Rank != Rank.Five && card.Rank != Rank.Ten;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = false;

            // Reset the game
            blackjackGame = new BlackjackGame(this);

            // Clear images in PictureBoxes
            ClearPictureBoxes();

            // Enable betting controls
            txtBet.Enabled = true;
            btnBet.Enabled = true;

            // Clear the bet textbox
            txtBet.Text = string.Empty;

            // Update scores
            UpdateScores();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Reset the game
            blackjackGame = new BlackjackGame(this);

            // Clear the bet TextBox
            txtBet.Text = string.Empty;

            // Clear images in PictureBoxes
            ClearPictureBoxes();

            // Enable betting controls
            txtBet.Enabled = true;
            btnBet.Enabled = true;

            // Update scores
            UpdateScores();

            // Reset player's balance
            playerBalance = 1000;
            lblBalance.Text = $"Balance: £{playerBalance}";

            // Reset bank balance
            bankBalance = 1000000;
            lblBank.Text = $"Bank: £{bankBalance}";

            // Disable buttons
            btnStart.Enabled = false;
            btnTwist.Enabled = false;
            btnStick.Enabled = false;
            btnSplit.Enabled = false;
            btnDoubleDown.Enabled = false;
            btnReset.Enabled = false;

            // Reset game statistice to 0
            gamesWon = 0;
            gamesLost = 0;
            gamesTied = 0;
            totalGamesPlayed = 0;
        }

        private void btnBet_Click(object sender, EventArgs e)
        {
            // Validate and parse the bet amount
            if (int.TryParse(txtBet.Text, out int betAmount) && IsValidBet(betAmount))
            {
                // Check if the player has enough balance for the bet
                if (betAmount > playerBalance)
                {
                    MessageBox.Show("Insufficient balance. Please enter a lower bet.", "Insufficient Balance");
                }
                else
                {
                    // Set the player's bet in the BlackjackGame instance
                    blackjackGame.PlayerBet = betAmount;

                    // Update player's balance
                    playerBalance -= betAmount;

                    // Disable betting controls
                    txtBet.Enabled = false;
                    btnBet.Enabled = false;

                    // Enable start button
                    btnStart.Enabled = true;
                    btnReset.Enabled = true;

                }
            }
            else
            {
                MessageBox.Show($"Invalid bet amount. Please enter a bet between £{MinBet} and £{MaxBet} in multiples of £1.", "Invalid Bet");
            }
        }

        // Method to check if the bet amount is valid
        private bool IsValidBet(int betAmount)
        {
            return betAmount >= MinBet && betAmount <= MaxBet && betAmount % 1 == 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Programmer: Amirah Yahaya", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HowToPlay gameInstructions = new HowToPlay();
            gameInstructions.ShowDialog();
        }

        private void scoreboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scoreboard scoreboard = new Scoreboard();

            int playerScore = (int)blackjackGame.PlayerHand.CalculateScore();
            int dealerScore = (int)blackjackGame.DealerHand.CalculateScore();

            scoreboard.UpdateScores(playerScore, dealerScore, gamesWon, gamesLost, gamesTied, totalGamesPlayed);
            scoreboard.ShowDialog();
        }
    }

    public class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public Image CardImage { get; }

        public Card(Suit suit, Rank rank, Image cardImage)
        {
            Suit = suit;
            Rank = rank;
            CardImage = cardImage;
        }

        // Method to get the numerical value of the card (considering Ace as 11)
        public int GetValue()
        {
            if (Rank == Rank.Ace)
            {
                return 11;
            }
            else if ((int)Rank >= 10)
            {
                return 10;
            }
            else
            {
                return (int)Rank;
            }
        }
    }

    public class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            InitialiseDeck();
            ShuffleDeck();
        }

        private void InitialiseDeck()
        {
            // Initialize the deck with cards and their corresponding images
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    string imageName;

                    // Adjusted naming convention for special cards
                    if (rank == Rank.Ace)
                    {
                        imageName = $"ace_of_{suit.ToString().ToLower()}";
                    }
                    else if (rank == Rank.Jack || rank == Rank.Queen || rank == Rank.King)
                    {
                        imageName = $"{rank.ToString().ToLower()}_of_{suit.ToString().ToLower()}";
                    }
                    else
                    {
                        // For numeric cards (2 to 10)
                        imageName = $"{(int)rank}_of_{suit.ToString().ToLower()}";
                    }

                    Image cardImage = Properties.Resources.ResourceManager.GetObject(imageName) as Image;

                    cards.Add(new Card(suit, rank, cardImage));
                }
            }
        }

        private void ShuffleDeck()
        {
            // Shuffle the deck using the Fisher-Yates algorithm
            Random random = new Random();
            int n = cards.Count;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public Card DealCard()
        {
            if (cards.Count == 0)
            {
                InitialiseDeck();
                ShuffleDeck();

                if (cards.Count == 0)
                {
                    // Display a message using MessageBox if the deck is still empty after reinitialization
                    MessageBox.Show("The deck is still empty after reinitialization.", "Error");
                    throw new InvalidOperationException("The deck is empty.");
                }
            }

            Card card = cards[0];
            cards.RemoveAt(0);

            return card;
        }
    }

    public class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public List<Card> Cards
        {
            get { return cards; }
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int CalculateScore()
        {
            // Calculate the total score of the hand, adjusting for Aces
            int score = 0;
            int aceCount = 0;

            foreach (Card card in cards)
            {
                score += card.GetValue();
                if (card.Rank == Rank.Ace)
                {
                    aceCount++;
                }
            }

            while (aceCount > 0 && score > 21)
            {
                score -= 10;
                aceCount--;
            }

            return score;
        }
    }

    public class BlackjackGame
    {
        private Blackjack form1;

        private Deck deck;
        public Hand DealerHand { get; }
        public Hand PlayerHand { get; set; }
        public int PlayerBet { get; set; }
        public int PlayerBalance { get; set; }
        public int BankBalance { get; set; }

        public BlackjackGame(Blackjack form)
        {
            form1 = form;
            deck = new Deck();
            DealerHand = new Hand();
            PlayerHand = new Hand();
            PlayerBet = 2; // Initial bet
            PlayerBalance = 0;
            BankBalance = 0;
        }

        public void StartGame()
        {
            DealerHand.AddCard(deck.DealCard());
            PlayerHand.AddCard(deck.DealCard());
            PlayerHand.AddCard(deck.DealCard());
        }

        public void PlayerTwist(Hand playerHand)
        {
            // Player draws an additional card
            Card drawnCard = deck.DealCard();
            PlayerHand.AddCard(drawnCard);
        }

        public void DealerTwist()
        {
            // Dealer takes cards until score is at least 17
            while (DealerHand.CalculateScore() < 17)
            {
                Card drawnCard = deck.DealCard();
                DealerHand.AddCard(drawnCard);
            }
        }

        public void DetermineWinner()
        {
            int dealerScore = DealerHand.CalculateScore();
            int playerScore = PlayerHand.CalculateScore();

            // Check for player blackjack with ace and picture card
            if (PlayerHand.Cards.Count == 2 && playerScore == 21)
            {
                // Check for dealer blackjack
                if (DealerHand.Cards.Count == 2 && dealerScore == 21)
                {
                    MessageBox.Show("It's a draw! Both have blackjack.", "Game Over");
                    PlayerBalance += PlayerBet;
                    GameTied();
                    return;
                }

                // Player wins with blackjack
                MessageBox.Show("You win with blackjack!", "Game Over");
                PlayerBalance += (int)(PlayerBet * 2.5);
                BankBalance -= (int)(PlayerBet * 1.5);
                GameWon();
                return;
            }

            // Player busts
            if (playerScore > 21)
            {
                MessageBox.Show("You lose!", "Game Over");
                BankBalance += PlayerBet;
                GameLost();
                return;
            }

            // Dealer draws until reaching 17 or higher
            while (dealerScore < 17)
            {
                Card drawnCard = deck.DealCard();
                DealerHand.AddCard(drawnCard);
            }
            dealerScore = DealerHand.CalculateScore();

            // Dealer busts
            if (dealerScore > 21)
            {
                MessageBox.Show("You win!", "Game Over");
                PlayerBalance += PlayerBet * 2;
                BankBalance -= PlayerBet;
                GameWon();
                return;
            }

            // Compare scores to determine the winner
            if (playerScore == dealerScore)
            {
                MessageBox.Show("It's a draw!", "Game Over");
                PlayerBalance += PlayerBet;
                GameTied();
            }
            else if (playerScore < dealerScore || dealerScore == 21)
            {
                MessageBox.Show("You lose!", "Game Over");
                BankBalance += PlayerBet;
                GameLost();
            }
            else if (playerScore > dealerScore || playerScore == 21)
            {
                MessageBox.Show("You win!", "Game Over");
                PlayerBalance += PlayerBet * 2;
                BankBalance -= PlayerBet;
                GameWon();
            }
            else
            {
                if (playerScore == 21 && (dealerScore < 21 || dealerScore > 21))
                {
                    MessageBox.Show("You win!", "Game Over");
                    PlayerBalance += PlayerBet * 2;
                    BankBalance -= PlayerBet;
                    GameWon();
                }
                else if (dealerScore == 21 && (playerScore < 21 || playerScore > 21))
                {
                    MessageBox.Show("You lose!", "Game Over");
                    BankBalance += PlayerBet;
                    GameLost();
                }
            }
        }

        private void GameWon()
        {
            form1.gamesWon++;
            form1.totalGamesPlayed++;
        }

        private void GameLost()
        {
            form1.gamesLost++;
            form1.totalGamesPlayed++;
        }

        private void GameTied()
        {
            form1.gamesTied++;
            form1.totalGamesPlayed++;
        }
    }
}