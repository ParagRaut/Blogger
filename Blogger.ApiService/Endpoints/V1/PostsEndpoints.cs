using Blogger.ApiService.Contracts;
using Blogger.ApiService.Contracts.Mappers;
using Blogger.UseCases.AuthorUseCases.Entities;
using Blogger.UseCases.AuthorUseCases.Queries;
using Blogger.UseCases.PostUseCases.Commands;
using Blogger.UseCases.PostUseCases.Entities;
using Blogger.UseCases.PostUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.ApiService.Endpoints.V1;

public static class PostsEndpoints
{
    public static IEndpointRouteBuilder MapPostEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("post", GetAllPosts);

        app.MapGet("post/{id}", GetPostById);

        app.MapPost("post", CreatePost);

        app.MapDelete("post/{id}", DeletePostById);

        return app;
    }

    public static async Task<Results<Ok<List<PostResponse>>, NotFound>> GetAllPosts(
        ISender sender,
        CancellationToken cancellationToken,
        bool includeAuthor = false,
        int page = 1,
        int pageSize = 10)
    {
        var query = new GetAllQuery(page, pageSize);

        IList<Post> posts = await sender.Send(query, cancellationToken);

        if (posts is null || !posts.Any()) return TypedResults.NotFound();

        if (includeAuthor)
        {
            List<AuthorId> authorIds = posts.Select(p => p.AuthorId).Distinct().ToList();
            List<Author> authors = await GetAuthorsAsync(sender, authorIds, cancellationToken);

            List<PostResponse> postResponse = posts
                .Select(p => PostResponseMapper.MapToPostResponse(p, authors.FirstOrDefault(a => a.Id == p.AuthorId)))
                .ToList();

            return TypedResults.Ok(postResponse);
        }
        else
        {
            List<PostResponse> postResponse = posts
                .Select(p => PostResponseMapper.MapToPostResponse(p))
                .ToList();

            return TypedResults.Ok(postResponse);
        }
    }

    public static async Task<Results<Ok<PostResponse>, NotFound>> GetPostById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken,
        bool includeAuthor = false)
    {
        var postByIdQuery = new GetByIdQuery(new PostId(id));

        Post? post = await sender.Send(postByIdQuery, cancellationToken);

        if (post is null) return TypedResults.NotFound();

        if (includeAuthor)
        {
            var authorByIdQuery = new GetAuthorByIdQuery(post.AuthorId);
            Author? author = await sender.Send(authorByIdQuery, cancellationToken);

            return TypedResults.Ok(PostResponseMapper.MapToPostResponse(post, author));
        }

        return TypedResults.Ok(PostResponseMapper.MapToPostResponse(post));
    }

    public static async Task<Created<Guid>> CreatePost(
        ISender sender,
        [FromBody] CreateNewPostRequest request,
        CancellationToken ct)
    {
        var command = new CreatePostCommand(
            new AuthorId(request.AuthorId),
            request.Title,
            request.Description,
            request.Content);

        PostId? postId = await sender.Send(command, ct);

        return TypedResults.Created($"/api/v1/post/{postId.Value}", postId.Value);
    }

    public static async Task<Results<NoContent, NotFound>> DeletePostById(
        ISender sender,
        Guid id,
        CancellationToken ct)
    {
        var command = new DeletePostByIdCommand(
            new PostId(id));

        await sender.Send(command, ct);

        return TypedResults.NoContent();
    }

    private static async Task<List<Author>> GetAuthorsAsync(
        ISender sender,
        List<AuthorId> authorIds,
        CancellationToken cancellationToken)
    {
        var authors = new List<Author>();

        foreach (AuthorId authorId in authorIds)
        {
            var authorByIdQuery = new GetAuthorByIdQuery(authorId);

            Author? author = await sender.Send(authorByIdQuery, cancellationToken);

            if (author != null) authors.Add(author);
        }

        return authors;
    }
}
