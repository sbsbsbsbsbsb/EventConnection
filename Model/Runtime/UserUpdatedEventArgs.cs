using System;
using Model.DTO;

namespace Model.Runtime
{
    public class UserUpdatedEventArgs : EventArgs
    {
        public UserModel User { get; set; }
    }
}
