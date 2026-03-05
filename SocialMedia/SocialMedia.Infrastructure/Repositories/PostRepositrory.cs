namespace SocialMedia.Infrastructure.Repositories;
using SocialMedia.Domain.Interfaces.Posts;
using SocialMedia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entities;


public class PostRepository : IPostRepository
{
    private readonly SocialMediaContext _context;
    public PostRepository(SocialMediaContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetAllAsync(int pages = 1, int pageSize = 10)
    {
        
       var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.Comments) // Coloque o Include aqui
                .OrderByDescending(c => c.CreatedAt) // Geralmente posts novos vêm primeiro
                .Skip((pages - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        if (posts == null || posts.Count == 0)
        {
            throw new KeyNotFoundException("No posts found.");
        }
        return posts;
    }
    public async Task<Post> GetByIdAsync(int id)
    {
        // include comments when loading a single post
        var post = await _context.Posts
            .Include(p => p.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PostId == id);

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }
        return post;
    }

    public async Task CreateAsync(Post post)
    {
        try
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new InvalidOperationException("Failed to create post.");
        }
    }
    public async Task UpdateAsync(Post post)
    {
        try
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new InvalidOperationException("Failed to update post.");
        }
    }
    public async Task DeleteAsync(int id)
    {
        try
        {
            var post = await GetByIdAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new InvalidOperationException("Failed to delete post.");
        }
    }

    // Aditional method for give likes to a post
    public async Task LikePostAsync(int postId, int userId)
    {
        try
        {
            var existingLike = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (existingLike != null)
            {
                throw new InvalidOperationException("User has already liked this post.");
            }

            var like = new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Likes.Add(like);

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            if (post == null)            {
                throw new KeyNotFoundException($"Post with ID {postId} not found.");
            }
            post.LikesCount += 1;
            _context.Posts.Update(post);

            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            throw new InvalidOperationException($"Failed to like post: {ex.Message}");
        }
    }
    public async Task UnlikePostAsync(int postId, int userId)
    {
        try
        {
            var existingLike = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (existingLike == null)
            {
                throw new InvalidOperationException("User has not liked this post.");
            }

            _context.Likes.Remove(existingLike);

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {postId} not found.");
            }
            post.LikesCount -= 1;
            _context.Posts.Update(post);

            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}