﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace notes.ViewModel
{
    public class Command : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;
        public Command(Action execute)
            : this(execute, null)
        {
        }

        public Command(Action execute, Func<bool> canExecute)
        {
            if (execute is null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
