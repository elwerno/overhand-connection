using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TcpBrokerTest
{
    public class DataHub : Hub
    {
        private RecordingConfig recordingConfig;
        public DataHub(RecordingConfig recordingConfig)
        {
            this.recordingConfig = recordingConfig;
        }
        
        public async Task SendData(string trackerId, int[,] data)
        {
            await Clients.All.SendAsync("NewData", trackerId, data);
        }

        public void StartRecording()
        {
            Console.WriteLine("should start recording now, if not already started");
            recordingConfig.IsRecording = true;
            
            recordingConfig.CreateNewFile();
        }

        public void StopRecording()
        {
            // TODO: write file to disk
            recordingConfig.IsRecording = false;
            Console.WriteLine("stop recording");
        }
    }
}