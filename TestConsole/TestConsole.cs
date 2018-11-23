using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestConsole
{
    public class TestConsole
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Running...");

            var hubUrl = args[0];
            var delay = args.Length > 1 ? (TimeSpan?)TimeSpan.FromMilliseconds(int.Parse(args[1])) : null;

            using (var context = new BloggingContext(hubUrl, delay))
            {
                for (int i = 0; i < 4; i++)
                {
                    context.Add(new Blog
                    {
                        Posts = new List<Post>
                        {
                            new Post(),
                            new Post()
                        }
                    });
                }

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
