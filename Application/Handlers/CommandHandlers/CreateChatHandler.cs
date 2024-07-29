using Bazaro.Application.Commands;
using Bazaro.Application.Mapper;
using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using Bazaro.Application.Responses.Invoice;
using Bazaro.Application.Services;
using Bazaro.Application.Validator;
using Bazaro.Core.Entities;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CreateChatHandler : IRequestHandler<CreateChatCommand, CommandResponse>
    {
        private readonly IInboxRepository _inboxRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IInboxStatusRepository _inboxStatusRepository;

        public CreateChatHandler(IInboxRepository inboxRepository, IChatRepository chatRepository, IInboxStatusRepository inboxStatusRepository)
        {
            _inboxRepository = inboxRepository;
            _chatRepository = chatRepository;
            _inboxStatusRepository = inboxStatusRepository;
        }

        public async Task<CommandResponse> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var _now = DateTime.Now;
            // InboxStatusResponse inboxStatusResponse = null;
            var results = new CreateChatCommandValidator().Validate(request);

            if (!results.IsValid)
                return new HasError(results);

            var chat = new Chat()
            {
                InboxId = request.InboxId,
                TypeId = request.TypeId,
                Body = request.Body,
                ShopUserId = request.ShopUserId,
                Seen = false,
                IsActive = true,
                CreatedAt = _now,
            };

            try
            {
                if (request.File != null && request.File.Length > 0)
                {
                    // var isAudioFile = true;//request.File.ContentType.ToLower().Contains("audio");
                    var id = Guid.NewGuid().ToString("N");
                    var extension = Path.GetExtension(request.File.FileName);
                    var fileName = $"{id}{extension}";
                    var folder = $"C:\\content\\storage\\upload\\chat\\";

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    //string ConvertTempKey = isAudioFile ? "temp_" : "";

                    //string temp_filePath = $"C:\\content\\storage\\upload\\chat\\{ConvertTempKey}{fileName}";
                    string temp_filePath = $"C:\\content\\storage\\upload\\chat\\{fileName}";
                    using (Stream fileStream = new FileStream(temp_filePath, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Position = 0;
                        request.File.CopyTo(fileStream);
                    }

                    //if (isAudioFile)
                    //{
                    //    string targetFilePath = $"C:\\content\\storage\\upload\\chat\\{fileName}";
                    //    new FileConvertor().Convert2Mp3(Path.Combine(AppContext.BaseDirectory, "Tools\\ffmpeg\\bin\\ffmpeg.exe"),
                    //        temp_filePath, targetFilePath);

                    //   // File.Delete(temp_filePath);
                    //}

                    chat.File = $"https://api.bazaro.ir/upload/chat/{fileName}";
                }
            }
            catch (Exception e)
            {
                return new CommandResponse() { Success = false, StatusCode = "500", Message = e.Message };
            }

            var resultModel = await _chatRepository.AddAsync(chat);

            var inbox = await _inboxRepository.GetByIdAsync(request.InboxId);
            inbox.LastChatId = resultModel.Id;

            //if (inbox.InboxStatusId != (int)ShopInboxStatus.NewOrder)
            //{
            //    inbox.InboxStatusId = (int)ShopInboxStatus.NewOrder;

            //    var inboxStatus = await _inboxStatusRepository.GetByIdAsync((int)ShopInboxStatus.NewOrder);
            //    inboxStatusResponse = InboxStatusMapper.Mapper.Map<InboxStatusResponse>(inboxStatus);
            //}

            //inbox.UpdatedAt = _now;
            await _inboxRepository.UpdateAsync(inbox);

            return new Success()
            {
                Id = resultModel.Id,
                Data = ChatMapper.Mapper.Map<ChatResponse>(resultModel)

                //Data = new CreateInvoiceResponse()
                //{
                //    ChatData = ChatMapper.Mapper.Map<ChatResponse>(resultModel),
                //    InboxStatusData = inboxStatusResponse
                //}
            };
        }
    }
}
