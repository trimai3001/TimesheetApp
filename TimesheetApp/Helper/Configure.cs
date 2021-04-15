using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TimesheetApp.Helper
{
    public class Configure
    {
        public static string DB_CONFIG_LOCATION = Directory.GetParent(Utilities.GetWorkingDir()).FullName + "\\Properties\\databaseSetting.json";
    }
}
