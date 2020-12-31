namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    partial class UC_FRAME_DONGBO
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnFilter = new DevExpress.XtraEditors.SimpleButton();
            this.dateTimeTuNgay = new System.Windows.Forms.DateTimePicker();
            this.dateTimeDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnDongBo = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblNgayPhatSinh = new DevExpress.XtraEditors.LabelControl();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.dgvLstPhieuMua = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAPHIEU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGUOIBAN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THANHTIEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUONG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnFilterXml = new DevExpress.XtraEditors.SimpleButton();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLstPhieuMua)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnFilterXml);
            this.pnlHeader.Controls.Add(this.btnFilter);
            this.pnlHeader.Controls.Add(this.dateTimeTuNgay);
            this.pnlHeader.Controls.Add(this.dateTimeDenNgay);
            this.pnlHeader.Controls.Add(this.btnDongBo);
            this.pnlHeader.Controls.Add(this.labelControl1);
            this.pnlHeader.Controls.Add(this.lblNgayPhatSinh);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1519, 77);
            this.pnlHeader.TabIndex = 0;
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
            this.btnDongBo.Location = new System.Drawing.Point(714, 24);
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
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.dgvLstPhieuMua);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 77);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1519, 489);
            this.pnlContent.TabIndex = 1;
            // 
            // dgvLstPhieuMua
            // 
            this.dgvLstPhieuMua.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLstPhieuMua.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.MAPHIEU,
            this.NGUOIBAN,
            this.THANHTIEN,
            this.SOLUONG});
            this.dgvLstPhieuMua.Location = new System.Drawing.Point(0, 0);
            this.dgvLstPhieuMua.Name = "dgvLstPhieuMua";
            this.dgvLstPhieuMua.Size = new System.Drawing.Size(1519, 383);
            this.dgvLstPhieuMua.TabIndex = 0;
            // 
            // STT
            // 
            this.STT.Frozen = true;
            this.STT.HeaderText = "STT";
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            // 
            // MAPHIEU
            // 
            this.MAPHIEU.Frozen = true;
            this.MAPHIEU.HeaderText = "Mã phiếu";
            this.MAPHIEU.Name = "MAPHIEU";
            this.MAPHIEU.ReadOnly = true;
            // 
            // NGUOIBAN
            // 
            this.NGUOIBAN.Frozen = true;
            this.NGUOIBAN.HeaderText = "Người bán";
            this.NGUOIBAN.Name = "NGUOIBAN";
            this.NGUOIBAN.ReadOnly = true;
            // 
            // THANHTIEN
            // 
            this.THANHTIEN.Frozen = true;
            this.THANHTIEN.HeaderText = "Thành Tiền";
            this.THANHTIEN.Name = "THANHTIEN";
            this.THANHTIEN.ReadOnly = true;
            // 
            // SOLUONG
            // 
            this.SOLUONG.Frozen = true;
            this.SOLUONG.HeaderText = "Tổng số lượng";
            this.SOLUONG.Name = "SOLUONG";
            this.SOLUONG.ReadOnly = true;
            // 
            // btnFilterXml
            // 
            this.btnFilterXml.Appearance.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilterXml.Appearance.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnFilterXml.Appearance.Options.UseFont = true;
            this.btnFilterXml.Appearance.Options.UseForeColor = true;
            this.btnFilterXml.Location = new System.Drawing.Point(824, 24);
            this.btnFilterXml.Name = "btnFilterXml";
            this.btnFilterXml.Size = new System.Drawing.Size(104, 31);
            this.btnFilterXml.TabIndex = 52;
            this.btnFilterXml.Text = "Lọc XML";
            this.btnFilterXml.Click += new System.EventHandler(this.btnFilterXml_Click);
            // 
            // UC_FRAME_DONGBO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "UC_FRAME_DONGBO";
            this.Size = new System.Drawing.Size(1519, 685);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLstPhieuMua)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.DateTimePicker dateTimeTuNgay;
        private System.Windows.Forms.DateTimePicker dateTimeDenNgay;
        private DevExpress.XtraEditors.SimpleButton btnDongBo;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblNgayPhatSinh;
        private DevExpress.XtraEditors.SimpleButton btnFilter;
        private System.Windows.Forms.DataGridView dgvLstPhieuMua;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAPHIEU;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGUOIBAN;
        private System.Windows.Forms.DataGridViewTextBoxColumn THANHTIEN;
        private System.Windows.Forms.DataGridViewTextBoxColumn SOLUONG;
        private DevExpress.XtraEditors.SimpleButton btnFilterXml;
    }
}
