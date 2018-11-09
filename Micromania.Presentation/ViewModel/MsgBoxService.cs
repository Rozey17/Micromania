﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Micromania.Presentation.ViewModel
{
    public class MsgBoxService : IMsgBoxService
    {
        public void ShowNotification(string message)
        {
            MessageBox.Show(message, "Notification", MessageBoxButton.OK);
        }

        public bool AskForConfirmation(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Are you sure?", MessageBoxButton.OKCancel);
            return result.HasFlag(MessageBoxResult.OK);
        }
    }
}
