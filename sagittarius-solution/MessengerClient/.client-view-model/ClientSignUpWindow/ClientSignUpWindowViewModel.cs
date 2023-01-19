using Common.Objects.Common;
using MessengerClient.Model;
using Net.Transmition;

namespace MessengerClient.ViewModel.ClientSignUpWindow
{
    /// <summary>
    /// A view-model instance for the ClientSignUpWindow (a.k.a. registration window).
    /// <br />
    /// Экземпляр вью-модели для окна ClientSignUpWindow (так же известного, как окно регистрации).
    /// </summary>
    public partial class ClientSignUpWindowViewModel : INotifyPropertyChanged
    {


        #region STATE



        /// <inheritdoc cref="UserData"/>
        private UserClientTechnicalDTO _userData;



        /// <inheritdoc cref="RepeatedPassword"/>
        private string _repeatedPassword;



        /// <summary>
        /// An instance of the transmitter for client-service communication.
        /// <br />
        /// Экземпляр трансмиттера для связи клиента и сервиса.
        /// </summary>
        private ClientTransmitter transmitter;



        /// <summary>
        /// An instance of an object to encapsulate user data transfered to another windows.
        /// <br />
        /// Экземпляр объекта для инкапсуляции пользовательских данных для передачи другим окнам.
        /// </summary>
        public UserClientTechnicalDTO UserData
        {
            get { return _userData; }
            set
            {
                _userData = value;
                OnPropertyChanged(nameof(UserData));
            }
        }



        /// <summary>
        /// A temporal property for user password correctness check.
        /// <br />
        /// Временное свойство для проверки корректности пароля пользователя.
        /// </summary>
        public string RepeatedPassword
        {
            get { return _repeatedPassword; }
            set
            {
                _repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
            }
        }



        #endregion STATE





        #region COMMANDS



        /// <summary>
        /// A Prism command to handle 'Register' button click.
        /// <br />
        /// Команда Prism, для обработки клика по кнопке "Register".
        /// </summary>
        public DelegateCommand RegisterCommand { get; }


        /// <summary>
        /// A Prism command to handle the 'Get Back' button click.
        /// <br />
        /// Команда Prism, для обработки клика по кнопке "Get Back".
        /// </summary>
        public DelegateCommand GetBackCommand { get; }



        #endregion COMMANDS





        #region CONSTRUCTION



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public ClientSignUpWindowViewModel()
        {
            RegisterCommand = new(OnRegisterButtonClick);
            GetBackCommand = new(OnGetBackCommandButtonClick);
            _userData = new();
        }




        /// <summary>
        /// Parametrized constructor.
        /// <br />
        /// Параметризованный конструктор.
        /// </summary>
        public ClientSignUpWindowViewModel(UserClientTechnicalDTO userData, ClientTransmitter transmitter) : this()
        {
            this.transmitter = transmitter;
            UserData.Login = userData.Login;
        }



        #region Property changed


        /// <summary>
        /// Propery changed event handler;
        /// <br />
        /// Делегат-обработчик события 'property changed';
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;


        /// <summary>
        /// Handler-method of the 'property changed' delegate;
        /// <br />
        /// Метод-обработчик делегата 'property changed';
        /// </summary>
        /// <param name="propName">The name of the property;<br />Имя свойства;</param>
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        #endregion Property changed



        #endregion CONSTRUCTION


    }
}
