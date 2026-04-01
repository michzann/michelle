using System;
using System.Collections.Generic;
using System.Text;

namespace grocerseeker
{
    public static class UserSession
    {
        public static string UserRole { get; set; }

        public static string PhoneNumber { get; set; }

        public static string Username { get; set; }

        public static int VendorID { get; set; }

        public static int UserID { get; set; }
        public static double latitude { get; set; }
        public static double longitude { get; set; }

    }
}
