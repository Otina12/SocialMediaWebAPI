﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> MemberExists(string memberId)
        {
            var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId);
            return member != null;
        }

        public async Task<Member?> GetMemberById(string id)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            return member;
        }

        public async Task<List<Community>> GetAllFollowingsOfMember(string memberId)
        {
            var communities = await _context.Followings.Where(f => f.FollowerId == memberId).Select(f => f.Community).ToListAsync();
            return communities;
        }

        public bool Update(Member member)
        {
            _context.Update(member);
            return Save();
        }
        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}