﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public static class AppData
    {
        public static onlineEventsEntities db = new onlineEventsEntities();
        public static User CurrentUser = new User();
    }
}
