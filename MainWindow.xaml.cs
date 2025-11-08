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
        private void ToggleActionStatus(ActionItem action)
        {
            action.IsDone = !action.IsDone;
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
        private void DeleteTask(TaskItem task)
        {
            // 配下のアクションを削除
            var target = task.Actions.ToList();
            foreach(ActionItem action in target)
            {
                DeleteAction(action);
            }
            // Rootのリストから削除
            _root.Tasks.Remove(task);
        }

        // アクションの削除
        private void DeleteAction(ActionItem action)
        {
            // Rootのリストから削除
            _root.Actions.Remove(action);
            // 親タスクのリストから削除
            var parentTask = _root.Tasks.FirstOrDefault(t => t.Id == action.TaskId);
            parentTask.Actions.Remove(action);
        }

        // タスクの新規作成
        private void CreateTask(string name)
        {
            TaskItem newtask = new TaskItem();
            newtask.Name = name;
            newtask.Id = Guid.NewGuid().ToString();
            _root.Tasks.Add(newtask);
        }

        // アクションの新規作成
        private void CreateAction(TaskItem task, string name)
        {
            ActionItem newaction = new ActionItem();
            newaction.Name = name;
            newaction.Id = Guid.NewGuid().ToString();
            _root.Actions.Add(newaction);
            task.Actions.Add(newaction);
        }

        // 以降はイベント処理
        private void TaskDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var ctxMenu = menuItem.Parent as ContextMenu;
            var listBoxItem = ctxMenu.PlacementTarget as FrameworkElement;
            var task = listBoxItem.DataContext as TaskItem;

            DeleteTask(task);
        }

        private void ActionDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (System.Windows.Controls.MenuItem)sender;
            var contextMenu = (System.Windows.Controls.ContextMenu)menuItem.Parent;
            var card = (FrameworkElement)contextMenu.PlacementTarget;
            var action = (ActionItem)card.DataContext;

            DeleteAction(action);
        }

        private void NewTask_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateTask("Task Name ?");
        }
    }
}
