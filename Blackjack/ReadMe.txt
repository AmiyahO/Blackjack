Blackjack Game Documentation

Overview
This document serves as a README file for the provided Blackjack game code. The purpose is to provide comprehensive information about the implemented features, any incomplete tasks, and explanations for certain design decisions.

Introduction
The provided code is a C# implementation of a simple Blackjack game. The game features a graphical user interface (GUI) built using Windows Forms. Players can place bets, receive cards, and make decisions such as twisting, sticking, doubling down, and splitting.


Implemented Features
- Card Representation: The game uses enums (Suit and Rank) to represent the suits and ranks of playing cards.

- Card Class: The Card class encapsulates information about a playing card, including its suit, rank, and an associated image.

- Deck Class: The Deck class represents a deck of playing cards, with methods for initialization, shuffling, and dealing cards.

- Hand Class: The Hand class represents a player's or dealer's hand, with methods for adding cards, calculating the total score, and handling aces.

- BlackjackGame Class: This class manages the game logic, including player and dealer hands, bets, balances, and determining the winner.

- Betting: Players can place bets within the specified minimum and maximum limits (£2 to £100).

- Gameplay: Players can twist, stick, double down, and split based on the rules of Blackjack.

- Balances: Player and bank balances are displayed and updated throughout the game. To simulate real life playing, the player and bank balances are initialised to £1,000 and £1,000,000 respectively.

- Game Statistics: Wins, losses, ties, and total games played are tracked and displayed in the scoreboard.


Design Decisions
- Splitting Logic: The game allows players to split their hand if they have a pair, and the pair is eligible for splitting. The eligibility criteria for splitting are based on the rank of the first two cards (excluding 5s and 10s).

- Doubling Down: Players can double down if their initial hand score is between 9 and 11. This strategic decision allows players to double their bet and draw only one additional card.

- Betting Controls: Betting controls are disabled after placing a bet to prevent changes during gameplay. This is to ensure game integrity.

- Game Reset: The "Reset" button resets the game to its initial state, including player and bank balances and the scoreboard. This allows for a fresh start without restarting the application.


How to Use
- Starting the Game: Click the "Start" button to begin the game after placing a bet.

- Placing Bets: Enter a bet amount in the provided textbox and click the "Bet" button. Ensure the bet is within the specified limits (£2 to £100).

- Gameplay Actions: Use the "Twist," "Stick," "Double Down," and "Split" buttons to make gameplay decisions.

- Continuing After Round: After a round is complete, click the "Continue" button to proceed to the next round.

- Resetting the Game: Use the "Reset" button to reset the game to its initial state.

- Viewing Scoreboard: Access the scoreboard from the menu to view game statistics. It displays key statistics, including the player's score, the dealer's score, the number of games won, games lost, games tied, and the total number of games played.

- Viewing How To Play: Access the How To Play form from the menu to access detailed instructions on how to play the Blackjack game. The "How to Play" form provides information about the rules and mechanics of the game, assisting players in understanding the gameplay better.


Additional Notes
- User Feedback: The game is designed to provide feedback messages, ensuring players are informed of important events such as insufficient balance for a bet or invalid bet amounts.

- About and Exit Options: The "About" option in the menu displays information about the programmer. The "Exit" option allows users to close the application.


Conclusion
The Blackjack game provides an interactive and functional implementation of the classic card game. Players can enjoy the game while tracking their performance through the scoreboard. Further improvements and features can be considered for future updates.

Programmer: Amirah Yahaya