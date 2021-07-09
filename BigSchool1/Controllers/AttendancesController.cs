﻿using BigSchool1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool1.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            BigSchool1Context context = new BigSchool1Context();
            if(context.Attendance.Any(p => p.Attendee == userID && p.CourseID == attendanceDto.Id))
            {
                return BadRequest("The attendance already exists");

            }
            var attendance = new Attendance() { CourseID = attendanceDto.Id, Attendee = User.Identity.GetUserId() };
            context.Attendance.Add(attendance);
            context.SaveChanges();
            return Ok();
        }
    }
}
