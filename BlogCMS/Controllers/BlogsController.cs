using BlogCMS.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogCMS.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public BlogsController()
        {
        }

        public BlogsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }        

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Blogs
        public ActionResult Index(int? index)
        {
            int _startIndex = 0;
            int pageCount = 5;

            if (index != null)
            {
                if (index > 0)
                {
                    index = index - 1;
                }
                _startIndex = (int)index * pageCount;
            }

            int _endIndex = _startIndex + pageCount;

            
            int articleCount = db.Posts.Count();
            //classify = (input > 0) ? "positive" : "negative";
            ViewBag.Pages = (articleCount % pageCount == 0) ? articleCount / pageCount : (articleCount / pageCount) + 1;
            List<Post> list = db.Posts.OrderBy(a => a.Id).Skip(_startIndex).Take(_endIndex - _startIndex).ToList();

            int charCount = 100;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Description?.Length > charCount)
                {
                    string shortDesc = list[i].Description.Substring(0, charCount) + "...";
                    list[i].Description = shortDesc;
                }

            }
            return View(list);
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                post.PublisherId = user.Id;
                post.Published = DateTime.Now;

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                post.PublisherId = user.Id;
                post.Published = DateTime.Now;

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Post post = db.Posts.Find(id);
                db.Posts.Remove(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
