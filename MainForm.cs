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
        cNotifycation Notifycation = new cNotifycation() { Timeout = 1000 };

        public MainForm()
        {
            InitializeComponent();
        }

        private void Notifycation_MessageClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }



        private string _filePath = "";
        private Excel.Application _excelApp;

        private List<KeyValuePair<string, List<KeyValuePair<int, string>>>> ExcelSheets = new List<KeyValuePair<string, List<KeyValuePair<int, string>>>>();
        private KeyValuePair<string, List<KeyValuePair<int, string>>> NowSheet = new KeyValuePair<string, List<KeyValuePair<int, string>>>();

        private string FilePath { get { return _filePath; }
            set
            {
                _filePath = value;
                tbPath.Invoke(new MethodInvoker(delegate { tbPath.Text = _filePath; }));
                /// 해당 파일 패스 DB에 저장
                /// 
                _excelApp = new Excel.Application();
                Excel.Workbook workbook = _excelApp.Workbooks.Open(_filePath);

                foreach (Excel.Worksheet worksheet in workbook.Sheets.Cast<Excel.Worksheet>())
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
                    FilePath = filePath;
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
            lvData.Items.Clear();

            foreach(var vv in NowSheet.Value)
            {
                ListViewItem _item = new ListViewItem(new string[] { vv.Value, "" });

                lvData.Items.Add(_item);
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
    }
}
