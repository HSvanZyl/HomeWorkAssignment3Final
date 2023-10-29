using LibraryManagementV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using PagedList.Mvc;
using PagedList;
using System.Web.UI.WebControls;

namespace LibraryManagementV2.Models
{
    public class CombinedViewModel
    {
        public IPagedList<books> Books { get; set; }
        public IPagedList<students> Students { get; set; }
        public List<string> Status { get; set; }
    }
}