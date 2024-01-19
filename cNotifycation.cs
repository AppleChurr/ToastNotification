using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToastNotification
{
    public class cNotifycation : IDisposable
    {

        #region Properties

        #region prevate
        private NotifyIcon _notifyIcon { get; set; }
        private string _lastMessage { get; set; } = "";
        #endregion

        #region public
        public int Timeout { get; set; } = 1000;

        #endregion
        #endregion

        #region Variable

        #region prevate
        #endregion
        #region public


        #endregion
        #endregion

        #region event

        public event EventHandler MessageClicked;
        public event EventHandler MessageClosed;

        #endregion


        public cNotifycation()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = SystemIcons.Information;
            _notifyIcon.Visible = false;

            _notifyIcon.BalloonTipClicked += _notifyIcon_BalloonTipClicked;
            _notifyIcon.BalloonTipClosed += _notifyIcon_BalloonTipClosed;
        }

        public bool Visible { get { return _notifyIcon.Visible; } set { _notifyIcon.Visible = value; } }

        public ContextMenu ContextMenu { get { return _notifyIcon.ContextMenu;} set {  _notifyIcon.ContextMenu = value; } }

        private void _notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            MessageClosed?.Invoke(sender, e);
        }

        private void _notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            MessageClicked?.Invoke(_lastMessage, e);
        }

        public bool ShowMessage(string Title, string Msg)
        {
            try
            {
                _lastMessage = Msg;
                _notifyIcon.ShowBalloonTip(Timeout, Title, _lastMessage.Split('&')[0], ToolTipIcon.Info);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            _notifyIcon.BalloonTipClicked -= _notifyIcon_BalloonTipClicked;
            _notifyIcon.BalloonTipClosed -= _notifyIcon_BalloonTipClosed;

            _notifyIcon.Dispose();
        }
    }
}
