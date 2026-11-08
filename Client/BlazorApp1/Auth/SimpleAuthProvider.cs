using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp1.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private const string StorageKey = "currentUser";

    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task LoginAsync(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("auth/login", new LoginRequest
        {
            UserName = userName,
            Password = password
        });
        UserDto user = await ApiResponse.ReadAsync<UserDto>(response);

        // Kept in the browser, because this scoped service is recreated when the page is refreshed.
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", StorageKey, JsonSerializer.Serialize(user));

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(CreatePrincipal(user))));
    }

    public async Task LogoutAsync()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", StorageKey);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? userAsJson;
        try
        {
            userAsJson = await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", StorageKey);
        }
        catch (Exception e) when (e is InvalidOperationException or JSDisconnectedException)
        {
            // JavaScript is unavailable while there is no connection to the browser.
            return new AuthenticationState(new ClaimsPrincipal());
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        UserDto user = JsonSerializer.Deserialize<UserDto>(userAsJson)!;
        return new AuthenticationState(CreatePrincipal(user));
    }

    private static ClaimsPrincipal CreatePrincipal(UserDto user)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.Name, user.UserName),
            new(ClaimsPrincipalExtensions.UserIdClaimType, user.Id.ToString())
        ];
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
    }
}
