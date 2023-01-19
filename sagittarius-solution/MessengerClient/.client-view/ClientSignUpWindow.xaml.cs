using System;
using System.Collections.Generic;
using Net.Transmition;
using MessengerClient.ViewModel.ClientSignUpWindow;
using MessengerClient.ViewModel.ClientStartupWindow;
using Common.Objects.Common;

namespace MessengerClient.View
{
    /// <summary>
    /// Interaction logic for ClientSignUpWindow.xaml
    /// </summary>
    public partial class ClientSignUpWindow : Window
    {


        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public ClientSignUpWindow()
        {
            InitializeComponent();

            DataContext = new ClientSignUpWindowViewModel();

            Name = nameof(ClientSignUpWindow);

            Title = "Sagittarius – sign up";
        }



        /// <summary>
        /// Parametrized constructor.
        /// <br />
        /// Параметризованный конструктор.
        /// </summary>
        public ClientSignUpWindow(UserClientTechnicalDTO user, ClientTransmitter transmitter) : this()
        {
            DataContext = new ClientSignUpWindowViewModel(user, transmitter);
        }



        /// <summary>
        /// To be initiated on window closing.
        /// <br />
        /// Выполнить при закрытии окна.
        /// </summary>
        public void OnClosing(object? sender, EventArgs args)
        {
            Application.Current.Shutdown();
        }


    }
}
