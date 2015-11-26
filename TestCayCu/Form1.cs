using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TestCayCu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public TcpListener tcpListener;
        public TcpListener tcpListener2;
        public string fileName;
        public string filePath;
        public IPAddress LocalAddress;
        public const int PORT_RECEIVE = 1025;
        public const int PORT_SEND    = 1026;
        public List<string> list_Files;
        //public string[] files = new string[10];
        public const int BUFFER = 1024;
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
            
        public void listenClient()
        {
            LocalAddress = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(LocalAddress,PORT_RECEIVE);
            tcpListener.Start();
             

            while(true)
            {
                Socket socket = tcpListener.AcceptSocket();
                NetworkStream netStream = new NetworkStream(socket);
                StreamReader sReader = new StreamReader(netStream);
                fileName = sReader.ReadLine();
                if (list_Files.Contains(fileName)&&fileName=="Quit") break;
                list_Files.Add(fileName);
				string Path="E:/" + fileName;              
                FileStream InputFile = new FileStream(Path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                BinaryWriter BinaryStream = new BinaryWriter(InputFile); 
                byte[] data = new byte[BUFFER];
                do
                {
                    socket.Receive(data);
                    BinaryStream.Write(data);

                } while (data.Length != 0);
				BinaryStream.Flush();
                BinaryStream.Close();
                InputFile.Close();

            }			
        }
		public void SendFile(string fileName)
        {
            tcpListener2 = new TcpListener(LocalAddress,PORT_SEND);
            tcpListener2.Start();
			
			while(true)
            {
                Socket socket  = tcpListener2.AcceptSocket();


                NetworkStream netStream = new NetworkStream(socket);
                StreamWriter sWriter = new StreamWriter(netStream);
				
				string Path="E:/" + fileName; 
                FileInfo File = new FileInfo(Path);
                if (list_Files.Contains(fileName)&&fileName=="Quit") break;
                list_Files.Add(fileName);
                filePath= "E:/" + fileName;
                FileStream InputFile = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                BinaryWriter BinaryStream = new BinaryWriter(InputFile); 
                byte[] data = new byte[BUFFER];
                do
                {
                    socket.Receive(data);
                    BinaryStream.Write(data);

                } while (data.Length != 0);
				BinaryStream.Flush();
                BinaryStream.Close();
                InputFile.Close();

            }			
        }   
       
    }
}
