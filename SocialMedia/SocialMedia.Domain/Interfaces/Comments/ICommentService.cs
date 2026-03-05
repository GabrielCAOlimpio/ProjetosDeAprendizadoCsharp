using SocialMedia.Domain.DTOs.Comments;

namespace SocialMedia.Domain.Interfaces.Comments;

public interface ICommentService
{
    Task AddCommentAsync(CommentRequestDTO comment);
    Task DeleteCommentAsync(int commentId, int userId);
}