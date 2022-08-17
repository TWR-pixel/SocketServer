using System.Net;
using System.Net.Sockets;
using System.Text;

var ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3003);

var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
// подключаемся к удаленному хосту
socket.Connect(ipPoint);
Console.Write("Введите сообщение:");
var message = Console.ReadLine();
var data = Encoding.Unicode.GetBytes(message);
socket.Send(data);