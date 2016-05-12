using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace AsiaMoneyer.EntityFramework
{
    public class AsiaMoneyerDbContext : DbContextBase
    {
        //TODO: Define an IDbSet for each Entity...
        public virtual IDbSet<Entities.Project> ProjectDbSet { get; set; }
        public virtual IDbSet<Transaction> TransactionDbSet { get; set; }
        public virtual IDbSet<Entities.AuditLog> AuditLogDbSet { get; set; }
        public virtual IDbSet<UserComment> UserCommentDbSet { get; set; }
        public virtual IDbSet<ProjectPermission> ProjectPermissionDbSet { get; set; }
        public virtual IDbSet<Entities.Account> AccountDbSet { get; set; }
        public virtual IDbSet<Entities.Category> CategoryDbSet { get; set; }
        public virtual IDbSet<Entities.CategoryBudget> CategoryBudgetDbSet { get; set; }
        public virtual IDbSet<RecurringTransaction> RecurringTransationDbSet { get; set; }
        public virtual IDbSet<TimeFrequency> TimeFrequencyDbSet { get; set; }
        public virtual IDbSet<ProjectMember> ProjectMemberDbSet { get; set; }
        public virtual IDbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestionDbSet { get; set; }
        public virtual IDbSet<Client> ClientDbSet { get; set; }
        public virtual IDbSet<UserPhoto> UserPhotoDbSet { get; set; }
        public virtual IDbSet<Entities.User> UserDbSet { get; set; }
        public virtual IDbSet<Entities.AutoMessagingType> AutoMessagingTypeDbSet { get; set; }
        public virtual IDbSet<Entities.AutoMessagingSender> AutoMessagingSenderDbSet { get; set; }
        public virtual IDbSet<Entities.AutoMessagingTemplate> AutoMessagingTemplateDbSet { get; set; }
        public virtual IDbSet<Entities.AutoMessagingTemplateContent> AutoMessagingTemplateContentDbSet { get; set; }
        public virtual IDbSet<Entities.AutoMessagingDataMapping> AutoMessagingDataMappingDbSet { get; set; }
        public virtual IDbSet<Entities.AutoMessagingMessage> AutoMessagingMessageDbSet { get; set; }
        public virtual IDbSet<Entities.SysSetting> SysSettingDbSet { get; set; }
        // Billing
        public virtual IDbSet<Entities.TargetMarket> TargetMarketDbSet { get; set; }
        public virtual IDbSet<Entities.SubscriptionType> SubscriptionTypeDbSet { get; set; }
        public virtual IDbSet<Entities.Product> ProductDbSet { get; set; }
        public virtual IDbSet<Entities.ProductPrice> ProductPriceDbSet { get; set; }
        public virtual IDbSet<Entities.Subscription> SubscriptionDbSet { get; set; }
        public virtual IDbSet<Entities.PaymentMethod> PaymentMethodDbSet { get; set; }
        public virtual IDbSet<Entities.Invoice> InvoiceDbSet { get; set; }
        public virtual IDbSet<Entities.PaymentGateway> PaymentGatewayDbSet { get; set; }
        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public AsiaMoneyerDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in AsiaMoneyerDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of AsiaMoneyerDbContext since ABP automatically handles it.
         */
        public AsiaMoneyerDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.ProjectMember>()
                   .HasKey(c => new { c.ProjectId, c.ClientId });

            /*modelBuilder.Entity<Entities.Project>()
                .HasMany(c => c.ProjectUserProfiles)
                .WithRequired()
                .HasForeignKey(c => c.UserProfileId);

            modelBuilder.Entity<Entities.UserProfile>()
                .HasMany(c => c.ProjectUserProfiles)
                .WithRequired()
                .HasForeignKey(c => c.ProjectId);  */
        }

        // This is overridden to prevent someone from calling SaveChanges without specifying the user making the change
        public override int SaveChanges()
        {
            throw new InvalidOperationException("User ID must be provided");
        }

        public int SaveChanges(string userId)
        {
            // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
            foreach (var ent in this.ChangeTracker.Entries<Base.IAuditable>().Where(p => p.State == System.Data.Entity.EntityState.Added || p.State == System.Data.Entity.EntityState.Deleted || p.State == System.Data.Entity.EntityState.Modified))
            {
                // For each changed record, get the audit record entries and add them
                foreach (Entities.AuditLog x in GetAuditRecordsForChange(ent, userId))
                {
                    this.AuditLogDbSet.Add(x);
                }
            }

            // Call the original SaveChanges(), which will save both the changes made and the audit records
            return base.SaveChanges();
        }

        private List<Entities.AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            List<Entities.AuditLog> result = new List<Entities.AuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            // Get the Table() attribute, if one exists
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;

            if (dbEntry.State == System.Data.Entity.EntityState.Added)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                {
                    result.Add(new Entities.AuditLog()
                    {
                        Id = Guid.NewGuid(),
                        UserID = userId,
                        EventDateUTC = changeTime,
                        EventType = Commons.Constants.AUDIT_LOG_EVENT_TYPE_ADD,
                        TableName = tableName,
                        RecordID = Convert.ToString(dbEntry.CurrentValues.GetValue<object>(keyName)),
                        ColumnName = propertyName,
                        NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                    }
                    );
                }
            }
            else if (dbEntry.State == System.Data.Entity.EntityState.Deleted)
            {
                // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                result.Add(new Entities.AuditLog()
                {
                    Id = Guid.NewGuid(),
                    UserID = userId,
                    EventDateUTC = changeTime,
                    EventType = Commons.Constants.AUDIT_LOG_EVENT_TYPE_DELETE, // Deleted
                    TableName = tableName,
                    RecordID = Convert.ToString(dbEntry.OriginalValues.GetValue<object>(keyName)),
                    ColumnName = "*ALL",
                    //NewValue = (dbEntry.OriginalValues.ToObject() is IDescribableEntity) ? (dbEntry.OriginalValues.ToObject() as IDescribableEntity).Describe() : dbEntry.OriginalValues.ToObject().ToString()
                }
                    );
            }
            else if (dbEntry.State == System.Data.Entity.EntityState.Modified)
            {
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {
                    // For updates, we only want to capture the columns that actually changed
                    var originalValue = dbEntry.OriginalValues.GetValue<object>(propertyName);
                    var currentValue = dbEntry.CurrentValues.GetValue<object>(propertyName);
                    if (!object.Equals(originalValue, currentValue))
                    {
                        result.Add(new Entities.AuditLog()
                        {
                            Id = Guid.NewGuid(),
                            UserID = userId,
                            EventDateUTC = changeTime,
                            EventType = Commons.Constants.AUDIT_LOG_EVENT_TYPE_MODIFY,    // Modified
                            TableName = tableName,
                            RecordID = Convert.ToString(dbEntry.OriginalValues.GetValue<object>(keyName)),
                            ColumnName = propertyName,
                            OriginalValue = originalValue == null ? null : originalValue.ToString(),
                            NewValue = currentValue == null ? null : currentValue.ToString()
                        }
                        );
                    }
                }
            }
            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities

            return result;
        }
    }
}
