using Xamarin.Forms;

namespace SloperMobile.CustomControls
{
    public class LabelWithId : Label
    {
        public LabelWithId(long id)
        {
            PointId = id;
        }

        public long PointId { get; set; }
    }
}
