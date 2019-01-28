using System;

namespace SafeTodoExample.Model
{
    [Serializable]
    public class TodoItem
    {
        public string Title { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsCompleted { get; set; }
    }
}
