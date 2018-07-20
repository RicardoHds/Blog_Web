using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.ViewModels;
using Blog.Models;
namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        // Next code lines are own
        public ActionResult Index()
        {
            // Get all posts from DB and send to index view
            using (var db = new PostContext())
            {
                return View(db.Posts.ToList());
            }
        }

        // The following method works to show the "Add View"
        // if have ID return the information if not have ID only return only ViewModel
        [HttpGet]
        public ActionResult Add(int? id)
        {
            if (!id.HasValue)
            {
                using (var db = new PostContext())
                {
                    List<Category> categories = db.Categories.ToList();

                    PostVM pvm = new PostVM();
                    pvm.categories = categories;

                    return View(pvm);
                }
            }
            else
            {
                using (var db = new PostContext())
                {
                    List<Post> posts = db.Posts.ToList();
                    var postEdit = posts.First(p => p.Id == id.Value);

                    List<Category> mycategories = db.Categories.ToList();

                    PostVM pvm = new PostVM();
                    pvm.post = postEdit;
                    pvm.categories = mycategories;

                    return View(pvm);
                }
            }
        }
        
        // This method works to POST ind DB (new or edit)
        [HttpPost]
        public ActionResult Add(Post post)
        {
            // New post
            if (post.Id == 0)
            {
                using (var db = new PostContext())
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            // Edit post
            else
            {
                using (var db = new PostContext())
                {
                    db.Database.ExecuteSqlCommand("UPDATE dbo.Posts SET Title = '" + post.Title + "', Category = '" + post.Category + "', Date = '" + post.Date + "', Description = '" + post.Description + "' WHERE Id =" + post.Id);
                    db.SaveChanges();
                }
                    return RedirectToAction("Index");
            }
            
        }

        // Method only to show a specific post
        [HttpGet]
        public ActionResult Show(int id)
        {
            using (var db = new PostContext())
            {
                // Obtenemos el post a mostrar
                List<Post> posts = db.Posts.ToList();
                var postShow = posts.First(p => p.Id == id);

                // Obtenemos la lista de comentarios relacionados con el ID del post
                List<Comment> comments = db.Comments.ToList();
                var commentsShow = comments.FindAll(c => c.Id_Post == id);

                PostVM pvm = new PostVM();
                pvm.comments = commentsShow;
                pvm.post = postShow;

                return View(pvm);
            }
        }

        // Method to save comments in DB and save a relation with Post_Id
        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            using (var db = new PostContext())
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Method works show a voiew confirmation to delete post
        public ActionResult Delete(int id)
        {
            using (var db = new PostContext())
            {
                Post post = db.Posts.First(p => p.Id == id);

                return View(post);
            }
        }

        // Methos works to delete post in DB and comments with the same Post_Id
        [HttpPost]
        public ActionResult Delete(Post post)
        {
            using (var db = new PostContext())
            {
                Post postDelete = db.Posts.First(p => p.Id == post.Id);

                foreach(var comment in db.Comments.Where(c => c.Id_Post == post.Id))
                {
                  db.Comments.Remove(comment);
                }

                db.Posts.Remove(postDelete);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}
