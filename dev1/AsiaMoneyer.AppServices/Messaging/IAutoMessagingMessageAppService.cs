using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public interface IAutoMessagingMessageAppService
    {
        Dtos.AutoMessagingMessageDto SendEmail(String templateId, Dictionary<string, string> values);
        List<Dtos.AutoMessagingTemplateDto> GetMessagingTemplates();
        Dtos.GetMessagingTemplateDto GetMessagingContent();
        List<Dtos.AutoMessagingMessageDto> GetMessages();
        Dtos.GetMessageDto GetMessageTitles();
        int CountMessages();

        Dtos.GetTemplateContentListDto GetTemplateContentTitles(String templateId);
        int CountTemplateContents(String templateId);

        Dtos.AutoMessagingMessageDto GetMailMessage(String messageId);
        Dtos.TemplateContentDto GetMailTemplateContent(String contentId);

        Dtos.TemplateContentDto SaveMailTemplateContent(Dtos.TemplateContentDto contentDto);
    }
}
