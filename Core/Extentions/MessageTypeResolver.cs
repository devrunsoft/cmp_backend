using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Extentions
{
	public static class MessageTypeResolver
	{
        public static MessageType ResolveMessageType(string? contentType, string? extension)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    return MessageType.IMAGE;
                if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
                    return MessageType.VIDEO;
                if (contentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
                    return MessageType.AUDIO;
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                var ext = extension.ToLowerInvariant();
                if (ext is "jpg" or "jpeg" or "png" or "gif" or "bmp" or "webp")
                    return MessageType.IMAGE;
                if (ext is "mp4" or "mov" or "avi" or "mkv" or "webm")
                    return MessageType.VIDEO;
                if (ext is "mp3" or "wav" or "aac" or "ogg" or "flac")
                    return MessageType.AUDIO;
            }

            return MessageType.FILE;
        }
    }
}

