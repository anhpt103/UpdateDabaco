namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    partial class frmDongBo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnFilter = new DevExpress.XtraEditors.SimpleButton();
            this.dateTimeTuNgay = new System.Windows.Forms.DateTimePicker();
            this.dateTimeDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnDongBo = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblNgayPhatSinh = new DevExpress.XtraEditors.LabelControl();
            this.grbServer = new System.Windows.Forms.GroupBox();
            this.grbLocal = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHoaDonSV = new System.Windows.Forms.Label();
            this.lblMatHangSV = new System.Windows.Forms.Label();
            this.lblMatHangMT = new System.Windows.Forms.Label();
            this.lblHoaDonMT = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlContent.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.grbServer.SuspendLayout();
            this.grbLocal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.grbLocal);
            this.pnlContent.Controls.Add(this.grbServer);
            this.pnlContent.Controls.Add(this.pnlHeader);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1287, 598);
            this.pnlContent.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnFilter);
            this.pnlHeader.Controls.Add(this.dateTimeTuNgay);
            this.pnlHeader.Controls.Add(this.dateTimeDenNgay);
            this.pnlHeader.Controls.Add(this.btnDongBo);
            this.pnlHeader.Controls.Add(this.labelControl1);
            this.pnlHeader.Controls.Add(this.lblNgayPhatSinh);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1287, 77);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnFilter
            // 
            this.btnFilter.Appearance.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Appearance.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnFilter.Appearance.Options.UseFont = true;
            this.btnFilter.Appearance.Options.UseForeColor = true;
            this.btnFilter.Location = new System.Drawing.Point(614, 24);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(104, 31);
            this.btnFilter.TabIndex = 51;
            this.btnFilter.Text = "Lọc";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // dateTimeTuNgay
            // 
            this.dateTimeTuNgay.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeTuNgay.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeTuNgay.Location = new System.Drawing.Point(117, 25);
            this.dateTimeTuNgay.Name = "dateTimeTuNgay";
            this.dateTimeTuNgay.Size = new System.Drawing.Size(181, 26);
            this.dateTimeTuNgay.TabIndex = 50;
            // 
            // dateTimeDenNgay
            // 
            this.dateTimeDenNgay.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeDenNgay.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeDenNgay.Location = new System.Drawing.Point(395, 26);
            this.dateTimeDenNgay.Name = "dateTimeDenNgay";
            this.dateTimeDenNgay.Size = new System.Drawing.Size(179, 26);
            this.dateTimeDenNgay.TabIndex = 49;
            // 
            // btnDongBo
            // 
            this.btnDongBo.Appearance.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDongBo.Appearance.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnDongBo.Appearance.Options.UseFont = true;
            this.btnDongBo.Appearance.Options.UseForeColor = true;
            this.btnDongBo.Location = new System.Drawing.Point(719, 24);
            this.btnDongBo.Name = "btnDongBo";
            this.btnDongBo.Size = new System.Drawing.Size(104, 31);
            this.btnDongBo.TabIndex = 47;
            this.btnDongBo.Text = "Đồng bộ";
            this.btnDongBo.Click += new System.EventHandler(this.btnDongBo_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(308, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 22);
            this.labelControl1.TabIndex = 46;
            this.labelControl1.Text = "Đến ngày";
            // 
            // lblNgayPhatSinh
            // 
            this.lblNgayPhatSinh.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayPhatSinh.Appearance.Options.UseFont = true;
            this.lblNgayPhatSinh.Location = new System.Drawing.Point(40, 28);
            this.lblNgayPhatSinh.Name = "lblNgayPhatSinh";
            this.lblNgayPhatSinh.Size = new System.Drawing.Size(70, 22);
            this.lblNgayPhatSinh.TabIndex = 44;
            this.lblNgayPhatSinh.Text = "Từ ngày";
            // 
            // grbServer
            // 
            this.grbServer.Controls.Add(this.lblMatHangSV);
            this.grbServer.Controls.Add(this.lblHoaDonSV);
            this.grbServer.Controls.Add(this.label2);
            this.grbServer.Controls.Add(this.label1);
            this.grbServer.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbServer.Location = new System.Drawing.Point(0, 77);
            this.grbServer.Name = "grbServer";
            this.grbServer.Size = new System.Drawing.Size(639, 521);
            this.grbServer.TabIndex = 2;
            this.grbServer.TabStop = false;
            this.grbServer.Text = "Server";
            // 
            // grbLocal
            // 
            this.grbLocal.Controls.Add(this.lblMatHangMT);
            this.grbLocal.Controls.Add(this.lblHoaDonMT);
            this.grbLocal.Controls.Add(this.label5);
            this.grbLocal.Controls.Add(this.label6);
            this.grbLocal.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbLocal.Location = new System.Drawing.Point(639, 77);
            this.grbLocal.Name = "grbLocal";
            this.grbLocal.Size = new System.Drawing.Size(645, 521);
            this.grbLocal.TabIndex = 3;
            this.grbLocal.TabStop = false;
            this.grbLocal.Text = "Máy trạm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(75, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Số hóa đơn : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(75, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Số  mặt hàng: ";
            // 
            // lblHoaDonSV
            // 
            this.lblHoaDonSV.AutoSize = true;
            this.lblHoaDonSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoaDonSV.Location = new System.Drawing.Point(264, 61);
            this.lblHoaDonSV.Name = "lblHoaDonSV";
            this.lblHoaDonSV.Size = new System.Drawing.Size(23, 25);
            this.lblHoaDonSV.TabIndex = 2;
            this.lblHoaDonSV.Text = "_";
            // 
            // lblMatHangSV
            // 
            this.lblMatHangSV.AutoSize = true;
            this.lblMatHangSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatHangSV.Location = new System.Drawing.Point(264, 117);
            this.lblMatHangSV.Name = "lblMatHangSV";
            this.lblMatHangSV.Size = new System.Drawing.Size(23, 25);
            this.lblMatHangSV.TabIndex = 3;
            this.lblMatHangSV.Text = "_";
            // 
            // lblMatHangMT
            // 
            this.lblMatHangMT.AutoSize = true;
            this.lblMatHangMT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatHangMT.Location = new System.Drawing.Point(244, 117);
            this.lblMatHangMT.Name = "lblMatHangMT";
            this.lblMatHangMT.Size = new System.Drawing.Size(23, 25);
            this.lblMatHangMT.TabIndex = 7;
            this.lblMatHangMT.Text = "_";
            // 
            // lblHoaDonMT
            // 
            this.lblHoaDonMT.AutoSize = true;
            this.lblHoaDonMT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoaDonMT.Location = new System.Drawing.Point(244, 61);
            this.lblHoaDonMT.Name = "lblHoaDonMT";
            this.lblHoaDonMT.Size = new System.Drawing.Size(23, 25);
            this.lblHoaDonMT.TabIndex = 6;
            this.lblHoaDonMT.Text = "_";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(55, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "Số  mặt hàng: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(55, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 25);
            this.label6.TabIndex = 4;
            this.label6.Text = "Số hóa đơn : ";
            // 
            // frmDongBo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 598);
            this.Controls.Add(this.pnlContent);
            this.Name = "frmDongBo";
            this.Text = "Đồng bộ";
            this.pnlContent.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grbServer.ResumeLayout(false);
            this.grbServer.PerformLayout();
            this.grbLocal.ResumeLayout(false);
            this.grbLocal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlHeader;
        private DevExpress.XtraEditors.SimpleButton btnFilter;
        private System.Windows.Forms.DateTimePicker dateTimeTuNgay;
        private System.Windows.Forms.DateTimePicker dateTimeDenNgay;
        private DevExpress.XtraEditors.SimpleButton btnDongBo;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblNgayPhatSinh;
        private System.Windows.Forms.GroupBox grbLocal;
        private System.Windows.Forms.GroupBox grbServer;
        private System.Windows.Forms.Label lblMatHangMT;
        private System.Windows.Forms.Label lblHoaDonMT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblMatHangSV;
        private System.Windows.Forms.Label lblHoaDonSV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}