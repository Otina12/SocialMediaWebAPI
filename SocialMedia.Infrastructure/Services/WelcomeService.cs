﻿using SocialMedia.Application.Abstractions;

namespace SocialMedia.Infrastructure.Services
{
    public class WelcomeService : IWelcomeService
    {
        public void WelcomeMessage(string username)
        {
            // needs implementation in frontend to do something, but for now, print in console
            Console.WriteLine($"Welcome aboard, {username}!");
        }
    }
}
