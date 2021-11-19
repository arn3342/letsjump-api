using Dapper;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Extensions;
using LetsJump.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
namespace LetsJump.DataAccess
{
    public class ReviewAccess : DataCredential
    {
        private string spPrefix = "sp_review_";

        public (bool IsSuccess, ReviewData Review) Create(Review review)
        {
            connection.Open();
            var newReview = 0;
            var id = 0;
            ReviewData reviewData = new ReviewData();
            #region Checking if values are OK
            if (connection.IsValidUser(review) && IsSuccess(review.UserID, review.OrganizationID, review.ReviewText))
            {
                //If ReviewID is passed, the below will update an existing record.
                var random = new Random();
                id = random.Next(1000, 9999);
                if (!IsSuccess(review.ReviewID)) {
                    review.ReviewID = id;
                }
                newReview = connection.Execute(spPrefix + "create", review, commandType: CommandType.StoredProcedure);
                var addPoints = connection.Execute("sp_user_updatePoints", new { point = 5, uid = review.UserID }, commandType: CommandType.StoredProcedure);

                reviewData = GetReviews(new Review { ReviewID = review.ReviewID })[0];
            }
            #endregion

            connection.Close();

            return (IsSuccess(newReview), reviewData);
        }

        public List<ReviewData> GetReviews(Review review)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            //if (data.Parameter.Contains("accessToken"))
            //{
            //    data.Parameter = data.Parameter.Replace("accessToken", "userID");

            //    CommonData common = new CommonData
            //    {
            //        AccessToken = data.Value
            //    };
            //    data.Value = data.Value.Replace(data.Value, common.UserID.ToString());
            //}
            #region Constructing the query
            string query = "SELECT checkin.checkinid, checkin.CheckInDate, reviews.ReviewId, reviews.ReviewText, reviews.ReviewRating, " +
                "reviews.ReviewMediaURLs, reviews.OrganizationID, user_details.FirstName, user_details.LastName, user_details.UserID, " +
                "user_details.ImageUrl From reviews Inner Join user_details ON {conditions}" +
                " AND user_details.UserID=reviews.UserID INNER JOIN checkin ON checkin.checkInID = reviews.checkInID";

            string tablePrefix = "reviews.";
            string condition = "";
            foreach (PropertyInfo info in review.GetType().GetProperties())
            {
                var value = info.GetValue(review);
                if (IsSuccess(value) && info.Name != "AccessToken")
                {
                    if (info.Name == "UserID")
                    {
                        CommonData currentUser = new CommonData() { UserID = review.UserID, AccessToken = review.AccessToken };
                        if (UserAccess.VerifyToken(currentUser))
                        {
                            condition += tablePrefix + info.Name + "=" + value + " AND ";
                        }
                    }
                    else condition += tablePrefix + info.Name + "=" + value + " AND ";
                }
            }
            condition = condition.ReplaceLastOccurrence("AND", "");
            #endregion
            query = query.Replace("{conditions}", condition);

            var reviewData = connection.Query<ReviewData>(query).ToList();
            var reviewDataList = reviewData == null ? new List<ReviewData>() : reviewData;

            //Checking if editable
            if (IsSuccess(review.AccessToken))
            {
                CommonData user = new CommonData() { AccessToken = review.AccessToken };
                reviewDataList.All(x => { user.UserID = x.UserID; x.SetEditable(user); return true; });
            }
            connection.Close();
            return reviewDataList;
        }

        public bool AddReport(Report report)
        {
            connection.Open();
            var newReport = 0;
            #region Checking if values are OK
            newReport = connection.Execute(spPrefix + "report_create", report, commandType: CommandType.StoredProcedure);
            #endregion

            connection.Close();

            return IsSuccess(newReport);
        }

        public bool UpdateReport(Report report)
        {
            connection.Open();
            var newReport = 0;
            #region Checking if values are OK
            newReport = connection.Execute("Update review_reports SET IsRemoved=" + report.isRemoved + " where ReportID=" + report.reportID);
            #endregion

            connection.Close();

            return IsSuccess(newReport);
        }

        public (bool IsSuccess, object Reports) GetReport(Report report)
        {
            connection.Open();
            object reports;
            #region Checking if values are OK
            if (report.reportID != 0) {
                reports = connection.QuerySingle("Select * from review_reports Where reportID=" + report.reportID);
            }
            else
            {
                reports = connection.Query("Select * from review_reports");
            }
            #endregion

            connection.Close();

            return (IsSuccess(report), reports);
        }
        public Review AddReact(Review review)
        {
            connection.Open();

            #region Constructing the query
            string query = "Update reviews SET {0} = {1} + 1 Where ReviewID='" + review.ReviewID.ToString() + "'";
            query = string.Format(query, review.ReviewLikeCount > 0 ? "ReviewLikeCount" : review.ReviewDislikeCount > 0 ? "ReviewDislikeCount" : "",
                review.ReviewLikeCount > 0 ? "ReviewLikeCount" : review.ReviewDislikeCount > 0 ? "ReviewDislikeCount" : "");
            #endregion

            var addReact = connection.Execute(query);
            if (IsSuccess(addReact))
            {
                var newReview = connection.QuerySingle<Review>("Select * from Reviews where ReviewID=@reviewId", new { reviewId = review.ReviewID });
                return newReview;
            }
            return null;
        }
    }
}
