using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace FinanceAccounting.WebUI.Services.AuthProvider
{
    public static class JwtParser
    {
        public static bool TryParseJwt(string jwt, out List<Claim> claims)
        {
            claims = null;
            if (string.IsNullOrWhiteSpace(jwt) || !IsJwt(jwt, out byte[] payload))
            {
                return false;
            }

            claims = new List<Claim>();
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(payload);
            claims.AddRange(keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));
            return true;
        }

        private static bool IsJwt(string jwt, out byte[] payloadAsJsonBytes)
        {
            payloadAsJsonBytes = default;
            string[] jwtSplitted = jwt.Split('.');
            if (jwtSplitted.Length != 3)
            {
                return false;
            }

            try
            {
                byte[] headerAsJsonBytes = Convert.FromBase64String(jwtSplitted[0]);
                JsonDocument headerJson = JsonDocument.Parse(new ReadOnlyMemory<byte>(headerAsJsonBytes));
                if (!IsHeaderValid(headerJson))
                {
                    return false;
                }

                payloadAsJsonBytes = ParseBase64WithoutPadding(jwtSplitted[1]);
                JsonDocument payloadJson = JsonDocument.Parse(new ReadOnlyMemory<byte>(payloadAsJsonBytes));
                if (payloadJson.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return false;
                }
            }
            catch (JsonException)
            {
                return false;
            }

            return true;
        }

        private static bool IsHeaderValid(JsonDocument headerJson)
        {
            return headerJson.RootElement.TryGetProperty("alg", out _)
                   && headerJson.RootElement.TryGetProperty("typ", out JsonElement tokenType)
                   && tokenType.GetString() == "JWT";
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
