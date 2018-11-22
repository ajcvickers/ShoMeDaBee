using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestConsole
{
    public class BloggingContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseShoMeDaBe("http://localhost:5000/dabeehub", TimeSpan.FromMilliseconds(1000))
                .UseInMemoryDatabase("Spleen");

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }

    public class Blog
    {
        public int Id { get; set; }

        public ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}