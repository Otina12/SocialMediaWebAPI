﻿using SocialMedia.Domain.Entites;

namespace SocialMedia.Domain.Interfaces
{
    public interface IFollowRepository
    {
        Task<List<Following>> GetFollowingsOfMember(string memberId);
        Task<List<Following>> GetFollowingsOfCommunity(Guid communityId);
        Task<Following?> GetFollowingById(string memberId, Guid communityId);
        bool Follow(string memberId, Guid communityId);
        bool Unfollow(Following following);
    }
}
