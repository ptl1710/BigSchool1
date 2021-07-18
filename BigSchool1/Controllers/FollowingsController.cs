using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BigSchool1.Models;
using Microsoft.AspNet.Identity;

namespace BigSchool1.Controllers
{
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            //user login là người theo dõi, follow.FolloweeId là người được theo dõi
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Please login first!");
            if (userID == follow.FolloweeId)
                return BadRequest("Can not follow myself!");
            BigShool1Context context = new BigShool1Context();
            //kiểm tra xem mã userID đã được theo dõi chưa
            Following find = context.Following.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId);
            if (find != null)
            {
                context.Following.Remove(context.Following.SingleOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId));
                context.SaveChanges();
                return Ok("cancel");
            }
            //set object follow
            follow.FollowerId = userID;
            context.Following.Add(follow);
            context.SaveChanges();
            return Ok();
        }
    }
}
