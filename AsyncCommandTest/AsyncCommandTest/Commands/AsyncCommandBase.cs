using AsyncCommandTest.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCommandTest.Commands
{
    internal abstract class AsyncCommandBase<TParam> : CommandBase<TParam>, IAsyncCommand
    {

        protected AsyncCommandBase(Func<TParam, bool> canExecuteMethod)
            : base(canExecuteMethod)
        { }

        public virtual NotifyTaskCompletion Execution {get; protected set;}
        
        public override async void Execute(TParam parameter)
        {
            await ExecuteAsync(parameter);
        }

        public abstract Task ExecuteAsync(TParam parameter);

        Task IAsyncCommand.ExecuteAsync(object parameter)
        {
            return ExecuteAsync((TParam)parameter);
        }

    }
}
