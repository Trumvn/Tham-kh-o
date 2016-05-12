using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project
{
    public interface ICategoryAppService : IAppService
    {
        List<Dtos.CategoryDto> GetCategories(string projectId, bool isIncludeClosedCategory);
        List<Dtos.CategoryDto> GetProjectCategories(string projectId);
        Task CreateCategory(Dtos.CategoryDto categoryDto);
        Dtos.CategoryDto SaveCategory(Dtos.CategoryDto categoryDto);
        void SaveCategoryBudget(Dtos.CategoryBudgetDto categoryBudgetDto);
        void SaveParentCategory(Dtos.CategoryDto categoryDto);
        void DeleteBudget(string budgetId);
        List<Dtos.CategoryBudgetDto> GetCategoryBudgets(string categoryId);
        void SoftDeleteCategory(string categoryId);
        List<Dtos.CategoryDto> GetRootCategories(string projectId, string categoryId, bool isIncome);
        bool CheckParent(Dtos.CategoryDto categoryDTo);
    }
}
