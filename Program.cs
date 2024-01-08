using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Windows.UI.Notifications;

namespace ToastNotification
{
    static class Program
    {
        static cNotifycation Notifycation = new cNotifycation() { Timeout = 1000 };

        static MainForm MainForm;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Notifycation.Visible = true;

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("표시", (sender, e) => MainForm.Show());
            contextMenu.MenuItems.Add("종료", (sender, e) => ExitApplication());
            Notifycation.ContextMenu = contextMenu;

            MainForm = new MainForm();

            Application.Run();
        }

        static void ExitApplication()
        {
            // 애플리케이션을 종료할 때 호출됩니다.
            Notifycation.Visible = false;
            Application.Exit();
        }
    }
}
