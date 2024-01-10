using ClosedXML.Excel;
using sCommon.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ToastNotification
{
    public partial class MainForm : Form
    {
        private string _filePath = "";

        private XLWorkbook _excelWorkbook;

        private List<Sheet> ExcelSheets = new List<Sheet>();
        private Sheet NowSheet = new Sheet();


        public event EventHandler ShowAlertMessage;

        public MainForm()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            try
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

                SetNotifyMessage();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message); 
            }
        }

        #region Button Event
        private void btnPath_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message); 
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string NowSheetName = NowSheet.Name;
                string Date = "";
                string Data = "";
                foreach (int date in lvRefData.CheckedIndices)
                {
                    if (Date != "") Date += "|" + date.ToString();
                    else Date += date.ToString();
                }

                foreach (int data in lvNotifyData.CheckedIndices)
                {
                    if (Data != "") Data += "|" + data.ToString();
                    else Data += data.ToString();
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
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message); 
            }
        }
        #endregion

        #region ComboBox Event
        private void cbSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cb = (ComboBox)sender;

                if (cb.SelectedIndex >= 0)
                {
                    NowSheet = ExcelSheets.Find(x => x.Name == (string)cb.SelectedItem);
                    SetTable();
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message); 
            }
        }
        #endregion

        #region Setting
        private void SetFilePath(string value)
        {
            try
            {
                _filePath = value;

                if (tbPath.IsHandleCreated)
                    tbPath.Invoke(new MethodInvoker(delegate { tbPath.Text = _filePath; }));

                _excelWorkbook = new XLWorkbook(_filePath);

                foreach (IXLWorksheet worksheet in _excelWorkbook.Worksheets)
                {
                    List<DataTitle> lColum = new List<DataTitle>();

                    int rowCount = worksheet.RowsUsed().Count();
                    int colCount = worksheet.ColumnsUsed().Count();
#if DEBUG
                    Console.WriteLine(worksheet.Name + " >> " + rowCount + ", " + colCount);
#endif
                    for (int ii = 1; ii <= colCount; ii++)
                    {
                        var cellValue = worksheet.Cell(1, ii).Value;

                        if (!cellValue.IsBlank)
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            SetAlertDataTable();
            SetSheets();
        }

        private void SetSheets()
        {
            try
            {
                cbSheets.Items.Clear();

                foreach (var sheet in ExcelSheets)
                    cbSheets.Items.Add(sheet.Name);

                if (cbSheets.Items.Count > 0)
                    cbSheets.SelectedIndex = 0;
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message); 
            }
        }
        private void SetTable()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
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

                        var worksheet = _excelWorkbook.Worksheet(sheet.Name);

                        int rowCount = worksheet.RowsUsed().Count();
                        int colCount = worksheet.ColumnsUsed().Count();
#if DEBUG
                        Console.WriteLine(worksheet.Name + " >> " + rowCount + ", " + colCount);
#endif


                        foreach (DataLine line in sheet.lLines)
                        {
                            line.lAlert.Clear();
                            line.lData.Clear();
                        }
                        sheet.lLines.Clear();







                        for (int ii = 2; ii <= rowCount; ii++)
                        {
                            DataLine line = new DataLine();

                            foreach (DataTitle dt in sheet.lDataTitle)
                            {
                                if (dt.isAlert)
                                {
                                    try
                                    {
                                        var cell = worksheet.Cell(ii, dt.Index);
                                        if ((cell.DataType == XLDataType.Text) && ((string)cell.CachedValue == ""))
                                        {
                                            //dt.lData.Add(new DateTime());
                                            line.lAlert.Add(new DateTime());
                                        }
                                        else
                                        {
                                            var cellValue = worksheet.Cell(ii, dt.Index).Value;

                                            if (!cellValue.IsBlank)
                                            {
                                                DateTime datetime = new DateTime();

                                                if (cellValue.IsDateTime)
                                                    datetime = cellValue.GetDateTime();
                                                else if (cellValue.IsNumber)
                                                    datetime = ConvertFromExcelDate(cellValue.GetNumber());

                                                //dt.lData.Add(datetime);
                                                line.lAlert.Add(datetime);

#if DEBUG
                                                Console.WriteLine("\t" + datetime.ToString("yyyy-MM-dd"));
#endif
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
#if DEBUG
                                        Console.WriteLine(ex.Message);
#else
                MessageBox.Show(ex.Message);
#endif
                                        //dt.lData.Add(new DateTime());
                                        line.lAlert.Add(new DateTime());
                                    }
                                }
                                else if (dt.isNotify)
                                {
                                    try
                                    {
                                        var cellValue = worksheet.Cell(ii, dt.Index).Value;
                                        if (!cellValue.IsBlank)
                                        {
                                            if (cellValue.IsText)
                                            {
                                                string data = cellValue.ToString();
                                                //dt.lData.Add(data);
                                                line.lData.Add(data);

#if DEBUG
                                                Console.WriteLine("\t" + data);
#endif
                                            }
                                        }
                                        else
                                        {
                                            //dt.lData.Add("");
                                            line.lData.Add("");
                                        }

                                    }
                                    catch (Exception ex)
                                    {
#if DEBUG
                                        Console.WriteLine(ex.Message);
#else
                MessageBox.Show(ex.Message);
#endif
                                        //dt.lData.Add("");
                                        line.lData.Add("");
                                    }
                                }
                            }

                            sheet.lLines.Add(line);
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
            try
            {
                foreach (string numberString in strArray)
                {
                    if (int.TryParse(numberString, out int number))
                    {
                        DataTitle dt = sheet.lDataTitle.Find(x => x.Index == number + 1);

                        if (isAlert)
                        {
                            dt.isAlert = true;
#if DEBUG
                            Console.WriteLine(dt.Title + " is Alert");
#endif
                        }
                        else
                        {
                            dt.isNotify = true;

#if DEBUG
                            Console.WriteLine(dt.Title + " is Notify");
#endif
                        }
                    }

                    else
                    {
#if DEBUG
                        Console.WriteLine($"Failed to convert '{numberString}' to int.");
#endif
                    }
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message); 
            }
        }

        private void SetNotifyMessage()
        {
            try
            {
                string msg = "";

                foreach (Sheet sheet in ExcelSheets)
                {
                    Tuple<string, string, string> alert = cSystemDB.ReadAlert(sheet.Name);

                    if (alert != null)
                    {
                        msg += "오늘은 " + DateTime.Today.ToString("yyyy-MM-dd") + "입니다.\r\n";
                        int CountAlert = 0;

                        foreach (DataLine line in sheet.lLines)
                        {
                            if (line.lAlert.FindIndex(x => ((DateTime)x).Equals(DateTime.Today.AddDays(21))) >= 0)
                            {
                                foreach (string data in line.lData)
                                    msg += (data + ", ");

                                msg += "\r\n";

                                CountAlert += 1;
                            }
                        }

                        msg += sheet.Name + " 에서 총 " + CountAlert + " 개의 확인이 필요합니다. \r\n";

                    }
                }

                ShowAlertMessage?.Invoke(msg, new EventArgs());
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message); 
            }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }

    public class Sheet
    {
        public string Name { get; set; } = "sheets";
        public List<DataTitle> lDataTitle { get; set; } = new List<DataTitle>();
        public List<DataLine> lLines { get; set; } = new List<DataLine>();
    }

    public class DataTitle
    {
        public int Index { get; set; } = -1;
        public string Title { get; set; } = "";
        public bool isAlert { get; set; } = false;
        public bool isNotify { get; set; } = false;
    }

    public class DataLine
    {
        public List<DateTime> lAlert { get; set; } = new List<DateTime>();
        public List<string> lData { get; set; } = new List<string>();
    }

}