#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wakeApp.Models;

namespace wakeApp.Data
{
    public class wakeAppContext : DbContext
    {
        public wakeAppContext (DbContextOptions<wakeAppContext> options)
            : base(options)
        {
        }

        public DbSet<wakeApp.Models.Video> Videos { get; set; }
        public DbSet<PostVideo> PostVideos { get; set; }
    }
}
