//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectBack.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersApplicationsView
    {
        public Nullable<long> RN { get; set; }
        public int UserID { get; set; }
        public int AppID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Allowed { get; set; }
        public string UserIP { get; set; }
    }
}
