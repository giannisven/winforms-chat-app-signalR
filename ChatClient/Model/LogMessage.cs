using System.Drawing;

namespace ChatClient
{
    public class LogMessage
    {
        public Color MessageColor { get; }

        public string Content { get; }

        public LogMessage(Color messageColor, string content)
        {
            MessageColor = messageColor;
            Content = content;
        }
    }

}
