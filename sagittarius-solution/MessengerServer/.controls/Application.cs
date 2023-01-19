global using System;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Linq;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Diagnostics;
global using System.Net;
global using System.Net.Sockets;
global using System.Runtime.CompilerServices;

global using Tools.Interfaces;
global using Tools.Collections.Generic;
global using Tools.Formatting;

global using Common.Assets.Enum.Messenger;
global using Common.Assets.Misc;
global using Common.Dependencies;
global using Common.Net.Config;
global using Common.Net;
global using Common.Objects.Common;
global using Common.Packages;
global using Common.Processing;
global using Common.Style.Messenger;
global using Common.Style.Common;



global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using MessengerService.Model.Entities;
global using MessengerService.LocalService;

using MessengerService.Datalink;
using Spectre.Console;

namespace MessengerService.Controls
{
    public class Application
    {



        #region STATE



        /// <summary>
        /// The global instance of the client controller to dispose all the tcp clients used by the application.
        /// <br />
        /// Глобальный экземпляр контроллера, чтобы диспозить все tcp клиенты, использованные в приложении.
        /// </summary>
        public static TcpClientController tcpClientController = new ();



        #endregion STATE




        #region API - public Contract


        /// <summary>
        /// Loop the 'Run' method.
        /// <br />
        /// Зациклить метод "Run".
        /// </summary>
        public async Task Start()
        {
            await Run();
        }


        #endregion API - public Contract





        #region LOGIC



        /// <summary>
        /// Run the application.
        /// <br />
        /// Запустить приложение.
        /// </summary>
        private async Task Run()
        {
            AnsiConsole.Write(new Markup(ConsoleServiceStyleCommon.GetMessengerGreeting()));
            ServiceController controller = new();
            var task1 = controller.ListenAuthorizerAsync();
            var task2 = controller.ListenClientsAsync();
            await Task.WhenAll(task1, task2);
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
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnConsoleClosing);
        }



        #endregion CONSTRUCTION



    }
}
