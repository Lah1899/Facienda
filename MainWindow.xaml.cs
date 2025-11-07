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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace Facienda
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private Root _root; // 全データを格納するオブジェクト
        public MainWindow()
        {
            InitializeComponent();
            LoadJson();
        }

        // Jsonからのデータ読み込み
        private void LoadJson()
        {
            var json = File.ReadAllText("facienda.json");
            _root = JsonConvert.DeserializeObject<Root>(json);
        }
    }
}
