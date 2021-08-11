using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLibraries.ClientApplication;
using CommonLibraries.Core.Extensions;
using Microsoft.Extensions.Configuration;
using QuotesService.Api.Enum;
using QuotesService.Api.Models.RequestResponse;
using QuotesService.Api.Services;

namespace QuotesService.GUI.Forms
{
    public partial class MainWindow : Form
    {
        private readonly IGetDataRemoteCallService _getDataRemoteCallService;
        private readonly ISetDataRemoteCallService _setDataRemoteCallService;
        private readonly IConfiguration _configuration;

        public MainWindow()
        {
            InitializeComponent();

            _configuration = ServicesFactory.GetInstance<IConfiguration>();
            _getDataRemoteCallService = ServicesFactory.GetInstance<IGetDataRemoteCallService>();
            _setDataRemoteCallService = ServicesFactory.GetInstance<ISetDataRemoteCallService>();

            this.Text += $" (Server: {_configuration.GetValue<string>("QuotesService.Api.SchemeAndHost")})";

            Cmb_QuotesProviders.Items.AddRange(Enum.GetNames(typeof(QuotesProviderEnum)));
        }

        private void Cmb_QuotesProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_QuotesProviders.SelectedItem != null)
            {
                Btn_AddTicker.Visible = true;
                Btn_AddMarket.Visible = true;
            }
            else
            {
                Btn_AddTicker.Visible = false;
                Btn_AddMarket.Visible = false;
            }
        }

        private async void Btn_AddTicker_Click(object sender, EventArgs e)
        {
            var quotesProviderType = (QuotesProviderEnum)Enum.Parse(typeof(QuotesProviderEnum), Cmb_QuotesProviders.SelectedItem.ToString());

            var markets = await _getDataRemoteCallService.GetAllMarketsNames();
            markets.RequiredNotNull(nameof(markets));

            new AddTickerWindow(quotesProviderType, markets).ShowDialog();
        }

        private async void Btn_AddMarket_Click(object sender, EventArgs e)
        {
            string marketName = null;

            var input = new SimpleInput("Добавление рынка", "Название рынка", ((x) => marketName = x));
            input.ShowDialog();

            if (string.IsNullOrWhiteSpace(marketName) == false)
            {
                var addMarketRequest = new AddMarketRequest()
                {
                    MarketName = marketName
                };

                var addMarketResult = await _setDataRemoteCallService.AddMarket(addMarketRequest);
                addMarketResult.RequiredNotNull(nameof(addMarketResult), addMarketRequest);

                if (addMarketResult.IsSuccess)
                {
                    MessageBox.Show($"Рынок {marketName} успешно добавлен");
                }
                else
                {
                    MessageBox.Show(addMarketResult.Message);
                }
            }
        }
    }
}
