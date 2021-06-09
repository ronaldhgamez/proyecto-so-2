using System;
using System.Text;
using System.Threading;
// Timer
using System.Net.NetworkInformation;
using System.Timers;
// Sockets
using System.Net;
using System.Net.Sockets;
// To check total of nodes connected to the server
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace console_proyecto_so_2
{
    class Server
    {
        /* Host Configuration */
        private IPHostEntry host;
        private IPAddress iPAddress;
        private int port;
        private IPEndPoint endPoint;
        private Socket socket_server;

        /* Clients Nodes */
        private Socket s_client_1;
        private Socket s_client_2;
        private Socket s_client_3;

        /* Timer to execute function every minute */
        private System.Timers.Timer aTimer;

        private string response;

        public Server(string ip, int port)
        {
            this.host = Dns.GetHostEntry(ip);
            this.iPAddress = host.AddressList[0];
            this.port = port;
            this.endPoint = new IPEndPoint(iPAddress, port);

            this.socket_server = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.socket_server.Bind(endPoint);
            this.socket_server.Listen(1);

            this.s_client_1 = null;
            this.s_client_2 = null;
            this.s_client_3 = null;

            this.response = "[]";
        }

        public void Start()
        {
            InicitializeTimer();
            DisplayConnectedMessage();
            while (true)
            {

                /* Waiting for client connections */
                if (s_client_1 == null)
                {
                    s_client_1 = socket_server.Accept();
                    Console.WriteLine(">>> Client 1 accepted!");
                }
                else if (s_client_2 == null)
                {
                    s_client_2 = socket_server.Accept();
                    Console.WriteLine(">>> Client 2 accepted!");
                }
                else if (s_client_3 == null)
                {
                    s_client_3 = socket_server.Accept();
                    Console.WriteLine(">>> Client 3 accepted!");
                }
            }

        }
        public void InicitializeTimer()
        {
            int minutes = 6;
            Console.WriteLine($"Data will be updated every {minutes} minutes!");
            // Create a timer with a six second interval.
            aTimer = new System.Timers.Timer(minutes * 1000);
            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += UpdateDataNews;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void UpdateDataNews(Object source, ElapsedEventArgs e)
        {
            response = "[]";
            CloseConections(); /* Make null the nodes that have lost the connection to the server */

            if (s_client_1 == null && s_client_2 == null && s_client_3 == null)
            {
                Console.WriteLine("Waiting for nodes to start scraping!");
                return;
            }
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var httpConnections = (from connection in properties.GetActiveTcpConnections()
                                   where connection.LocalEndPoint.Port == port
                                   select connection);

            int total = httpConnections.Count();

            Console.WriteLine("Clients connected: {0}", total);
            switch (total)
            {
                case 1:
                    Socket client = SearchNodeClients(1)[0];
                    Send(client, Program.bbc_1 + "$" + Program.bbc_2 + "$" + Program.bbc_3 + "$" + Program.ElMundo);
                    GetClientResponse(client); /* Wait for response */
                    return;

                case 2:
                    List<Socket> clients = SearchNodeClients(2);
                    /* Distribute two links to first node and second node */
                    Send((clients[0]), Program.bbc_1 + "$" + Program.bbc_2);
                    StartClient((clients[0]));
                    Send((clients[1]), Program.bbc_3 + "$" + Program.ElMundo);
                    StartClient((clients[1]));
                    return;

                case 3:
                    List<Socket> clients3 = SearchNodeClients(3);
                    /* Distribute two links to first node and one to the second and third node */
                    Send((clients3[0]), Program.bbc_1 + "$" + Program.bbc_2);
                    StartClient((clients3[0]));
                    Send((clients3[1]), Program.bbc_3);
                    StartClient((clients3[1]));
                    Send((clients3[2]), Program.ElMundo);
                    StartClient((clients3[2]));
                    return;

                default:
                    break;
            }
        }

        private void StartClient(Socket s_client)
        {
            Thread t;
            /* Starts cliente execution */
            t = new Thread(GetClientResponse);
            t.Start(s_client);
            Console.WriteLine("Waiting for cliente response...\n");
        }

        public void GetClientResponse(Object s)
        {
            Socket s_client = (Socket)s;
            byte[] buffer;
            try
            {
                /* Waits and save client response */
                buffer = new byte[1024];
                s_client.Receive(buffer);

                /* conver buffer into string */
                string response = Encoding.ASCII.GetString(buffer);
                //SerializeResponses(response);
                Console.WriteLine(response);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Node disconected: {0}", e.Message);
            }

        }

        public void Send(Socket s_client, string msg)
        {
            byte[] msg_byte = Encoding.ASCII.GetBytes(msg);
            try
            {
                s_client.Send(msg_byte);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Node disconected: {0}", e.Message);
            }
        }

        private void DisplayConnectedMessage()
        {
            Console.WriteLine("\n*****************************************");
            Console.WriteLine("*\t Server started! \t\t*");
            Console.WriteLine("*****************************************\n");
        }


        private List<Socket> SearchNodeClients(int total)
        {
            List<Socket> list = new List<Socket>();
            if (s_client_1 != null)
                list.Add(s_client_1);

            if (list.Count == total)
                return list;

            if (s_client_2 != null)
                list.Add(s_client_2);

            if (list.Count == total)
                return list;

            if (s_client_3 != null)
                list.Add(s_client_3);

            if (list.Count == total)
                return list;
            return list;
        }

        private void CloseConections()
        {
            if (s_client_1 != null && !IsSocketConnected(s_client_1))
                s_client_1 = null;

            if (s_client_2 != null && !IsSocketConnected(s_client_2))
                s_client_2 = null;

            if (s_client_3 != null && !IsSocketConnected(s_client_3))
                s_client_3 = null;
        }

        private bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        private void SerializeResponses(string json_string)
        {
            object sync = new object();
            lock (sync)
            {
                //List<Delete> ObjOrderList = (List<Delete>)JsonConvert.DeserializeObject(response);
                //List<Delete> ObjOrderList2 = (List<Delete>)JsonConvert.DeserializeObject(json_string);

                //ObjOrderList.AddRange(ObjOrderList2);

                //this.response = JsonConvert.SerializeObject(ObjOrderList);
            }
            
            Console.WriteLine("response: \n{0}\n", response);
        }
    }
}
