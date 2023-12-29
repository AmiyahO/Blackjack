using Blackjack.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Blackjack.Deck;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

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

    public partial class Form1 : Form
    {
        private BlackjackGame blackjackGame;

        // Additional PictureBox controls for displaying cards
        private PictureBox[] playerPictureBoxes;
        private PictureBox[] dealerPictureBoxes;

        private const int MinBet = 2;
        private const int MaxBet = 100;
        private const int MaxStartingAmount = 50;
        private int playerBalance;
        private int bankBalance;

        private int visiblePlayerCards;
        private int visibleDealerCards;

        public Form1()
        {
            InitializeComponent();
            blackjackGame = new BlackjackGame();
            playerBalance = MaxStartingAmount;
            bankBalance = 1000;
            visiblePlayerCards = 0;
            visibleDealerCards = 0;

            playerPictureBoxes = new PictureBox[] { pictureBox1Player, pictureBox2Player, pictureBox3Player, pictureBox4Player, pictureBox5Player, pictureBox6Player, pictureBox7Player, pictureBox8Player, pictureBox9Player, pictureBox10Player, pictureBox11Player };
            dealerPictureBoxes = new PictureBox[] { pictureBox1Dealer, pictureBox2Dealer, pictureBox3Dealer, pictureBox4Dealer, pictureBox5Dealer, pictureBox6Dealer, pictureBox7Dealer, pictureBox8Dealer, pictureBox9Dealer, pictureBox10Dealer, pictureBox11Dealer };

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
            visiblePlayerCards = 2;
            visibleDealerCards = 1;

            // Check if the player can double down
            if (blackjackGame.PlayerHand.CalculateScore() >= 9 && blackjackGame.PlayerHand.CalculateScore() <= 11)
            {
                btnDoubleDown.Enabled = true;
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

        private void UpdateScores()
        {
            // Display player and dealer scores
            lblPlayerScore.Text = ((int)blackjackGame.PlayerHand.CalculateScore()).ToString();
            lblDealerScore.Text = ((int)blackjackGame.DealerHand.CalculateScore()).ToString();
            lblBalance.Text = $"Balance: £{playerBalance}";
            lblBank.Text = $"Bank: £{bankBalance}";
        }

        private void UpdatePictureBoxes()
        {
            UpdatePlayerPictureBoxes();
            UpdateDealerPictureBoxes();
        }

        private void UpdatePlayerPictureBoxes()
        {
            // Display player's cards using PictureBox controls
            for (int i = 0; i < visiblePlayerCards; i++)
            {
                if (i < playerPictureBoxes.Length)
                {
                    playerPictureBoxes[i].Image = blackjackGame.PlayerHand.Cards[i].CardImage;
                    playerPictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    playerPictureBoxes[i].Visible = true; // Make the PictureBox visible
                }
            }

            // Hide remaining player PictureBoxes
            for (int i = visiblePlayerCards; i < playerPictureBoxes.Length; i++)
            {
                if (i < playerPictureBoxes.Length)
                {
                    playerPictureBoxes[i].Visible = false;
                }
            }
        }

        private void UpdateDealerPictureBoxes()
        {
            // Display dealer's cards using PictureBox controls
            for (int i = 0; i < visibleDealerCards; i++)
            {
                if (i < dealerPictureBoxes.Length)
                {
                    dealerPictureBoxes[i].Image = blackjackGame.DealerHand.Cards[i].CardImage;
                    dealerPictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    dealerPictureBoxes[i].Visible = true; // Make the PictureBox visible
                }
            }

            // Hide remaining dealer PictureBoxes
            for (int i = visibleDealerCards; i < dealerPictureBoxes.Length; i++)
            {
                if (i < dealerPictureBoxes.Length)
                {
                    dealerPictureBoxes[i].Visible = false;
                }
            }
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
            else
            {
                // Increment the number of visible player cards
                visiblePlayerCards++;
            }
        }

        private void btnStick_Click(object sender, EventArgs e)
        {
            // Dealer takes cards until reaching at least 17
            blackjackGame.DealerTwist();

            UpdatePictureBoxes();
            UpdateScores();

            // Determine the winner after the dealer has completed their turn
            blackjackGame.DetermineWinner();
            UpdateBalances();
            DisablePlayerButtons();

            // Set the number of visible dealer cards based on the actual number of dealer cards
            visibleDealerCards = blackjackGame.DealerHand.Cards.Count;
        }

        private void btnDoubleDown_Click(object sender, EventArgs e)
        {
            // Double the player's bet
            blackjackGame.PlayerBet *= 2;

            // update players balance
            playerBalance -= blackjackGame.PlayerBet / 2;

            // Player takes one more card
            blackjackGame.PlayerTwist(blackjackGame.PlayerHand);

            // Dealer takes cards until reaching at least 17
            blackjackGame.DealerTwist();

            // Update PictureBoxes and scores
            UpdatePictureBoxes();
            UpdateScores();

            // Update the txtBet text box
            txtBet.Text = blackjackGame.PlayerBet.ToString();

            // Determine the winner
            blackjackGame.DetermineWinner();
            UpdateBalances();
            DisablePlayerButtons();
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Reset the game
            blackjackGame = new BlackjackGame();

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
            playerBalance = MaxStartingAmount;
            lblBalance.Text = $"Balance: £{playerBalance}";

            // Reset bank balance
            bankBalance = 1000;
            lblBank.Text = $"Bank: £{bankBalance}";

            btnStart.Enabled = false;
            btnTwist.Enabled = false;
            btnStick.Enabled = false;
            btnSplit.Enabled = false;
            btnDoubleDown.Enabled = false;
            btnReset.Enabled = false;
        }

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
                MessageBox.Show($"Invalid bet amount. Please enter a bet between {MinBet} and {MaxBet} in multiples of £1.", "Invalid Bet");
            }
        }

        private bool IsValidBet(int betAmount)
        {
            return betAmount >= MinBet && betAmount <= MaxBet && betAmount % 1 == 0;
        }

        private void DisablePlayerButtons()
        {
            // Disable twist, stick, double down, and split buttons
            btnTwist.Enabled = false;
            btnStick.Enabled = false;
            btnDoubleDown.Enabled = false;
            btnSplit.Enabled = false;
            btnContinue.Enabled = true;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            // Reset the game
            blackjackGame = new BlackjackGame();

            // Clear images in PictureBoxes
            ClearPictureBoxes();

            // Enable betting controls
            txtBet.Enabled = true;
            btnBet.Enabled = true;

            // Clear the bet TextBox
            txtBet.Text = string.Empty;

            // Update PictureBoxes and scores
            UpdateScores();
        }

        private void UpdateBalances()
        {
            playerBalance += blackjackGame.PlayerBalance;
            bankBalance += blackjackGame.BankBalance;

            lblBalance.Text = $"Balance: £{playerBalance}";
            lblBank.Text = $"Bank: £{bankBalance}";
        }
    }

    public class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public Image CardImage { get; }

        public Card (Suit suit, Rank rank, Image cardImage) 
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

        // Method to represent the card as a string (e.g., "10 of Hearts")
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
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

        public override string ToString()
        {
            return string.Join(", ", cards);
        }
    }

    public class BlackjackGame
    {
        private Deck deck ;
        public Hand DealerHand { get; }
        public Hand PlayerHand { get; }
        public int PlayerBet { get; set; }
        public int PlayerBalance { get; set; }
        public int BankBalance { get; set; }

        public BlackjackGame()
        {
            deck = new Deck();
            DealerHand = new Hand();
            PlayerHand = new Hand();
            PlayerBet = 2; // Initial bet, adjust as needed
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
            // Dealer takes cards until reaching at least 17
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
                    return;
                }

                // Player wins with blackjack
                MessageBox.Show("You win with blackjack!", "Game Over");
                PlayerBalance += (int)(PlayerBet * 2.5);
                BankBalance -= (int)(PlayerBet * 1.5);
                return;
            }

            // Player busts
            if (playerScore > 21)
            {
                MessageBox.Show("You lose!", "Game Over");
                BankBalance += PlayerBet;
                return;
            }

            // Dealer draws until reaching 17 or higher
            while (dealerScore < 17)
            {
                Card drawnCard = deck.DealCard(); // Assuming deck is accessible here
                DealerHand.AddCard(drawnCard);
                dealerScore = DealerHand.CalculateScore();
            }

            // Dealer busts
            if (dealerScore > 21)
            {
                MessageBox.Show("You win!", "Game Over");
                PlayerBalance += PlayerBet * 2;
                BankBalance -= PlayerBet;
                return;
            }

            // Compare scores to determine the winner
            if (playerScore < dealerScore || dealerScore == 21)
            {
                MessageBox.Show("You lose!", "Game Over");
                BankBalance += PlayerBet;
            }
            else if (playerScore > dealerScore || playerScore == 21)
            {
                MessageBox.Show("You win!", "Game Over");
                PlayerBalance += PlayerBet * 2;
                BankBalance -= PlayerBet;
            }
            else if (playerScore == dealerScore)
            {
                MessageBox.Show("It's a draw!", "Game Over");
                PlayerBalance += PlayerBet;
            }
            else
            {
                if (playerScore == 21 && (dealerScore < 21 || dealerScore > 21))
                {
                    MessageBox.Show("You win!", "Game Over");
                    PlayerBalance += PlayerBet * 2;
                    BankBalance -= PlayerBet;
                }
                else if (dealerScore == 21 && (playerScore < 21 || playerScore > 21))
                {
                    MessageBox.Show("You lose!", "Game Over");
                    BankBalance += PlayerBet;
                }
            }
        }
    }
}