using Xamarin.Forms;

namespace SloperMobile.Model
{
    public class PointWithId
    {
        public PointWithId(double x, double y, long id)
        {
            X = x;
            Y = y;
            PointId = id;
        }

        //
        // Summary:
        //     Location along the horizontal axis.
        //
        // Remarks:
        //     To be added.
        public double X { get; set; }

        //
        // Summary:
        //     Location along the vertical axis.
        //
        // Remarks:
        //     To be added.
        public double Y { get; set; }

        public long PointId { get; set; }
    }
}
