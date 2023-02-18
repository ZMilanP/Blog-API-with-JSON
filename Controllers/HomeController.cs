using Blog_API_with_JSON.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Blog_API_with_JSON.Controllers
{
    public class BlogController : Controller
    {
        
        public ActionResult Index()
        {
            var blogs = PostManager.Read();
            if (blogs == null)
            {
                ViewBag.Empty = true;
                return View();
            }
            else
            {
                blogs = (from blog in blogs
                         orderby blog.CreateTime descending
                         select blog).ToList();
                ViewBag.Empty = false;
                return View(blogs);
            }
            
        }
        [Route ("blog/read/{id}")]
        public ActionResult Read (int id)
        {
            var blogs = PostManager.Read();
            BlogPostModel post = null;
            
            if (blogs != null && blogs.Count> 0)
            {
                post = blogs.Find(x => x.ID == id);
            }
            if (post == null)
            {
                ViewBag.PostFound = false;
                return View();
            }
            else
            {
                ViewBag.Empty = true;
                return View(post);
            }
        }
        public ActionResult Create()
        {
            if (Request.HttpMethod == "POST")
            {
                var title = Request.Form["title"].ToString();
                var tags = Request.Form["tags"].ToString().Split(new char[] { ',' });
                var content = Request.Form["content"].ToString();

                var post = new BlogPostModel
                {
                    Title = title,
                    CreateTime = DateTime.Now,
                    Content = content,
                    Tags = tags.ToList()
                };
                PostManager.Create(JsonConvert.SerializeObject(post));

                Response.Redirect("~/blog");
            }
            return View();
        }
        [Route("blog/edit/{id}")]
        public ActionResult Edit(int id)
        {
            if(Request.HttpMethod == "POST")
            {
                var title = (Request.Form["title"].ToString());
                var tags = Request.Form["tags"].ToString().Split(new char[] { ',' });
                var content = (Request.Form["content"].ToString());

                var post = new BlogPostModel
                {
                    Title = title,
                    CreateTime = DateTime.Now,
                    Content = content,
                    Tags = tags.ToList()
                };
                Response.Redirect("~/blog");
            }
            else
            {
                var post = PostManager.Read().Find(x => x.ID == id);

                if(post != null)
                {
                    ViewBag.Found = true;
                    ViewBag.Title = post.Title;
                    ViewBag.Tags = post.Tags;
                    ViewBag.Content = post.Content;
                }
                else
                {
                    ViewBag.Found = false;
                }
            }
            return View();
        }
    }
}
