using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace AVMImagCreate
{
    public partial class Form1 : Form

        
    {
        bool isConnected = false;
        String[] ports;
        SerialPort port;

        private string buffer { get; set; }

        public Form1()
        {
            InitializeComponent();
            disableControls();

            ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comboBox1.SelectedItem = ports[0];
                }
            }
        }

        private void connectToArduino()
        {
            isConnected = true;
            string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            port = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
            SerialPortProgram();
            port.Open();
            port.Write("#STAR\n");
            
            button1.Text = "Disconnect";
            enableControls();
        }

        private void SerialPortProgram()
        {
            Console.WriteLine("Incoming Data:");
            // Attach a method to be called when there
            // is data waiting in the port's buffer 
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            // Begin communications 
            //port.Open();
            // Enter an application loop to keep this thread alive 
            //Application.Run();
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Show all the incoming data in the port's buffer
            //Console.WriteLine(port.ReadExisting());
            buffer += port.ReadExisting();
            
            //test for termination character in buffer
            if (buffer.Contains("\n"))
            {
                //run code on data received from serial port
                MessageBox.Show(buffer);
                buffer = "";
            }
        }

        private void enableControls()
        {
            button2.Enabled = true;
            textBox1.Enabled = true;
            
        }

        private void disableControls()
        {
            button2.Enabled = false;
            textBox1.Enabled = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if ( isConnected )
            {
                port.Write("#TEXT" + textBox1.Text + "#\n");
            }
            
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                connectToArduino();
            }
            else
            {
                disconnectFromArduino();
            }

        }

        private void disconnectFromArduino()
        {
            isConnected = false;
            port.Write("#STOP\n");
            port.Close();
            button1.Text = "Connect";
            disableControls();
            resetDefaults();
        }

        private void resetDefaults()
        {
            textBox1.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                port.Write("#LED1" + textBox2.Text + "#\n");
            }
        }
    }
}
