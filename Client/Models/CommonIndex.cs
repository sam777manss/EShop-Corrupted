﻿namespace Client.Models
{
    public class CommonIndex
    {
        public UserIndex? User { get; set; }
        public AdminIndex? Admin { get; set; }
        public IList<string>? Roles { get; set; }
        public Response? response { get; set; }
    }
}
