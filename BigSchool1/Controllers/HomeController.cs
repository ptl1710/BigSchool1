using BigSchool1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigShool1Context context = new BigShool1Context();
            var upcomingCourse = context.Course.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();

            //lấy user login hiện tại 
            var userID = User.Identity.GetUserId();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;
                if (userID != null)
                {
                    i.isLogin = true;
                    //ktra user đó chưa tham gia khóa học 
                    Attendance find = context.Attendance.FirstOrDefault(p =>
                    p.CourseID == i.Id && p.Attendee == userID);
                    if (find == null)
                        i.isShowGoing = true;
                    //ktra user đã theo dõi giảng viên của khóa học ? 
                    Following findFollow = context.Following.FirstOrDefault(p =>
                    p.FollowerId == userID && p.FolloweeId == i.LecturerId);
                    if (findFollow == null)
                        i.isShowFollow = true;
                }
            }

            return View(upcomingCourse);
            
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