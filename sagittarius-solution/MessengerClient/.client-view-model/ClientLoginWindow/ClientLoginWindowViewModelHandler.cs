using MessengerClient.View;
using MessengerClient.LocalService;
using MessengerClient.ViewModel.ClientChatWindow;

namespace MessengerClient.ViewModel.ClientStartupWindow
{
    public partial class ClientLoginWindowViewModel
    {



        #region HANDLING



        /// <summary>
        /// Handle the 'Sign In' button click.
        /// <br />
        /// It uses try-catch syntax to recursively make three efforts of connection.
        /// If all of them are in vain, 'throw' a message box into the user.
        /// <br />
        /// <br />
        /// Обработать клик по кнопке 'Sign In'. Использует синтакс try-catch, чтобы рекурсивно сделать три попытки подключиться.
        /// <br />
        /// Если все они провалились, "выбросить" юхеру месседжбокс.
        /// </summary>
        private async void OnSignInButtonClick()
        {
            await MakeConnectionEffort();
        }


        /// <summary>
        /// Make an effort to connect to the authorization service.
        /// <br />
        /// Сделать попытку подклюяиться к сервису авторизации.
        /// </summary>
        private async Task MakeConnectionEffort()
        {

            if (!string.IsNullOrEmpty(_localUserTechnicalData.Password) && !string.IsNullOrEmpty(_localUserTechnicalData.Login))
            {
                if (ServiceTransmitter.Disposed) RenewTransmitter();
                
                try
                {
                    bool isAuthorized = default, isAuthorizerDown = default;

                    try
                    {
                        isAuthorized = await ServiceTransmitter.ConnectAndAuthorize(_localUserTechnicalData);
                    }
                    catch 
                    {
                        isAuthorizerDown = true;
                    }
                    if (!isAuthorizerDown)
                    {
                        if (isAuthorized)
                        {
                            bool isLoginSentToService = default;
                            try
                            {
                                isLoginSentToService = await ServiceTransmitter.ConnectAndSendLoginToService(_localUserTechnicalData);
                            }
                            catch
                            {
                                // timeout notification exception, handled on the lower level
                            }
                            if (isLoginSentToService)
                            {
                                var result = ServiceTransmitter.GetResponseData();
                                FullUserServiceData = result.userData;
                                var memberList = result.memberList;

                                await WpfWindowsManager.MoveFromLoginToChat(FullUserServiceData, memberList, ServiceTransmitter);
                            }
                        }
                        else
                        {
                            MessageBox.Show("You have entered incorrect login or password.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Server is down. Concider connecting later.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        RenewTransmitter();
                    }
                }
                catch (Exception ex)
                {
                    BeginDisconnection();
                }
            }
            else
            {
                MessageBox.Show("Both login and password fields are required to proceed.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }



        /// <summary>
        /// A handler for the registration button click which sends user to the registration window.
        /// <br />
        /// Обработчик клика кнопки регистрации, который отсылает пользователя в окно регистрации.
        /// </summary>
        private async void OnSignUpButtonClick()
        {
            WpfWindowsManager.MoveFromLoginToRegister(_localUserTechnicalData, serviceTransmitter);
        }



        /// <summary>
        /// Debug method.
        /// <br />
        /// Метод для дебага.
        /// </summary>
        private void ShowNotificationMessage(string message)
        {
            MessageBox.Show($"{message}", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }




        /// <summary>
        /// Start disconnection procedure.
        /// <br />
        /// Начать процедуру дисконнекта.
        /// </summary>
        private void BeginDisconnection()
        {
            var nullReferenceCheck = Application.Current?.MainWindow?.Name;
            if (nullReferenceCheck is not null)
            {
                if (Application.Current.MainWindow.Name.Equals(nameof(ClientMessengerWindow)))
                {
                    var vmRef = Application.Current.MainWindow.DataContext as ClientMessengerWindowViewModel;

                    if (!vmRef.AlreadyDisconnected)
                    {
                        MessageBox.Show("Server is down. Please, concider connecting later.", "Server down", MessageBoxButton.OK, MessageBoxImage.Information);
                        WpfWindowsManager.MoveFromChatToLogin(LocalUserTechnicalData.Login);
                    }
                }
            }
        }



        #endregion HANDLING





        #region LOGIC



        /// <summary>
        /// Renew the transmitter instance.
        /// <br />
        /// Обновить экземпляр трансмиттера.
        /// </summary>
        private void RenewTransmitter()
        {
            ServiceTransmitter.Dispose();
            ServiceTransmitter = new();
            serviceTransmitter.SendOutput += this.ShowNotificationMessage;
        }



        #endregion LOGIC



    }
}
