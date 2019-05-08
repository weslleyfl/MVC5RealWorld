﻿using System.Collections.Generic;

namespace MVC5RealWorld.Models.ViewModel
{
    public class UserDataView
    {
        public IEnumerable<UserProfileView> UserProfile { get; set; }
        public UserRoles UserRoles { get; set; }
        public UserGender UserGender { get; set; }
    }

}
