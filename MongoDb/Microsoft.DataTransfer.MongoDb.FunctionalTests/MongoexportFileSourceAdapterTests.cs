﻿using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.MongoDb.Source.Mongoexport;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.FunctionalTests
{
    [TestClass]
    public sealed class MongoexportFileSourceAdapterTests : DataTransferTestBase
    {
        private static readonly Dictionary<string, object>[] ExpectedData = new[]
            {
                new Dictionary<string, object>
                {
                    { "_id", "53bacf27d1e6f947c094bbbc" },
                    { "name", "Employee 1" },
                    { "email", "email1@email.com" },
                    { "createddate", DateTime.Parse("2014-07-07T09:47:35.902-0700").ToUniversalTime() },
                },
                new Dictionary<string, object>
                {
                    { "_id", "53bacf27d1e6f947c094bbbd" },
                    { "name", "Employee 2" },
                    { "email", "email2@email.com" },
                    { "createddate", DateTime.Parse("2014-07-07T09:47:40.903-0700").ToUniversalTime() },
                }
            };

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\mongoexport.json", @"InputData")]
        public async Task ReadSampleFile_AllFieldsRead()
        {
            DataItemCollectionAssert.AreEquivalent(
                ExpectedData, await ReadData(@"InputData\mongoexport.json"), TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\mongoexport_emptylines.json", @"InputData")]
        public async Task ReadSampleFileWithEmptyLines_AllFieldsRead()
        {
            DataItemCollectionAssert.AreEquivalent(
                ExpectedData, await ReadData(@"InputData\mongoexport_emptylines.json"), TestResources.InvalidDocumentsRead);
        }

        private static async Task<List<IDataItem>> ReadData(string fileName)
        {
            var configuration = Mocks
                    .Of<IMongoexportFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { fileName })
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = await (new MongoexportFileSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance)))
            {
                var readOutput = new ReadOutputByRef();

                IDataItem dataItem;
                while ((dataItem = await adapter.ReadNextAsync(readOutput, CancellationToken.None)) != null)
                {
                    readResults.Add(dataItem);

                    Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                    readOutput.Wipe();
                }
            }

            return readResults;
        }
    }
}
