﻿using System;
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

namespace window_disaster_noti
{
    /// <summary>
    /// setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class setting : Window
    {
        public setting()
        {
            InitializeComponent();

            this.Left = SystemParameters.WorkArea.Width - this.Width - 100;
            this.Top = SystemParameters.WorkArea.Height - this.Height - 100;
        }
    }
}
