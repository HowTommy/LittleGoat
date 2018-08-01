namespace Biquette.ViewModels
{
    using Biquette.DataAccess;
    using System;
    using System.Collections.Generic;

    public class PlayViewModel
    {
        public List<GameCard> FirstTwoCards { get; set; }

        public DateTime DateStopShowFirstTwoCards { get; set; }

        public GameCard CardFromDeck { get; internal set; }
    }
}