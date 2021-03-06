﻿namespace Wlog.Test.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BLL.Entities;
    using Library.BLL.Reporitories;
    using Library.Scheduler.Jobs;
    using Attributes;
    using Xunit;

    /// <summary>
    /// Test for moving logs into bing
    /// </summary>
    public class MoveToBinJobTest : IDisposable
    {
        private readonly List<LogEntity> _logs = new List<LogEntity>();

        #region test methods
        public MoveToBinJobTest()
        {
            //Create directory for index and tell to use it
            string foldername = ".\\MoveToBinJobTest\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Directory.CreateDirectory(foldername);
            IndexRepository.BasePath = foldername;

            // insert test logs in DB
            InsertTestLogsInDB();

            // run MoveToBinJob
            ExecuteMoveToBin();

           
        }

        private void InsertTestLogsInDB()
        {
            _logs.Add(new LogEntity
            {
                CreateDate = DateTime.Now.AddDays(-40),
                SourceDate = DateTime.UtcNow,
                UpdateDate = DateTime.Now,
                Level="ERROR",
                Message = "Message 1"
            });
            _logs.Add(new LogEntity
            {
                CreateDate = DateTime.Now.AddDays(-40),
                SourceDate = DateTime.UtcNow.AddDays(-48),
                UpdateDate = DateTime.Now,
                Level = "INFO",
                Message = "Message 2"

            });
            _logs.Add(new LogEntity
            {
                CreateDate = DateTime.Now.AddDays(-40),
                SourceDate = DateTime.UtcNow.AddDays(-60),
                UpdateDate = DateTime.Now,
                Level = "INFO",
                Message = "Message 3"
            });

            foreach (var log in _logs)
            {
                RepositoryContext.Current.Logs.Save(log);
            }
        }

        private static void ExecuteMoveToBin()
        {
            var moveToBin = new MoveToBinJob(1, 30);
            moveToBin.Execute();
        }

        #endregion

        [Fact, TestPriority(0), Trait("Category", "ExcludedFromCI")]
        public void MoveToBinJob_CheckLogsTable_Success()
        {
            var allLogs = RepositoryContext.Current.Logs.GetAllLogEntities();

            Assert.True(allLogs.Any(x => x.Uid == _logs[0].Uid));
            Assert.False(allLogs.Any(x => x.Uid == _logs[1].Uid));
            Assert.False(allLogs.Any(x => x.Uid == _logs[2].Uid));
        }

        [Fact, TestPriority(1), Trait("Category", "ExcludedFromCI")]
        public void MoveToBinJob_CheckDeletedLogsTable_Success()
        {
            var allDeletedLogs = RepositoryContext.Current.DeletedLogs.GetAllDeletedLogEntities();

            Assert.False(allDeletedLogs.Any(x => x.Uid == _logs[0].Uid));
            Assert.True(allDeletedLogs.Any(x => x.LogId == _logs[1].Uid));
            Assert.True(allDeletedLogs.Any(x => x.LogId == _logs[2].Uid));
        }

        /// <summary>
        /// Clean Up after the UnitTest
        /// </summary>
        public void Dispose()
        {
            // get deleted logs for clean up
            var deletedLogs = RepositoryContext.Current.DeletedLogs.GetAllDeletedLogEntities()
                .Where(x => _logs.Any(u => u.Uid == x.LogId));

            RepositoryContext.Current.Logs.RemoveLogEntity(_logs[0]);

            if (deletedLogs.Any())
            {
                // if tests passed clean up the deleted Log table
                foreach (var deletedLog in deletedLogs)
                {
                    RepositoryContext.Current.DeletedLogs.RemoveDeletedLogEntity(deletedLog);
                }
            }
            else
            {
                // if tests failed and nothing iserted
                RepositoryContext.Current.Logs.RemoveLogEntity(_logs[1]);
                RepositoryContext.Current.Logs.RemoveLogEntity(_logs[2]);
            }

            GC.SuppressFinalize(this);
        }
    }
}
