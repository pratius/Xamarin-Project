using Xamarin.Forms;

namespace SloperMobile.CustomControls
{
    public class BoxViewWithId : BoxView
    {
        public BoxViewWithId(long id)
        {
            PointId = id;
        }

        public long PointId { get; set; }
    }
}
