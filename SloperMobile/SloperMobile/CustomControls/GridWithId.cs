using Xamarin.Forms;

namespace SloperMobile.CustomControls
{
    public class GridWithId : Grid
    {
		string color;
		string routeCount;
		int labelTextFontSize;

        public GridWithId(long id, string color, string routeCount, int labelTextFontSize)
        {
            PointId = id;
			this.color = color;
			this.routeCount = routeCount;
			this.labelTextFontSize = labelTextFontSize;
        }

        public long PointId { get; set; }

		protected override void OnParentSet()
		{
			base.OnParentSet();
			var innerBoxView = new BoxView
			{
				HeightRequest = 18,
				WidthRequest = 18,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Color.White,
				Color = Color.White,
				Rotation = 45
			};

			var boxView = new BoxView
			{
				HeightRequest = 16,
				WidthRequest = 16,
				Color = Color.FromHex(color),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Rotation = 45,
				AutomationId = PointId.ToString(),
			};

			var labelWithId = new Label();
			labelWithId.HorizontalTextAlignment = TextAlignment.Center;
			labelWithId.VerticalTextAlignment = TextAlignment.Center;
			labelWithId.Text = routeCount;
			labelWithId.TextColor = Color.White;
			labelWithId.FontSize = labelTextFontSize;

			Children.Add(innerBoxView);
			Grid.SetRow(innerBoxView, 0);
			Grid.SetColumn(innerBoxView, 0);

			Children.Add(boxView);
			Grid.SetRow(boxView, 0);
			Grid.SetColumn(boxView, 0);

			Children.Add(labelWithId);
			Grid.SetRow(labelWithId, 0);
			Grid.SetColumn(labelWithId, 0);
		}
	}
}
