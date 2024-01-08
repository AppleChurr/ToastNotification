
namespace ToastNotification
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            Notifycation.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPath = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.pnlFilePath = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.Partition = new System.Windows.Forms.Label();
            this.lbIncludeData = new System.Windows.Forms.Label();
            this.lbRef = new System.Windows.Forms.Label();
            this.lbSheets = new System.Windows.Forms.Label();
            this.lvIncludeData = new System.Windows.Forms.ListView();
            this.chSelectItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvRefData = new System.Windows.Forms.ListView();
            this.chDataName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSave = new System.Windows.Forms.Button();
            this.cbSheets = new System.Windows.Forms.ComboBox();
            this.pnlFilePath.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPath
            // 
            this.btnPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPath.Location = new System.Drawing.Point(386, 10);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(75, 23);
            this.btnPath.TabIndex = 0;
            this.btnPath.Text = "파일 선택";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.Location = new System.Drawing.Point(12, 12);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(370, 21);
            this.tbPath.TabIndex = 1;
            // 
            // pnlFilePath
            // 
            this.pnlFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilePath.Controls.Add(this.tbPath);
            this.pnlFilePath.Controls.Add(this.btnPath);
            this.pnlFilePath.Location = new System.Drawing.Point(0, 0);
            this.pnlFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFilePath.Name = "pnlFilePath";
            this.pnlFilePath.Size = new System.Drawing.Size(473, 46);
            this.pnlFilePath.TabIndex = 2;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.Partition);
            this.pnlMain.Controls.Add(this.lbIncludeData);
            this.pnlMain.Controls.Add(this.lbRef);
            this.pnlMain.Controls.Add(this.lbSheets);
            this.pnlMain.Controls.Add(this.lvIncludeData);
            this.pnlMain.Controls.Add(this.lvRefData);
            this.pnlMain.Controls.Add(this.btnSave);
            this.pnlMain.Controls.Add(this.cbSheets);
            this.pnlMain.Controls.Add(this.pnlFilePath);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(473, 477);
            this.pnlMain.TabIndex = 3;
            // 
            // Partition
            // 
            this.Partition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Partition.AutoSize = true;
            this.Partition.BackColor = System.Drawing.Color.Black;
            this.Partition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Partition.Location = new System.Drawing.Point(236, 92);
            this.Partition.MaximumSize = new System.Drawing.Size(1, 350);
            this.Partition.MinimumSize = new System.Drawing.Size(1, 350);
            this.Partition.Name = "Partition";
            this.Partition.Size = new System.Drawing.Size(1, 350);
            this.Partition.TabIndex = 10;
            this.Partition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbIncludeData
            // 
            this.lbIncludeData.AutoSize = true;
            this.lbIncludeData.Location = new System.Drawing.Point(241, 77);
            this.lbIncludeData.Name = "lbIncludeData";
            this.lbIncludeData.Size = new System.Drawing.Size(101, 12);
            this.lbIncludeData.TabIndex = 8;
            this.lbIncludeData.Text = "알람 시 포함 항목";
            // 
            // lbRef
            // 
            this.lbRef.AutoSize = true;
            this.lbRef.Location = new System.Drawing.Point(10, 77);
            this.lbRef.Name = "lbRef";
            this.lbRef.Size = new System.Drawing.Size(113, 12);
            this.lbRef.TabIndex = 7;
            this.lbRef.Text = "기준 날짜 항목 선택";
            // 
            // lbSheets
            // 
            this.lbSheets.AutoSize = true;
            this.lbSheets.Location = new System.Drawing.Point(10, 55);
            this.lbSheets.Name = "lbSheets";
            this.lbSheets.Size = new System.Drawing.Size(65, 12);
            this.lbSheets.TabIndex = 6;
            this.lbSheets.Tag = "";
            this.lbSheets.Text = "시트 선택 :";
            // 
            // lvIncludeData
            // 
            this.lvIncludeData.CheckBoxes = true;
            this.lvIncludeData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSelectItem});
            this.lvIncludeData.HideSelection = false;
            this.lvIncludeData.Location = new System.Drawing.Point(243, 92);
            this.lvIncludeData.Name = "lvIncludeData";
            this.lvIncludeData.Size = new System.Drawing.Size(218, 350);
            this.lvIncludeData.TabIndex = 5;
            this.lvIncludeData.UseCompatibleStateImageBehavior = false;
            this.lvIncludeData.View = System.Windows.Forms.View.Details;
            // 
            // chSelectItem
            // 
            this.chSelectItem.Text = "항목 명";
            this.chSelectItem.Width = 190;
            // 
            // lvRefData
            // 
            this.lvRefData.CheckBoxes = true;
            this.lvRefData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDataName});
            this.lvRefData.HideSelection = false;
            this.lvRefData.Location = new System.Drawing.Point(12, 92);
            this.lvRefData.Name = "lvRefData";
            this.lvRefData.Size = new System.Drawing.Size(218, 350);
            this.lvRefData.TabIndex = 5;
            this.lvRefData.UseCompatibleStateImageBehavior = false;
            this.lvRefData.View = System.Windows.Forms.View.Details;
            // 
            // chDataName
            // 
            this.chDataName.Text = "항목 명";
            this.chDataName.Width = 190;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(386, 448);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbSheets
            // 
            this.cbSheets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSheets.FormattingEnabled = true;
            this.cbSheets.Location = new System.Drawing.Point(81, 52);
            this.cbSheets.Name = "cbSheets";
            this.cbSheets.Size = new System.Drawing.Size(283, 20);
            this.cbSheets.TabIndex = 3;
            this.cbSheets.SelectedIndexChanged += new System.EventHandler(this.cbSheets_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 477);
            this.Controls.Add(this.pnlMain);
            this.MaximumSize = new System.Drawing.Size(489, 516);
            this.MinimumSize = new System.Drawing.Size(489, 516);
            this.Name = "MainForm";
            this.Text = "ToastNotification";
            this.pnlFilePath.ResumeLayout(false);
            this.pnlFilePath.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Panel pnlFilePath;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ComboBox cbSheets;
        private System.Windows.Forms.ListView lvRefData;
        private System.Windows.Forms.ColumnHeader chDataName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lbSheets;
        private System.Windows.Forms.Label lbIncludeData;
        private System.Windows.Forms.Label lbRef;
        private System.Windows.Forms.ListView lvIncludeData;
        private System.Windows.Forms.ColumnHeader chSelectItem;
        private System.Windows.Forms.Label Partition;
    }
}

