using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TcpBrokerTest
{
    public class RecordingConfig
    {
        private IHostingEnvironment hostingEnvironment;
        
        public RecordingConfig(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        
        public bool IsRecording { get; set; } = false;

        public Stream Stream { get; set; }
        public StreamWriter StreamWriter { get; set; }

        public void CreateNewFile()
        {
            var date = DateTime.Now;
            var formattedDate = date.Year + "-" + date.Month + "-" + date.Day + "-" + date.Hour + "-" + date.Minute + "-" + date.Second;
            var filePath = hostingEnvironment.WebRootPath + "/recordings/recording-" + formattedDate + ".txt";

            Console.WriteLine(filePath);
            Stream = File.Create(filePath);
            StreamWriter = new StreamWriter(Stream);
        }
    }
}