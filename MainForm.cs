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

        public MainForm()
        {
            InitializeComponent();
            cSystemDB.InitializeDatabase();


            _filePath = cSystemDB.ReadDataPath();

            if (_filePath != null)
            {
                Console.WriteLine($"Data Path: {_filePath}");
                if (IsExcelFile(_filePath))
                    SetFilePath(_filePath);
                else
                    MessageBox.Show("파일 경로가 변경되었거나 파일이 삭제되었습니다.");
            }

            MessageBox.Show("프로그램 대기");
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if(this.Visible)
                tbPath.Invoke(new MethodInvoker(delegate { tbPath.Text = _filePath; }));

            base.OnVisibleChanged(e);
        }

        private string _filePath = "";
        private Excel.Application _excelApp;

        private List<KeyValuePair<string, List<KeyValuePair<int, string>>>> ExcelSheets = new List<KeyValuePair<string, List<KeyValuePair<int, string>>>>();
        private KeyValuePair<string, List<KeyValuePair<int, string>>> NowSheet = new KeyValuePair<string, List<KeyValuePair<int, string>>>();

        Excel.Workbook nowWorkbook;
        Excel.Worksheet nowWorksheet;
        private void SetFilePath(string value)
        {
            _filePath = value;

            if(tbPath.IsHandleCreated)
                tbPath.Invoke(new MethodInvoker(delegate { tbPath.Text = _filePath; }));

            _excelApp = new Excel.Application();
            nowWorkbook = _excelApp.Workbooks.Open(_filePath);

            foreach (Excel.Worksheet worksheet in nowWorkbook.Sheets.Cast<Excel.Worksheet>())
            {
                List<KeyValuePair<int, string>> lColum = new List<KeyValuePair<int, string>>();

                int rowCount = worksheet.UsedRange.Rows.Count;
                int colCount = worksheet.UsedRange.Columns.Count;

                Console.WriteLine(worksheet.Name + " >> " + rowCount + ", " + colCount);

                for (int ii = 1; ii <= colCount; ii++)
                {
                    object cellValue = (worksheet.Cells[1, ii] as Excel.Range)?.Value2;

                    if (cellValue != null)
                    {
                        Console.WriteLine("\t" + cellValue.ToString());
                        lColum.Add(new KeyValuePair<int, string>(ii, cellValue.ToString()));
                    }
                    else
                    {
                        Console.WriteLine("\t" + "Empty or null cell");
                    }
                }

                KeyValuePair<string, List<KeyValuePair<int, string>>> sheet = new KeyValuePair<string, List<KeyValuePair<int, string>>>(worksheet.Name, lColum);

                ExcelSheets.Add(sheet);
            }

            SetSheets();
        }


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
        private bool IsExcelFile(string filePath)
        {
            // 파일 확장자가 .xlsx인지 확인
            return filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase);
        }

        private void SetSheets()
        {
            cbSheets.Items.Clear();

            foreach(var vv in ExcelSheets)
                cbSheets.Items.Add(vv.Key);

            if(cbSheets.Items.Count > 0)
                cbSheets.SelectedIndex = 0;
        }
        private void SetTable()
        {
            lvRefData.Items.Clear();
            lvIncludeData.Items.Clear();

            List<int> refIndex = new List<int>();
            List<int> notifyIndex = new List<int>();

            Tuple<string, string, string> alert = cSystemDB.ReadAlert(NowSheet.Key);

            if (alert != null)
            {
                string[] strRefIndex = alert.Item2.Split('|');
                string[] strNotifyIndex = alert.Item3.Split('|');
                
                foreach (string numberString in strRefIndex)
                {
                    if (int.TryParse(numberString, out int number))
                        refIndex.Add(number);
                    else
                        Console.WriteLine($"Failed to convert '{numberString}' to int.");
                }

                foreach (string numberString in strNotifyIndex)
                {
                    if (int.TryParse(numberString, out int number))
                        notifyIndex.Add(number);
                    else
                        Console.WriteLine($"Failed to convert '{numberString}' to int.");
                }
            }

            foreach (var vv in NowSheet.Value)
            {
                lvRefData.Items.Add(vv.Value);
                lvIncludeData.Items.Add(vv.Value);
            }

            Console.WriteLine("알람 참조 데이터");

            foreach (int ii in refIndex)
            {
                lvRefData.Items[ii].Checked = true;
                Console.WriteLine("\t" + lvRefData.Items[ii].Text);
            }

            Console.WriteLine("공지 데이터");

            foreach (int ii in notifyIndex)
            {
                lvIncludeData.Items[ii].Checked = true;
                Console.WriteLine("\t" + lvIncludeData.Items[ii].Text);
            }



            nowWorksheet = (Worksheet)nowWorkbook.Sheets[NowSheet.Key];

            int rowCount = nowWorksheet.UsedRange.Rows.Count;
            int colCount = nowWorksheet.UsedRange.Columns.Count;

            Console.WriteLine(nowWorksheet.Name + " >> " + rowCount + ", " + colCount);

            for (int ii = 2; ii <= rowCount; ii++)
            {
                foreach (int jj in refIndex)
                {
                    object cellValue = (nowWorksheet.Cells[ii, jj + 1] as Excel.Range)?.Value2;

                    if (cellValue != null)
                    {
                        string date = cellValue.ToString();
                        if (date.Length > 8)
                            date = "20" + date.Substring(date.Length - 7, 6);

                        Console.WriteLine("\t" + date);
                    }
                }
            }


        }

        private void cbSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            
            if(cb.SelectedIndex >= 0)
            {
                NowSheet = ExcelSheets.Find(x => x.Key == (string)cb.SelectedItem);

                SetTable();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            /// 해당 파일 패스 DB에 저장
            /// 프로그램 실행 시 자동으로 읽어오도록
            /// 파일 읽기
            /// 다음 저장된 시트 번호 / 항목 명 / 날짜 형식 확인
            /// 해당 파일을 읽고 해당 항목에 해당하는 

            string NowSheetName = NowSheet.Key;
            string Date = "";
            string Data = "";
            foreach(int date in lvRefData.CheckedIndices)
            {
                if (Date != "")
                    Date += "|" + date.ToString();
                else
                    Date += date.ToString();
            }

            foreach (int data in lvIncludeData.CheckedIndices)
            {
                if(Data != "")
                    Data += "|" + data.ToString();
                else
                    Data += data.ToString();
            }
            cSystemDB.SaveAlert(NowSheetName, Date, Data);


            MessageBox.Show("저장이 완료되었습니다.");

        }
    }
}
