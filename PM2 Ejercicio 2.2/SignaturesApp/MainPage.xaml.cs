using SignaturePad.Forms;
using SignaturesApp.Model;
using SignaturesApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SignaturesApp
{
    public partial class MainPage : ContentPage
    {
        string valueBase64;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            Stream image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);
            var mStream = (MemoryStream)image;
            byte[] data = mStream.ToArray();
            valueBase64 = Convert.ToBase64String(data);


            if(String.IsNullOrWhiteSpace(name.Text) || String.IsNullOrWhiteSpace(description.Text))
            {
                await DisplayAlert("ERROR", "Tiene que llenar todos los campos.", "Aceptar");
            }

            var signatureToSave = new Signatures
            {
                imageCode = valueBase64,
                name = name.Text,
                description = description.Text
            };

            var result = await App.BaseDatos.saveSignature(signatureToSave);

            if(result != 1)
            {
                await DisplayAlert("ERROR", "Ha occurido un Error, Vuelve a intentarlo.", "Aceptar");
            }

            await DisplayAlert("AVISO", "Se ha guardado con Exito.", "Aceptar");
        }
            
        private async void ViewSignaturesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignaturesList());
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            PadView.Clear();
            name.Text = "";
            description.Text = "";
        }
    }
}
