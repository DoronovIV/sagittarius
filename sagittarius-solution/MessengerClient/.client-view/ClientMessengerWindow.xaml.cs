using MessengerClient.ViewModel.ClientChatWindow;
using MessengerClient.Model;
using System.Windows.Shell;
using Net.Transmition;
using Common.Objects.Common;

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.MahApps;

using MaterialDesignColors;

namespace MessengerClient.View
{
    /// <summary>
    /// Interaction logic for ClientMessengerWindow.xaml
    /// </summary>
    public partial class ClientMessengerWindow : Window
    {


        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public ClientMessengerWindow()
        {
            InitializeComponent();

            DataContext = new ClientMessengerWindowViewModel();

            SetDefaultName();
        }



        /// <summary>
        /// Parametrized constructor.
        /// <br />
        /// Параметризованный конструктор.
        /// </summary>
        public ClientMessengerWindow(UserServerSideDTO userData, List<UserClientPublicDTO> memberList, ClientTransmitter clientRadio) : this()
        {
            DataContext = new ClientMessengerWindowViewModel(userData, memberList, clientRadio);
        }



        /// <summary>
        /// Start listener in the view model.
        /// <br />
        /// Начать прослушивание во вью модели.
        /// </summary>
        public async Task StartViewModelListenAsync()
        {
            var vmref = DataContext as ClientMessengerWindowViewModel;

            await vmref.StartListenningAsync();
        }



        /// <summary>
        /// Set the default window name.
        /// <br />
        /// Установить имя окна по умолчанию.
        /// </summary>
        public void SetDefaultName()
        {
            Name = nameof(ClientMessengerWindow);
        }



        /// <summary>
        /// To be initiated on the window closing.
        /// <br />
        /// Выполнить при закрытии окна.
        /// </summary>
        public void OnClosing(object? sender, CancelEventArgs args)
        {
            var vmRef = DataContext as ClientMessengerWindowViewModel;
            vmRef.AlreadyDisconnected = true;
            vmRef.ServiceTransmitter.Dispose();
            Application.Current.Shutdown();
        }


    }
}
