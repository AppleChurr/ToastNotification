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
        static cNotifycation Notifycation = new cNotifycation() { Timeout = 30000 };

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

            MainForm = new MainForm();
            MainForm.FormClosing += MainForm_FormClosing;
            MainForm.ShowAlertMessage += MainForm_ShowAlertMessage;

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("표시", (sender, e) => MainForm.Show());
            contextMenu.MenuItems.Add("종료", (sender, e) => ExitApplication());
            Notifycation.ContextMenu = contextMenu;

            MainForm.Initialize();


            Application.Run();
        }

        private static void MainForm_ShowAlertMessage(object sender, EventArgs e)
        {
            string msg = (string)sender;

            Notifycation.ShowMessage("알람", msg);

            Notifycation.MessageClicked += Notifycation_MessageClicked;
        }

        private static void Notifycation_MessageClicked(object sender, EventArgs e)
        {
            string msg = (string)sender;
            DialogResult result = MessageBox.Show(msg + "확인을 누르시면 클립보드로 복사됩니다.");
            if(result == DialogResult.OK)
                Clipboard.SetText(msg);
        }

        private static void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitApplication();
        }

        static void ExitApplication()
        {
            MainForm.Dispose();
            Notifycation.Visible = false;
            Application.Exit();
        }
    }
}
