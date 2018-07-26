using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using TextTranslator.TranslationHubIntegration;
using TranslateApp.Settings;
using System.Collections.Generic;

namespace TextTranslator
{
    public partial class MainForm : Form
    {
        private List<TranslationSetting> _translationsettings;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnTranslate_Click(object sender, EventArgs e)
        {
            await Translate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _translationsettings = TranslationSetting.ReadFromConfig();
                foreach(var translationsetting in _translationsettings)
                {
                    cmbLanguage.Items.Add(translationsetting.From + " -> " + translationsetting.To);
                }
                cmbLanguage.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error occured: {0}", ex.Message));
            }
        }

        private void txtFrom_TextChanged(object sender, EventArgs e)
        {
            lblStatus.Text = "Changed";
            lblStatus.ForeColor = System.Drawing.Color.Red;
        }

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
            lblStatus.Text = "Changed";
            lblStatus.ForeColor = System.Drawing.Color.Red;
        }

        private async void txtFrom_KeyDown(object sender, KeyEventArgs e)
        {
            if(chkTranslateOnEnter.Checked)
            { 
                if (e.KeyValue == 13 && !(e.Shift || e.Alt || e.Control))
                {
                    await Translate();
                }
            }
        }

        private async Task<string> Translate()
        {
            try
            {
                var activeSetting = _translationsettings[cmbLanguage.SelectedIndex];
                txtTo.Text = await Translate(txtFrom.Text, activeSetting);
                lblStatus.Text = "Translated";
                lblStatus.ForeColor = System.Drawing.Color.Green;

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error occured: {0}", ex.Message));
            }

            return null;
        }


        private static async Task<string> Translate(string text, TranslationSetting activeSetting)
        {
            var authTokenSource = new AzureAuthToken(activeSetting.SubscriptionKey.Trim());
            string authToken;
            try
            {
                authToken = await authTokenSource.GetAccessTokenAsync();
            }
            catch (HttpRequestException)
            {
                if (authTokenSource.RequestStatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ApplicationException("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                }
                if (authTokenSource.RequestStatusCode == HttpStatusCode.Forbidden)
                {
                    throw new ApplicationException("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                }
                throw;
            }

            return await TranslateApi.TranslateAsync(authToken, text, activeSetting.From, activeSetting.To, activeSetting.Category);
        }
    }
}
