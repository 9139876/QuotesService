
namespace QuotesService.GUI.Forms
{
    partial class SimpleInput
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
            this.Tb_Input = new System.Windows.Forms.TextBox();
            this.Lb_Label = new System.Windows.Forms.Label();
            this.Btn_Confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Tb_Input
            // 
            this.Tb_Input.Location = new System.Drawing.Point(12, 31);
            this.Tb_Input.Name = "Tb_Input";
            this.Tb_Input.Size = new System.Drawing.Size(175, 23);
            this.Tb_Input.TabIndex = 0;
            // 
            // Lb_Label
            // 
            this.Lb_Label.AutoSize = true;
            this.Lb_Label.Location = new System.Drawing.Point(13, 13);
            this.Lb_Label.Name = "Lb_Label";
            this.Lb_Label.Size = new System.Drawing.Size(38, 15);
            this.Lb_Label.TabIndex = 1;
            this.Lb_Label.Text = "label1";
            // 
            // Btn_Confirm
            // 
            this.Btn_Confirm.Location = new System.Drawing.Point(112, 60);
            this.Btn_Confirm.Name = "Btn_Confirm";
            this.Btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.Btn_Confirm.TabIndex = 2;
            this.Btn_Confirm.Text = "ОК";
            this.Btn_Confirm.UseVisualStyleBackColor = true;
            this.Btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // SimpleInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(196, 92);
            this.Controls.Add(this.Btn_Confirm);
            this.Controls.Add(this.Lb_Label);
            this.Controls.Add(this.Tb_Input);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleInput";
            this.Text = "SimpleInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Tb_Input;
        private System.Windows.Forms.Label Lb_Label;
        private System.Windows.Forms.Button Btn_Confirm;
    }
}