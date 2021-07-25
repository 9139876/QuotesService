
namespace QuotesService.GUI.Forms
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Cmb_QuotesProviders = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_AddTicker = new System.Windows.Forms.Button();
            this.Btn_AddMarket = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Cmb_QuotesProviders
            // 
            this.Cmb_QuotesProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_QuotesProviders.Location = new System.Drawing.Point(12, 31);
            this.Cmb_QuotesProviders.Name = "Cmb_QuotesProviders";
            this.Cmb_QuotesProviders.Size = new System.Drawing.Size(150, 23);
            this.Cmb_QuotesProviders.TabIndex = 0;
            this.Cmb_QuotesProviders.SelectedIndexChanged += new System.EventHandler(this.Cmb_QuotesProviders_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Поставщик котировок";
            // 
            // Btn_AddTicker
            // 
            this.Btn_AddTicker.Location = new System.Drawing.Point(186, 30);
            this.Btn_AddTicker.Name = "Btn_AddTicker";
            this.Btn_AddTicker.Size = new System.Drawing.Size(120, 23);
            this.Btn_AddTicker.TabIndex = 2;
            this.Btn_AddTicker.Text = "Добавить тикер";
            this.Btn_AddTicker.UseVisualStyleBackColor = true;
            this.Btn_AddTicker.Visible = false;
            this.Btn_AddTicker.Click += new System.EventHandler(this.Btn_AddTicker_Click);
            // 
            // Btn_AddMarket
            // 
            this.Btn_AddMarket.Location = new System.Drawing.Point(312, 30);
            this.Btn_AddMarket.Name = "Btn_AddMarket";
            this.Btn_AddMarket.Size = new System.Drawing.Size(120, 23);
            this.Btn_AddMarket.TabIndex = 3;
            this.Btn_AddMarket.Text = "Добавить рынок";
            this.Btn_AddMarket.UseVisualStyleBackColor = true;
            this.Btn_AddMarket.Visible = false;
            this.Btn_AddMarket.Click += new System.EventHandler(this.Btn_AddMarket_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Btn_AddMarket);
            this.Controls.Add(this.Btn_AddTicker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cmb_QuotesProviders);
            this.Name = "MainWindow";
            this.Text = "Менеджер котировок";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Cmb_QuotesProviders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_AddTicker;
        private System.Windows.Forms.Button Btn_AddMarket;
    }
}

