﻿namespace HPlusSport2.Api.Models
{
    public class User
    {
        public int Id
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }
       
        public virtual List<Order> Orders
        {
            get; set;
        }

    }
}
