using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Responses.ChatCommon;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Mapper
{
    public static class ChatCommonSessionMapper
    {
        public static async Task<List<ChatCommonSessionEntity>> MapListAsync(
            IEnumerable<ChatCommonSession> sessions,
            IDriverRepository driverRepository,
            IProviderReposiotry providerReposiotry)
        {
            var result = new List<ChatCommonSessionEntity>();

            foreach (var session in sessions)
            {
                result.Add(await MapAsync(session, driverRepository, providerReposiotry));
            }

            return result;
        }

        public static async Task<ChatCommonSessionEntity> MapAsync(
            ChatCommonSession session,
            IDriverRepository driverRepository,
            IProviderReposiotry providerReposiotry)
        {
            var entity = new ChatCommonSessionEntity
            {
                Id = session.Id,
                ParticipantId = session.ParticipantId,
                PersonId = session.PersonId.ToString(),
                IsActive = session.IsActive,
                ParticipantType = session.ParticipantType,
                CreatedAt = session.CreatedAt.ToString("O"),
                ClosedAt = session.ClosedAt?.ToString("O"),
                Messages = session.Messages?.OrderBy(x => x.Id).Select(MapMessage).ToList(),
                UnRead = session.UnRead
            };

            switch (session.ParticipantType)
            {
                case ParticipantType.Driver:
                    var driver = (await driverRepository.GetAsync(
                        x => x.Id == session.ParticipantId,
                        query => query.Include(x => x.Person))).FirstOrDefault();

                    if (driver != null)
                    {
                        var fullName = driver.Person == null
                            ? null
                            : $"{driver.Person.FirstName} {driver.Person.LastName}".Trim();

                        entity.DisplayName = fullName;
                        entity.FullName = fullName;
                        entity.Name = fullName;
                        entity.Title = "Driver";
                        entity.Subtitle = driver.Email;
                        entity.ProfilePicture = driver.ProfilePhoto;
                    }
                    break;

                case ParticipantType.Provider:
                    var provider = (await providerReposiotry.GetAsync(x => x.Id == session.ParticipantId)).FirstOrDefault();

                    if (provider != null)
                    {
                        var fullName = $"{provider.ManagerFirstName} {provider.ManagerLastName}".Trim();
                        if (string.IsNullOrWhiteSpace(fullName))
                        {
                            fullName = provider.Name;
                        }

                        entity.DisplayName = provider.Name;
                        entity.FullName = fullName;
                        entity.Name = provider.Name;
                        entity.Title = "Provider";
                        entity.Subtitle = provider.Email;
                        entity.ProfilePicture = null;
                    }
                    break;
            }

            return entity;
        }

        private static ChatCommonMessageEntity MapMessage(ChatCommonMessage message)
        {
            return new ChatCommonMessageEntity
            {
                Id = message.Id,
                ChatCommonSessionId = message.ChatCommonSessionId,
                SenderType = message.SenderType,
                SenderId = message.SenderId,
                PersonId = message.PersonId.ToString(),
                Content = message.Content,
                SentAt = message.SentAt.ToString("O"),
                IsInternalNote = message.IsInternalNote,
                Type = message.Type,
                IsSeen = message.IsSeen,
                FileUrl = message.FileUrl,
                FileThumbnailUrl = message.FileThumbnailUrl,
                FileExtension = message.FileExtension,
                FileSize = message.FileSize,
                DurationSeconds = message.DurationSeconds,
                MessageNoteType = message is ChatCommonMessageNote note ? note.MessageNoteType : null
            };
        }
    }
}
