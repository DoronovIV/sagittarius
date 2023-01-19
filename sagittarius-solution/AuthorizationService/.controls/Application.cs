global using System.Collections.Generic;
global using System.Threading.Tasks;
global using System.Net.Sockets;
global using System.Text;
global using System.Linq;
global using System.Net;
global using System;

global using Tools.Flags;
global using Tools.Formatting;

global using Common.Assets.Enum.Authorizer;
global using Common.Assets.Misc;
global using Common.Dependencies;
global using Common.Net.Config;
global using Common.Net;
global using Common.Objects.Common;
global using Common.Packages;
global using Common.Processing;
global using Common.Style.Authorizer;
global using Common.Style.Common;


global using Microsoft.EntityFrameworkCore;
global using AuthorizationServiceProject.Net;
global using Microsoft.EntityFrameworkCore.Design;
global using AuthorizationServiceProject.Model.Context;

global using Spectre.Console;


namespace AuthorizationServiceProject.Controls
{
    public class Application
    {



        #region STATE



        /// <summary>
        /// The global instance of the client controller to dispose all the tcp clients used by the application.
        /// <br />
        /// Глобальный экземпляр контроллера, чтобы диспозить все tcp клиенты, использованные в приложении.
        /// </summary>
        public static TcpClientController tcpClientController = new();



        #endregion STATE




        #region API - public Contract



        /// <summary>
        /// Loop private 'Run' method.
        /// <br />
        /// Зациклить приватный метод "Run".
        /// </summary>
        public void Start()
        {
            Run();
        }



        #endregion API - public Contract





        #region LOGIC



        /// <summary>
        /// Run the application.
        /// <br />
        /// Запустить приложение.
        /// </summary>
        private void Run()
        {
            ServiceController controller = new();
            controller.ListenToClientConnections();
        }



        /// <summary>
        /// To be initiated when the console is to be closed.
        /// <br />
        /// Выполнить, когда консоль будет закрыта.
        /// </summary>
        private void OnConsoleClosing(object? sender, EventArgs e)
        {
            tcpClientController.TryDisposeAll();
        }



        #endregion LOGIC





        #region CONSTRUCTION



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public Application()
        {
            using (AuthorizationDatabaseContext context = new()) { }
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnConsoleClosing);
        }



        #endregion CONSTRUCTION



    }
}
