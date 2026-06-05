using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FcTrx.Model.ViewModels
{
    public class DocumentPagedDto
    {
        public class DocumentPagedDataDto
        {
            public int Id { get; set; }
            public string? Description { get; set; }
            public string? BranchId { get; set; }
            public string? BranchName { get; set; }
            public int? CategoryId { get; set; }
            public string? CategoryName { get; set; }
            public string? EntryByEmpCode { get; set; }
            public string? EntryTime { get; set; }
            public string? UpdateByEmpCode { get; set; }
            public string? UpdateTime { get; set; }
            public int? StatusId { get; set; }
            public string? StatusName { get; set; }
            public string? FileNames { get; set; }
            public string? TagValues { get; set; }
            public int TotalCount { get; set; }
        }
    }
}
