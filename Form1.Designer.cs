
namespace ToastNotification
{
    partial class Form1
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
            this.cbSheets = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lvData = new System.Windows.Forms.ListView();
            this.chDataName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlFilePath.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPath
            // 
            this.btnPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPath.Location = new System.Drawing.Point(745, 10);
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
            this.tbPath.Size = new System.Drawing.Size(724, 21);
            this.tbPath.TabIndex = 1;
            // 
            // pnlFilePath
            // 
            this.pnlFilePath.Controls.Add(this.tbPath);
            this.pnlFilePath.Controls.Add(this.btnPath);
            this.pnlFilePath.Location = new System.Drawing.Point(0, 0);
            this.pnlFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFilePath.Name = "pnlFilePath";
            this.pnlFilePath.Size = new System.Drawing.Size(832, 46);
            this.pnlFilePath.TabIndex = 2;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lvData);
            this.pnlMain.Controls.Add(this.btnSave);
            this.pnlMain.Controls.Add(this.cbSheets);
            this.pnlMain.Controls.Add(this.pnlFilePath);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(832, 476);
            this.pnlMain.TabIndex = 3;
            // 
            // cbSheets
            // 
            this.cbSheets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSheets.FormattingEnabled = true;
            this.cbSheets.Location = new System.Drawing.Point(70, 62);
            this.cbSheets.Name = "cbSheets";
            this.cbSheets.Size = new System.Drawing.Size(240, 20);
            this.cbSheets.TabIndex = 3;
            this.cbSheets.SelectedIndexChanged += new System.EventHandler(this.cbSheets_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(745, 441);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // lvData
            // 
            this.lvData.CheckBoxes = true;
            this.lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDataName,
            this.chType});
            this.lvData.HideSelection = false;
            this.lvData.Location = new System.Drawing.Point(70, 88);
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(750, 347);
            this.lvData.TabIndex = 5;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            // 
            // chDataName
            // 
            this.chDataName.Text = "항목 명";
            this.chDataName.Width = 272;
            // 
            // chType
            // 
            this.chType.Text = "항목 형식";
            this.chType.Width = 439;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 476);
            this.Controls.Add(this.pnlMain);
            this.Name = "Form1";
            this.Text = "Form1";
            this.pnlFilePath.ResumeLayout(false);
            this.pnlFilePath.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Panel pnlFilePath;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ComboBox cbSheets;
        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.ColumnHeader chDataName;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.Button btnSave;
    }
}

