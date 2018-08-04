namespace LittleGoat.ViewModels
{
    using LittleGoat.DataAccess;
    using LittleGoat.Models;
    using System;
    using System.Collections.Generic;

    public class PlayViewModel
    {
        public string SerieId { get; set; }

        public List<GameCard> FirstTwoCards { get; set; }

        public DateTime DateStopShowFirstTwoCards { get; set; }

        public GameCard CardFromDeck { get; internal set; }

        public List<ChatMessage> LastChatMessages { get; set; }
    }
}