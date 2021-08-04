namespace Prog3AT2_One
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            // Code copied from:
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.sessionending?view=net-5.0
            //
            // Ask the user if they want to allow the session to end
            string msg = $"{e.ReasonSessionEnding}. End session?";
            MessageBoxResult result = MessageBox.Show(msg, "Session Ending", MessageBoxButton.YesNo);

            // End session, if specified
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}