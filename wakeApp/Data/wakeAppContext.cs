#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wakeApp.Models;
using wakeApp.Dtos;

namespace wakeApp.Data
{
    public class wakeAppContext : DbContext
    {
        public wakeAppContext (DbContextOptions<wakeAppContext> options)
            : base(options)
        {
        }

        public DbSet<PostVideo> PostVideos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<wakeApp.Dtos.CommentDto> CommentDto { get; set; }
    }
}
