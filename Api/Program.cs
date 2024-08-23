using System;
using System.Net;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

class Program
{
    static void Main(string[] args)
    {

        var builder = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        var configuration = builder.Build();
        var dbName = configuration.GetValue<string?>("dbname", null);
        var port = configuration.GetValue<int>("port", 5000);
        var ipAddress = configuration.GetValue<string?>("ip", null);
        if (string.IsNullOrEmpty(dbName))
        {
            Console.WriteLine("dbname is missing");
            var dbv = configuration.GetDebugView();
            Console.WriteLine(dbv);
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("dbname is {0}", dbName);

        }
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        if (!string.IsNullOrEmpty(ipAddress))
            listener.Prefixes.Add($"http://{ipAddress}:{port}/");

        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName())
            .Where(ip => !IPAddress.IsLoopback(ip) && ip.AddressFamily == AddressFamily.InterNetwork)
            .ToArray();

        foreach (IPAddress ip in localIPs)
        {
            var ip4 = ip.MapToIPv4();

            listener.Prefixes.Add($"http://{ip4}:{port}/");
            //            Console.WriteLine($"IP Address: {ip4}");
        }

        var dbr = new DatabaseReader(dbName);
        // listener.Prefixes.Add("http://192.168.2.52:5000/");
        listener.Start();

        Console.WriteLine($"Listening on {string.Join(',', listener.Prefixes)}...");

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            if (request == null || request.Url == null) continue;

            response.AppendHeader("Access-Control-Allow-Origin", "*");
            if (request.HttpMethod == "OPTIONS")
            {
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                response.AddHeader("Access-Control-Max-Age", "1728000");
            }
            else
            {
                // Parse query parameters

                var query = System.Web.HttpUtility.ParseQueryString(request.Url.Query);
                int page = int.Parse(query.Get("page") ?? "1");
                int pageSize = int.Parse(query.Get("pageSize") ?? "10");

                // Generate paged list of items
                //            var items = Enumerable.Range(1, 100); // Example: list of numbers from 1 to 100
                // var items = Enumerable.Range(1, 100); // Example: list of numbers from 1 to 100
                // var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize);
                var pagedItems = dbr.GetMeasurements(page - 1, pageSize);// items.Skip((page - 1) * pageSize).Take(pageSize);

                string responseString = string.Join(", ", pagedItems);
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }


        }
    }
}
