using System.Text.Encodings.Web;
using System.Text.Json;
using FastSlnPresentation.BLL.Models.Graph.GraphData;

namespace FastSlnPresentation.BLL.Services.Static
{
    public static class JsonService
    {
        public static string Serialize(object graph)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };

            var json = JsonSerializer.Serialize(graph, serializeOptions);
            return json;
        }
    }
}
