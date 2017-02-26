﻿//******************************************************************************
// <copyright file="license.md" company="Wlog project  (https://github.com/arduosoft/wlog)">
// Copyright (c) 2016 Wlog project  (https://github.com/arduosoft/wlog)
// Wlog project is released under LGPL terms, see license.md file.
// </copyright>
// <author>Daniele Fontani, Emanuele Bucaelli</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Helpers;
using Wlog.Library.BLL.Interfaces;
using Wlog.Library.BLL.Reporitories;
using Wlog.Test.Attributes;
using Xunit;

namespace Wlog.Test.Tests
{
    /// <summary>
    /// This test contains repository related tests.
    /// </summary>
    public class RepositoryTests
    {
        Dictionary<string, object> repoInstances = new Dictionary<string, object>();
        Dictionary<string, Type> entityTypes = new Dictionary<string, Type>();
        Dictionary<string, Type> repoTypes = new Dictionary<string, Type>();


        public RepositoryTests()
        {

            Assembly entityAssembly = Assembly.GetAssembly(typeof(LogEntity));
            Type[] classes = ReflectionHelper.GetTypesInNamespace(entityAssembly, "Wlog.Library.BLL.Reporitories");
            IndexRepository.BasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Index\");
            foreach (Type repoClass in classes)
            {
                object o = Activator.CreateInstance(repoClass);
                if (o is IRepository && !(o is SystemRepository))
                {

                    repoInstances[repoClass.Name] = o;

                    repoTypes[repoClass.Name] = repoClass;

                    entityTypes[repoClass.Name] = ((IRepository)repoInstances[repoClass.Name]).GetEntityType();
                }
            }

        }


        [Fact, TestPriority(0), Trait("Category", "ExcludedFromCI")]
        public void TestAllBaseMethods()
        {
            foreach (string repoName in repoInstances.Keys)
            {
                object repoInstanceObj = repoInstances[repoName];
                Assert.NotNull(repoInstanceObj);
                Type typeEntity = entityTypes[repoName];
                Type repoType = repoTypes[repoName];

                object entity = Activator.CreateInstance(typeEntity);
                Guid assignedid = Guid.NewGuid();
                ((IEntityBase)entity).Id = assignedid;
                var instance = Activator.CreateInstance(repoType);
                MethodInfo methodInfo = repoType.GetMethod("Save", BindingFlags.Instance | BindingFlags.Public, null, new[] { entity.GetType()}, null);
                methodInfo.Invoke(instance, new object[] { entity });
            }
        }


        [Fact, TestPriority(0), Trait("Category", "ExcludedFromCI")]
        public void TestApplication()
        {
            ApplicationRepository repository = new ApplicationRepository();
            ApplicationEntity application = new ApplicationEntity();
            application.PublicKey = Guid.NewGuid();
            application.ApplicationName = "TEST INSERT";
            bool result = false;

            // save application
            result = repository.Save(application);
            Assert.True(result);
            int count = repository.Count(x => x.ApplicationName == "TEST INSERT");
            Assert.Equal(count, 1);

            // rename application
            application.ApplicationName = "TEST UPDATED";
            result = repository.Save(application);
            Assert.True(result);
            count = repository.Count(x => x.ApplicationName == "TEST INSERT");
            Assert.Equal(count, 0);
            count = repository.Count(x => x.ApplicationName == "TEST UPDATED");
            Assert.Equal(count, 1);

            // delete application
            repository.Delete(application);
            count = repository.Count(x => x.ApplicationName == "TEST UPDATED");
            Assert.Equal(count, 0);
        }
    }
}
