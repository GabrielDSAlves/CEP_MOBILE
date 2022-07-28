using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {   
                if (string.IsNullOrWhiteSpace(txtCEP.Text))
                    return;

                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync($"https://viacep.com.br/ws/{txtCEP.Text}/json/"))
                    {
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(content))
                            throw new InvalidOperationException();

                        var retorno = Newtonsoft.Json.JsonConvert.DeserializeObject<ViaCepDto>(content); 

                        if (retorno.erro)
                            throw new InvalidOperationException();

                        txtLogradouro.Text = retorno.logradouro;
                        txtComplemento.Text = retorno.logradouro;
                        txtBairro.Text = retorno.bairro;
                        txtLocalidade.Text = retorno.localidade;
                        txtUF.Text = retorno.uf;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("OOPS", "Houve um erro!", "OK");
            }
        }
    }



    public class ViaCepDto
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string ddd { get; set; }
        public string siafi { get; set; }
        public bool erro { get; set; } = false;
    }

}
