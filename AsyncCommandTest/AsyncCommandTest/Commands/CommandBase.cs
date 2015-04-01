using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncCommandTest.Commands
{
    internal abstract class CommandBase<TParam> : ICommand
    {

        protected static ArgumentException CreateArgumentException<T>(Exception innerException)
        {
            return new ArgumentException(String.Format("Argument could not be cast to <{0}>",
                typeof(T)), "parameter", innerException);
        }

        protected CommandBase() : this(null) { }

        protected CommandBase(Func<TParam, bool> canExecuteMethod)
        {
            _canExecuteMethod = canExecuteMethod;
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        
        public virtual bool CanExecute(TParam parameter)
        {
            try {
                return _canExecuteMethod == null || _canExecuteMethod(parameter);
            } catch (InvalidCastException ex) {
                throw CreateArgumentException<TParam>(ex);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual void Execute(TParam parameter)
        {
            if (CanExecute(parameter))
                Execute(parameter);
        }

        #region ICommand method implementations

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((TParam)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((TParam)parameter);
        }
        #endregion


        private readonly Func<TParam, bool> _canExecuteMethod;
        //private readonly Lazy<Type> _tParamType = new Lazy<Type>(() => typeof(TParam));
        
    }
}
