using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestConsole
{
    public class TestConsole
    {
        public static async Task Main()
        {
            Console.WriteLine("Running...");

            using (var context = new BloggingContext())
            {
                context.Add(new Blog
                {
                    Posts = new List<Post>
                                {
                                    new Post(),
                                    new Post()
                                }
                });

                context.SaveChanges();
            }

            Console.WriteLine("Ending...");
            Console.ReadLine();

            //var connection = new HubConnectionBuilder()
            //    .WithUrl("http://localhost:5000/dabeehub")
            //    .Build();
        }
    }
}
