using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json;
using System.Xml.Linq;
using Microsoft.Extensions.Hosting;

namespace Blog_API_with_JSON.Models
{
    public class BlogPostModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }

        public DateTime CreateTime { get; set; }
    }

        public class PostManager
        {
            private static string PostsFile = HttpContext.Current.Server.MapPath("~/App.Data/Posts.Json");
            private static List<BlogPostModel> posts = new List<BlogPostModel>();

            public static void Create(string PostJson)
            {
                var obj = JsonConvert.DeserializeObject<BlogPostModel>(PostJson);

                if (posts.Count > 0)
                {
                    posts = (from post in posts
                             orderby post.CreateTime
                             select post).ToList();
                    obj.ID = posts.Last().ID + 1;
                }
                else
                {
                    obj.ID = 1;
                }
            posts.Add(obj);
            save();
        }
            

            public static List<BlogPostModel> Read()
            {
                if (!File.Exists(PostsFile))
                {
                    File.Create(PostsFile).Close();
                    File.WriteAllText(PostsFile, "[]");
                }
                posts = JsonConvert.DeserializeObject<List<BlogPostModel>>
                (File.ReadAllText(PostsFile));
                return posts;
            }
            public static void Update(int id, string PostJson)
            {
                Delete(id);
                Create(PostJson);
                save();
            }
            public static void Delete(int id)
            {
                posts.Remove(posts.Find(x => x.ID == id));
                save();
            }
            private static void save()
            {
                File.WriteAllText(PostsFile, JsonConvert.SerializeObject(posts));
            }
        }
    
}
