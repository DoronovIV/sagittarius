using MessengerClient.LocalService;

namespace MessengerClient.ViewModel.ClientSignUpWindow
{
    public partial class ClientSignUpWindowViewModel
    {



        #region HANDLING



        /// <summary>
        /// Handle 'Register' button click.
        /// <br />
        /// Обработать клик кнопки "Register".
        /// </summary>
        private void OnRegisterButtonClick()
        {
            try
            {
                if (AllFieldsAreInitialized())
                {
                    if (RepeatedPassword.Equals(UserData.Password))
                    {
                        if (transmitter.RegisterNewUser(UserData))
                        {
                            WpfWindowsManager.FromRegisterToLogin(UserData, transmitter);
                        }
                        else
                        {
                            MessageBox.Show($"Something went wrong when attempting to register.\nTry using another data, or check out the servers status at our official web-resouce.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Passwords do not match.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"All fields are required to proceed.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server is down. Consider register later.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        /// <summary>
        /// Handle the 'Go Back' button click event.
        /// <br />
        /// Обработать событие клика по кнопке "Go Back".
        /// </summary>
        private void OnGetBackCommandButtonClick()
        {
            WpfWindowsManager.FromRegisterToLogin(null, null);
        }



        #endregion HANDLING






        #region AUXILIARY



        /// <summary>
        /// A check for user to enter all the required fields.
        /// <br />
        /// Проверка для пользователя на ввод всех необходимых полей.
        /// </summary>
        public bool AllFieldsAreInitialized()
        {
            return 
                   !string.IsNullOrEmpty(UserData.Login)
                && !string.IsNullOrEmpty(UserData.Password)
                && !string.IsNullOrEmpty(UserData.PublicId)
                && !string.IsNullOrEmpty(RepeatedPassword);
        }



        #endregion AUXILIARY


    }
}
