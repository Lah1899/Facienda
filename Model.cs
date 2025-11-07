using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facienda
{
    internal class Root
    {
        public List<TaskItem> Tasks { get; set; }
        public List<ActionItem> Actions { get; set; }
    }

    public class TaskItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DueDate { get; set; }
        public string Note{ get; set; }
    }

    public class ActionItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
        public string TaskId { get; set; }
    }
}
