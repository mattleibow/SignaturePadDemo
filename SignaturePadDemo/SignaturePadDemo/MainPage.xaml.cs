using System;
using Xamarin.Forms;
using SignaturePad.Forms;
using PCLStorage;

namespace SignaturePadDemo
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private async void OnShowImage(object sender, EventArgs e)
		{
			// get the file name
			var folder = FileSystem.Current.LocalStorage;
			var file = await folder.CreateFileAsync($"signature.jpg", CreationCollisionOption.GenerateUniqueName);

			// create some settings to control the output
			var settings = new ImageConstructionSettings
			{
				BackgroundColor = Color.White,
				StrokeColor = Color.Black,
			};

			// write the signature stream to the file stream
			using (var stream = await signaturePad.GetImageStreamAsync(SignatureImageFormat.Jpeg, settings))
			using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
			{
				// copy the bytes
				await stream.CopyToAsync(fileStream);
			}

			// success
			await DisplayAlert("Signature Saved", "Signature file saved successfully.\nLoading preview...", "OK");

			// show the image preview
			var previewPage = new ContentPage
			{
				Title = "View Signature",
				Content = new Image
				{
					Source = ImageSource.FromFile(file.Path),
					Margin = new Thickness(20)
				}
			};
			await Navigation.PushAsync(previewPage);
		}
	}
}
