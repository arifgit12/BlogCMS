using BlogCMS.Models;
using BlogCMS.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogCMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        IBlogService _dataService;

        public HomeController()
        {
            _dataService = BlogService.Instance;// new DataService();
        }

        public ActionResult Index(int page = 1, int pageSize = 3)
        {

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 5;
           

            #region list_of_articles_year_month
                ViewData["yearList"] = _dataService.getYearMonthList(db);
            #endregion

            // Get Articles
            List<Post> _list = new List<Post>();
            _list = db.Posts.Where(p => p.isPublished == true)
               .OrderByDescending(a => a.Published)
               .ToList();

            // Display Articles
            int charCount = 200;
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Description?.Length > charCount)
                {
                    string shortDesc = _list[i].Description.Substring(0, charCount) + "...";
                    _list[i].Description = shortDesc;
                }
            }
                        
            return View(_list.ToPagedList(page, pageSize));
        }

        public ActionResult Date(string id, int page = 1, int pageSize = 3)
        {
            ViewBag.dateQuery = String.IsNullOrEmpty(id) ? "" : id;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 5;

            ViewData["yearList"] = _dataService.getYearMonthList(db);

            string[] yearMonth = id.Split('-');
            int year = int.Parse(yearMonth[0]);
            int month = int.Parse(yearMonth[1]);

            var listOfDate = db.Posts
                                .Where(p => p.Published.Year == year && p.Published.Month == month)
                                .OrderByDescending(a => a.Published)
                                .ToList();

            return View("Index", listOfDate.ToPagedList(page, pageSize));
        }

        public ActionResult Search(string q, int page = 1, int pageSize = 3)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 25;

            ViewData["yearList"] = _dataService.getYearMonthList(db);

            var listOfSearch = db.Posts.Where(p => p.Title.Contains(q) ||
            p.Description.Contains(q))
            .OrderByDescending(p => p.Published)
            .ToList();

            return View("Index", listOfSearch.ToPagedList(page, pageSize));
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                id = 1;
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            #region list_of_articles_year_month
            ViewData["yearListDetails"] = _dataService.getYearMonthList(db);
            #endregion
            return View(post);
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