using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通信助手
{
    class DataClass
    {
    }

    public class Com_Port : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //串口端口名
        private string[] _comports;
        public string[] comports
        {
            get
            {
                return _comports;
            }
            set
            {
                _comports = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("comports"));
                }
            }
        }

        //串口接收数据
        private string _r_text;
        public string r_text
        {
            get
            {
                return _r_text;
            }
            set
            {
                _r_text = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("r_text"));
                }
            }
        }

        //接收数据显示格式16进制或者默认
        private bool _r_text_showmod;
        public bool r_text_showmod
        {
            get
            {
                return _r_text_showmod;
            }
            set
            {
                _r_text_showmod = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("r_text_showmod"));
                }
            }
        }

        //发送数据格式16进制或者默认
        private bool _t_text_mod;
        public bool t_text_mod
        {
            get
            {
                return _t_text_mod;
            }
            set
            {
                _t_text_mod = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("t_text_mod"));
                }
            }
        }


    }

    public class Ethernet_Port : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //IP地址
        private string _IP;
        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IP"));
                }
            }
        }
    }


}
