using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using FInalProject.DATA.EF;
using Microsoft.AspNet.Identity;
using FInalProject.UI.MVC.Models;

namespace FInalProject.UI.MVC.Controllers
{
    public class LessonsController : Controller
    {
        private FinalProjectEntities db = new FinalProjectEntities();

        // GET: Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Course);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int id, CourseCompletionEmailViewModel ccevm)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            #region Youtube


            if (lesson.VideoURL != null)
            {
                var v = lesson.VideoURL.IndexOf("v=");
                var amp = lesson.VideoURL.IndexOf("&", v);
                string vid;
                // if the video id is the last value in the url
                if (amp == -1)
                {
                    vid = lesson.VideoURL.Substring(v + 2);
                    // if there are other parameters after the video id in the url
                }
                else
                {
                    vid = lesson.VideoURL.Substring(v + 2, amp - (v + 2));
                }
                ViewBag.VideoID = vid;
            }





            #endregion

            #region Lesson view
            string userid = User.Identity.GetUserId();
            UserDetail ud = db.UserDetails.Find(userid);
            LessonView lessonView = new LessonView();
            lessonView.LessonId = id;
            lessonView.UserId = userid;
            lessonView.DateViewed = DateTime.Now;
            db.LessonViews.Add(lessonView);
            db.SaveChanges();
            
            //var lessonView = db.LessonViews.Include(c => c.DateViewed).Include(c => c.LessonId);
            //return View(lessonView.ToList());
            #endregion

            #region Coursecompletion and email
            int nbrLs = db.Lessons.Where(x => x.CourseId == lesson.CourseId).Count();
            int nbrLvs = db.LessonViews.Where(x => x.UserId == userid && x.Lesson.CourseId == lesson.CourseId).Count();
            if (nbrLs == nbrLvs)
            {
                
              
                CourseCompletion courseCompletion = new CourseCompletion();
                courseCompletion.CourseId = lesson.CourseId;
                courseCompletion.UserId = userid;
                courseCompletion.DateCompleted = DateTime.Now;
                db.CourseCompletions.Add(courseCompletion);
                db.SaveChanges();

               
            }

            #endregion

            return View(lesson);
        }

        // GET: Lessons/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonId,LessonTitle,CourseId,Introduction,VideoURL,PdfFileName,IsActive")] Lesson lesson, HttpPostedFileBase coverImage)
        {
            if (ModelState.IsValid)
            {
                string pdfName = "No Document Provided.pdf";
                if (coverImage != null)
                {
                    pdfName = coverImage.FileName;
                    string ext = pdfName.Substring(pdfName.LastIndexOf('.'));
                    string[] goodExts = { ".pdf" };
                    if (goodExts.Contains(ext.ToLower()))
                    {
                        coverImage.SaveAs(Server.MapPath("~/Content/" + pdfName));
                    }
                    else
                    {
                        pdfName = "No Document Provided.pdf";
                    }
                }
                lesson.PdfFileName = pdfName;

                db.Lessons.Add(lesson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", lesson.CourseId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", lesson.CourseId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonId,LessonTitle,CourseId,Introduction,VideoURL,PdfFileName,IsActive")] Lesson lesson, HttpPostedFileBase coverImage)
        {
            if (ModelState.IsValid)
            {
                string pdfName = "No Document Provided.pdf";
                if (coverImage != null)
                {
                    pdfName = coverImage.FileName;
                    string ext = pdfName.Substring(pdfName.LastIndexOf('.'));
                    string[] goodExts = { ".pdf" };
                    if (goodExts.Contains(ext.ToLower()))
                    {
                        coverImage.SaveAs(Server.MapPath("~/Content/" + pdfName));
                    }
                    lesson.PdfFileName = pdfName;
                }


                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", lesson.CourseId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
