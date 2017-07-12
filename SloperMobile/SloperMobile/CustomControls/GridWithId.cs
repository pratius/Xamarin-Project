using Xamarin.Forms;

namespace SloperMobile.CustomControls
{
    public class GridWithId : Grid
    {
		string color;
		string routeCount;
		int labelTextFontSize;
        double width;
        double height;
        BoxView innerBoxView;
        BoxView mainBoxView;

        public GridWithId(long id, string color, string routeCount, int labelTextFontSize, double width, double height)
        {
            PointId = id;
			this.color = color;
			this.routeCount = routeCount;
			this.labelTextFontSize = labelTextFontSize;
            this.height = height;
            this.width = width;
            Padding = new Thickness(2);
            BackgroundColor = Color.White;
            Rotation = 45;
        }

        public long PointId { get; set; }

        public void ChangeBounds(double scaleFactor)
        {
            Padding = new Thickness(2 / scaleFactor);
            mainBoxView.WidthRequest = (width - 2) / scaleFactor;
            mainBoxView.VerticalOptions = LayoutOptions.CenterAndExpand;
            mainBoxView.HeightRequest = (height - 2) / scaleFactor;
            mainBoxView.HorizontalOptions = LayoutOptions.CenterAndExpand;
        }

		protected override void OnParentSet()
		{
			base.OnParentSet();
			

			mainBoxView = new BoxView
			{
				HeightRequest = height - 2,
				WidthRequest = width - 2,
				Color = Color.FromHex(color),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				AutomationId = PointId.ToString(),
			};

			//var labelWithId = new Label();
			//labelWithId.HorizontalTextAlignment = TextAlignment.Center;
			//labelWithId.VerticalTextAlignment = TextAlignment.Center;
			//labelWithId.Text = routeCount;
			//labelWithId.TextColor = Color.White;
			//labelWithId.FontSize = labelTextFontSize;

			Children.Add(mainBoxView);
			Grid.SetRow(mainBoxView, 0);
			Grid.SetColumn(mainBoxView, 0);

			//Children.Add(labelWithId);
			//Grid.SetRow(labelWithId, 0);
			//Grid.SetColumn(labelWithId, 0);
		}
	}
}
