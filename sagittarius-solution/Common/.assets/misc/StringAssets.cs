using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Assets.Misc
{
    /// <summary>
    /// A list of predefined standart information for the type 'string'.
    /// <br />
    /// Список предопределённой стандартизированной информации для типа "string".
    /// </summary>
    public static class StringAssets
    {


        /// <summary>
        /// The application default date pattern => "dd/MM/yyyy".
        /// <br />
        /// Дефолтный паттерн даты в приложении => "дд/ММ/гггг".
        /// </summary>
        public static readonly string DatePattern = "dd/MM/yyyy";


        /// <summary>
        /// The application default time pattern (down to seconds) => "HH:mm:ss".
        /// <br />
        /// Дефолтный паттерн времени в приложении (с точностью до секунды) => "чч:мм:сс".
        /// </summary>
        public static readonly string TimeSecondPattern = "HH:mm:ss";


        /// <summary>
        /// The application default time pattern (down to milliseconds) => "HH:mm:ss:fff".
        /// <br />
        /// Дефолтный паттерн времени в приложении (с точностью до милисекунды) => "чч:мм:сс:fff".
        /// </summary>
        public static readonly string TimeMillisecondPattern = "HH:mm:ss:fff";



        /// <summary>
        /// The application default date format.
        /// <br />
        /// Дефолтный формат даты в приложении.
        /// </summary>
        public static readonly string DateFormat = DateTime.Now.ToString(DatePattern, CultureInfo.InvariantCulture);


        /// <summary>
        /// The application default time format (down to seconds).
        /// <br />
        /// Дефолтный формат времени в приложении (с точностью до секунды).
        /// </summary>
        public static readonly string TimeSecondFormat = DateTime.Now.ToString(TimeSecondPattern, CultureInfo.InvariantCulture);


        /// <summary>
        /// The application default time format (down to milliseconds).
        /// <br />
        /// Дефолтный формат времени в приложении (с точностью до милисекунды).
        /// </summary>
        public static readonly string TimeMillisecondFormat = DateTime.Now.ToString(TimeMillisecondPattern, CultureInfo.InvariantCulture);



        /// <summary>
        /// Represents a mock string for the cases when the information was not specified.
        /// <br />
        /// Представляет собой строку-затычку для случаев, когда информация не была задана явно.
        /// </summary>
        public static readonly string NonAccessable = "n/a";


    }
}
