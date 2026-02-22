namespace Lab4.Models
{
    public class ChatMessage
    {
        public string Role { get; set; } = "";     // "user" hoặc "assistant"
        public string Content { get; set; } = "";  // nội dung tin nhắn
    }
}