using BlogCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogCMS.Services
{
    public interface IBlogService
    {
        Dictionary<DateTime, int> getYearMonthList(ApplicationDbContext db);
    }
    public class BlogService : IBlogService
    {
        private static BlogService instance = new BlogService();

        public static BlogService Instance
        {
            get
            {
                return instance;
            }
        }

        private BlogService()
        {
        }

        private ILookup<DateTime, Post> _ordersByYearMonth;
        private Dictionary<DateTime, int> _yearmonthList;
        public Dictionary<DateTime, int> getYearMonthList(ApplicationDbContext db)
        {
            var _posts = db.Posts.OrderByDescending(p => p.Published);

            #region list_of_articles_year_month
            
            _ordersByYearMonth =
                _posts.ToLookup(order => new DateTime(order.Published.Year, order.Published.Month, 1));

            _yearmonthList = new Dictionary<DateTime, int>();

            foreach (var item in _ordersByYearMonth)
            {
                _yearmonthList.Add(item.Key, item.Count());
            }

            #endregion

            return _yearmonthList;
        }
    }
}