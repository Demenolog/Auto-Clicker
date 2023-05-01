﻿using System;
using System.Drawing;
using System.Threading;
using AutoClicker.Models.MouseClass;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WpfTest.Infrastructure.Commands;
using WpfTest.ViewModels.Base;
using System.Windows;

namespace WpfTest.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region TextBoxOne : string - test

        private string _TextBoxOne = "0";

        public string TextBoxOne
        {
            get => _TextBoxOne;
            set => SetField(ref _TextBoxOne, value);
        }

        #endregion TextBoxOne : string - test

        #region ButtonVisible : bool - test

        private bool _isButtonEnable = true;

        public bool IsButtonEnable
        {
            get => _isButtonEnable;
            set => SetField(ref _isButtonEnable, value);
        }

        #endregion ButtonVisible : bool - test

        #region TextBoxTwo : string - test

        private string _TextBoxTwo = "0";

        public string TextBoxTwo
        {
            get => _TextBoxTwo;
            set => SetField(ref _TextBoxTwo, value);
        }

        #endregion TextBoxTwo : string - test

        #region Title : string - string

        private string _Title = "Not empty";

        public string Title
        {
            get => _Title;
            set => SetField(ref _Title, value);
        }

        #endregion Title : string - string

        public ICommand Test { get; }

        private bool CanTestExecuted(object p) => true;

        internal void OnTestExecute(object p)
        {
            MessageBox.Show("kEK");
        }


        public MainWindowViewModel()
        {
            Test = new LambdaCommand(OnTestExecute, CanTestExecuted);
        }
    }
}