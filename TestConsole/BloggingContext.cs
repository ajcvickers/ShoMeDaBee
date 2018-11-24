using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestConsole
{
    public class BloggingContext : DbContext
    {
        private readonly string _hubUrl;
        private readonly TimeSpan? _delay;

        public BloggingContext(
            string hubUrl,
            TimeSpan? delay = null)
        {
            _hubUrl = hubUrl;
            _delay = delay;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseShoMeDaBe(_hubUrl, _delay)
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