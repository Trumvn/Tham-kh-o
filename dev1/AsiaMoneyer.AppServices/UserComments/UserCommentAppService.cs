using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.UserComments
{
    public class UserCommentAppService : AppServiceBase, IUserCommentAppService
    {
        #region Private Methods
        #endregion

        #region Override Methods
        public List<Dtos.UserCommentDto> GetUserComments(int typeId, string objId)
        {
            List<Entities.UserComment> commentEntities = this.UnitOfWork.UserCommentRepository.GetUserComments(typeId, objId);
            List<Dtos.UserCommentDto> commentDtos = AutoMapper.Mapper.Map<List<Entities.UserComment>, List<Dtos.UserCommentDto>>(commentEntities);

            /*foreach (Dtos.UserCommentDto userCommentDto in commentDtos)
            {

                Entities.UserPhoto userPhoto = this.UnitOfWork.UserPhotoRepository.List.Where(x => x.UserId == userCommentDto.UserId).OrderByDescending(x => x.Id).FirstOrDefault();
                if (userPhoto != null && userCommentDto.User != null)
                {
                    userCommentDto.User.Photo = userPhoto.Photo;
                }
            }*/
            return commentDtos;
        }

        public Dtos.UserCommentDto AddUserComment(int typeId, string objectId, string commentText)
        {
            var comment = new Entities.UserComment()
            {
                Id = Guid.NewGuid().ToString(),
                AppObjectTypeId = typeId,
                CommentText = Commons.Ultility.NormalizeSqlString(commentText),
                ObjectId = objectId,
                UserId = this.UserId,
                CommentDate = DateTime.Now
            };

            this.UnitOfWork.UserCommentRepository.Add(comment);
            this.UnitOfWork.Save(this.UserId);

            Dtos.UserCommentDto userCommentDto = AutoMapper.Mapper.Map<Entities.UserComment, Dtos.UserCommentDto>(comment);

            return userCommentDto;
        }

        #endregion
    }
}
