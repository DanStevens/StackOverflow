using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCommandTest.Tasks
{

    public class NotifyTaskCompletion : INotifyPropertyChanged
    {

        public NotifyTaskCompletion(Task task)
        {
            Task = task;
            if (!Task.IsCompleted) {
                TaskCompletion = watchTaskAsync(Task);
            }
        }

        public Task TaskCompletion { get; private set; }

        public TaskStatus Status
        {
            get { return Task.Status; }
        }

        public bool IsCompleted
        {
            get { return Task.IsCompleted; }
        }

        public bool IsNotCompleted
        {
            get { return !Task.IsCompleted; }
        }

        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task.Status == TaskStatus.RanToCompletion;
            }
        }

        public bool IsCanceled
        {
            get { return Task.IsCanceled; }
        }

        public bool IsFaulted
        {
            get { return Task.IsFaulted; }
        }

        public AggregateException Exception
        {
            get { return Task.Exception; }
        }

        public Exception InnerException
        {
            get
            {
                return (Exception == null) ? null : Exception.InnerException;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return (InnerException == null) ? null : InnerException.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Task Task { get; private set; }

        private async Task watchTaskAsync(Task task)
        {
            try {
                await task;
            } catch {
                // Swallow exceptions - consumer will use Exception and InnerException properties 
                // to retrieveExceptions
            }

            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));

            if (task.IsCanceled) {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            } else if (task.IsFaulted) {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            } else {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }
    }


    //public sealed class NotifyTaskCompletion<TResult> : NotifyTaskCompletion
    //{

    //    public NotifyTaskCompletion(Task<TResult> task)
    //        : base(task)
    //    { }

    //    public new Task<TResult> Task
    //    {
    //        get { return (Task<TResult>)Task; }
    //    }

    //    public TResult Result
    //    {
    //        get
    //        {
    //            return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult);
    //        }
    //    }

    //}
}
