using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AsiaMoneyer.Project.Dtos;
using Microsoft.AspNet.Identity;

namespace AsiaMoneyer.Portal.WebApi.Controllers
{
    [Authorize]
    public class ProjectsController : BaseApiController
    {
        Project.IProjectAppService _projectAppService;
        Project.ICategoryAppService _categoryAppService;
        Project.IAccountAppService _accountAppService;
        Project.ITransactionAppService _transactionAppService;
        Client.IClientAppService _clientAppService;

        public ProjectsController(Project.IProjectAppService projectAppService, Project.ITransactionAppService transactionAppService, Client.IClientAppService clientAppService, Project.ICategoryAppService categoryAppService, Project.IAccountAppService accountAppService)
        {
            this._projectAppService = projectAppService;
            this._categoryAppService = categoryAppService;
            this._accountAppService = accountAppService;
            this._transactionAppService = transactionAppService;
            this._clientAppService = clientAppService;
        }
        
        #region Project
        public IHttpActionResult GetProjectHeader(string projectId)
        {
            var projectHeaderDto = this._projectAppService.GetProjectHeader(projectId);
            return Ok(projectHeaderDto);
        }

        [HttpGet]
        public IHttpActionResult GetProject(string projectId)
        {
            var projectDto = this._projectAppService.GetProject(projectId);
            return Ok(projectDto);
        }

