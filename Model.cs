using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Facienda
{
    internal class Root
    {
        public List<TaskItem> Tasks { get; set; }
        public List<ActionItem> Actions { get; set; }
    }

    public class TaskItem : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _dueDate;
        private string _note;

        public ObservableCollection<ActionItem> Actions { get; } = new ObservableCollection<ActionItem>();

        public string Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string DueDate
        {
            get => _dueDate;
            set
            {
                if (_dueDate == value) return;
                _dueDate = value;
                OnPropertyChanged();
            }
        }

        public string Note
        {
            get => _note;
            set
            {
                if (_note == value) return;
                _note = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class ActionItem : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private bool _isDone;
        private string _taskId;

        public string Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (_isDone == value) return;
                _isDone = value;
                OnPropertyChanged();
            }
        }

        public string TaskId
        {
            get => _taskId;
            set
            {
                if (_taskId == value) return;
                _taskId = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
