using Microsoft.Office.Interop.Excel;
using sCommon.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ToastNotification
{
    public partial class MainForm : Form
    {
        private string _filePath = "";

        private Excel.Application _excelApp;
        private Excel.Workbook nowWorkbook;

        private List<Sheet> ExcelSheets = new List<Sheet>();
        private Sheet NowSheet = new Sheet();

        public event EventHandler ShowAlertMessage;

        public MainForm()
        {
            InitializeComponent();



        }

        public void Initialize()
        {
            cSystemDB.InitializeDatabase();

            _filePath = cSystemDB.ReadDataPath();

            if (_filePath != null)
            {
#if DEBUG
                Console.WriteLine($"Data Path: {_filePath}");
#endif
                if (IsExcelFile(_filePath))
                    SetFilePath(_filePath);
                else
                    MessageBox.Show("파일 경로가 변경되었거나 파일이 삭제되었습니다.");
            }

            MessageBox.Show("프로그램 대기");

            SetNotifyMessage();
        }

        #region Button Event
        private void btnPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel 파일 (*.xlsx)|*.xlsx|모든 파일 (*.*)|*.*";
            openFileDialog.Title = "Excel 파일 선택";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                // 선택된 파일이 .xlsx인지 확인
                if (IsExcelFile(filePath))
                {
                    SetFilePath(filePath);
                    cSystemDB.SaveDataPath(filePath);
                }
                else
                {
                    MessageBox.Show("올바른 Excel 파일을 선택해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string NowSheetName = NowSheet.Name;
            string Date = "";
            string Data = "";
            foreach (int date in lvRefData.CheckedIndices)
            {
                if (Date != "") Date += "|" + date.ToString();
                else            Date += date.ToString();
            }

            foreach (int data in lvNotifyData.CheckedIndices)
            {
                if (Data != "") Data += "|" + data.ToString();
                else            Data += data.ToString();
            }


            if (Date == "")
            {
                cSystemDB.RemoveAlert(NowSheetName);
                MessageBox.Show("지정된 알람 기준 날짜가 없어 예약을 삭제합니다.", "알림", MessageBoxButtons.OK);
            }
            else
            {
                cSystemDB.SaveAlert(NowSheetName, Date, Data);
                DialogResult result = MessageBox.Show("저장이 완료되었습니다.\r\n알람을 바로 적용하시겠습니까?", "알림", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    SetAlertDataTable();
                    SetNotifyMessage();
                    MessageBox.Show("적용되었습니다.", "알림", MessageBoxButtons.OK);

                }
                else
                    MessageBox.Show("저장된 알람은 프로그램 재 실행 시 적용됩니다.", "알림", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region ComboBox Event
        private void cbSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.SelectedIndex >= 0)
            {
                NowSheet = ExcelSheets.Find(x => x.Name == (string)cb.SelectedItem);
                SetTable();
            }
        }
        #endregion

        #region Setting
        private void SetFilePath(string value)
        {
            _filePath = value;

            if (tbPath.IsHandleCreated)
                tbPath.Invoke(new MethodInvoker(delegate { tbPath.Text = _filePath; }));

            _excelApp = new Excel.Application();
            nowWorkbook = _excelApp.Workbooks.Open(_filePath);

            foreach (Excel.Worksheet worksheet in nowWorkbook.Sheets.Cast<Excel.Worksheet>())
            {
                List<DataTitle> lColum = new List<DataTitle>();

                int rowCount = worksheet.UsedRange.Rows.Count;
                int colCount = worksheet.UsedRange.Columns.Count;
#if DEBUG
                Console.WriteLine(worksheet.Name + " >> " + rowCount + ", " + colCount);
#endif
                for (int ii = 1; ii <= colCount; ii++)
                {
                    object cellValue = (worksheet.Cells[1, ii] as Excel.Range)?.Value2;

                    if (cellValue != null)
                    {
#if DEBUG
                        Console.WriteLine("\t" + cellValue.ToString());
#endif
                        lColum.Add(new DataTitle() { Index = ii, Title = cellValue.ToString() });
                    }
                    else
                    {
#if DEBUG
                        Console.WriteLine("\t" + "Empty or null cell");
#endif
                    }
                }

                Sheet sheet = new Sheet() { Name = worksheet.Name, lDataTitle = lColum };

                ExcelSheets.Add(sheet);
            }
            
            SetAlertDataTable();
            SetSheets();
        }
        private void SetSheets()
        {
            cbSheets.Items.Clear();

            foreach(var sheet in ExcelSheets)
                cbSheets.Items.Add(sheet.Name);

            if(cbSheets.Items.Count > 0)
                cbSheets.SelectedIndex = 0;
        }
        private void SetTable()
        {
            lvRefData.Items.Clear();
            lvNotifyData.Items.Clear();

            foreach (var sheet in NowSheet.lDataTitle)
            {
                ListViewItem _ref = new ListViewItem(sheet.Title) { Checked = sheet.isAlert };
                ListViewItem _noti = new ListViewItem(sheet.Title) { Checked = sheet.isNotify };

                lvRefData.Items.Add(_ref);
                lvNotifyData.Items.Add(_noti);
            }

        }
        private void SetAlertDataTable()
        {
            try
            {
                foreach (Sheet sheet in ExcelSheets)
                {
                    Tuple<string, string, string> alert = cSystemDB.ReadAlert(sheet.Name);

                    if (alert != null)
                    {
                        CheckList(sheet, alert.Item2.Split('|'), true);
                        CheckList(sheet, alert.Item3.Split('|'), false);

                        Worksheet Worksheet = (Worksheet)nowWorkbook.Sheets[sheet.Name];

                        int rowCount = Worksheet.UsedRange.Rows.Count;
                        int colCount = Worksheet.UsedRange.Columns.Count;
#if DEBUG
                        Console.WriteLine(Worksheet.Name + " >> " + rowCount + ", " + colCount);
#endif

                        foreach (DataTitle dt in sheet.lDataTitle)
                            dt.lData.Clear();

                        for (int ii = 2; ii <= rowCount; ii++)
                        {
                            foreach (DataTitle dt in sheet.lDataTitle)
                            {
                                if (dt.isAlert)
                                {
                                    object cellValue = (Worksheet.Cells[ii, dt.Index] as Excel.Range)?.Value2;

                                    if (cellValue != null)
                                    {
                                        string date = cellValue.ToString();
                                        //if (date.Length > 8)
                                        //    date = "20" + date.Substring(date.Length - 7, 6);

                                        if (date.Length == 5)
                                        {
                                            dt.lData.Add(ConvertFromExcelDate(int.Parse(date)));
#if DEBUG
                                            Console.WriteLine("\t" + ((DateTime)(dt.lData.Last<object>())).ToString("yyyy-MM-dd"));
#endif
                                        }
                                    }
                                }
                                else if (dt.isNotify)
                                {
                                    object cellValue = (Worksheet.Cells[ii, dt.Index] as Excel.Range)?.Value2;
                                    if (cellValue != null)
                                    {
                                        string data = cellValue.ToString();
                                        dt.lData.Add(data);
#if DEBUG
                                        Console.WriteLine("\t" + data);
#endif
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#else
                MessageBox.Show(e.Message);
#endif
            }
        }
        #endregion

        private bool IsExcelFile(string filePath)
        {
            // 파일 확장자가 .xlsx인지 확인
            return filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase);
        }
        public void CheckList(Sheet sheet, string[] strArray, bool isAlert)
        {
            foreach (string numberString in strArray)
            {
                if (int.TryParse(numberString, out int number))
                {
                    DataTitle dt = sheet.lDataTitle.Find(x => x.Index == number + 1);

                    if (isAlert) dt.isAlert = true;
                    else dt.isNotify = true;
                }

                else
                {
#if DEBUG
                    Console.WriteLine($"Failed to convert '{numberString}' to int.");
#endif
                }
            }
        }

        private void SetNotifyMessage()
        {
            string msg = "";

            foreach (Sheet sheet in ExcelSheets)
            {
                Tuple<string, string, string> alert = cSystemDB.ReadAlert(sheet.Name);

                if(alert != null)
                {

                    List<DataTitle> AlretList = sheet.lDataTitle.FindAll(x => x.isAlert);
                    List<DataTitle> NotifyList = sheet.lDataTitle.FindAll(x => x.isNotify);

                    List<int> lIndex = new List<int>();
                    foreach(DataTitle dataTitle in AlretList)
                    {
                        int nextIdx = 0;

                        while (true)
                        {
                            nextIdx = dataTitle.lData.FindIndex(nextIdx, x => ((DateTime)x).Equals(DateTime.Today.AddDays(21))); ;

                            if (nextIdx == -1)
                                break;

                            lIndex.Add(nextIdx);
                            nextIdx += 1;
                        }
                    }

                    lIndex.Sort();

                    msg += "오늘은 " + DateTime.Today.ToString("yyyy-MM-dd") + "입니다.\r\n";
                    msg += sheet.Name + " 에서 총 " + lIndex.Count + " 개의 확인이 필요합니다. \r\n";


                    foreach (int ii in lIndex)
                    {
                        foreach(DataTitle data in NotifyList)
                            msg += data.lData[ii].ToString() + ", ";

                        msg += "\r\n";
                    }
                }
            }

            ShowAlertMessage?.Invoke(msg, new EventArgs());
        }


#region override
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible)
                tbPath.Invoke(new MethodInvoker(delegate { tbPath.Text = _filePath; }));

            base.OnVisibleChanged(e);
        }
        #endregion

        static DateTime ConvertFromExcelDate(double excelDate)
        {
            // Excel의 날짜 기준인 "1900년 1월 1일"을 기준으로 경과된 일 수를 더합니다.
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime resultDate = baseDate.AddDays(excelDate - 2); // Excel의 날짜 기준으로 2일이 차이나므로 2를 빼줍니다.

            return resultDate;
        }
    }

    public class Sheet
    {
        public string Name { get; set; } = "sheets";
        public List<DataTitle> lDataTitle { get; set; } = new List<DataTitle>();
    }

    public class DataTitle
    {
        public int Index { get; set; } = -1;
        public string Title { get; set; } = "";
        public bool isAlert { get; set; } = false;
        public bool isNotify { get; set; } = false;
        public List<object> lData { get; set; } = new List<object>();
    }

}