        [HttpPost]
        public IHttpActionResult SearchProjects(Project.Dtos.ProjectFilterDto filter)
        {
            if (filter != null)
            {
                var l = this._projectAppService.FilterProjects(filter);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }

        [HttpPost]
        public IHttpActionResult CreateProject(Project.Dtos.ProjectDto projectDto)
        {
            try
            {
                var user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;

                if (user != null)
                {
                    this._projectAppService.UserId = user.Id;
                }

                this._projectAppService.CreateProject(projectDto);
                return Ok();
            }catch(Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateProject(Project.Dtos.ProjectDto projectDto)
        {
            try
            {
                this._projectAppService.UserId = User.Identity.GetUserId();
                this._projectAppService.SaveProject(projectDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult SetProjectHighlightColor(Project.Dtos.ProjectDto projectDto)
        {
            try
            {
                this._projectAppService.UserId = User.Identity.GetUserId();
                this._projectAppService.SetProjectHighlightColor(projectDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteProject(Project.Dtos.ProjectDto projectDto)
        {
            try
            {
                this._projectAppService.UserId = User.Identity.GetUserId();
                this._projectAppService.DeleteProject(projectDto.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public IHttpActionResult GetProjectMembers(string projectId)
        {
            var projectMemberDto = this._projectAppService.GetProjectMembers(projectId);
            return Ok(projectMemberDto);
        }


        #endregion

        #region Account
        [HttpGet]
        public IHttpActionResult GetAccounts(string projectId)
        {
            var l = this._accountAppService.GetAccounts(projectId);
            return Ok(l);
        }

        [HttpPost]
        public IHttpActionResult SaveAccount(AccountDto accountDto)
        {
            this._accountAppService.UserId = User.Identity.GetUserId();
            var savedAccountDto = this._accountAppService.SaveAccount(accountDto);
            return Ok(savedAccountDto);
        }

        [HttpPost]
        public IHttpActionResult setAccountPrimary(AccountDto accountDto)
        {
            this._accountAppService.UserId = User.Identity.GetUserId();
            this._accountAppService.setAccountPrimary(accountDto.Id);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult closeAccount(AccountDto accountDto)
        {
            this._accountAppService.UserId = User.Identity.GetUserId();
            bool isClosed = accountDto.IsClosed.HasValue && accountDto.IsClosed.Value ? true : false;
            this._accountAppService.setAccountClosed(accountDto.Id, isClosed);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteAccount(AccountDto accountDto)
        {
            this._accountAppService.UserId = User.Identity.GetUserId();
            this._accountAppService.SoftDeleteAccount(accountDto.Id);
            return Ok(accountDto);
        }
        #endregion

        #region Category
        [HttpGet]
        public IHttpActionResult GetCategories(string projectId)
        {
            bool isIncludeClosedCategory = true;
            var l = this._categoryAppService.GetCategories(projectId, isIncludeClosedCategory);
            return Ok(l);
        }

        [HttpGet]
        public IHttpActionResult GetAvailableCategories(string projectId)
        {
            bool isIncludeClosedCategory = false;
            var l = this._categoryAppService.GetCategories(projectId, isIncludeClosedCategory);
            return Ok(l);
        }

        public async Task<IHttpActionResult> CreateCategory(CategoryDto categoryDto)
        {
            await this._categoryAppService.CreateCategory(categoryDto);
            return Ok();
        }

        public IHttpActionResult SaveCategory(CategoryDto categoryDto)
        {
            this._categoryAppService.UserId = User.Identity.GetUserId();
            var savedCategoryDto = this._categoryAppService.SaveCategory(categoryDto);
            return Ok(savedCategoryDto);
        }

        [HttpPost]
        public IHttpActionResult CloseCategory(CategoryDto categoryDto)
        {
            this._categoryAppService.UserId = User.Identity.GetUserId();
            var savedCategoryDto = this._categoryAppService.SaveCategory(categoryDto);
            return Ok(savedCategoryDto);
        }

        [HttpPost]
        public IHttpActionResult DeleteCategory(CategoryDto categoryDto)
        {
            this._categoryAppService.UserId = User.Identity.GetUserId();
            this._categoryAppService.SoftDeleteCategory(categoryDto.Id);
            return Ok(categoryDto);
        }

        public IHttpActionResult GetCategoryBudgets(string categoryId)
        {
            var l = this._categoryAppService.GetCategoryBudgets(categoryId);
            return Ok(l);
        }

        public IHttpActionResult SaveCategoryBudget(CategoryBudgetDto categoryBudgetDto)
        {
            this._categoryAppService.UserId = User.Identity.GetUserId();
            this._categoryAppService.SaveCategoryBudget(categoryBudgetDto);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteBudget(CategoryBudgetDto budgetDto)
        {
            this._categoryAppService.UserId = User.Identity.GetUserId();
            this._categoryAppService.DeleteBudget(budgetDto.Id);

            var l = this._categoryAppService.GetCategoryBudgets(budgetDto.CategoryId);
            return Ok(l);
        }

        #endregion

        #region Transaction
        [HttpGet]
        public IHttpActionResult GetTransactions(string projectId)
        {
            var l = this._transactionAppService.GetAvailableTransactions(projectId);
            return Ok(l);
        }
        [HttpGet]
        public IHttpActionResult GetAccountCurrentBalance(string projectId)
        {
            var l = this._accountAppService.GetAccountCurrentBalance(projectId);
            return Ok(l);
        }

        [HttpPost]
        public IHttpActionResult SearchTransactions(Project.Dtos.TransactionFilterDto filterDto)
        {
            this._transactionAppService.UserId = User.Identity.GetUserId();
            var l = this._transactionAppService.SearchTransactions(filterDto);
            return Ok(l);
        }

        [HttpPost]
        public IHttpActionResult SaveProjectFilter(Project.Dtos.TransactionFilterDto filterDto)
        {
            this._projectAppService.UserId = User.Identity.GetUserId();

            string categoryId = (filterDto.Category != null && filterDto.Category.Id != null) ? filterDto.Category.Id : String.Empty;
            string accountId = (filterDto.Account != null && filterDto.Account.Id != null) ? filterDto.Account.Id : String.Empty;

            this._projectAppService.SaveProjectFilter(filterDto.ProjectId, (int)filterDto.FilterTypeId, categoryId, accountId, filterDto.FromDate, filterDto.EndDate);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult GetProjectFilter(Project.Dtos.TransactionFilterDto filterDto)
        {
            this._projectAppService.UserId = User.Identity.GetUserId();
            var l = this._projectAppService.GetProjectFilter(filterDto.ProjectId);
            return Ok(l);
        }

        public IHttpActionResult SaveTransaction(TransactionDto transDto)
        {
            try
            {
                this._transactionAppService.UserId = User.Identity.GetUserId();

                var savedTransDto = this._transactionAppService.SaveTransaction(transDto);

                return Ok(savedTransDto);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public IHttpActionResult DeleteTransaction(TransactionDto transDto)
        {
            this._transactionAppService.UserId = User.Identity.GetUserId();

            this._transactionAppService.DeleteTransaction(transDto);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult LoadTransactionSummaryByMonthYear(Project.Dtos.TransactionFilterDto filter)
        {
            if (filter != null)
            {
                var l = this._transactionAppService.GetTransactionSummaryByMonthYear(filter);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }

        [HttpPost]
        public IHttpActionResult LoadTransactionSummaryByCategory(Project.Dtos.TransactionFilterDto filter)
        {
            if (filter != null)
            {
                var l = this._transactionAppService.GetTransactionSummaryByCategory(filter);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }

        [HttpPost]
        public IHttpActionResult LoadTransactionSummaryByAccount(Project.Dtos.TransactionFilterDto filter)
        {
            if (filter != null)
            {
                var l = this._transactionAppService.GetTransactionSummaryByAccount(filter);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }

        [HttpPost]
        public IHttpActionResult LoadTransactionCategorySummaryByMonthYear(Project.Dtos.TransactionFilterDto filter)
        {
            if (filter != null)
            {
                var l = this._transactionAppService.LoadTransactionCategorySummaryByMonthYear(filter);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }

        [HttpPost]
        public IHttpActionResult LoadProjectAnalyseInformation(Project.Dtos.TransactionFilterDto filter)
        {
            if (filter != null)
            {
                var l = this._transactionAppService.LoadProjectAnalyseInformation(filter);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }
        
        [HttpPost]
        public IHttpActionResult LoadMonthTotalIncome(Project.Dtos.TransactionFilterDto filter)
        {
            if (filter != null && !String.IsNullOrEmpty(filter.ProjectId) && filter.FromDate.HasValue)
            {
                var l = this._transactionAppService.GetTotalIncomeByMonth(filter.ProjectId, filter.FromDate.Value);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }

        #endregion

        #region Project Members
        [HttpPost]
        public IHttpActionResult InviteMember(Project.Dtos.InviteMemberDto inviteMemberDto)
        {
            try
            {
                var user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;
                if (user != null)
                {
                    this._projectAppService.UserId = user.Id;
                }
                var projectMemberDto = this._projectAppService.InviteMember(inviteMemberDto.ProjectId, inviteMemberDto.Email);
                return Ok(projectMemberDto);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult RemoveMember(Project.Dtos.ProjectMemberDto projectMemberDto)
        {
            try
            {
                var user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;
                if (user != null)
                {
                    this._projectAppService.UserId = user.Id;
                }
                var projectMembers = this._projectAppService.RemoveMember(projectMemberDto.Id);
                return Ok(projectMembers);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult AcceptInvitition(string code)
        {
            try
            {
                var user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;
                if (user != null)
                {
                    this._projectAppService.UserId = user.Id;
                }
                this._projectAppService.AcceptInvitition(code);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Recurring
        [HttpPost]
        public IHttpActionResult SaveRecurringTransaction(RecurringTransactionSubmitDto recurringTransactionSubmitDto)
        {
            try
            {
                this._transactionAppService.UserId = User.Identity.GetUserId();
                var savedRecurringTransactionDto = this._transactionAppService.SaveRecurringTransaction(recurringTransactionSubmitDto);
                return Ok(savedRecurringTransactionDto);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public IHttpActionResult RemoveRecurringTransaction(RecurringTransactionSubmitDto recurringTransactionSubmitDto)
        {
            try
            {
                this._transactionAppService.UserId = User.Identity.GetUserId();
                this._transactionAppService.RemoveRecurringTransaction(recurringTransactionSubmitDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        #endregion
    }
}