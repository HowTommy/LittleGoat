namespace LittleGoat.Models
{
    public class Card
    {
        public CardValue Value { get; set; }

        public CardSymbol Symbol { get; set; }

        public CardColor Color
        {
            get
            {
                return Symbol == CardSymbol.Hearts || Symbol == CardSymbol.Diamonds ? CardColor.Red : CardColor.Black;
            }
        }

        public int Score { get; set; }

        public string FileName { get; set; }
    }
}