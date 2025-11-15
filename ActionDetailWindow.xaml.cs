using System;
using System.Collections;
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

namespace Facienda
{
    /// <summary>
    /// ActionDetailWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ActionDetailWindow : Window
    {
        private ActionItem _action;
        public MainWindow owner;
        public ActionDetailWindow(ActionItem action)
        {
            InitializeComponent();
            this._action = action;

            // ウィンドウ起動時にアクション名を入れておく
            InputBox.Text = _action.Name;

            // アクション名を全選択した状態でフォーカス
            this.Loaded += (s, e) =>
            {
                InputBox.Focus();
                InputBox.SelectAll();
            };
        }

        // Enterが押されたらアクションのリネームを実行する
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                var input = InputBox.Text;
                owner.RenameAction(_action, input);

                // モーダルを閉じたいならこれで戻る
                DialogResult = true;
                Close();
            }
        }
    }
}
