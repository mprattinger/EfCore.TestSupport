﻿// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System.Linq;
using DataLayer.BookApp;
using DataLayer.EfCode.BookApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using TestSupport.DesignTimeServices;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.EfSchemaCompare
{
    public class TestScaffolder 
    {
        private readonly ITestOutputHelper _output;
        private readonly string _connectionString;
        public TestScaffolder(ITestOutputHelper output)
        {
            _output = output;
            var options = this
                .CreateUniqueClassOptions<BookContext>();

            using (var context = new BookContext(options))
            {
                _connectionString = context.Database.GetDbConnection().ConnectionString;
                context.Database.EnsureCreated();
            }
        }


        [Fact]
        public void GetDatabaseModel()
        {
            //SETUP
            var serviceProvider = new SqlServerDesignTimeServices().GetDesignTimeProvider();
            var factory = serviceProvider.GetService<IDatabaseModelFactory>();

            //ATTEMPT 
            var model = factory.Create(_connectionString, new string[] { }, new string[] { });

            //VERIFY
            model.ShouldNotBeNull();
        }


        [Fact]
        public void GetScalarEntityInModel()
        {
            //SETUP
            var serviceProvider = new SqlServerDesignTimeServices().GetDesignTimeProvider();
            var factory = serviceProvider.GetService<IScaffoldingModelFactory>();

            var model = factory.Create(_connectionString, new string[] { }, new string[] { }, false);

            //ATTEMPT 
            var entities = model?.GetEntityTypes();
            var entity = model?.GetEntityTypes().FirstOrDefault(x => x.Name == nameof(BookContext.Books));

            //VERIFY
            entity.ShouldNotBeNull();
        }

    }
}