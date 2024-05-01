using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Application.Abstractions.Messaging;

public interface ICommand : IBaseCommand
{

}

public interface ICommand<TResponse> : IBaseCommand
{

}


public interface IBaseCommand
{

}

