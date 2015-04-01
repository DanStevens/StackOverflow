using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncCommandTest.Commands
{
    public static class Command
    {
        public static ICommand Create(Action executeMethod, Func<bool> canExecuteMethod)
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMethod");
            if (canExecuteMethod == null)
                return Create(executeMethod);
            return new Command<object>(o => executeMethod(), o => canExecuteMethod());
        }

        public static ICommand Create(Action executeMethod)
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMethod");
            return new Command<object>(o => executeMethod());
        }

        public static ICommand Create<TParam>(Action<TParam> executeMethod,
            Func<TParam, bool> canExecuteMethod)
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMethod");
            return new Command<TParam>(executeMethod, canExecuteMethod);
        }

        public static ICommand Create<TParam>(Action<TParam> executeMethod)
        {
            return Create<TParam>(executeMethod);
        }

    }

    internal class Command<TParam> : CommandBase<TParam>
    {

        public Command(Action<TParam> executeMethod, Func<TParam, bool> canExecuteMethod)
            : base(canExecuteMethod)
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMethod");
            _executeMethod = executeMethod;
        }

        public Command(Action<TParam> executeMethod) : this(executeMethod, null) { }

        private readonly Action<TParam> _executeMethod;
    }
}
