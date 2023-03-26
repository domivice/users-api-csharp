using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Domivice.Users.Web.Tests.Extensions;

public static class HttpClientJsonExtensions
{
    public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(this HttpClient client, string? requestUri,
        TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));

        var content = JsonContent.Create(value, null, options);
        return client.PatchAsync(requestUri, content, cancellationToken);
    }
}