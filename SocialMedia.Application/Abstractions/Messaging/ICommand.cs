using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Application.Abstractions.Messaging;

public interface ICommand : IBaseCommand, IRequest
{

}

public interface ICommand<TResponse> : IBaseCommand, IRequest<TResponse>
{

}


public interface IBaseCommand
{

}

