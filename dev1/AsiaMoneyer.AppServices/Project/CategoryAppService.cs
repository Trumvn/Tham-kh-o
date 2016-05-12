using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Commons;

namespace AsiaMoneyer.Project
{
    public class CategoryAppService : AppServiceBase, ICategoryAppService
    {

        #region Private
        
        #endregion

        #region Override
        public List<Dtos.CategoryDto> GetCategories(string projectId, bool isIncludeClosedCategory)
        {
            List<AsiaMoneyer.Entities.Category> categoryEntities = this.UnitOfWork.CategoryRepository.GetCategories(projectId, isIncludeClosedCategory);
            List<Dtos.CategoryDto> categoryDtos = AutoMapper.Mapper.Map<List<Entities.Category>, List<Dtos.CategoryDto>>(categoryEntities);
            if (categoryDtos != null && categoryDtos.Count > 0)
            {
                string projectCurrency = this.UnitOfWork.ProjectRepository.GetProjectCurrency(projectId);

                foreach (Dtos.CategoryDto catDto in categoryDtos)
                {
                    Entities.Category cate = this.UnitOfWork.CategoryRepository.Get(catDto.Id);
                    catDto.Parent = AutoMapper.Mapper.Map<Entities.Category, Dtos.CategoryDto>(cate);
                    Entities.CategoryBudget categoryBudget = this.UnitOfWork.CategoryBudgetRepository.GetCurrentCategoryBudget(catDto.Id);
                    Dtos.CategoryBudgetDto budgetDto = AutoMapper.Mapper.Map<Entities.CategoryBudget, Dtos.CategoryBudgetDto>(categoryBudget);
                    if (budgetDto != null)
                    {
                        budgetDto.Currency = projectCurrency;
                    }
                    catDto.CurrentBudget = budgetDto;
                }
            }
            return categoryDtos;
        }

        public List<Dtos.CategoryDto> GetProjectCategories(string projectId)
        {
            List<AsiaMoneyer.Entities.Category> categoryEntities = this.UnitOfWork.CategoryRepository.GetProjectCategories(projectId);
            List<Dtos.CategoryDto> categoryDtos = AutoMapper.Mapper.Map<List<Entities.Category>, List<Dtos.CategoryDto>>(categoryEntities);            
 
            if (categoryDtos != null && categoryDtos.Count > 0)
            {
                string projectCurrency = this.UnitOfWork.ProjectRepository.GetProjectCurrency(projectId);

                foreach (Dtos.CategoryDto catDto in categoryDtos)
                {
                    
                    Entities.CategoryBudget categoryBudget = this.UnitOfWork.CategoryBudgetRepository.GetCurrentCategoryBudget(catDto.Id);
                    Dtos.CategoryBudgetDto budgetDto = AutoMapper.Mapper.Map<Entities.CategoryBudget, Dtos.CategoryBudgetDto>(categoryBudget);
                    if (budgetDto != null)
                    {
                        budgetDto.Currency = projectCurrency;
                    }
                    catDto.CurrentBudget = budgetDto;                                        
                }
            }

            List<Dtos.CategoryDto> categoryDtoList = categoryDtos.Where(x => x.ParentId == null).ToList();
            foreach (Dtos.CategoryDto catDto in categoryDtoList)
            {
                catDto.Childs = categoryDtos.Where(x => x.ParentId == catDto.Id).ToList();
                foreach(Dtos.CategoryDto cat in catDto.Childs)
                {
                    cat.Parent = null;
                }
            }

            return categoryDtoList;
        }

        public List<Dtos.CategoryDto> GetRootCategories(string projectId, string categoryId, bool isIncome)
        {
            List<AsiaMoneyer.Entities.Category> categoryEntities = this.UnitOfWork.CategoryRepository.GetRootCategories(projectId, categoryId,isIncome);
            List<Dtos.CategoryDto> categoryDtos = AutoMapper.Mapper.Map<List<Entities.Category>, List<Dtos.CategoryDto>>(categoryEntities);
            return categoryDtos;
        }

        public void SaveParentCategory(Dtos.CategoryDto categoryDto)
        {
            if (!String.IsNullOrEmpty(categoryDto.Id)) // update
            {
                var parentcat = new Entities.Category
                {
                    Id = categoryDto.Id,
                    ParentId=categoryDto.ParentId
                };

                this.UnitOfWork.CategoryRepository.Update(parentcat,x=>x.ParentId);
                this.UnitOfWork.Save(this.UserId);
            } 
        }
        public async Task CreateCategory(Dtos.CategoryDto categoryDto)
        {
            var category = new Entities.Category()
            {
                Id = Guid.NewGuid().ToString(),
                CategoryTitle = Commons.Ultility.NormalizeSqlString(categoryDto.CategoryTitle),
                CategoryDescription = Commons.Ultility.NormalizeSqlString(categoryDto.CategoryDescription),
                ProjectId = categoryDto.ProjectId,
                IsClosed = false,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            };

            await Task.Factory.StartNew(() =>
            {
                this.UnitOfWork.CategoryRepository.Add(category);
                this.UnitOfWork.Save(this.UserId);
            });
        }

