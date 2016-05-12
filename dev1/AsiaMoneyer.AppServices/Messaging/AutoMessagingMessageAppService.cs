using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public class AutoMessagingMessageAppService : AppServiceBase, IAutoMessagingMessageAppService
    {        
        private readonly IAutoMessagingDatabindingHelperAppService databindingHelper;

        public AutoMessagingMessageAppService(IAutoMessagingDatabindingHelperAppService databindingHelper)
        {
            this.databindingHelper = databindingHelper;
        }

        #region Override Methods
        public Dtos.AutoMessagingMessageDto SendEmail(string templateId, Dictionary<string, string> values)
        {
            AutoMessagingTemplateContent messageTemplate = this.GetAvailableMessageTemplate(templateId);

            int senderId = this.UnitOfWork.AutoMessagingTemplateRepository.List.Where(x => x.Id == templateId).Select(s => s.AutoMessagingSenderId).FirstOrDefault();

            AutoMessagingMessage message = new AutoMessagingMessage()
            {
                Id = Guid.NewGuid().ToString(),
                AutoMessagingTemplateContentId = messageTemplate.Id,
                AutoMessagingSenderId = senderId,
                MessagingSubject = databindingHelper.bind(messageTemplate.MessagingSubject, values),
                MessagingFromName = databindingHelper.bind(messageTemplate.MessagingFromName, values),
                MessagingFromEmailAddress = databindingHelper.bind(messageTemplate.MessagingFromEmailAddress, values),
                MessagingTo = databindingHelper.bind(messageTemplate.MessagingTo, values),
                MessagingCc = databindingHelper.bind(messageTemplate.MessagingCc, values),
                MessagingBcc = databindingHelper.bind(messageTemplate.MessagingBcc, values),
                MessagingContent = databindingHelper.bind(messageTemplate.MessagingContent, values),
                Tags = messageTemplate.Tags,
                IsSent = false,
                IsMarkedAsRead = false,
                SentDate = null,
                CreatedDate = DateTime.Now
            };

            this.UnitOfWork.AutoMessagingMessageRepository.Add(message);
            this.UnitOfWork.Save(this.UserId);

            return null;
        }

        public List<Dtos.AutoMessagingTemplateDto> GetMessagingTemplates()
        {
            List<AutoMessagingTemplate> messagingTemplates = this.UnitOfWork.AutoMessagingTemplateRepository.List.OrderBy(x => x.MessagingTemplateName).ToList();
            List<Dtos.AutoMessagingTemplateDto> messagingTemplateDtos = AutoMapper.Mapper.Map<List<Entities.AutoMessagingTemplate>, List<Dtos.AutoMessagingTemplateDto>>(messagingTemplates);
            return messagingTemplateDtos;
        }

        public Dtos.GetMessagingTemplateDto GetMessagingContent()
        {
            Dtos.GetMessagingTemplateDto messagingContentDto = new Dtos.GetMessagingTemplateDto();
            messagingContentDto.MessagingTemplates = this.GetMessagingTemplates();

            return messagingContentDto;
        }

        public List<Dtos.AutoMessagingMessageDto> GetMessages()
        {
            List<AutoMessagingMessage> messages = this.UnitOfWork.AutoMessagingMessageRepository.List.OrderByDescending(x => x.CreatedDate).ToList();
            List<Dtos.AutoMessagingMessageDto> messagingDtos = AutoMapper.Mapper.Map<List<Entities.AutoMessagingMessage>, List<Dtos.AutoMessagingMessageDto>>(messages);
            return messagingDtos;
        }

        public Dtos.GetMessageDto GetMessageTitles()
        {
            Dtos.GetMessageDto messagesDto = new Dtos.GetMessageDto();
            List<AutoMessagingMessageModel> messageModels = this.UnitOfWork.AutoMessagingMessageRepository.GetMessageTitles();
            List<Dtos.AutoMessagingMessageDto> messagingDtos = AutoMapper.Mapper.Map<List<Entities.AutoMessagingMessageModel>, List<Dtos.AutoMessagingMessageDto>>(messageModels);
            int totalMessage = this.CountMessages();
            messagesDto.Messages = messagingDtos;
            messagesDto.Total = totalMessage;
            return messagesDto;
        }

        public int CountMessages()
        {
            return this.UnitOfWork.AutoMessagingMessageRepository.CountMessages();
        }

        public Dtos.GetTemplateContentListDto GetTemplateContentTitles(String templateId)
        {
            Dtos.GetTemplateContentListDto templateContentTitleDto = new Dtos.GetTemplateContentListDto();

            List<TemplateContentModel> templateContentModels = this.UnitOfWork.AutoMessagingTemplateContentRepository.GetTemplateContentTitles(templateId);
            List<Dtos.TemplateContentDto> templateContentDtos = AutoMapper.Mapper.Map<List<Entities.TemplateContentModel>, List<Dtos.TemplateContentDto>>(templateContentModels);

            templateContentTitleDto.TemplateId = templateId;
            templateContentTitleDto.TemplateName = this.UnitOfWork.AutoMessagingTemplateRepository.List.Where(x=>x.Id == templateId).Select(x=>x.MessagingTemplateName).FirstOrDefault();

            int total = this.CountTemplateContents(templateId);
            templateContentTitleDto.TemplateContentList = templateContentDtos;
            templateContentTitleDto.Total = total;

            return templateContentTitleDto;
        }

        public int CountTemplateContents(String templateId)
        {
            return this.UnitOfWork.AutoMessagingTemplateContentRepository.CountTemplateContent(templateId);
        }

        public Dtos.AutoMessagingMessageDto GetMailMessage(String messageId)
        {
            Entities.AutoMessagingMessage message = this.UnitOfWork.AutoMessagingMessageRepository.Get(messageId);
            Dtos.AutoMessagingMessageDto messageDto = AutoMapper.Mapper.Map<Entities.AutoMessagingMessage, Dtos.AutoMessagingMessageDto>(message);

            return messageDto;
        }

        public Dtos.TemplateContentDto GetMailTemplateContent(String contentId)
        {
            Entities.AutoMessagingTemplateContent templateContent = this.UnitOfWork.AutoMessagingTemplateContentRepository.Get(contentId);
            Dtos.TemplateContentDto contentDto = AutoMapper.Mapper.Map<Entities.AutoMessagingTemplateContent, Dtos.TemplateContentDto>(templateContent);

            return contentDto;

        }

        public Dtos.TemplateContentDto SaveMailTemplateContent(Dtos.TemplateContentDto contentDto)
        {
            if (contentDto != null)
            {
                if(!String.IsNullOrEmpty(contentDto.Id))
                {
                    AutoMessagingTemplateContent templateContentEntity = AutoMapper.Mapper.Map<Messaging.Dtos.TemplateContentDto, AutoMessagingTemplateContent>(contentDto);
                    this.UnitOfWork.AutoMessagingTemplateContentRepository.Update(templateContentEntity, x => x.MessagingFromName, x => x.MessagingFromEmailAddress, x => x.MessagingTo, x => x.MessagingCc, x => x.MessagingBcc, x => x.MessagingSubject, x => x.Lang, x => x.IsPublish, x => x.FromDate, x => x.EndDate);
                }
                else
                {
                    contentDto.Id = Guid.NewGuid().ToString();
                    contentDto.CreatedDate = DateTime.Now;
                    AutoMessagingTemplateContent templateContentEntity = AutoMapper.Mapper.Map<Messaging.Dtos.TemplateContentDto, AutoMessagingTemplateContent>(contentDto);
                    this.UnitOfWork.AutoMessagingTemplateContentRepository.Add(templateContentEntity);
                }
                this.UnitOfWork.Save();

                return GetMailTemplateContent(contentDto.Id);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        #endregion
        #region Private Methods
        private AutoMessagingTemplateContent GetAvailableMessageTemplate(string templateId)
        {
            DateTime today = DateTime.Now;
            AutoMessagingTemplateContent messageContent = this.UnitOfWork.AutoMessagingTemplateContentRepository.List.Where(x => x.AutoMessagingTemplateId == templateId && x.IsPublish == true && x.FromDate < today && (x.EndDate == null || x.EndDate > today)).FirstOrDefault();
            return messageContent;
        }
        #endregion

    }
}
