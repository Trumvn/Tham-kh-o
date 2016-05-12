using Castle.Core;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace AsiaMoneyer
{
    public abstract class DbContextBase : DbContext//, IInitializable
    {
        
        // Summary:
        //     Constructor.  Uses Abp.Configuration.Startup.IAbpStartupConfiguration.DefaultNameOrConnectionString
        //     as connection string.
        protected DbContextBase():base()
        {
            
        }
        //
        // Summary:
        //     Constructor.
        protected DbContextBase(DbCompiledModel model):
            base(model)
        {
            
        }
        //
        // Summary:
        //     Constructor.
        protected DbContextBase(string nameOrConnectionString)
            :base(nameOrConnectionString)
        {
            
        }
        //
        // Summary:
        //     Constructor.
        protected DbContextBase(DbConnection existingConnection, bool contextOwnsConnection)
        {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Constructor.
        protected DbContextBase(ObjectContext objectContext, bool dbContextOwnsObjectContext)
        {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Constructor.
        protected DbContextBase(string nameOrConnectionString, DbCompiledModel model)
        {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Constructor.
        protected DbContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
        {
            throw new NotImplementedException();
        }
        
        //
        // Summary:
        //     Used to trigger entity change events.
        public IEntityChangedEventHelper EntityChangedEventHelper { get; set; }
      
        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
        public override int SaveChanges()
        {
            throw new NotImplementedException();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }*/
        
    }
}