        public void SaveCategoryBudget(Dtos.CategoryBudgetDto categoryBudgetDto)
        {
            if (!String.IsNullOrEmpty(categoryBudgetDto.Id)) // update
            {
                var catBudget = new Entities.CategoryBudget
                {
                    Id = categoryBudgetDto.Id,
                    BudgetAmount = categoryBudgetDto.BudgetAmount,
                    StartDate = categoryBudgetDto.StartDate,
                    EndDate = categoryBudgetDto.EndDate,
                    TimeFrequencyId = categoryBudgetDto.TimeFrequencyId,
                };

                this.UnitOfWork.CategoryBudgetRepository.Update(catBudget, x => x.StartDate, x => x.EndDate, x => x.TimeFrequencyId, x => x.BudgetAmount);
                this.UnitOfWork.Save(this.UserId);
            }
            else // add new
            {
                var catBudget = new Entities.CategoryBudget
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryId = categoryBudgetDto.CategoryId,
                    BudgetAmount = categoryBudgetDto.BudgetAmount,
                    StartDate = categoryBudgetDto.StartDate,
                    EndDate = categoryBudgetDto.EndDate,
                    IsDeleted = false,
                    TimeFrequencyId = categoryBudgetDto.TimeFrequencyId,
                    CreatedDate = DateTime.Now,
                };

                this.UnitOfWork.CategoryBudgetRepository.CreateCategoryBudget(catBudget);
                this.UnitOfWork.Save(this.UserId);
            }
        }

        public List<Dtos.CategoryBudgetDto> GetCategoryBudgets(string categoryId)
        {
            List<AsiaMoneyer.Entities.CategoryBudget> catBudgetEntities = this.UnitOfWork.CategoryBudgetRepository.GetCategorieBudgets(categoryId);
            List<Dtos.CategoryBudgetDto> catBudgetDtos = AutoMapper.Mapper.Map<List<Entities.CategoryBudget>, List<Dtos.CategoryBudgetDto>>(catBudgetEntities);
            if (catBudgetDtos != null && catBudgetDtos.Count > 0)
            {
                string projectCurrency = this.UnitOfWork.ProjectRepository.GetProjectCurrencyByCategory(categoryId);
                foreach(Dtos.CategoryBudgetDto budgetDto in catBudgetDtos)
                {
                    budgetDto.Currency = projectCurrency;
                }
            }
            return catBudgetDtos;

        }
        public bool CheckParent(Dtos.CategoryDto categoryDTo)
        {
            bool result = false;
            List<Entities.Category> categories = this.UnitOfWork.CategoryRepository.CheckParent(categoryDTo.Id);
            if (categories.Count>0)
            {
                result = true;
            }
            return result;
        }
        public Dtos.CategoryDto SaveCategory(Dtos.CategoryDto categoryDto)
        {
            Dtos.CategoryDto savedCategoryDto = null;
            if (!String.IsNullOrEmpty(categoryDto.Id))
            {
                var category = new Entities.Category()
                {
                    
                    Id = categoryDto.Id,
                    CategoryTitle = Commons.Ultility.NormalizeSqlString(categoryDto.CategoryTitle),
                    CategoryDescription = Commons.Ultility.NormalizeSqlString(categoryDto.CategoryDescription),
                    IsClosed = categoryDto.IsClosed,
                    ParentId=categoryDto.ParentId,
                    HighlightColor = categoryDto.HighlightColor,
                    IsDeleted = categoryDto.IsDeleted,
                };

                this.UnitOfWork.CategoryRepository.Update(category, u => u.CategoryTitle, u => u.CategoryDescription, u => u.HighlightColor, u => u.IsClosed, u => u.IsDeleted, u=>u.ParentId);
                this.UnitOfWork.Save(this.UserId);

                category = this.UnitOfWork.CategoryRepository.Get(category.Id);
                category.Parent = this.UnitOfWork.CategoryRepository.Get(category.ParentId);
                savedCategoryDto = AutoMapper.Mapper.Map<Entities.Category, Dtos.CategoryDto>(category);
                savedCategoryDto.Parent = AutoMapper.Mapper.Map<Entities.Category, Dtos.CategoryDto>(category.Parent);
            }
            else
            {
                var category = new Entities.Category()
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryTitle = Commons.Ultility.NormalizeSqlString(categoryDto.CategoryTitle),
                    CategoryDescription = Commons.Ultility.NormalizeSqlString(categoryDto.CategoryDescription),
                    HighlightColor = categoryDto.HighlightColor,
                    ProjectId = categoryDto.ProjectId,
                    ParentId = categoryDto.ParentId,
                    IsClosed = false,
                    IsIncome = categoryDto.IsIncome,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                this.UnitOfWork.CategoryRepository.Add(category);
                this.UnitOfWork.Save(this.UserId);

                category = this.UnitOfWork.CategoryRepository.Get(category.Id);
                category.Parent = this.UnitOfWork.CategoryRepository.Get(category.ParentId);
                savedCategoryDto = AutoMapper.Mapper.Map<Entities.Category, Dtos.CategoryDto>(category);
                savedCategoryDto.Parent = AutoMapper.Mapper.Map<Entities.Category, Dtos.CategoryDto>(category.Parent);

            }

            return savedCategoryDto;
        }

        public void DeleteBudget(string budgetId)
        {
            this.UnitOfWork.CategoryBudgetRepository.UpdateDeleteBudget(budgetId);
            this.UnitOfWork.Save(this.UserId);
        }

        public void SoftDeleteCategory(string categoryId)
        {
            if (!String.IsNullOrEmpty(categoryId))
            {
                var category = new Entities.Category()
                {
                    Id = categoryId,
                    IsDeleted = true,
                };

                this.UnitOfWork.CategoryRepository.Update(category, u => u.IsDeleted);

                this.UnitOfWork.TransactionRepository.SoftDeleteTransactionsInCategory(categoryId);

                this.UnitOfWork.Save(this.UserId);
            }
            else
            {
                throw new Exception(Constants.EXCEPTION_MESSAGE_NOT_FOUND);
            }
        }
        #endregion

    }
}
