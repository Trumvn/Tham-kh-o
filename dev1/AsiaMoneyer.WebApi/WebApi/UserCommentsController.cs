using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AsiaMoneyer.UserComments;
using System.Web.Http;
using System.Threading.Tasks;
using AsiaMoneyer.Commons;
using Microsoft.AspNet.Identity;

namespace AsiaMoneyer.WebApi.Controllers
{
    public class UserCommentsController : BaseApiController
    {
        IUserCommentAppService _userCommentAppService;

        public UserCommentsController(IUserCommentAppService userCommentAppService)
        {
            this._userCommentAppService = userCommentAppService;
        }

        #region AuditLogs
        public IHttpActionResult GetTransactionUserComments(string transactionId)
        {
            var l = this._userCommentAppService.GetUserComments(Commons.Constants.APP_OBJECT_TYPE_ID_TRANSACTION_LOGS, transactionId);
            return Ok(l);
        }

        public IHttpActionResult CreateTransactionUserComment(UserComments.Dtos.UserCommentDto commentDto)
        {
            this._userCommentAppService.UserId = User.Identity.GetUserId();
            commentDto = this._userCommentAppService.AddUserComment(Commons.Constants.APP_OBJECT_TYPE_ID_TRANSACTION_LOGS, commentDto.ObjectId, commentDto.CommentText);

            return Ok(commentDto);
        }

        public IHttpActionResult GetProjectAccountUserComments(string accountId)
        {
            this._userCommentAppService.UserId = User.Identity.GetUserId();
            var l = this._userCommentAppService.GetUserComments(Commons.Constants.APP_OBJECT_TYPE_ID_ACCOUNT_LOGS, accountId);
            return Ok(l);
        }

        public IHttpActionResult CreateProjectAccountUserComment(UserComments.Dtos.UserCommentDto commentDto)
        {

            this._userCommentAppService.UserId = User.Identity.GetUserId();
            commentDto = this._userCommentAppService.AddUserComment(Commons.Constants.APP_OBJECT_TYPE_ID_ACCOUNT_LOGS, commentDto.ObjectId, commentDto.CommentText);

            return Ok(commentDto);
        }

        public IHttpActionResult GetProjectCategoryUserComments(string categoryId)
        {
            this._userCommentAppService.UserId = User.Identity.GetUserId();
            var l = this._userCommentAppService.GetUserComments(Commons.Constants.APP_OBJECT_TYPE_ID_CATEGORY_LOGS, categoryId);
            return Ok(l);
        }

        public IHttpActionResult CreateProjectCategoryUserComment(UserComments.Dtos.UserCommentDto commentDto)
        {
            this._userCommentAppService.UserId = User.Identity.GetUserId();

            commentDto = this._userCommentAppService.AddUserComment(Commons.Constants.APP_OBJECT_TYPE_ID_CATEGORY_LOGS, commentDto.ObjectId, commentDto.CommentText);

            return Ok(commentDto);
        }

        #endregion
    }

}