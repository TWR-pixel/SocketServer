using System.Net;
using System.Net.Sockets;
using System.Text;

var connections = new Dictionary<int, Socket>();
var ipPoint = new IPEndPoint(IPAddress.Parse("5.63.157.232"), 3003);
var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

listener.Bind(ipPoint);
listener.Listen(1000);

Console.WriteLine("Start...");

while (true)
{
    

    var acceptThread = new Thread( () =>
    {
        var clientSocket = listener.Accept();

        var connectionThread = new Thread(() => handleClient(clientSocket));
        
        connectionThread.Start();

    });
    acceptThread.Start();

    // acceptAsync new Thread
    var res = Console.ReadLine();
    Console.WriteLine(res);
}

void handleClient(Socket socket)
{
    connections.Add(connections.Count + 1, socket);
    var builder = new StringBuilder();
    int bytes = 0; // количество полученных байтов
    var data = new byte[256]; // буфер для получаемых данных

    do
    {
        bytes = socket.Receive(data);
        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
    }
    while (socket.Available > 0);

    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
}
