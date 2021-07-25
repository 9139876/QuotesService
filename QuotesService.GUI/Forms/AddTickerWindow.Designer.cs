
namespace QuotesService.GUI.Forms
{
    partial class AddTickerWindow
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
            this.Tb_Symbol = new System.Windows.Forms.TextBox();
            this.Lbl_Symbol = new System.Windows.Forms.Label();
            this.Lbl_Name = new System.Windows.Forms.Label();
            this.Tb_Name = new System.Windows.Forms.TextBox();
            this.Lbl_TickerType = new System.Windows.Forms.Label();
            this.Cmb_TickerType = new System.Windows.Forms.ComboBox();
            this.Dtp_StartDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Tb_Currency = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Tb_VolumeCode = new System.Windows.Forms.TextBox();
            this.Btn_CheckConfirm = new System.Windows.Forms.Button();
            this.Lbl_QuotesProviderName = new System.Windows.Forms.Label();
            this.Lbl_Market = new System.Windows.Forms.Label();
            this.Cmb_Markets = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Tb_Symbol
            // 
            this.Tb_Symbol.Location = new System.Drawing.Point(11, 154);
            this.Tb_Symbol.Name = "Tb_Symbol";
            this.Tb_Symbol.Size = new System.Drawing.Size(201, 23);
            this.Tb_Symbol.TabIndex = 0;
            this.Tb_Symbol.TextChanged += new System.EventHandler(this.OnChangeValues);
            // 
            // Lbl_Symbol
            // 
            this.Lbl_Symbol.AutoSize = true;
            this.Lbl_Symbol.Location = new System.Drawing.Point(11, 136);
            this.Lbl_Symbol.Name = "Lbl_Symbol";
            this.Lbl_Symbol.Size = new System.Drawing.Size(47, 15);
            this.Lbl_Symbol.TabIndex = 1;
            this.Lbl_Symbol.Text = "Symbol";
            // 
            // Lbl_Name
            // 
            this.Lbl_Name.AutoSize = true;
            this.Lbl_Name.Location = new System.Drawing.Point(11, 180);
            this.Lbl_Name.Name = "Lbl_Name";
            this.Lbl_Name.Size = new System.Drawing.Size(59, 15);
            this.Lbl_Name.TabIndex = 3;
            this.Lbl_Name.Text = "Название";
            // 
            // Tb_Name
            // 
            this.Tb_Name.Location = new System.Drawing.Point(11, 198);
            this.Tb_Name.Name = "Tb_Name";
            this.Tb_Name.Size = new System.Drawing.Size(201, 23);
            this.Tb_Name.TabIndex = 2;
            this.Tb_Name.TextChanged += new System.EventHandler(this.OnChangeValues);
            // 
            // Lbl_TickerType
            // 
            this.Lbl_TickerType.AutoSize = true;
            this.Lbl_TickerType.Location = new System.Drawing.Point(12, 47);
            this.Lbl_TickerType.Name = "Lbl_TickerType";
            this.Lbl_TickerType.Size = new System.Drawing.Size(28, 15);
            this.Lbl_TickerType.TabIndex = 4;
            this.Lbl_TickerType.Text = "Тип";
            // 
            // Cmb_TickerType
            // 
            this.Cmb_TickerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_TickerType.FormattingEnabled = true;
            this.Cmb_TickerType.Location = new System.Drawing.Point(12, 66);
            this.Cmb_TickerType.Name = "Cmb_TickerType";
            this.Cmb_TickerType.Size = new System.Drawing.Size(200, 23);
            this.Cmb_TickerType.TabIndex = 5;
            this.Cmb_TickerType.SelectedIndexChanged += new System.EventHandler(this.OnChangeValues);
            // 
            // Dtp_StartDate
            // 
            this.Dtp_StartDate.Location = new System.Drawing.Point(11, 242);
            this.Dtp_StartDate.Name = "Dtp_StartDate";
            this.Dtp_StartDate.Size = new System.Drawing.Size(200, 23);
            this.Dtp_StartDate.TabIndex = 6;
            this.Dtp_StartDate.ValueChanged += new System.EventHandler(this.OnChangeValues);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Начальная дата";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 268);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Валюта";
            // 
            // Tb_Currency
            // 
            this.Tb_Currency.Location = new System.Drawing.Point(11, 286);
            this.Tb_Currency.Name = "Tb_Currency";
            this.Tb_Currency.Size = new System.Drawing.Size(201, 23);
            this.Tb_Currency.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Единица измерения";
            // 
            // Tb_VolumeCode
            // 
            this.Tb_VolumeCode.Location = new System.Drawing.Point(11, 330);
            this.Tb_VolumeCode.Name = "Tb_VolumeCode";
            this.Tb_VolumeCode.Size = new System.Drawing.Size(201, 23);
            this.Tb_VolumeCode.TabIndex = 10;
            // 
            // Btn_CheckConfirm
            // 
            this.Btn_CheckConfirm.Location = new System.Drawing.Point(10, 375);
            this.Btn_CheckConfirm.Name = "Btn_CheckConfirm";
            this.Btn_CheckConfirm.Size = new System.Drawing.Size(201, 23);
            this.Btn_CheckConfirm.TabIndex = 12;
            this.Btn_CheckConfirm.Text = "Проверка";
            this.Btn_CheckConfirm.UseVisualStyleBackColor = true;
            this.Btn_CheckConfirm.Click += new System.EventHandler(this.Btn_CheckConfirm_Click);
            // 
            // Lbl_QuotesProviderName
            // 
            this.Lbl_QuotesProviderName.AutoSize = true;
            this.Lbl_QuotesProviderName.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.Lbl_QuotesProviderName.Location = new System.Drawing.Point(12, 13);
            this.Lbl_QuotesProviderName.Name = "Lbl_QuotesProviderName";
            this.Lbl_QuotesProviderName.Size = new System.Drawing.Size(60, 22);
            this.Lbl_QuotesProviderName.TabIndex = 13;
            this.Lbl_QuotesProviderName.Text = "label7";
            // 
            // Lbl_Market
            // 
            this.Lbl_Market.AutoSize = true;
            this.Lbl_Market.Location = new System.Drawing.Point(11, 92);
            this.Lbl_Market.Name = "Lbl_Market";
            this.Lbl_Market.Size = new System.Drawing.Size(43, 15);
            this.Lbl_Market.TabIndex = 15;
            this.Lbl_Market.Text = "Рынок";
            // 
            // Cmb_Markets
            // 
            this.Cmb_Markets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Markets.FormattingEnabled = true;
            this.Cmb_Markets.Location = new System.Drawing.Point(11, 110);
            this.Cmb_Markets.Name = "Cmb_Markets";
            this.Cmb_Markets.Size = new System.Drawing.Size(200, 23);
            this.Cmb_Markets.TabIndex = 16;
            // 
            // AddTickerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 411);
            this.Controls.Add(this.Cmb_Markets);
            this.Controls.Add(this.Lbl_Market);
            this.Controls.Add(this.Lbl_QuotesProviderName);
            this.Controls.Add(this.Btn_CheckConfirm);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Tb_VolumeCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Tb_Currency);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Dtp_StartDate);
            this.Controls.Add(this.Cmb_TickerType);
            this.Controls.Add(this.Lbl_TickerType);
            this.Controls.Add(this.Lbl_Name);
            this.Controls.Add(this.Tb_Name);
            this.Controls.Add(this.Lbl_Symbol);
            this.Controls.Add(this.Tb_Symbol);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddTickerWindow";
            this.Text = "Добавление тикера";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Tb_Symbol;
        private System.Windows.Forms.Label Lbl_Symbol;
        private System.Windows.Forms.Label Lbl_Name;
        private System.Windows.Forms.TextBox Tb_Name;
        private System.Windows.Forms.Label Lbl_TickerType;
        private System.Windows.Forms.ComboBox Cmb_TickerType;
        private System.Windows.Forms.DateTimePicker Dtp_StartDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Tb_Currency;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Tb_VolumeCode;
        private System.Windows.Forms.Button Btn_;
        private System.Windows.Forms.Label Lbl_QuotesProviderName;
        private System.Windows.Forms.Button Btn_CheckConfirm;
        private System.Windows.Forms.Label Lbl_Market;
        private System.Windows.Forms.ComboBox Cmb_Markets;
    }
}