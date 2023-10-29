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
    public class MaintainViewModel
    {
        public IPagedList<authors> Authors { get; set; }
        public IPagedList<types> Types { get; set; }
        public IPagedList<borrows> Borrows { get; set; }
    }
}