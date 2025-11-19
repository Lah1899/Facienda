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
using System.Collections.ObjectModel;

namespace Facienda
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private Root _root; // 全データを格納するオブジェクト
        private const string JSON_FN = "facienda.json"; // データ保存用JSON
        private const string JSON_BK_FN = "facienda_bk.json"; // アプリ起動時に作成されるバックアップ

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(JSON_FN))
            {
                File.Create(JSON_FN).Close(); // Jsonがなければ作成
            }
            File.Copy(JSON_FN, JSON_BK_FN, true); // Jsonのバックアップ
            LoadJson();
            DataContext = _root;
            SidebarTabs.SelectedIndex = 0;
            ContentTabs.SelectedIndex = 0;
        }

        // Jsonからのデータ読み込み
        private void LoadJson()
        {
            var json = File.ReadAllText("facienda.json");
            _root = JsonConvert.DeserializeObject<Root>(json);

            // _rootがNullの場合は初期化しておく
            if(_root == null) { _root = new Root(); }
            if(_root.Tasks == null) { _root.Tasks = new ObservableCollection<TaskItem>(); }
            if(_root.Actions == null) { _root.Actions = new ObservableCollection<ActionItem>(); }

            // アクションをタスクに所属させる
            foreach (TaskItem task in _root.Tasks) 
            {
                task.Actions.Clear(); // タスク配下のアクションを一度クリア
                foreach (ActionItem action in _root.Actions)
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
            var json = JsonConvert.SerializeObject(_root, Formatting.Indented);
            File.WriteAllText("facienda.json", json);
        }

        // アクションカードの完了ステータス反転
        private void ToggleActionStatus(ActionItem action)
        {
            action.IsDone = !action.IsDone;

            SaveJson();
        }

        // タスク編集ウィンドウの起動
        private void OpenTaskWindow(TaskItem task)
        {
            if (task == null) { return; } // タスクが指定されていない場合は処理を終了する
            var dlg = new TaskDetailWindow(task);
            dlg.owner = this;
            dlg.ShowDialog();
        }

        // アクション編集ウィンドウの起動
        private void OpenActionWindow(ActionItem action)
        {
            var dlg = new ActionDetailWindow(action);
            dlg.owner = this;
            dlg.ShowDialog();
        }

        // タスクのリネーム
        public void RenameTask(TaskItem task, string name)
        {
            task.Name = name;

            SaveJson();
        }

        // アクションのリネーム
        public void RenameAction(ActionItem action, string name)
        {
            action.Name = name;

            SaveJson();
        }

        // 完了アクションのクリア
        private void ClearActions(TaskItem task)
        {
            if(task == null) { return; } // タスクが指定されていない場合は処理を終了する
            var target = task.Actions.ToList();
            foreach(ActionItem action in target)
            {
                if (action.IsDone)
                {
                    DeleteAction(action);
                }
            }

            SaveJson();
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

            SaveJson();
        }

        // アクションの削除
        private void DeleteAction(ActionItem action)
        {
            // Rootのリストから削除
            _root.Actions.Remove(action);
            // 親タスクのリストから削除
            var parentTask = _root.Tasks.FirstOrDefault(t => t.Id == action.TaskId);
            parentTask.Actions.Remove(action);

            SaveJson();
        }

        // タスクの新規作成
        private void CreateTask(string name)
        {
            TaskItem newtask = new TaskItem();
            newtask.Name = name;
            newtask.Id = Guid.NewGuid().ToString();
            _root.Tasks.Add(newtask);

            SaveJson();

            // タスク作成直後に命名をさせる
            OpenTaskWindow(newtask);
        }

        // アクションの新規作成
        private void CreateAction(TaskItem task, string name)
        {
            if(task == null)
            {
                // タスクが指定されていない場合は処理を終了する
                MessageBox.Show("Select a task before addind actions.");
                return;
            }
            ActionItem newaction = new ActionItem();
            newaction.Name = name;
            newaction.Id = Guid.NewGuid().ToString();
            newaction.TaskId = task.Id;
            _root.Actions.Add(newaction);
            task.Actions.Add(newaction);

            SaveJson();

            // アクション作成直後に命名をさせる
            OpenActionWindow(newaction);
        }

        // 以降はイベント処理
        private void TaskRename_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var ctxMenu = menuItem.Parent as ContextMenu;
            var listBoxItem = ctxMenu.PlacementTarget as FrameworkElement;
            var task = listBoxItem.DataContext as TaskItem;

            OpenTaskWindow(task);
        }

        private void TaskDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var ctxMenu = menuItem.Parent as ContextMenu;
            var listBoxItem = ctxMenu.PlacementTarget as FrameworkElement;
            var task = listBoxItem.DataContext as TaskItem;

            DeleteTask(task);
        }

        private void ActionRename_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (System.Windows.Controls.MenuItem)sender;
            var contextMenu = (System.Windows.Controls.ContextMenu)menuItem.Parent;
            var card = (FrameworkElement)contextMenu.PlacementTarget;
            var action = (ActionItem)card.DataContext;

            OpenActionWindow(action);
        }

        private void ActionDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (System.Windows.Controls.MenuItem)sender;
            var contextMenu = (System.Windows.Controls.ContextMenu)menuItem.Parent;
            var card = (FrameworkElement)contextMenu.PlacementTarget;
            var action = (ActionItem)card.DataContext;

            DeleteAction(action);
        }

        private void ActionCard_Click(object sender, RoutedEventArgs e)
        {
            var card = (FrameworkElement)sender;
            var action = (ActionItem)card.DataContext;

            ToggleActionStatus(action);
        }

        private void NewTask_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateTask("Task Name ?");
        }

        private void NewAction_Button_Click(object sender, RoutedEventArgs e)
        {
            TaskItem task = SidebarTabs.SelectedItem as TaskItem;
            CreateAction(task, "Action Name ?");
        }

        private void ClearActions_Button_Click(Object sender, RoutedEventArgs e)
        {
            TaskItem task = SidebarTabs.SelectedItem as TaskItem;
            ClearActions(task);
        }

        private void SidebarTabs_DoubleClick(object sender, RoutedEventArgs e)
        {
            TaskItem task = SidebarTabs.SelectedItem as TaskItem;
            OpenTaskWindow(task);
        }
    }
}
