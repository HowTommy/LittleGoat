namespace LittleGoat.Models
{
    using System;

    public class ChatMessage
    {
        public int Id { get; set; }

        public string PlayerId { get; set; }

        public string PlayerName { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}