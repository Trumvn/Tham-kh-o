using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Category
{
    public interface ICategoryRepository : IRepository<AsiaMoneyer.Entities.Category, string>
    {
        List<AsiaMoneyer.Entities.Category> GetCategories(string projectId, bool isIncludeClosedCategory);
        List<AsiaMoneyer.Entities.Category> GetProjectCategories(string projectId);
        string GetCategoryTitle(string categoryId);
        List<AsiaMoneyer.Entities.Category> GetRootCategories(string projectId, string categoryId, bool isIncome);
        string GetParentCategoryTitle(string categoryId);
        void AddParentCategory(string categoryId, string parentId);
        List<Entities.Category> CheckParent(string categoryId);
    }
}
