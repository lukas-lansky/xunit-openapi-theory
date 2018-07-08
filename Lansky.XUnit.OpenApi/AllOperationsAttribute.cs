using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Xunit.Sdk;

namespace Lansky.XUnit.OpenApi
{
    public class AllOperationsAttribute : DataAttribute
    {
        private readonly string _specLocation;

        public AllOperationsAttribute(string specLocation)
        {
            _specLocation = specLocation;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var openApiDoc = new OpenApiStringReader().Read(File.ReadAllText(_specLocation), out var _);
            foreach (var path in openApiDoc.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    yield return new object[] { OperationToMethod(operation.Key), path.Key };
                }
            }
        }

        private HttpMethod OperationToMethod(OperationType operation)
        {
            switch (operation)
            {
                case OperationType.Get:
                    return HttpMethod.Get;

                case OperationType.Post:
                    return HttpMethod.Post;

                case OperationType.Put:
                    return HttpMethod.Put;

                case OperationType.Patch:
                    return new HttpMethod("PATCH");

                case OperationType.Options:
                    return HttpMethod.Options;

                case OperationType.Head:
                    return HttpMethod.Head;

                case OperationType.Delete:
                    return HttpMethod.Delete;

                case OperationType.Trace:
                    return HttpMethod.Trace;
            }

            throw new ArgumentException($"I can't translate this kind of OpenAPI operation: {operation}");
        }
    }
}
