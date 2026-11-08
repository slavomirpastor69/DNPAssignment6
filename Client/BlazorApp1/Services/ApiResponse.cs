using System.Text.Json;

namespace BlazorApp1.Services;

internal static class ApiResponse
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // Returns the deserialized body, or throws with the API's error message so pages can display it.
    public static async Task<T> ReadAsync<T>(HttpResponseMessage response)
    {
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(content)
                ? $"The request failed with status code {(int)response.StatusCode}."
                : content);
        }

        return JsonSerializer.Deserialize<T>(content, JsonOptions)!;
    }
}
