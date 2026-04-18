using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class ChatMessageNote : ChatMessage
    {
        public MessageNoteType MessageNoteType { get; set; }
        public string Payload { get; set; }
    }
}