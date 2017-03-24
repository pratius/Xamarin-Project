using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.DataBase;
using SloperMobile.Model;
using SloperMobile.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class AscentSummaryModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private List<string> grades;
        private List<string> listimgs;
        private DateTime sendsdate = DateTime.Now.Date;
        private string sendsgrade;
        private string sendclimbingstyle;
        private string sendholdtype;
        private string sendroutecharacteristics;
        private string commenttext;
        private string routeid;
        private bool isenable = true;
        private bool isdisplaymsg;

        private bool isdisplaysummaryimg = false;
        private bool isdisplaysummaryweb = true;

        private ImageSource summaryimage;

        private T_ROUTE routeData;

        private string routeName = "";
        private string sendsTypeName = "";
        private int sendRating = 0;
        private string commandText = "";

        private ImageSource topAngle = null;
        private ImageSource topRouteChar = null;


        public string RouteName
        {
            get { return routeName; }
            set { routeName = value; OnPropertyChanged(); }
        }
        public string SendsTypeName
        {
            get { return sendsTypeName; }
            set { sendsTypeName = value; OnPropertyChanged(); }
        }
        public int SendRating
        {
            get { return sendRating; }
            set { sendRating = value; OnPropertyChanged(); }
        }

        public ImageSource TopAngle
        {
            get { return topAngle; }
            set { topAngle = value; OnPropertyChanged(); }
        }
        public ImageSource TopRouteChar
        {
            get { return topRouteChar; }
            set { topRouteChar = value; OnPropertyChanged(); }
        }
        public string CommandText
        {
            get { return commandText; }
            set { commandText = value; OnPropertyChanged(); }
        }


        public List<string> AscentGrades
        {
            get { return grades; }
            set { grades = value; OnPropertyChanged(); }
        }

        public List<string> ListImages
        {
            get { return listimgs; }
            set { listimgs = value; OnPropertyChanged(); }
        }


        public DateTime SendsDate
        {
            get { return sendsdate; }
            set { sendsdate = value; OnPropertyChanged(); }
        }
        public string SendsGrade
        {
            get { return sendsgrade; }
            set { sendsgrade = value; OnPropertyChanged(); }
        }

        public string SendClimbingStyle
        {
            get { return sendclimbingstyle; }
            set { sendclimbingstyle = value; OnPropertyChanged(); }
        }
        public string SendHoldType
        {
            get { return sendholdtype; }
            set { sendholdtype = value; OnPropertyChanged(); }
        }
        public string SendRouteCharacteristics
        {
            get { return sendroutecharacteristics; }
            set { sendroutecharacteristics = value; OnPropertyChanged(); }
        }

        public ImageSource SummaryImage
        {
            get { return summaryimage; }
            set { summaryimage = value; OnPropertyChanged(); }
        }

        public string RouteId
        {
            get { return routeid; }
            set { routeid = value; OnPropertyChanged(); }
        }


        public AscentSummaryModel(INavigation navigation, string routeid, Send send)
        {
            SummaryImage = Cache.SelctedCurrentSector?.SectorImage;
            RouteId = routeid;
            routeData = App.DAUtil.GetRouteDataByRouteID(RouteId);
            PageHeaderText = (routeData.route_name).ToUpper();
            PageSubHeaderText = Cache.SelctedCurrentSector.SectorName;
            //PageSubHeaderText = "";
            var grades = App.DAUtil.GetTtechGrades(routeData.grade_type_id);
            AscentGrades = grades;
            if (grades.Count > 0)
            {
                SendsGrade = routeData.tech_grade; //grades[0];
            }
            _navigation = navigation;

            SendsTypeName = send.Ascent_Type_Description;
            SendRating = send.Rating;
        }
    }
}
