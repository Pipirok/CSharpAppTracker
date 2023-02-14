using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectBack.Models.EntityFramework;

namespace ProjectBack
{
    public class AppUsersModel
    {
        public string AppName;
        public List<UsersAndApplication> usersAndApplications;
    }
}