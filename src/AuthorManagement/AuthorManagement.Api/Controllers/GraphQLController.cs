using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Controllers
{
    public class GraphQLRequest
    {
        public string Query { get; set; }
        public string OperationName { get; set; }
        [JsonConverter(typeof(ObjectDictionaryConverter))]
        public Dictionary<string, object> Variables { get; set; }
    }

    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;

        public GraphQLController(IDocumentExecuter executer, IDocumentWriter writer)
        {
            _executer = executer;
            _writer = writer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GraphQLRequest request, [FromServices]ISchema schema)
        {
            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.OperationName = request.OperationName;
                _.Query = request.Query;
                _.Inputs = request.Variables.ToInputs();
                _.EnableMetrics = true;
            });

            var json = await _writer.WriteToStringAsync(result);

            var httpResult = result.Errors?.Count > 0
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.OK;

            return Content(json, "application/json");
        }
    }
}
