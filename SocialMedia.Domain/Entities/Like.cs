﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Domain.Entites
{
    [PrimaryKey(nameof(PostId), nameof(MemberId))]
    public class Like
    {
        public Guid PostId { get; set; }
        public string MemberId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
        public Member Member { get; set; }
        public DateTime Time { get; set; }
    }
}
