namespace LittleGoat.Models
{
    using System.Collections.Generic;

    public class Cards
    {
        private List<Card> _cards;

        private Cards(List<Card> cards)
        {
            _cards = cards;
        }

        public List<Card> GetAllCards()
        {
            return _cards;
        }
        
        public static Cards AllCards
        {
            get
            {
                List<Card> cards = new List<Card>();

                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Ace, Score = -1, FileName = "ace_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Two, Score = 2, FileName = "2_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Three, Score = 3, FileName = "3_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Four, Score = 4, FileName = "4_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Five, Score = 5, FileName = "5_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Six, Score = 6, FileName = "6_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Seven, Score = 0, FileName = "7_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Eight, Score = 8, FileName = "8_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Nine, Score = 9, FileName = "9_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Ten, Score = 10, FileName = "10_of_clubs.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Jack, Score = 11, FileName = "jack_of_clubs2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.Queen, Score = 12, FileName = "queen_of_clubs2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Clubs, Value = CardValue.King, Score = 13, FileName = "king_of_clubs2.jpg" });

                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Ace, Score = -1, FileName = "ace_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Two, Score = 2, FileName = "2_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Three, Score = 3, FileName = "3_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Four, Score = 4, FileName = "4_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Five, Score = 5, FileName = "5_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Six, Score = 6, FileName = "6_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Seven, Score = 0, FileName = "7_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Eight, Score = 8, FileName = "8_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Nine, Score = 9, FileName = "9_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Ten, Score = 10, FileName = "10_of_spades.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Jack, Score = 11, FileName = "jack_of_spades2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.Queen, Score = 12, FileName = "queen_of_spades2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Spades, Value = CardValue.King, Score = 13, FileName = "king_of_spades2.jpg" });

                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Ace, Score = -1, FileName = "ace_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Two, Score = 2, FileName = "2_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Three, Score = 3, FileName = "3_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Four, Score = 4, FileName = "4_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Five, Score = 5, FileName = "5_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Six, Score = 6, FileName = "6_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Seven, Score = 0, FileName = "7_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Eight, Score = 8, FileName = "8_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Nine, Score = 9, FileName = "9_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Ten, Score = 10, FileName = "10_of_hearts.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Jack, Score = 11, FileName = "jack_of_hearts2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.Queen, Score = 12, FileName = "queen_of_hearts2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Hearts, Value = CardValue.King, Score = 0, FileName = "king_of_hearts2.jpg" });

                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Ace, Score = -1, FileName = "ace_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Two, Score = 2, FileName = "2_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Three, Score = 3, FileName = "3_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Four, Score = 4, FileName = "4_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Five, Score = 5, FileName = "5_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Six, Score = 6, FileName = "6_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Seven, Score = 0, FileName = "7_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Eight, Score = 8, FileName = "8_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Nine, Score = 9, FileName = "9_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Ten, Score = 10, FileName = "10_of_diamonds.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Jack, Score = 11, FileName = "jack_of_diamonds2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.Queen, Score = 12, FileName = "queen_of_diamonds2.jpg" });
                cards.Add(new Card() { Symbol = CardSymbol.Diamonds, Value = CardValue.King, Score = 0, FileName = "king_of_diamonds2.jpg" });

                var gameCards = new Cards(cards);
                return gameCards;
            }
        }
    }
}