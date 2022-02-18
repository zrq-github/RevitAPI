using RQ.RevitUtils.ExternalEventUtility;
using RQ.Test.RevtDotNet.测试扩展数据.Commands;
using RQ.Test.RevtDotNet.测试扩展数据;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RQ.Test.RevtDotNet.测试部件Transfrom.Commands
{
    /// <summary>
    /// T0_ShowWin.xaml 的交互逻辑
    /// </summary>
    public partial class TestAssemblyTransfromPlane : Window
    {
        TestAssemblyTransfromPlaneModel TransfromModel { get; set; } = new TestAssemblyTransfromPlaneModel();

        public TestAssemblyTransfromPlane()
        {
            InitializeComponent();
            this.DataContext = TransfromModel;
            this.Loaded += TestAssemblyTransfromPlane_Loaded;
        }

        private void TestAssemblyTransfromPlane_Loaded(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.CreateExternalEvent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T1_逆时针旋转45(TransfromModel);
            ExternalEventHandler.Event.Raise();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T1_逆时针旋转45_唯一部件_旋转一次_一次事务(TransfromModel);
            ExternalEventHandler.Event.Raise();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T1_逆时针旋转45_唯一部件_旋转二次_二次事务(TransfromModel);
            ExternalEventHandler.Event.Raise();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T1_逆旋45_双实例_单次事务(TransfromModel);
            ExternalEventHandler.Event.Raise();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T2_旋转部件(TransfromModel);
            ExternalEventHandler.Event.Raise();
        }
    }
}
