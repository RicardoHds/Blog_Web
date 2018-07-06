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
        public ActionResult Index()
        {
            // Accedemos a todos los posts que tenemos en la base de datos y enviamos a la vista
            using (var db = new PostContext())
            {
                return View(db.Posts.ToList());
            }
        }

        // Metodo para vista agregar y editar
        [HttpGet]
        public ActionResult Add(int? id)
        {
            // Si no recibe ID enviamos a la vista para agregar nuevo registro, sino usamos instrucciones para enviar a vista editar
            if (!id.HasValue)
            {
                using (var db = new PostContext())
                {
                    // Cargar la lista de categorias que tenemos en la DB
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
                    // Buscamos el ID recibido en parametro y buscamos en la DB para editar
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
        
        // Metodo para agregar y editar
        [HttpPost]
        public ActionResult Add(Post post)
        {
            // Si temos un ID diferente de 0 modificamos el registro del post
            if (post.Id == 0)
            {
                using (var db = new PostContext())
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
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

        // Metodo para mostrar el post
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

        // Metodo para agregar comentario
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

        // Metodo para enviar a pantalla de confirmacion eliminar
        public ActionResult Delete(int id)
        {
            using (var db = new PostContext())
            {
                Post post = db.Posts.First(p => p.Id == id);
                return View(post);
            }
        }

        // Metodo para eliminar
        [HttpPost]
        public ActionResult Delete(Post post)
        {
            using (var db = new PostContext())
            {
                Post postDelete = db.Posts.First(p => p.Id == post.Id);
                db.Posts.Remove(postDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}