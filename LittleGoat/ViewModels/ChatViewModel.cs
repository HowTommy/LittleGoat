namespace LittleGoat.ViewModels
{
    using LittleGoat.Models;
    using System.Collections.Generic;

    public class ChatViewModel
    {
        public string SerieId { get; set; }

        public List<ChatMessage> ChatMessages { get; set; }

        public bool Collapsed { get; set; }
    }
}