﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dwapi.SettingsManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.SettingsManagement.Infrastructure.Tests
{
    [TestFixture]
    [Category("Db")]
    public class SettingsContextTests
    {

        private IServiceProvider _serviceProvider;
        private IServiceProvider _serviceProviderMysql;
        private const string MysqlConnection = "server=127.0.0.1;port=3307;database=dwapidevb;user=root;password=test";

        [OneTimeSetUp]
        public void Init()
        {
            _serviceProvider = TestInitializer.ServiceProvider;
            _serviceProviderMysql = TestInitializer.ServiceProviderMysql;

            //mysql 5.5
            _serviceProviderMysql = new ServiceCollection()
                .AddDbContext<SettingsContext>(x => x.UseMySql(MysqlConnection))
                .AddTransient<IAppDatabaseManager, AppDatabaseManager>()
                .BuildServiceProvider();
        }

        [Test]
        public void should_Setup_Mssql_Database()
        {
            var ctx = _serviceProvider.GetService<SettingsContext>();
            ctx.Database.EnsureDeleted();
            ctx.Database.Migrate();
            ctx.EnsureSeeded();

            Assert.True(ctx.Dockets.Any());
            Assert.True(ctx.EmrSystems.Any());
            Assert.True(ctx.DatabaseProtocols.Any());
            Assert.True(ctx.Extracts.Any());
            Assert.True(ctx.CentralRegistries.Any());

            Console.WriteLine(ctx.Database.ProviderName);
        }
        [Test]
        public void should_Setup_MySql_Database()
        {
           var ctx = _serviceProviderMysql.GetService<SettingsContext>();
            Console.WriteLine(ctx.Database.GetDbConnection().ConnectionString);
            ctx.Database.EnsureDeleted();
            ctx.Database.Migrate();
            ctx.EnsureSeeded();

            Assert.True(ctx.Dockets.Any());
            Assert.True(ctx.EmrSystems.Any());
            Assert.True(ctx.DatabaseProtocols.Any());
            Assert.True(ctx.Extracts.Any());
            Assert.True(ctx.CentralRegistries.Any());

            Console.WriteLine(ctx.Database.ProviderName);
        }
    }
}