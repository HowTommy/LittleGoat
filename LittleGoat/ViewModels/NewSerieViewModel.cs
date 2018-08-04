namespace LittleGoat.ViewModels
{
    using LittleGoat.Models;
    using System.Collections.Generic;

    public class NewSerieViewModel
    {
        public string Key { get; set; }

        public bool IsCreator { get; set; }

        public List<string> Players { get; set; }

        public string CurrentPlayerName { get; set; }

        public List<ChatMessage> LastChatMessages { get; set; }
    }
}