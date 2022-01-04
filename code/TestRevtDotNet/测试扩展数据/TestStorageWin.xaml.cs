using RQ.RevitUtils.ExternalEventUtility;
using RQ.Test.RevtDotNet.测试扩展数据.Commands;
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

namespace RQ.Test.RevtDotNet.测试扩展数据
{
    /// <summary>
    /// TextStorageWin.xaml 的交互逻辑
    /// </summary>
    public partial class TextStorageWin : Window
    {
        internal TestStorageWinModel TestStorageWinModel { get; set; }

        public TextStorageWin()
        {
            InitializeComponent();
            this.Loaded += TextStorageWin_Loaded;
        }

        private void TextStorageWin_Loaded(object sender, RoutedEventArgs e)
        {
            this.TestStorageWinModel = new TestStorageWinModel();
            this.DataContext = TestStorageWinModel;
            ExternalEventHandler.CreateExternalEvent();
        }
        private void btn_SetStorage_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T1_SetStorageCommand(TestStorageWinModel);
            ExternalEventHandler.Event.Raise();
        }

        private void btn_GetStorage_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T1_GetStorageCommand(TestStorageWinModel);
            ExternalEventHandler.Event.Raise();
        }

        private void btn_StartUpdate_Click(object sender, RoutedEventArgs e)
        {
            T1StorageData.IsStarUpdata = true;
        }

        private void btn_CloseUpdate_Click(object sender, RoutedEventArgs e)
        {
            T1StorageData.IsStarUpdata = false;
        }

        private void btn_ClassUpdate_Click(object sender, RoutedEventArgs e)
        {
            ExternalEventHandler.IEventBase = new T2_ClassUpdateCommand(TestStorageWinModel);
            ExternalEventHandler.Event.Raise();
        }
    }
}
