using AsyncCommandTest.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCommandTest.Commands
{
    
    public static class AsyncCommand
    {
        public static IAsyncCommand Create(Func<Task> executeAsyncMethod,
            Func<bool> canExecuteMethod)
        {
            if (executeAsyncMethod == null)
                throw new ArgumentNullException("executeAsyncMethod");
            if (canExecuteMethod == null)
                return Create(executeAsyncMethod);
            return new AsyncCommand<object>(o => executeAsyncMethod(), o => canExecuteMethod());
        }

        public static IAsyncCommand Create(Func<Task> executeAsyncMethod)
        {
            if (executeAsyncMethod == null)
                throw new ArgumentNullException("executeAsyncMethod");
            return new AsyncCommand<object>(o => executeAsyncMethod());
        }

        public static IAsyncCommand Create<TParam>(Func<TParam, Task> executeAsyncMethod,
            Func<TParam, bool> canExecuteMethod)
        {
            if (executeAsyncMethod == null)
                throw new ArgumentNullException("executeAsyncMethod");
            return new AsyncCommand<TParam>(executeAsyncMethod, canExecuteMethod);
        }

        public static IAsyncCommand Create<TParam>(Func<TParam, Task> executeAsyncMethod)
        {
            return Create(executeAsyncMethod);
        }
    }
    
    internal class AsyncCommand<TParam> : AsyncCommandBase<TParam>, INotifyPropertyChanged
    {
        public AsyncCommand(Func<TParam, Task> executeAsyncMethod,
            Func<TParam, bool> canExecuteMethod) : base(canExecuteMethod)
        {
            if (executeAsyncMethod == null)
                throw new ArgumentNullException("executeAsyncMethod");
            _executeAsyncMethod = executeAsyncMethod;
        }

        public AsyncCommand(Func<TParam, Task> executeAsyncMethod)
            : this(executeAsyncMethod, null)
        { }


        public override NotifyTaskCompletion Execution
        {
            get
            {
                return base.Execution;
            }
            protected set
            {
                base.Execution = value;
                OnPropertyChanged();
            }
        }

        public override bool CanExecute(TParam parameter)
        {
            return (Execution == null || Execution.IsCompleted) && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(TParam parameter)
        {
            if (!CanExecute(parameter))
                throw new InvalidOperationException("CanExecute(object) returned false");

            Execution = new NotifyTaskCompletion(_executeAsyncMethod(parameter));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            RaiseCanExecuteChanged();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected readonly Func<TParam, Task> _executeAsyncMethod;

    }

    //internal class AsyncCommand<TParam, TResult> : AsyncCommand<TParam>
    //{
    //    public AsyncCommand(Func<TParam, Task<TResult>> executeAsyncMethod,
    //        Func<TParam, bool> canExecuteMethod) : base(executeAsyncMethod, canExecuteMethod)
    //    { }

    //    public AsyncCommand(Func<TParam, Task<TResult>> executeAsyncMethod)
    //        : this(executeAsyncMethod, null)
    //    { }

    //    public new NotifyTaskCompletion<TResult> Execution
    //    {
    //        get { return (NotifyTaskCompletion<TResult>)base.Execution; }
    //        protected set { base.Execution = value; }
    //    }

    //}
}
