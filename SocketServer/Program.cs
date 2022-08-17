using System.Net;
using System.Net.Sockets;
using System.Text;

var connections = new Dictionary<int, Socket>();
var ipPoint = new IPEndPoint(IPAddress.Parse("5.63.157.232"), 3003);
var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

listener.Bind(ipPoint);
listener.Listen(1000);

Console.WriteLine("Start...");

var acceptThread = new Thread(() =>
{ // while true
    while (true)
    {
        var clientSocket = listener.Accept();

        var connectionThread = new Thread(() => handleClient(clientSocket));

        connectionThread.Start();
    }
});

acceptThread.Start();

while (true)
{
    // acceptAsync new Thread
    var res = Console.ReadLine();

    if (res == "connected-machines")
        foreach(var conn in connections)
            Console.WriteLine($"{conn.Key}|{conn.Value}"); // Id|Socket

    if (res == "send-msg")
    {
        Console.Write("Enter id: ");

        var input = Console.ReadLine();
        var client = default(Socket);

        connections.TryGetValue(Convert.ToInt32(input), out client);
        Console.Write("Enter message: ");

        var message = Console.ReadLine();
        var data = new byte[256];

        data = Encoding.Unicode.GetBytes(message);

        client.Send(data);
    }
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
