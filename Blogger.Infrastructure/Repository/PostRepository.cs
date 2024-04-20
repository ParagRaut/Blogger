using Blogger.Infrastructure.Data;
using Blogger.UseCases.PostUseCases.Entities;
using Blogger.UseCases.PostUseCases.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Infrastructure.Repository;

public class PostRepository(ApplicationDbContext dbContext) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken)
    {
        Post? post = await dbContext.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return post;
    }

    public async Task<PostId> CreatePostAsync(Post post, CancellationToken cancellationToken)
    {
        dbContext.Add(post);

        await dbContext.SaveChangesAsync(cancellationToken);

        return post.Id;
    }

    public async Task DeletePostAsync(Post post, CancellationToken cancellationToken)
    {
        dbContext.Remove(post);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Post>> GetAllPostsAsync(CancellationToken cancellationToken, int page, int pageSize)
    {
        List<Post> posts = await dbContext.Posts
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return posts;
    }
}
