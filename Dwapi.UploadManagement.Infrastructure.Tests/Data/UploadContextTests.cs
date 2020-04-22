﻿using System;
using System.Linq;
using System.Reflection;
using Dwapi.ExtractsManagement.Core.Model.Destination;
using Dwapi.ExtractsManagement.Infrastructure;
using Dwapi.SettingsManagement.Core.Model;
using Dwapi.UploadManagement.Core.Model.Dwh;
using Dwapi.UploadManagement.Infrastructure.Data;
using Dwapi.UploadManagement.Infrastructure.Tests.TestArtifacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.UploadManagement.Infrastructure.Tests.Data
{
    [TestFixture]
    public class UploadContextTests
    {
        private UploadContext _context;


        [OneTimeSetUp]
        public void Init()
        {
            _context = TestInitializer.ServiceProvider.GetService<UploadContext>();

        }


        [Test]
        public void should_load_MPI_From_References()
        {
            Assert.True(_context.ClientMasterPatientIndices.Any());
        }

        [Test]
        public void should_load_CT_From_References()
        {
            Assert.True(_context.ClientPatientExtracts.Any());
            Assert.True(_context.ClientPatientArtExtracts.Any());
            Assert.True(_context.ClientPatientBaselinesExtracts.Any());
            Assert.True(_context.ClientPatientLaboratoryExtracts.Any());
            Assert.True(_context.ClientPatientPharmacyExtracts.Any());
            Assert.True(_context.ClientPatientStatusExtracts.Any());
            Assert.True(_context.ClientPatientVisitExtracts.Any());
            Assert.True(_context.ClientPatientAdverseEventExtracts.Any());
        }

        [Test]
        public void should_load_HTS_From_References()
        {
            Assert.True(_context.ClientPatientExtracts.Any());
            Assert.True(_context.ClientPatientArtExtracts.Any());
            Assert.True(_context.ClientPatientBaselinesExtracts.Any());
            Assert.True(_context.ClientPatientLaboratoryExtracts.Any());
            Assert.True(_context.ClientPatientPharmacyExtracts.Any());
            Assert.True(_context.ClientPatientStatusExtracts.Any());
            Assert.True(_context.ClientPatientVisitExtracts.Any());
            Assert.True(_context.ClientPatientAdverseEventExtracts.Any());
        }

        [Test]
        public void should_load_Metrics_From_References()
        {
            Assert.True(_context.AppMetrics.Any());
            Assert.True(_context.EmrMetrics.Any());
        }
    }
}
