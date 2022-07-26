﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace notes.ViewModel
{
    public class ActionCommand<T> : ICommand
	{
        public event EventHandler CanExecuteChanged;
        private Action<T> _action;

        public ActionCommand(Action<T> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                //var castParameter = (T)Convert.ChangeType(parameter, typeof(T));
                var castParameter = (T)parameter;
                _action(castParameter);
            }
        }
    }
}
