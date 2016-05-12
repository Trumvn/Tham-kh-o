using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AsiaMoneyer.Category;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class CategoryRepository : AsiaMoneyerRepositoryBase<Entities.Category>, ICategoryRepository
    {
        public CategoryRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
    
        }

        public List<Entities.Category> GetCategories(string projectId, bool isIncludeClosedCategory)
        {
            List<Entities.Category> categories = this.Get(x => x.ProjectId == projectId && x.IsDeleted != true && (isIncludeClosedCategory || x.IsClosed == isIncludeClosedCategory)).ToList();
            return categories;
        }

        public List<AsiaMoneyer.Entities.Category> GetProjectCategories(string projectId)
        {
            List<Entities.Category> categories = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false && x.IsClosed == false).ToList();
            return categories;
        }
        public string GetCategoryTitle(string categoryId)
        {
            Entities.Category category = this.Get(x=>x.Id==categoryId).FirstOrDefault();
            return category.CategoryTitle;
        }
        public List<Entities.Category> GetRootCategories(string projectId, string categoryId,bool isIncome)
        {
            List<Entities.Category> categories = this.Get(x => x.ProjectId == projectId && x.IsDeleted != true && x.Id!=categoryId && x.ParentId==null && x.IsIncome==isIncome).ToList();
            return categories;
        }
        public List<Entities.Category> CheckParent(string categoryId)
        {
            List<Entities.Category> categories = this.Get(x=>x.ParentId==categoryId).ToList();
            return categories;
        }
        public string GetParentCategoryTitle(string categoryId)
        {
            Entities.Category category = this.Get(x=>x.Id==categoryId).FirstOrDefault();
            if (category.ParentId!=null)
            {
                return GetCategoryTitle(category.ParentId);
            }
            else
            {
                return string.Empty;
            }
            
        }
        public void AddParentCategory(string categoryId, string parentId)
        {
            Entities.Category category = this.Get(x =>x.Id==categoryId).FirstOrDefault();
            
            if (category!=null)
            {
                category.ParentId = parentId;
            }
        }
        #region Override IRepository

        #endregion
    }
}
