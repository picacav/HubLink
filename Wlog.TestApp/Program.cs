﻿using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.WebLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.TestApp
{
    class Program
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {


            //Manual call to log service to test plain performance
            DateTime d1 = DateTime.Now;
            WebTarget.DoRequest("Http://localhost:55044/api/log", JsonConvert.SerializeObject(new NLog.WebLog.WebTarget.LogEntry() 
            { 
                Message="TEST MANUAL MESSAGE" + DateTime.Now,
                SourceDate=DateTime.Now
            }));

            double ms = DateTime.Now.Subtract(d1).TotalMilliseconds;


            //Test with log
            
            logger.Error("TEST MESSAGE" + DateTime.Now);
        }
    }
}
