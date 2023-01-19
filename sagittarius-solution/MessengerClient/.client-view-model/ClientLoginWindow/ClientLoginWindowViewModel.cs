using Common.Objects.Common;
using Net.Transmition;

namespace MessengerClient.ViewModel.ClientStartupWindow
{
    /// <summary>
    /// A client authorization window view-model.
    /// <br />
    /// Вью-модель окна авторизации клиента.
    /// </summary>
    public partial class ClientLoginWindowViewModel : INotifyPropertyChanged
    {


        #region STATE


        /// <inheritdoc cref="LocalUserTechnicalData"/>
        private UserClientTechnicalDTO _localUserTechnicalData;


        /// <inheritdoc cref="FullUserServiceData">
        private UserServerSideDTO _fullUserServiceData;


        /// <inheritdoc cref="ServiceTransmitter"/>
        private ClientTransmitter serviceTransmitter;



        /// <summary>
        /// An instance of an object to encapsulate user data transfered to another windows.
        /// <br />
        /// Экземпляр объекта для инкапсуляции пользовательских данных для передачи другим окнам.
        /// </summary>
        public UserClientTechnicalDTO LocalUserTechnicalData
        {
            get { return _localUserTechnicalData; }
            set
            {
                _localUserTechnicalData = value;
                OnPropertyChanged(nameof(LocalUserTechnicalData));
            }
        }


        /// <summary>
        /// An instance of an object to encapsulate user data got from messenger service response.
        /// <br />
        /// Экземпляр объекта, хранящий данные пользователя, полученные от сервиса.
        /// </summary>
        public UserServerSideDTO FullUserServiceData
        {
            get { return _fullUserServiceData; }
            set
            {
                _fullUserServiceData = value;
                OnPropertyChanged(nameof(FullUserServiceData));
            }
        }


        /// <summary>
        /// An instance of a 'ClientTransmitter' to communicate with the service;
        /// <br />
        /// Экземпляр класса "ClientTransmitter" для коммуникации с сервисом;
        /// </summary>
        public ClientTransmitter ServiceTransmitter
        {
            get { return serviceTransmitter; }
            set { serviceTransmitter = value; }
        }


        #endregion STATE





        #region COMMANDS



        /// <summary>
        /// A Prism command to handle 'SignIn' button click.
        /// <br /> 
        /// Команда Prism, для обработки клика по кнопке "SignIn".
        /// </summary>
        public DelegateCommand SignInCommand { get; }

        /// <summary>
        /// A Prism command to handle 'SignUp' button click.
        /// <br /> 
        /// Команда Prism, для обработки клика по кнопке "SignUp".
        /// </summary>
        public DelegateCommand SignUpCommand { get; }



        #endregion COMMANDS





        #region CONSTRUCTION



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public ClientLoginWindowViewModel()
        {
            _localUserTechnicalData = new UserClientTechnicalDTO();

            serviceTransmitter = new();
            serviceTransmitter.SendOutput += ShowErrorMessage;

            SignInCommand = new(OnSignInButtonClick);
            SignUpCommand = new(OnSignUpButtonClick);
        }


        /// <summary>
        /// Parametrized constructor.
        /// <br />
        /// Параметризованный конструктор.
        /// </summary>
        public ClientLoginWindowViewModel(UserClientTechnicalDTO userData, ClientTransmitter transmitter) : this()
        {
            LocalUserTechnicalData.Login = userData.Login;
            serviceTransmitter = transmitter;
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
