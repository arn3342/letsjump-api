using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsJump.DataAccess;
using LetsJump.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsJump.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private ReviewAccess reviewAccess;

        public ReviewsController()
        {
            reviewAccess = new ReviewAccess();
        }

        [HttpPost]
        public ActionResult<string> Create(Review review)
        {
            var newReview = reviewAccess.Create(review);
            return newReview.IsSuccess ? StatusCode(200, newReview.Review) : StatusCode(401, "There was a problem with the call. Please check the data.");
        }
        [HttpPost("report/create")]
        public ActionResult<string> CreateReport(Report report)
        {
            var report_new = reviewAccess.AddReport(report);
            return report_new ? StatusCode(200, "Report submitted successfully") : StatusCode(401, "There was a problem with the call. Please check the data.");
        }
        [HttpPost("report/update")]
        public ActionResult<string> UpdateReport(Report report)
        {
            var report_new = reviewAccess.UpdateReport(report);
            return report_new ? StatusCode(200, "Report banned/restored successfully") : StatusCode(401, "There was a problem with the call. Please check the data.");
        }
        [HttpPost("report/get")]
        public ActionResult<object> GetReport(Report report)
        {
            var report_new = reviewAccess.GetReport(report);
            return report_new.IsSuccess ? StatusCode(200, report_new.Reports) : StatusCode(401, "There was a problem with the call. Please check the data.");
        }
        [HttpPost("get")]
        public ActionResult<IEnumerable<Review>> GetReviewes(Review review)
        {
            var reviews = reviewAccess.GetReviews(review);

            return StatusCode(201, reviews);
        }
        [HttpPost("react")]
        public ActionResult<Review> AddReact(Review review)
        {
            Review reactReview = reviewAccess.AddReact(review);
            return reactReview;
        } 
    }
}
