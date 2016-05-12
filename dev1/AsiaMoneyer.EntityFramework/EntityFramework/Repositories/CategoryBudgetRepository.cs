using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Category;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class CategoryBudgetRepository : AsiaMoneyerRepositoryBase<Entities.CategoryBudget>, ICategoryBudgetRepository
    {
        public CategoryBudgetRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<Entities.CategoryBudget> GetCategorieBudgets(string categoryId)
        {
            List<Entities.CategoryBudget> categoryBudgets = this.Get(x => x.CategoryId == categoryId && x.IsDeleted == false).ToList();
            return categoryBudgets;
        }

        #region Override IRepository

        public void CreateCategoryBudget(Entities.CategoryBudget entity)
        {
            // Update current entity for EndDate
            List<Entities.CategoryBudget> entities = this.Get(x => x.CategoryId == entity.CategoryId && x.EndDate.HasValue == false).ToList();
            foreach(var ent in entities)
            {
                ent.EndDate = entity.StartDate;
                this.Save(ent);
            }

            this.Add(entity);
        }

        public Entities.CategoryBudget GetCurrentCategoryBudget(string categoryId)
        {
            DateTime now = DateTime.Now;
            Entities.CategoryBudget entity = this.Get(x => x.CategoryId == categoryId && (!x.IsDeleted.HasValue || x.IsDeleted == false) && ((!x.StartDate.HasValue || x.StartDate <= now) && (!x.EndDate.HasValue || x.EndDate >= now))).FirstOrDefault();
            return entity;
        }

        public void UpdateDeleteBudget(string budgetId)
        {
            Entities.CategoryBudget entity = new Entities.CategoryBudget()
            {
                Id = budgetId,
                IsDeleted = true
            };
            this.Update(entity, x=>x.IsDeleted);
        }

        public decimal GetCategoryMonthlyBudget(string categoryId, DateTime fromDate, DateTime endDate)
        {
            Entities.CategoryBudget entity = this.Get(x => x.CategoryId == categoryId && (!x.IsDeleted.HasValue || x.IsDeleted == false) && ((!x.StartDate.HasValue || x.StartDate <= fromDate) && (!x.EndDate.HasValue || x.EndDate >= endDate))).FirstOrDefault();
            if (entity != null)
            {
                decimal budget = entity.BudgetAmount;
                switch(entity.TimeFrequencyId)
                {
                    case (int)Commons.Constants.TIME_FREQUENCY.DAILY:
                        budget *= 30;
                        break;
                    case (int)Commons.Constants.TIME_FREQUENCY.WEEKLY:
                        budget *= 4;
                        break;
                    case (int)Commons.Constants.TIME_FREQUENCY.YEARLY:
                        budget /= 12;
                        break;
                    case (int)Commons.Constants.TIME_FREQUENCY.MONTHLY:
                        budget *= 1;
                        break;
                    default:
                        budget = 0;
                        break;
                }
                return budget;
            }
            return 0;
        }
        #endregion
    }
}
