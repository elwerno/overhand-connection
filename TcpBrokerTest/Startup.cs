using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.AspNetCore;
using MQTTnet.Server;

namespace TcpBrokerTest
{
    public class Startup
    {
        // In class _Startup_ of the ASP.NET Core 2.0 project.
        public void ConfigureServices(IServiceCollection services)
        {
            //this adds a hosted mqtt server to the services
            var ipAddress = new byte[] {0, 0, 0, 0};
            var ipObject = new System.Net.IPAddress(ipAddress);
            services.AddHostedMqttServer(builder => builder.WithDefaultEndpointBoundIPAddress(ipObject).WithDefaultEndpointPort(1883));

            //this adds tcp server support based on System.Net.Socket
            services.AddMqttTcpServerAdapter();

            //this adds websocket support
            services.AddMqttWebSocketServerAdapter();

            services.AddMvc();
            services.AddSignalR();

            services.AddSingleton<DataHub>();
            services.AddSingleton<RecordingConfig>();
            // services.AddTransient<IHostingEnvironment>(prov => new HostingEnvironment());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // this maps the websocket to an mqtt endpoint
            app.UseMqttEndpoint();
            // other stuff
            app.UseStaticFiles();

            // TODO: log debug infos

            app.UseSignalR(routes => { routes.MapHub<DataHub>("/dataHub"); });

            var server = app.ApplicationServices.GetRequiredService<IMqttServer>();
            var dataHub = app.ApplicationServices.GetRequiredService<DataHub>();
            var recordingConfig = app.ApplicationServices.GetRequiredService<RecordingConfig>();


            server.ApplicationMessageReceived += (s, e) =>
            {
                int[,] parsedData = DataParser.ParseByte(e.ApplicationMessage.Payload);

                dataHub.SendData(e.ClientId, parsedData);

                if (recordingConfig.IsRecording)
                {
                    string toWrite = "";
                    for (int i = 0; i < 10; i++)
                    {
                        // TODO: maybe use StringBuilder later
                        toWrite += e.ClientId + ",";
                        for (int j = 0; j < 9; j++)
                        {
                            toWrite += parsedData[i, j] + ",";
                        }

                        if (i < 9)
                        {
                            toWrite += "\n";
                        }
                    }

                    recordingConfig.StreamWriter.WriteLine(toWrite);
                }
            };
        }
    }
}