using BigSchool1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool1.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Create()
        {
            BigSchool1Context context = new BigSchool1Context();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Category.ToList();
            
            return View(objCourse);
            
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchool1Context context = new BigSchool1Context();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Category.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            context.Course.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            BigSchool1Context context = new BigSchool1Context();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach(Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LecturerName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }

        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchool1Context context = new BigSchool1Context();
            var courses = context.Course.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach(Course i in courses)
            {
                i.LecturerName = currentUser.Name;
            }
            return View(courses);
        }

        public ActionResult EditCourse(int id)
        {
            BigSchool1Context context = new BigSchool1Context();

            Course db = context.Course.SingleOrDefault(p => p.Id == id);
            db.ListCategory = context.Category.ToList();

            return View(db);
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditCourse(Course courses)
        {
            BigSchool1Context context = new BigSchool1Context();
            Course dbUpdate = context.Course.SingleOrDefault(p => p.Id == courses.Id);
            if (dbUpdate != null)
            {
                context.Course.AddOrUpdate(courses);
                context.SaveChanges();

            }

            return RedirectToAction("Mine");
        }

        public ActionResult Delete(int id)
        {
            BigSchool1Context context = new BigSchool1Context();
            Course db = context.Course.SingleOrDefault(p => p.Id == id);
            if (db == null)
            {
                return HttpNotFound();
            }
            return View(db);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(Course c)
        {
            BigSchool1Context context = new BigSchool1Context();
            Course db = context.Course.SingleOrDefault(p => p.Id == c.Id);
            if (db != null)
            {
                context.Course.Remove(db);
                context.SaveChanges();
            }

            return RedirectToAction("Mine");
        }

    }
}