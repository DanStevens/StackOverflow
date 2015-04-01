using AsyncCommandTest.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncCommandTest.Commands
{
    public interface IAsyncCommand : ICommand
    {
        NotifyTaskCompletion Execution { get; }

        Task ExecuteAsync(object parameter);
    }
}
    
