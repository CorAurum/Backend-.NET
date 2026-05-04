using System.Text.Json;

namespace TuPenca.Application.Tests
{
    public static class JsonHelpers
    {
        public static Guid ExtractFirstId(JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Array)
            {
                var first = json[0];

                if (first.TryGetProperty("id", out var idProp))
                    return idProp.GetGuid();

                return first.GetGuid();
            }

            if (json.TryGetProperty("id", out var id))
                return id.GetGuid();

            if (json.TryGetProperty("ids", out var ids))
                return ids[0].GetGuid();

            throw new Exception("No se pudo extraer ID");
        }

        public static Guid ExtractSecondId(JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Array)
            {
                var second = json[1];

                if (second.TryGetProperty("id", out var idProp))
                    return idProp.GetGuid();

                return second.GetGuid();
            }

            throw new Exception("No se pudo extraer segundo ID");
        }

        public static string GetString(JsonElement json, string prop)
        {
            return json.GetProperty(prop).GetString();
        }
    }
}
