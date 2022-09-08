using Autodesk.Revit.DB;
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

namespace ZRQ.RevitTest.RevitAsync
{
    /// <summary>
    /// Interaction logic for MyWindow.xaml
    /// </summary>
    public partial class MyWindow : Window
    {
        public MyWindow()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //var button = new Button
            //{
            //    Content = "Button",
            //    Command = new ButtonCommand(),
            //    VerticalAlignment = VerticalAlignment.Center,
            //    HorizontalAlignment = HorizontalAlignment.Center
            //};
            //Content = button;
        }

        private async void btn_task_rotate_Click(object sender, RoutedEventArgs e)
        {
            Task task_Rotation = TestRevitTask.DoTastRotation();
            await task_Rotation;
        }

        private async void btn_task_rotate_Copy_Click(object sender, RoutedEventArgs e)
        {
            Task task_Move = TestRevitTask.DoTaskMove();
            await task_Move;
        }

        private async void btnTask_MoveRotate(object sender, RoutedEventArgs e)
        {
            Task task_Move = TestRevitTask.DoTaskMove();
            await task_Move;
            Task task_Rotation = TestRevitTask.DoTastRotation();
            await task_Rotation;
        }

        private async void btnTask_RotateMove(object sender, RoutedEventArgs e)
        {
            Task task_Rotation = TestRevitTask.DoTastRotation();
            await task_Rotation;
            Task task_Move = TestRevitTask.DoTaskMove();
            await task_Move;
        }

        private void btnTask_thread(object sender, RoutedEventArgs e)
        {
            Task threadTask = new Task(async () =>
            {
                Task task_Rotation = TestRevitTask.DoTastRotation();
                await task_Rotation;
                MessageBox.Show("我旋转了");
                Task task_Move = TestRevitTask.DoTaskMove();
                await task_Rotation;
                MessageBox.Show("我移动了");
            });
            threadTask.Start();

            MessageBox.Show("函数调用完成");
        }
    }
}
