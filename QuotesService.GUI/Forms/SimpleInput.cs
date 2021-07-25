using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuotesService.GUI.Forms
{
    public partial class SimpleInput : Form
    {
        private readonly Action<string> _callBackAction;
        private readonly bool _valueMayBeEmpty;

        public SimpleInput(string title, string hint, Action<string> callBackAction, bool valueMayBeEmpty = false)
        {
            _callBackAction = callBackAction;
            _valueMayBeEmpty = valueMayBeEmpty;
            InitializeComponent();

            this.Text = title;
            Lb_Label.Text = hint;
        }

        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            if (_valueMayBeEmpty == false && string.IsNullOrWhiteSpace(Tb_Input.Text))
            {
                Lb_Label.ForeColor = Color.Red;
                return;
            }

            _callBackAction.Invoke(Tb_Input.Text);
            this.Close();
        }
    }
}
