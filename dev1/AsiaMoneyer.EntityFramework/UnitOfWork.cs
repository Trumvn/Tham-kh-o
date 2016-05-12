using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.EntityFramework;
using AsiaMoneyer.EntityFramework.Repositories;

namespace AsiaMoneyer
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed = false;

        public IDbContextProvider<EntityFramework.AsiaMoneyerDbContext> _dbContextProvider = new AsiaMoneyerDbContextProvider();

        #region Members
        private Account.IAccountRepository _accountRepository = null;
        private Project.IProjectRepository _projectRepository = null;
        private Faq.IFaqRepository _faqRepository = null;
        private Category.ICategoryRepository _categoryRepository = null;
        private Category.ICategoryBudgetRepository _categoryBudgetRepository = null;
        private Project.ITransactionRepository _transactionRepository = null;
        private Project.IRecurringTransactionRepository _recurringTransactionRepository = null;

        private AuditLog.IAuditLogRepository _auditLogRepository = null;
        private UserComments.IUserCommentRepository _userCommentRepository = null;
        private User.IClientRepository _clientRepository = null;
        private User.IUserRepository _userRepository = null;
        private User.IUserPhotoRepository _userPhotoRepository = null;
        private SysSettingRepositoty _sysSettingRepositoty = null;

        private Messaging.IAutoMessagingTypeRepository _autoMessagingTypeRepository = null;
        private Messaging.IAutoMessagingSenderRepository _autoMessagingSenderRepository = null;
        private Messaging.IAutoMessagingTemplateRepository _autoMessagingTemplateRepository = null;
        private Messaging.IAutoMessagingTemplateContentRepository _autoMessagingTemplateContentRepository = null;
        private Messaging.IAutoMessagingDataMappingRepository _autoMessagingDataMappingRepository = null;
        private Messaging.IAutoMessagingMessageRepository _autoMessagingMessageRepository = null;

        public Account.IAccountRepository AccountRepository
        {
            get
            {
                if(this._accountRepository == null)
                {
                    this._accountRepository = new AccountRepository(_dbContextProvider);
                }
                return this._accountRepository;
            }
        }

        public Project.IProjectRepository ProjectRepository
        {
            get
            {
                if (this._projectRepository == null)
                {
                    this._projectRepository = new ProjectRepository(_dbContextProvider);
                }
                return this._projectRepository;
            }
        }

        public Faq.IFaqRepository FaqRepository
        {
            get
            {
                if (this._faqRepository == null)
                {
                    this._faqRepository = new FaqRepository(_dbContextProvider);
                }
                return this._faqRepository;
            }
        }

        public Category.ICategoryRepository CategoryRepository
        {
            get
            {
                if (this._categoryRepository == null)
                {
                    this._categoryRepository = new CategoryRepository(_dbContextProvider);
                }
                return this._categoryRepository;
            }
        }

        public Category.ICategoryBudgetRepository CategoryBudgetRepository
        {
            get
            {
                if (this._categoryBudgetRepository == null)
                {
                    this._categoryBudgetRepository = new CategoryBudgetRepository(_dbContextProvider);
                }
                return this._categoryBudgetRepository;
            }
        }

        public Project.ITransactionRepository TransactionRepository 
        {
            get
            {
                if (this._transactionRepository == null)
                {
                    this._transactionRepository = new TransactionRepository(_dbContextProvider);
                }
                return this._transactionRepository;
            }
        }

        public Project.IRecurringTransactionRepository RecurringTransactionRepository
        {
            get
            {
                if (this._recurringTransactionRepository == null)
                {
                    this._recurringTransactionRepository = new RecurringTransactionRepository(_dbContextProvider);
                }
                return this._recurringTransactionRepository;
            }
        }

        public AuditLog.IAuditLogRepository AuditLogRepository
        {
            get
            {
                if (this._auditLogRepository == null)
                {
                    this._auditLogRepository = new AuditLogRepository(_dbContextProvider);
                }
                return this._auditLogRepository;
            }
        }

        public UserComments.IUserCommentRepository UserCommentRepository
        {
            get
            {
                if (this._userCommentRepository == null)
                {
                    this._userCommentRepository = new UserCommentRepository(_dbContextProvider);
                }
                return this._userCommentRepository;
            }
        }

        public User.IClientRepository ClientRepository {
            get
            {
                if (this._clientRepository == null)
                {
                    this._clientRepository = new ClientRepository(_dbContextProvider);
                }
                return this._clientRepository;
            }
        }

        public User.IUserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(_dbContextProvider);
                }
                return this._userRepository;
            }
        }

        public User.IUserPhotoRepository UserPhotoRepository
        {
            get
            {
                if (this._userPhotoRepository == null)
                {
                    this._userPhotoRepository = new UserPhotoRepository(_dbContextProvider);
                }
                return this._userPhotoRepository;
            }
        }

        public Messaging.IAutoMessagingTypeRepository AutoMessagingTypeRepository {
            get
            {
                if (this._autoMessagingTypeRepository == null)
                {
                    this._autoMessagingTypeRepository = new AutoMessagingTypeRepository(_dbContextProvider);
                }
                return this._autoMessagingTypeRepository;
            }            
        }

        public Messaging.IAutoMessagingSenderRepository AutoMessagingSenderRepository {
            get
            {
                if (this._autoMessagingSenderRepository == null)
                {
                    this._autoMessagingSenderRepository = new AutoMessagingSenderRepository(_dbContextProvider);
                }
                return this._autoMessagingSenderRepository;
            }
        }

        public Messaging.IAutoMessagingTemplateRepository AutoMessagingTemplateRepository {
            get
            {
                if (this._autoMessagingTemplateRepository == null)
                {
                    this._autoMessagingTemplateRepository = new AutoMessagingTemplateRepository(_dbContextProvider);
                }
                return this._autoMessagingTemplateRepository;
            }
        }

        public Messaging.IAutoMessagingTemplateContentRepository AutoMessagingTemplateContentRepository
        {
            get
            {
                if (this._autoMessagingTemplateContentRepository == null)
                {
                    this._autoMessagingTemplateContentRepository = new AutoMessagingTemplateContentRepository(_dbContextProvider);
                }
                return this._autoMessagingTemplateContentRepository;
            }
        }

        public Messaging.IAutoMessagingDataMappingRepository AutoMessagingDataMappingRepository
        {
            get
            {
                if (this._autoMessagingDataMappingRepository == null)
                {
                    this._autoMessagingDataMappingRepository = new AutoMessagingDataMappingRepository(_dbContextProvider);
                }
                return this._autoMessagingDataMappingRepository;
            }
        }

        public Messaging.IAutoMessagingMessageRepository AutoMessagingMessageRepository {
            get
            {
                if (this._autoMessagingMessageRepository == null)
                {
                    this._autoMessagingMessageRepository = new AutoMessagingMessageRepository(_dbContextProvider);
                }
                return this._autoMessagingMessageRepository;
            }
        }

        public SysSettingRepositoty SysSettingRepositoty
        {
            get
            {
                if (this._sysSettingRepositoty == null)
                {
                    this._sysSettingRepositoty = new SysSettingRepositoty(_dbContextProvider);
                }
                return this._sysSettingRepositoty;
            }
        }
        #endregion

        #region Billing
        private Billing.ITargetMarketRepository _targetMarketRepository = null;
        private Billing.ISubscriptionTypeRepository _subscriptionTypeRepository = null;
        private Billing.IProductPriceRepository _productPriceRepository = null;

        private Billing.IProductRepository _productRepository = null;
        private Billing.ISubscriptionRepository _subscriptionRepository = null;
        private Billing.IPaymentMethodRepository _paymentMethodRepository = null;
        private Billing.IInvoiceRepository _invoiceRepository = null;

        public Billing.IProductRepository ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new ProductRepository(_dbContextProvider);
                }
                return this._productRepository;
            }
        }

        public Billing.ISubscriptionRepository SubscriptionRepository
        {
            get
            {
                if (this._subscriptionRepository == null)
                {
                    this._subscriptionRepository = new SubscriptionRepository(_dbContextProvider);
                }
                return this._subscriptionRepository;
            }
        }

        public Billing.IPaymentMethodRepository PaymentMethodRepository
        {
            get
            {
                if (this._paymentMethodRepository == null)
                {
                    this._paymentMethodRepository = new PaymentMethodRepository(_dbContextProvider);
                }
                return this._paymentMethodRepository;
            }
        }

        public Billing.IInvoiceRepository InvoiceRepository
        {
            get
            {
                if (this._invoiceRepository == null)
                {
                    this._invoiceRepository = new InvoiceRepository(_dbContextProvider);
                }
                return this._invoiceRepository;
            }
        }

        public Billing.ITargetMarketRepository TargetMarketRepository
        {
            get
            {
                if (this._targetMarketRepository == null)
                {
                    this._targetMarketRepository = new TargetMarketRepository(_dbContextProvider);
                }
                return this._targetMarketRepository;
            }
        }

        public Billing.ISubscriptionTypeRepository SubscriptionTypeRepository
        {
            get
            {
                if (this._subscriptionTypeRepository == null)
                {
                    this._subscriptionTypeRepository = new SubscriptionTypeRepository(_dbContextProvider);
                }
                return this._subscriptionTypeRepository;
            }
        }

        public Billing.IProductPriceRepository ProductPriceRepository
        {
            get
            {
                if (this._productPriceRepository == null)
                {
                    this._productPriceRepository = new ProductPriceRepository(_dbContextProvider);
                }
                return this._productPriceRepository;
            }
        }

        #endregion

        public void Save()
        {
            string userId = String.Empty;
            this._dbContextProvider.DbContext.SaveChanges(userId);            
        }

        public void Save(string userId)
        {
            this._dbContextProvider.DbContext.SaveChanges(userId);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContextProvider.DbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
