
using Microsoft.Web.Services3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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

namespace EmailService
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //Soap soap = new Soap12();

        }
        //TcpEmailService client = new TcpEmailService("127.0.0.1", 2222);
        //IEmailService service = new SoapEmailService();


        public ObservableCollection<string> EmailAddrs { get; set; } = new ObservableCollection<string>()
        {
            "1312336106@qq.com"
        };
        //public string ServiceType { get; set; }

        public string Payload { get; set; } = "Hello!";

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = NewEmailService();
                char res = service.SendEmailBatch(EmailAddrs.ToArray(), Payload);
                MessageBox.Show(res == 'Y' ? "发送成功。" : "发送失败。");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddAddr_Click(object sender, RoutedEventArgs e)
        {
            string addr = emailToAdd.Text;

            if (addr == "")
            {
                return;
            }
            var task = new Task(() =>
            {

                var service = NewEmailService();
                char valid = service.ValidateEmailAddress(addr);
                if (valid == 'Y')
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        EmailAddrs.Add(addr);
                        emailToAdd.Text = "";

                    }));

                }
                else
                {
                    MessageBox.Show("Invalid email address!");
                }
            });
            task.Start();

        }

        private IEmailService NewEmailService()
        {
            IEmailService service = null;
            var list = new RadioButton[] { rdBtnSoap, rdBtnRest };
            if ((bool)rdBtnSoap.IsChecked)
            {
                service = new SoapEmailService();
            }else if ((bool)rdBtnRest.IsChecked)
            {
                service = new RestEmailService();
            }

            return service;
        }
    }
}
