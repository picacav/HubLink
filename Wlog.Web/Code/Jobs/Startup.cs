﻿using Hangfire;
using Hangfire.Dashboard;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


//Wlog.Web.Code.Jobs.Startup
namespace Wlog.Web.Code.Jobs
{
   
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new DashboardOptions
            {
                 AppPath = VirtualPathUtility.ToAbsolute("~"),
                AuthorizationFilters = new[]
            {
                
                new HangFireAuthorizationFilter()
            }
            };

            app.UseHangfireDashboard("/private/hangfire", options);
            app.UseHangfireServer();
        }
    }
}