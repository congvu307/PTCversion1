//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PTC_versionORANGE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EM_List
    {
        public string EM_Position_Code { get; set; }
        public string EM_Position_Full_Name { get; set; }
        public int EM_ID { get; set; }
        public string User_Code { get; set; }
        public string EM_Code { get; set; }
        public string EM_Full_Name { get; set; }
        public string Contract_Code { get; set; }
        public Nullable<int> Signal_Day { get; set; }
        public Nullable<int> Signal_Month { get; set; }
        public Nullable<int> Signal_Year { get; set; }
        public string EM_Status { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
    }
}