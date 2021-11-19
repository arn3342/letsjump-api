using LetsJump.DataAccess;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Models;
using LetsJump.DataAccess.Security;
using LetsJump.DataAccess.Tools;
using Microsoft.AspNetCore.Mvc;

namespace LetsJump.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserAccess userAccess;
        public UserController()
        {
            userAccess = new UserAccess();
        }
        [HttpPost]
        public ActionResult<string> Create(UserLogin user)
        {
            var createUser = userAccess.Create(user);
            if (createUser.IsSuccess)
            {
                Messaging messaging = new Messaging()
                {
                    EmailOrPhone = user.Email,
                    messageType = Messaging.MessageType.Verification
                };
                messaging.SendMessage(user);
            }

            return createUser.IsSuccess ? StatusCode(201, createUser.User) : StatusCode(302, "User already exists.");
        }

        [HttpPost("profile/update")]
        public ActionResult<UserDetails> UpdateProfile(UserDetails user)
        {
            var updateProfile = userAccess.UpdateProfile(user);
            return updateProfile.IsSuccess ? StatusCode(201, updateProfile.newProfile) : StatusCode(502, "There specific user does not exsist.");
        }
        [HttpPost("profile/get")]
        public ActionResult<UserDetails> GetProfile(UserDetails user)
        {
            var profile = userAccess.GetProfile(user);
            return profile.IsSuccess ? StatusCode(201, profile.Profile) : StatusCode(502, "There specific user does not exsist.");
        }
        [HttpPost("login")]
        public ActionResult<object> Login(UserLogin user)
        {
            var currentUser = userAccess.LoginUser(user);
            return currentUser.IsSuccess ? StatusCode(200, currentUser.User) : StatusCode(401, "Email and password do not match.");
        }
        [HttpPost("Get")]
        public ActionResult<object> GetAllUsers(UserLogin user)
        {
            var currentUser = userAccess.GetAllUsers(user);
            return currentUser.IsSuccess ? StatusCode(200, currentUser.User) : StatusCode(401, "No users yet");
        }
        [HttpPost("GetByID")]
        public ActionResult<object> GetUserByID(UserLogin user)
        {
            var currentUser = userAccess.GetUserByID(user);
            return currentUser.IsSuccess ? StatusCode(200, currentUser.User) : StatusCode(401, "Email and password do not match.");
        }

        [HttpPost("verify")]
        public ActionResult<string> Verify(string email)
        {
            var verifiedUser = userAccess.VerifyUser(email);
            return verifiedUser ? StatusCode(201, "User has been verified.") : StatusCode(401, "Unable to verify user.");
        }

        [HttpGet("verify")]
        public ActionResult<string> Verify_Get(string email)
        {
            var checkVerification = userAccess.VerifyUser(email);
            return checkVerification ? StatusCode(200, "User is already verified.") : StatusCode(200, "User is not verified yet.");
        }
        [HttpPost("password/requestreset")]
        public ActionResult<string> RequestReset(string Email)
        {
            var requestReset = userAccess.RequestResetPassword(Email);
            return requestReset ? StatusCode(200, "The password reset request was successful for user with email/phone: " + Email ) : StatusCode(200, "There was a problem making the password reset request.");
        }

        [HttpPost("password/reset")]
        public ActionResult<string> ResetPassword(PasswordReset reset)
        {
            var updatePassword = userAccess.ResetPassword(reset);
            return updatePassword ? StatusCode(200, "Password updated successfully!") : StatusCode(200, "There was a problem updating the password.");
        }

        [HttpPost("compareToken")]
        public ActionResult<string> VerifyAccesstoken(CommonData user)
        {
            var verifyResult = UserAccess.VerifyToken(user);
            var userData = new CommonData() { AccessToken = user.AccessToken };
            return verifyResult ? StatusCode(200, "Token and ID Matched!") : StatusCode(401, "Token and ID mismatch");
        }
        //[HttpPost("test/accesstoken")]
        //public string TestAccess(int UserID)
        //{
        //    return TokenManager.GenerateAccessToken(UserID);
        //}

        //[HttpPost("test/getId")]
        //public int TestID(string accessToken)
        //{
        //    return TokenManager.DecipherID(accessToken);
        //}
    }
}
