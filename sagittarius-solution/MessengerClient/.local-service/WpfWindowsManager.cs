using MessengerClient.View;
using Net.Transmition;
using Common.Objects.Common;
using MessengerClient.ViewModel.ClientStartupWindow;
using MessengerClient.ViewModel.ClientChatWindow;

namespace MessengerClient.LocalService
{
    /// <summary>
    /// A service that provides a set of actions, required for working with multiple wpf windows in the same project.
    /// <br />
    /// Сервис, который предоставляет набор действий, необходимых для работы с несколькими окнами WPF в одном проекте.
    /// </summary>
    public static class WpfWindowsManager
    {


        /// <summary>
        /// Move from login window to the messenger one.
        /// <br />
        /// Переместиться из окна логина в окно чата.
        /// </summary>
        /// <param name="fullUserData">
        /// A set of data, which has been retrieved from MessengerService and is to be passed to the corresponding view-model.
        /// <br />
        /// Набор данных, полученных из MessengerService'а, для передачи в соответствующую вью-модель.
        /// </param>
        /// <param name="serviceTransmitter">
        /// An instance of a user's network reciever, set up and running.
        /// <br />
        /// Экземпляр пользовательского коммуникатора, настроенный и запущенный.
        /// </param>
        public static async Task MoveFromLoginToChat(UserServerSideDTO fullUserData, List<UserClientPublicDTO> memberList, ClientTransmitter serviceTransmitter)
        {
            await Application.Current.Dispatcher.Invoke(async () =>
            {
                ClientMessengerWindow window = new ClientMessengerWindow(fullUserData, memberList, serviceTransmitter);

                Window closeWindow = null;
                Window showWindow = null;

                foreach (Window win in Application.Current.Windows)
                {
                    if (win is ClientMessengerWindow)
                    {
                        showWindow = win;
                    }
                    else if (win is ClientLoginWindow)
                    {
                        closeWindow = win;
                    }
                }

                Application.Current.MainWindow = showWindow;
                showWindow.Show();
                closeWindow.Hide();
                await window.StartViewModelListenAsync();
            });
        }



        /// <summary>
        /// Move from the login window to the register one.
        /// <br />
        /// Перейти от окна логина к окну регистрации.
        /// </summary>
        public static void MoveFromLoginToRegister(UserClientTechnicalDTO userData, ClientTransmitter transmitter)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ClientSignUpWindow window = new(userData, transmitter);

                Window closeWindow = null;
                Window showWindow = null;

                foreach (Window win in Application.Current.Windows)
                {
                    if (win is ClientSignUpWindow)
                    {
                        showWindow = win;
                    }
                    else if (win is ClientLoginWindow)
                    {
                        closeWindow = win;
                    }
                }

                Application.Current.MainWindow = showWindow;
                showWindow.Show();
                closeWindow.Hide();
            });
        }



        /// <summary>
        /// Move from registration window to the login one.
        /// <br />
        /// Перейти от окна регистрации к окну логина.
        /// </summary>
        public static void FromRegisterToLogin(UserClientTechnicalDTO userData, ClientTransmitter transmitter)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ClientLoginWindow window = new(userData, transmitter);

                Window closeWindow = null;
                Window showWindow = null;

                foreach (Window win in Application.Current.Windows)
                {
                    if (win is ClientLoginWindow)
                    {
                        showWindow = win;
                    }
                    else if (win is ClientSignUpWindow)
                    {
                        closeWindow = win;
                    }
                }

                Application.Current.MainWindow = showWindow;
                showWindow.Show();
                closeWindow.Hide();
            });
        }



        /// <summary>
        /// Move from chat window to the login one. Typically is called when the user disconnects.
        /// <br />
        /// Переместиться из окна чата в окно логина. Обычно используется, когда пользователь утратит соединение.
        /// </summary>
        public static void MoveFromChatToLogin(string userLogin)
        {
            Window closeWindow = null;
            Window showWindow = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window win in Application.Current.Windows)
                {
                    if (win is ClientMessengerWindow)
                    {
                        closeWindow = win;
                    }
                    else if (win is ClientLoginWindow)
                    {
                        showWindow = win;
                    }
                }
                showWindow = new ClientLoginWindow();
                showWindow.Show();

                var vmRef = closeWindow.DataContext as ClientMessengerWindowViewModel;
                closeWindow.Hide();
                vmRef.AlreadyDisconnected = true;
                vmRef.ServiceTransmitter.Dispose();
            });
        }


    }
}
