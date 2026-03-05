namespace SocialMedia.Infrastructure.Repositories;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces.Comments;
using SocialMedia.Domain.Interfaces.Posts;
using SocialMedia.Infrastructure.Data;

public class CommentRepository : ICommentRepository
{
    private readonly SocialMediaContext _context;
    public CommentRepository(SocialMediaContext context)
    {
        _context = context;
    }   

    public async Task AddCommentAsync(Comment comment)
    {
        if (comment == null)
        {
            throw new ArgumentNullException(nameof(comment));
        }
        try
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new InvalidOperationException("Failed to add comment.");
        }


    }

    public async Task DeleteCommentAsync(int commentId, int userId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
            throw new KeyNotFoundException($"Comment with ID {commentId} not found.");

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with Id {userId} not found!");

        if (comment.UserId != userId)
            throw new UnauthorizedAccessException($"You can't delete this post. Only the creator of this post can delete it!");
        try
        {
            
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new InvalidOperationException("Failed to delete comment.");
        }
    }
}