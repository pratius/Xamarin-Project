using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class TopoImageResponse
    {
        public topoimage image { get; set; }
        public List<topodrawing> drawing { get; set; }

    }

    public class topoimage
    {
        public string name { get; set; }
        public string uploadDate { get; set; }
        public string type { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string size { get; set; }
        public string scale { get; set; }
        public string data { get; set; }
    }
    public class topodrawing
    {
        /// <summary>
        /// RouteID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// RouteName
        /// </summary>
        public string name { get; set; }
        public string grade { get; set; }
        public string gradeBucket { get; set; }
        public topoline line { get; set; }
    }
    public class topoline
    {        
        public List<topopoints> points { get; set; }
        public List<topopointstext> pointsText { get; set; }
        public topostyle style { get; set; }
        public topomarker marker { get; set; }
    }
    public class topopoints
    {
        public string x { set; get; }
        public string y { set; get; }
        public string type { set; get; }
        public string label { set; get; }
    }
    public class topopointstext
    {
        public string point_id { set; get; }
        public string text_id { set; get; }
        public string text_value { set; get; }
        public string isdirection { set; get; }
    }
    public class topostyle
    {
        public string type { get; set; }
        public string width { get; set; }
        public string color { get; set; }
        public List<int> dashPattern { get; set; }
        public string is_dark_checked { get; set; } 
        public toposhadow shadow { get; set; }
    }
    public class toposhadow
    {
        public string enabled { get; set; }
        public string offsetX { get; set; }
        public string offsetY { get; set; }
        public string color { get; set; }
        public string blur { get; set; }
    }
    public class topomarker
    {
        public string label { get; set; }
        public string foreColor { get; set; }
        public string fillColor { get; set; }
        public string shadow { get; set; }
    }
}
