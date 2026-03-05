namespace SocialMedia.Domain.Interfaces.Comments;

using SocialMedia.Domain.Entities;
public interface ICommentRepository
{
    Task AddCommentAsync(Comment comment);
    Task DeleteCommentAsync(int commentId, int userId);
}