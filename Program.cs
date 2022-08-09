using System.Diagnostics;
using System.Runtime.InteropServices;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;

internal class Program
{
    private static readonly string clientId = "L0Fn93racqyUPF42jGOZCaeZeHvtsJKD";
    private static readonly AuthenticationApiClient authenticationApiClient = new AuthenticationApiClient("dev-0vxuabd2.eu.auth0.com");
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to Auth0 CLI Demo!");
        Console.WriteLine("Press any key to start login into the CLI...");

        var tokenResponse = await StartDeviceCodeFLowForAudience("https://example.api");
        await ConfirmTokenWithBrowser(tokenResponse);       
       
        //wait interval suggested from auth0
        Thread.Sleep(tokenResponse.Interval * 1000);

        var tokenResult = await RequestAccessTokenForDeviceCode(tokenResponse);

        Console.WriteLine("Logged in!");
        Console.WriteLine("AccessToken:");
        Console.WriteLine("------------------------");
        Console.WriteLine(tokenResult.AccessToken);
        Console.WriteLine("------------------------");

        //request second audience
        var secondAudienceTokenResponse = await StartDeviceCodeFLowForAudience("https://example-2.api");
        await ConfirmTokenWithBrowser(secondAudienceTokenResponse);       
       
        //wait interval suggested from auth0
        Thread.Sleep(secondAudienceTokenResponse.Interval * 1000);

        var secondAudienceTokenResult = await RequestAccessTokenForDeviceCode(secondAudienceTokenResponse);

        Console.WriteLine("Logged in!");
        Console.WriteLine("AccessToken:");
        Console.WriteLine("------------------------");
        Console.WriteLine(secondAudienceTokenResult.AccessToken);
        Console.WriteLine("------------------------");
    }
    public static void OpenBrowser(string url)
{
    try
    {
        Process.Start(url);
    }
    catch
    {
        // hack because of this: https://github.com/dotnet/corefx/issues/10361
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
        else
        {
            throw;
        }
    }
}

    public static async Task<DeviceCodeResponse> StartDeviceCodeFLowForAudience(string audience) {
            var request = new DeviceCodeRequest
            {
                ClientId = clientId,
                Scope = "openid profile offline_access",
                Audience = audience
            };


            return await authenticationApiClient.StartDeviceFlowAsync(request);
    }

    public static async Task ConfirmTokenWithBrowser(DeviceCodeResponse response) {
        Console.WriteLine("This is your Device Code");
        Console.WriteLine("------------------------");
        Console.WriteLine(response.UserCode);
        Console.WriteLine("------------------------");
        Console.WriteLine("Opening Browser... Please confirm your Code...");
        OpenBrowser(response.VerificationUriComplete);
    }

    public static async Task<AccessTokenResponse> RequestAccessTokenForDeviceCode(DeviceCodeResponse response) {
            var tokenRequest = new DeviceCodeTokenRequest
            {
                ClientId = clientId,
                DeviceCode = response.DeviceCode
            };
            return await authenticationApiClient.GetTokenAsync(tokenRequest);
    }

}