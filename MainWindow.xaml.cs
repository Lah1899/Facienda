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
            // _root = LoadJson();
            _root = new Root {
                Tasks = new List<TaskItem>
                {
                    new TaskItem { Id = "1", Name = "ダミー1", DueDate = "2025-01-01", Note = "テスト" },
                    new TaskItem { Id = "2", Name = "ダミー2", DueDate = "2025-01-01", Note = "テスト" }
                },
                Actions = new List<ActionItem>
                {
                    new ActionItem { Id = "a1", Name = "ダミー1のアクション1", IsDone = false, TaskId = "1" },
                    new ActionItem { Id = "a2", Name = "ダミー1のアクション2", IsDone = true,  TaskId = "1" },
                    new ActionItem { Id = "a3", Name = "ダミー2のアクション1", IsDone = false, TaskId = "2" }
                },
            };
            _root.Tasks[0].Actions.Add(_root.Actions[0]); // テスト用のダミー処理
            _root.Tasks[0].Actions.Add(_root.Actions[1]); // テスト用のダミー処理
            _root.Tasks[1].Actions.Add(_root.Actions[2]); // テスト用のダミー処理
            DataContext = _root;
            SidebarTabs.SelectedIndex = 0;
            ContentTabs.SelectedIndex = 0;
            // MessageBox.Show(this, _root.Tasks[1].Name);
        }

        // Jsonからのデータ読み込み
        private void LoadJson()
        {
            var json = File.ReadAllText("facienda.json");
            _root = JsonConvert.DeserializeObject<Root>(json);
            foreach (ActionItem action in _root.Actions)
            {
                foreach (TaskItem task in _root.Tasks)
                {
                    if(task.Id == action.TaskId)
                    {
                        task.Actions.Add(action);
                    }
                }
            }
        }

        // Jsonへのデータ保存
        private void SaveJson()
        {

        }

        // アクションカードの完了ステータス反転
        private void ToggleActionStatus()
        {

        }

        // タスクのリネーム
        private void RenameTask()
        {

        }

        // アクションのリネーム
        private void RenameAction()
        {

        }

        // 完了アクションのクリア
        private void ClearActions()
        {

        }

        // タスクの削除
        private void DeleteTask()
        {

        }

        // アクションの削除
        private void DeleteAction()
        {

        }

        /*
        // タスクタブの追加
        private System.Windows.Controls.StackPanel AddTaskTab(TaskItem task)
        {
            // 左メニューにタスクを追加
            // TaskItemオブジェクトを画面項目のDataContextとしてセット
            var item = new System.Windows.Controls.ListBoxItem { Content = task.Name };
            item.DataContext = task;
            SidebarTabs.Items.Add(item);

            // テンプレートからタブ内容を生成
            var template = (System.Windows.DataTemplate)this.FindResource("TaskTabContentTemplate");
            var content = (System.Windows.FrameworkElement)template.LoadContent();
            var scroll = (System.Windows.Controls.ScrollViewer)content;
            var actionsHost = (System.Windows.Controls.StackPanel)scroll.Content;
            var tab = new System.Windows.Controls.TabItem { Content = content };
            ContentTabs.Items.Add(tab);

            return actionsHost;
        }

        // アクションカードの追加
        private void AddActionCard(System.Windows.Controls.StackPanel panel, ActionItem action)
        {
            var template = (System.Windows.DataTemplate)this.FindResource("ActionCardTemplate");
            var content = (System.Windows.FrameworkElement)template.LoadContent();
            content.DataContext = action;
            panel.Children.Add(content);
        }
        */
    }
}
