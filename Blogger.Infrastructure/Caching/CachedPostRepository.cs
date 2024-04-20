using Blogger.Infrastructure.Extensions;
using Blogger.UseCases.PostUseCases.Entities;
using Blogger.UseCases.PostUseCases.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Blogger.Infrastructure.Caching;

public class CachedPostRepository : IPostRepository
{
    private readonly IDistributedCache _cache;
    private readonly IPostRepository _decorated;

    public CachedPostRepository(IPostRepository postRepository, IDistributedCache cache)
    {
        this._decorated = postRepository;
        this._cache = cache;
    }

    public async Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken)
    {
        var cacheKey = $"post-{id}";

        Post? post = await this._cache.GetAsync(
            cacheKey,
            async token => await this._decorated.GetByIdAsync(id, token),
            CacheOptions.DefaultExpiration,
            cancellationToken);

        return post;
    }

    public async Task<PostId> CreatePostAsync(Post post, CancellationToken cancellationToken)
    {
        PostId postId = await this._decorated.CreatePostAsync(post, cancellationToken);

        var cacheKey = "posts-all";

        await this._cache.RemoveAsync(cacheKey, cancellationToken);

        return postId;
    }

    public async Task DeletePostAsync(Post post, CancellationToken cancellationToken)
    {
        await this._decorated.DeletePostAsync(post, cancellationToken);

        var cacheKey = "posts-all";

        await this._cache.RemoveAsync(cacheKey, cancellationToken);
    }

    public async Task<List<Post>> GetAllPostsAsync(CancellationToken cancellationToken, int page, int pageSize)
    {
        string cacheKey = page == 1 && pageSize == 10 ? "posts-all" : $"posts-all-{page}-{pageSize}";

        List<Post> posts = await this._cache.GetAsync(
            cacheKey,
            async token => await this._decorated.GetAllPostsAsync(token, page, pageSize),
            CacheOptions.DefaultExpiration,
            cancellationToken);

        return posts;
    }
}
