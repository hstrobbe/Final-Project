using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FInalProject.DATA.EF;

namespace FInalProject.UI.MVC.Controllers
{
    public class LessonViewsController : Controller
    {
        private FinalProjectEntities db = new FinalProjectEntities();

        // GET: LessonViews
        public ActionResult Index()
        {
            var lessonViews = db.LessonViews.Include(l => l.Lesson).Include(l => l.UserDetail);
            return View(lessonViews.ToList());
        }

        // GET: LessonViews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessonViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        // GET: LessonViews/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle");
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName");
            return View();
        }

        // POST: LessonViews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonViewId,UserId,LessonId,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                db.LessonViews.Add(lessonView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle", lessonView.LessonId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", lessonView.UserId);
            return View(lessonView);
        }

        // GET: LessonViews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessonViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle", lessonView.LessonId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", lessonView.UserId);
            return View(lessonView);
        }

        // POST: LessonViews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonViewId,UserId,LessonId,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lessonView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "LessonId", "LessonTitle", lessonView.LessonId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", lessonView.UserId);
            return View(lessonView);
        }

        // GET: LessonViews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessonViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        // POST: LessonViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LessonView lessonView = db.LessonViews.Find(id);
            db.LessonViews.Remove(lessonView);
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
