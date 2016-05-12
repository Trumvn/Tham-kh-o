using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Faq.Dtos;
using AutoMapper.Internal;

namespace AsiaMoneyer.Faq
{
    public class FaqAppService : AppServiceBase, IFaqAppService
    {
        public async Task<List<Dtos.FaqDto>> GetFaqs()
        {
            List<Entities.FrequentlyAskedQuestion> faqEntities = null;

            await Task.Factory.StartNew(() => { 
                faqEntities = this.UnitOfWork.FaqRepository.List.OrderByDescending(x=>x.CreatedDate).ToList();
            });

            List<Dtos.FaqDto> faqs = AutoMapper.Mapper.Map<List<AsiaMoneyer.Entities.FrequentlyAskedQuestion>, List<Dtos.FaqDto>>(faqEntities);

            return faqs;
        }

        public async Task createQuestion(Dtos.FaqDto faqDto)
        {
            var faq = new Entities.FrequentlyAskedQuestion()
            {
                Id = Guid.NewGuid().ToString(),
                Lang = faqDto.Lang,
                FullName = faqDto.FullName,
                EmailAddress = faqDto.EmailAddress,
                Question = faqDto.Question,
                AssistantName = faqDto.AssistantName,
                Answer = faqDto.Answer,
                Tags = faqDto.Tags,
                Voting = faqDto.Voting,
                DisplayOrder = faqDto.DisplayOrder,
                IsPublish = faqDto.IsPublish,
                CreatedDate = DateTime.Now
            };

            await Task.Factory.StartNew(() =>
            {
                this.UnitOfWork.FaqRepository.Add(faq);
                this.UnitOfWork.Save(this.UserId);
            });

        }

        public async Task DeleteFag(string id)
        {
            await Task.Factory.StartNew(() =>
            {
                this.UnitOfWork.FaqRepository.Delete(id);
                this.UnitOfWork.Save(this.UserId);
            });
        }

        public async Task editQuestion(string id, FaqDto faqDto)
        {
         
            var faq = new Entities.FrequentlyAskedQuestion()
            {
                Id = faqDto.Id,
                Lang = faqDto.Lang,
                FullName = faqDto.FullName,
                EmailAddress = faqDto.EmailAddress,
                Question = faqDto.Question,
                AssistantName = faqDto.AssistantName,
                Answer = faqDto.Answer,
                Tags = faqDto.Tags,
                Voting = faqDto.Voting,
                DisplayOrder = faqDto.DisplayOrder,
                IsPublish = faqDto.IsPublish,
                CreatedDate = DateTime.Now
            };

            await Task.Factory.StartNew(() => 
            { 
                this.UnitOfWork.FaqRepository.Save(faq);
                this.UnitOfWork.Save(this.UserId);
            });
        }

        public async Task<Dtos.FaqDto> GetFaq(string id)
        {
            Entities.FrequentlyAskedQuestion  faqEntity = null;

            faqEntity = await Task.Factory.StartNew(() => this.UnitOfWork.FaqRepository.Get(id));

            Dtos.FaqDto faqDto = AutoMapper.Mapper.Map<AsiaMoneyer.Entities.FrequentlyAskedQuestion, Dtos.FaqDto>(faqEntity);

            return faqDto;
        }
    }
}
