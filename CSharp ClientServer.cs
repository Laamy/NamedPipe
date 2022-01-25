using PMClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PMServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            setupServer();
            Task.Delay(1000).Wait();

            NamedPipeClientStream pipe = new NamedPipeClientStream("PMServer_k2d3Cl4");
            pipe.Connect();

            StreamReader sr = new StreamReader(pipe); 
            StreamWriter sw = new StreamWriter(pipe);

            while (true)
            {
                sw.WriteLine(Console.ReadLine());
                sw.Flush();

                var json = sr.ReadLine();
                print("Client", "Received: " + json);
            }
        }

        public static void setupServer()
        {
            Task.Factory.StartNew(() => {
                NamedPipeServerStream pipe = new NamedPipeServerStream("PMServer_k2d3Cl4");

                print("Server", "Waiting for connection...");
                pipe.WaitForConnection();
                print("Server", "Connection detected");

                StreamReader sr = new StreamReader(pipe);
                StreamWriter sw = new StreamWriter(pipe);

                while (true)
                {
                    var command = sr.ReadLine();

                    print("Server", "Received: " + command);

                    sw.WriteLine("Accepted Response");
                    sw.Flush();
                }
            });
        }

        //private static Packet ParsePacket(string json) => new JavaScriptSerializer().Deserialize<Packet>(json);
        //private static string ToJson(Packet packet) => new JavaScriptSerializer().Serialize(packet);

        private static void print(string name, string v) => Console.WriteLine($"[{name}]: " + v);
    }
}
