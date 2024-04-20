namespace Blogger.Web;

public class PostsApiClient(HttpClient httpClient)
{
    public async Task<PostResponse[]> GetPostsAsync(
        CancellationToken cancellationToken = default)
    {
        List<PostResponse>? postResponses = null;

        await foreach (var post in httpClient.GetFromJsonAsAsyncEnumerable<PostResponse>(
                           "/api/v1/post?includeAuthor=true&page=1&pageSize=50",
                           cancellationToken))
        {
            if (post is not null)
            {
                postResponses ??= [];
                postResponses.Add(post);
            }
        }

        return postResponses?.ToArray() ?? [];
    }
}

public class PostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public AuthorResponse? Author { get; set; }
}

public class AuthorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}