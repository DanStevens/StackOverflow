using AsyncCommandTest.Commands;
using AsyncCommandTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncCommandTest.ViewModels
{
    public class MultiplyViewModel : ViewModel
    {
        
        public MultiplyViewModel()
        {
            MultiplyAsyncCommand = AsyncCommand.Create(MultiplyAsync);
        }
        
        private string _number;

        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        private int? _multiplyResult;

        public int? MultiplyResult
        {
            get
            {
                return _multiplyResult;
            }
            private set
            {
                _multiplyResult = value;
                OnPropertyChanged();
            }
        }

        public IAsyncCommand MultiplyAsyncCommand { get; private set; }

        public async Task MultiplyAsync()
        {
            await TaskEx.Delay(TimeSpan.FromSeconds(6));
            MultiplyResult = int.Parse(Number) * 2;
        }
    }
}
