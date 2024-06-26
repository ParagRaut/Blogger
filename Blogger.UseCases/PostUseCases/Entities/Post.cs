﻿using Blogger.UseCases.AuthorUseCases.Entities;

namespace Blogger.UseCases.PostUseCases.Entities;

//Can be an auditable entity but not using it to keep things simple 
public class Post
{
    public Post(
        PostId id,
        AuthorId authorId,
        string title,
        string description,
        string content)
    {
        this.Id = id;
        this.AuthorId = authorId;
        this.Title = title;
        this.Description = description;
        this.Content = content;
    }

    private Post()
    {
    }

    public PostId Id { get; private set; }

    public AuthorId AuthorId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Content { get; private set; }
}

public record PostId(Guid Value);
