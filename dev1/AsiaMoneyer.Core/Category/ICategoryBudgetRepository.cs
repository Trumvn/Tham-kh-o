using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Category
{
    public interface ICategoryBudgetRepository : IRepository<AsiaMoneyer.Entities.CategoryBudget, string>
    {
        List<AsiaMoneyer.Entities.CategoryBudget> GetCategorieBudgets(string categoryId);
        void CreateCategoryBudget(Entities.CategoryBudget entity);
        Entities.CategoryBudget GetCurrentCategoryBudget(string categoryId);
        void UpdateDeleteBudget(string budgetId);
        decimal GetCategoryMonthlyBudget(string categoryId, DateTime fromDate, DateTime endDate);
    }

}
