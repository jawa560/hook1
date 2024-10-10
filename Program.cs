using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles(); // �ҥ��R�A�ɮתA��

app.MapPost("/webhook", async (HttpContext context) =>
{
    var request = context.Request;
    var response = context.Response;

    // Ū���ШD���e
    var requestBody = await new StreamReader(request.Body).ReadToEndAsync();

    // �o�̥i�H�B�z�ШD���e
    Console.WriteLine("Received webhook: " + requestBody);

    // �^��200 OK
    response.StatusCode = StatusCodes.Status200OK;
    await response.WriteAsync("Webhook received successfully");
});

// �w�ɶǰe�ɶ��\��
app.MapGet("/time", async (HttpContext context) =>
{
    var response = context.Response;
    response.Headers.Add("Content-Type", "text/event-stream");
    response.Headers.Add("Cache-Control", "no-cache");
    response.Headers.Add("Connection", "keep-alive");

    while (true)
    {
        var currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        await response.WriteAsync($"data: {currentTime}\n\n");
        await response.Body.FlushAsync();
        await Task.Delay(1000); // �C1��o�e�@��
    }
});

app.Run();
