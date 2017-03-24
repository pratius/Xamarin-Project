using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using SloperMobile.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(CustomListView))]
namespace SloperMobile.iOS
{
    class CustomListView : ViewCellRenderer
    {
        public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
                cell.SelectionStyle = UIKit.UITableViewCellSelectionStyle.None;
            return cell;
        }
    }
}