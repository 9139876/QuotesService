using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLibraries.ClientApplication;
using CommonLibraries.Core.Extensions;
using Newtonsoft.Json;
using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.Api.Services;

namespace QuotesService.GUI.Forms
{
    public partial class AddTickerWindow : Form
    {
        private readonly QuotesProviderEnum _quotesProviderType;
        private readonly List<Control> _validatedControls;
        private readonly IGetDataRemoteCallService _getDataRemoteCallService;

        private bool _checked;

        private bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                Btn_CheckConfirm.Text = _checked ? "Добавить" : "Проверить";
            }
        }

        public AddTickerWindow(QuotesProviderEnum quotesProviderType, List<string> marketsNames)
        {
            _quotesProviderType = quotesProviderType;
            _getDataRemoteCallService = ServicesFactory.GetInstance<IGetDataRemoteCallService>();

            InitializeComponent();

            Lbl_QuotesProviderName.Text = _quotesProviderType.ToString();
            Cmb_TickerType.Items.AddRange(Description.GetAllDescptions<TickerTypeEnum>());
            Cmb_Markets.Items.AddRange(marketsNames.ToArray());

            _validatedControls = new List<Control>()
            {
                Lbl_TickerType,
                Lbl_Market,
                Lbl_Symbol,
                Lbl_Name
            };

            Checked = false;
        }

        private async void Btn_CheckConfirm_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            if (Checked)
            {
                await Add();
            }
            else
            {
                await Check();
            }
        }

        private void OnChangeValues(object sender, EventArgs e)
        {
            Checked = false;
        }

        private bool ValidateInputs()
        {
            _validatedControls.ForEach(x => x.ForeColor = SystemColors.ControlText);

            var result = true;

            if (Cmb_TickerType.SelectedItem == null)
            {
                Lbl_TickerType.ForeColor = Color.Red;
                result = false;
            }

            if (Cmb_Markets.SelectedItem == null)
            {
                Lbl_Market.ForeColor = Color.Red;
                result = false;
            }

            if (string.IsNullOrWhiteSpace(Tb_Symbol.Text))
            {
                Lbl_Symbol.ForeColor = Color.Red;
                result = false;
            }

            if (string.IsNullOrWhiteSpace(Tb_Name.Text))
            {
                Lbl_Name.ForeColor = Color.Red;
                result = false;
            }

            return result;
        }

        private async Task Check()
        {
            Tb_Symbol.Text = Tb_Symbol.Text.ToUpper();
            Tb_Currency.Text = Tb_Currency.Text.ToUpper();

            var request = new CheckGetQuotesRequest()
            {
                GUID = Guid.NewGuid(),
                StartDate = Dtp_StartDate.Value,
                QuotesProvider = _quotesProviderType,
                Symbol = Tb_Symbol.Text
            };

            Btn_CheckConfirm.Text = "Wait...";
            Btn_CheckConfirm.Enabled = false;

            try
            {
                var result = await _getDataRemoteCallService.CheckGetQuotes(request);

                if (result.IsSuccess)
                {
                    Dtp_StartDate.Value = result.StartDate;
                    MessageBox.Show("Успешно");
                    Checked = true;
                }
                else
                {
                    MessageBox.Show($"Ошибка - {result.ErrorMessage}");
                    Checked = false;
                }
            }
            catch (Exception e)
            {
                Program.ShowErrorMessage(e);
                Checked = false;
            }
            finally
            {
                Btn_CheckConfirm.Enabled = true;
            }
        }

        private async Task Add()
        {

        }
    }
}
