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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    [Bind(Include = "User_ID,User_Code,Password,User_First_Name,User_Last_Name,User_Alternative_Name,User_DoB_Day,User_DoB_Month,User_DoB_Year,Gender,Address_Detail,Address_District,Email,Phone,Role,Client_Code,Education_Type_Code,Job_Type_Code,Job_Detail,Status,Note")]
    public partial class User_Basic
    {
        public int User_ID { get; set; }
        public string User_Code { get; set; }
        public string Password { get; set; }
        public string User_First_Name { get; set; }
        public string User_Last_Name { get; set; }
        public string User_Alternative_Name { get; set; }
        public Nullable<int> User_DoB_Day { get; set; }
        public Nullable<int> User_DoB_Month { get; set; }
        public Nullable<int> User_DoB_Year { get; set; }
        public string Gender { get; set; }
        public string Address_Detail { get; set; }
        public Nullable<int> Address_District { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Client_Code { get; set; }
        public string Education_Type_Code { get; set; }
        public string Job_Type_Code { get; set; }
        public string Job_Detail { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
