using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace AsiaMoneyer.Mapping
{
    public static class AutoMapper
    {
        public static void doMapping()
        {
            Mapper.CreateMap<Entities.Project, Project.Dtos.ProjectDto>();
            Mapper.CreateMap<Entities.Project, Project.Dtos.ProjectHeaderDto>();
            Mapper.CreateMap<Entities.FrequentlyAskedQuestion, Faq.Dtos.FaqDto>();
            Mapper.CreateMap<Entities.Category, Project.Dtos.CategoryDto>();
            Mapper.CreateMap<Entities.TimeFrequency, Project.Dtos.TimeFrequencyDto>();
            Mapper.CreateMap<Entities.CategoryBudget, Project.Dtos.CategoryBudgetDto>();
            //    .ForMember(x => x.TimeFrequency, opt => opt.MapFrom(o => new Category.Dtos.TimeFrequencyDto { Id = o.TimeFrequency.Id, TimeFrequencyName = o.TimeFrequency.TimeFrequencyName, Weeks = o.TimeFrequency.Weeks }));
            Mapper.CreateMap<Entities.Account, Project.Dtos.AccountDto>();
            Mapper.CreateMap<Entities.Transaction, Project.Dtos.TransactionDto>()
                .ForMember(x => x.Category, opt => opt.MapFrom(o => new Project.Dtos.CategoryDto { Id = o.CategoryId, CategoryTitle = o.Category.CategoryTitle, CreatedDate = o.Category.CreatedDate, ProjectId = o.Category.ProjectId, IsIncome = o.Category.IsIncome, IsDeleted = o.Category.IsDeleted, HighlightColor = o.Category.HighlightColor }))
                .ForMember(x => x.Account, opt => opt.MapFrom(o => new Project.Dtos.AccountDto { Id = o.AccountId, AccountTitle = o.Account.AccountTitle, CreatedDate = o.Account.CreatedDate, ProjectId = o.Account.ProjectId, IsClosed = o.Account.IsClosed, HighlightColor = o.Account.HighlightColor }))
                .ForMember(x => x.Client, opt => opt.MapFrom(o => new Client.Dtos.ClientDto { Id = o.ClientId, FirstName = o.Client.FirstName, LastName = o.Client.LastName, EmailAddress = o.Client.EmailAddress, UserId = o.Client.UserId, User = new Client.Dtos.UserDto { Id = o.Client.UserId } }));
            Mapper.CreateMap<Entities.RecurringTransaction, Project.Dtos.RecurringTransactionDto>();
            Mapper.CreateMap<Entities.Project, Project.Dtos.SearchResultProjectDto>();
            Mapper.CreateMap<Entities.Project, Dtos.NavigatorProjectDto>();
            Mapper.CreateMap<Entities.AuditLog, AuditLog.Dtos.AuditLogDto>()
                .ForMember(x => x.User, opt => opt.MapFrom(o => new Client.Dtos.UserDto { Id = o.UserID, FirstName = o.User.FirstName, LastName = o.User.LastName, Email = o.User.Email }));
            Mapper.CreateMap<Entities.UserComment, UserComments.Dtos.UserCommentDto>()
                .ForMember(x => x.User, opt => opt.MapFrom(o => new Client.Dtos.UserDto { Id = o.UserId, FirstName = o.User.FirstName, LastName = o.User.LastName, Email = o.User.Email }));
            Mapper.CreateMap<Entities.User, Client.Dtos.UserDto>();
            Mapper.CreateMap<Entities.Client, Client.Dtos.ClientDto>();
            Mapper.CreateMap<Entities.UserClientModel, Client.Dtos.ClientDto>();
            Mapper.CreateMap<Entities.ProjectMember, Project.Dtos.ProjectMemberDto>()
                .ForMember(x => x.Client, opt => opt.MapFrom(o => new Client.Dtos.ClientDto { Id = o.Client.Id, FirstName = o.Client.FirstName, LastName = o.Client.LastName, EmailAddress = o.Client.EmailAddress, UserId = o.Client.UserId, User = new Client.Dtos.UserDto { Id = o.Client.UserId } }));
            Mapper.CreateMap<Entities.AutoMessagingTemplate, Messaging.Dtos.AutoMessagingTemplateDto>();
            Mapper.CreateMap<Entities.AutoMessagingMessage, Messaging.Dtos.AutoMessagingMessageDto>();
            Mapper.CreateMap<Entities.AutoMessagingMessageModel, Messaging.Dtos.AutoMessagingMessageDto>();
            Mapper.CreateMap<Entities.AutoMessagingTemplateContent, Messaging.Dtos.TemplateContentDto>();
            Mapper.CreateMap<Entities.TemplateContentModel, Messaging.Dtos.TemplateContentDto>();
            Mapper.CreateMap<Messaging.Dtos.TemplateContentDto, Entities.AutoMessagingTemplateContent>()
                .ForMember(c => c.AutoMessagingTemplate, option => option.Ignore());
            Mapper.CreateMap<Entities.Subscription, Client.Dtos.SubscriptionDto>()
                .ForMember(x => x.Product, opt => opt.MapFrom(o => new Client.Dtos.ProductDto { Id = o.Product.Id, ProductName = o.Product.ProductName, ProductTitle = o.Product.ProductTitle}));
            Mapper.CreateMap<Entities.Product, Client.Dtos.ProductDto>();
            Mapper.CreateMap<Entities.ProductPrice, Client.Dtos.ProductPriceDto>()
                .ForMember(x => x.SubscriptionType, opt => opt.MapFrom(o => new Client.Dtos.SubscriptionTypeDto { Id = o.SubscriptionType.Id, SubscriptionTypeName = o.SubscriptionType.SubscriptionTypeName, SubscriptionTypeTitle = o.SubscriptionType.SubscriptionTypeTitle, MonthValue = o.SubscriptionType.MonthValue }));
            Mapper.CreateMap<Entities.SubscriptionType, Client.Dtos.SubscriptionTypeDto>();
            Mapper.CreateMap<Entities.Invoice, Client.Dtos.InvoiceDto>();
            Mapper.CreateMap<Entities.PaymentMethod, Client.Dtos.PaymentMethodDto>();
            Mapper.CreateMap<TransactionSumModel, Project.Dtos.TransactionSumDto>();
            Mapper.CreateMap<BudgetSumModel, Project.Dtos.BudgetSumDto>();
        }
    }
}
