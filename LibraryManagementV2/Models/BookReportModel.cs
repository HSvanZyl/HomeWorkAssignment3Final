using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementV2.Models
{
    public class BookReportModel
    {
        [AllowHtml]
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int BorrowCount { get; set; }
        public string Content { get; set; }

        public string Extension { get; set; }
        public string FileName { get; set; }
    }
}