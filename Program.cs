using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles(); // 啟用靜態檔案服務

app.MapPost("/webhook", async (HttpContext context) =>
{
    var request = context.Request;
    var response = context.Response;

    // 讀取請求內容
    var requestBody = await new StreamReader(request.Body).ReadToEndAsync();

    // 這裡可以處理請求內容
    Console.WriteLine("Received webhook: " + requestBody);

    // 回應200 OK
    response.StatusCode = StatusCodes.Status200OK;
    await response.WriteAsync("Webhook received successfully");
});

// 定時傳送時間功能
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
        await Task.Delay(1000); // 每1秒發送一次
    }
});

app.Run();
