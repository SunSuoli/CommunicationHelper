using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 通信助手
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private SerialPort ComDevice = new SerialPort();
        private Socket TCPDivce;
        private Socket UDPDivce;

        private Ports p = new Ports();
        public MainWindow()
        {
            InitializeComponent();

            Init_Com();
        }

        /// <summary>
        /// 串口调试功能的实现
        /// </summary>
        
        //更新串口列表
        private void ComPort_refresh(object sender, EventArgs e)
        {
            p.comports = SerialPort.GetPortNames();//将串口的硬件资源名更新到下拉列表中
        }
        //打开关闭串口
        private void Com_Open_Close_Click(object sender, RoutedEventArgs e)
        {
            if (ComDevice.IsOpen == false)
            {
                try
                {
                    Com_Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误");
                    return;
                }
                Com_Open_Close.Content = "关闭";
                //p.r_text = ComDevice.PortName.ToString() +"-"+ ComDevice.BaudRate.ToString() + "-" + ComDevice.Parity.ToString() + "-"+ ComDevice.DataBits.ToString()+ "-" + ComDevice.StopBits.ToString();
            }
            else
            {
                try
                {
                    ComDevice.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误");
                    return;
                }
                Com_Open_Close.Content = "打开";
                //p.r_text = "PORT CLOSED ";
            }
        }
        //接收串口数据事件
        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ///直接读取字符串
            //string data = string.Empty;
            //while (ComDevice.BytesToRead > 0)
            //{
            //    data += ComDevice.ReadExisting();  //数据读取,直到读完缓冲区数据
            //}

            byte[] ReDatas = new byte[ComDevice.BytesToRead];
            
            ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
            this.adddata(ReDatas);
        }
        //打开串口
        private void Com_Open()
        {
            ComDevice.PortName = Com_Port.Text;
            ComDevice.BaudRate = Convert.ToInt32(Com_BaudRate.SelectedItem.ToString());
            ComDevice.Parity = (Parity)Convert.ToInt32(Com_ParityBit.SelectedIndex.ToString());
            ComDevice.DataBits = Convert.ToInt32(Com_DataBit.SelectedItem.ToString());
            ComDevice.StopBits = (StopBits)Convert.ToInt32(Com_StopBit.SelectedItem.ToString());
            ComDevice.Open();
        }
        //初始化串口UI配置
        private void Init_Com()
        {
            ComDevice.DataReceived += Com_DataReceived;//注册串口接收事件
            ComDevice.ReceivedBytesThreshold = 1;//接收1个字节就触发事件

            //绑定接收数据
            Binding rtext_binding = new Binding();
            rtext_binding.Source = p;
            rtext_binding.Path = new PropertyPath("r_text");
            this.R_text.SetBinding(TextBox.TextProperty, rtext_binding);
            //绑定接收数据显示模式
            Binding rtext_mod_binding = new Binding();
            rtext_mod_binding.Source = p;
            rtext_mod_binding.Path = new PropertyPath("r_text_showmod");
            this.r_mod.SetBinding(CheckBox.IsCheckedProperty, rtext_mod_binding);
            //绑定发送数据模式
            Binding t_mod_binding = new Binding();
            t_mod_binding.Source = p;
            t_mod_binding.Path = new PropertyPath("t_text_mod");
            this.t_mod.SetBinding(CheckBox.IsCheckedProperty, t_mod_binding);
            //绑定串口名称
            Binding port_binding = new Binding();
            port_binding.Source = p;
            port_binding.Path = new PropertyPath("comports");
            this.Com_Port.SetBinding(ComboBox.ItemsSourceProperty, port_binding);
            

            string[] ArryPort = SerialPort.GetPortNames();
            p.comports = ArryPort;
            Com_Port.SelectedIndex = 0;

            Com_BaudRate.Items.Add("9600");
            Com_BaudRate.Items.Add("19200");
            Com_BaudRate.Items.Add("38400");
            Com_BaudRate.Items.Add("57600");
            Com_BaudRate.Items.Add("115200");
            Com_BaudRate.SelectedIndex = 0;


            Com_ParityBit.Items.Add("None");
            Com_ParityBit.Items.Add("奇");
            Com_ParityBit.Items.Add("偶");
            Com_ParityBit.Items.Add("Mark");
            Com_ParityBit.Items.Add("Space");
            Com_ParityBit.SelectedIndex = 0;

            Com_DataBit.Items.Add("5");
            Com_DataBit.Items.Add("6");
            Com_DataBit.Items.Add("7");
            Com_DataBit.Items.Add("8");
            Com_DataBit.SelectedIndex = 3;

            Com_StopBit.Items.Add("1");
            Com_StopBit.Items.Add("1.5");
            Com_StopBit.Items.Add("2");
            Com_StopBit.SelectedIndex = 0;

        }
        //清空串口接收数据
        private void R_text_clear_Click(object sender, RoutedEventArgs e)
        {
            p.r_text = "";
        }
        //串口发送数据
        private void T_text_send_Click(object sender, RoutedEventArgs e)
        {
            byte[] T_bytes;
            if (p.t_text_mod == true)
            {
                T_bytes = HexStringToString(T_text.Text);//将数据由字符串转成十六进制字节
            }
            else
            {
                T_bytes = Encoding.Default.GetBytes(T_text.Text);//将数据由字符串转成字节
            }
            
            try
            {
                ComDevice.Write(T_bytes, 0, T_bytes.Length) ;//以字节的形式发送数据

            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "错误");
                return;
            }
        }

        private  void adddata(byte[] ReDatas)
        {
            string data = string.Empty;
            if (p.r_text_showmod==true)
            {
                foreach (int ReData in ReDatas)
                {
                    data += Convert.ToString(ReData, 16)+" ";//将接收的字节转换成十六进制字符串
                }
            }
            else
            {
                data = Encoding.Default.GetString(ReDatas);//将接收的字节转换成ASCII字符串，防止中文乱码

            }
            p.r_text += data;
        }
        //将十六进制的字符串转为普通字符串
        //如字符串"EE FF EF FE FA"转成字符串"?稔?"
        private byte[] HexStringToString(string hs)
        {
           
            String hs_nospace = hs.Replace(" ", "");//清理原始字符串中的空格
            if (hs_nospace.Length%2 != 0)
            {
                hs_nospace += "0";//如果有效字符不够，则在后面补“0”
            }
            byte[] bytes=new byte[hs_nospace.Length/2];//创建一个字节数组
            for (int i = 0; i < hs_nospace.Length / 2; i++)
            {
                try
                {
                    //将有效字符的每两个字节组合成一个十六进制字符串，然后转换成数值
                    bytes[i] = Convert.ToByte(hs_nospace.Substring(i * 2, 2), 16);
                }
                catch
                {
                    
                }
                
            }
            return bytes;
        }

        /// <summary>
        /// 网口调试功能的实现
        /// </summary>
        



    }
}
