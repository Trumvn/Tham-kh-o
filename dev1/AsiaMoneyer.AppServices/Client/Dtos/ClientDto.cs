using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class ClientDto
    {
        public ClientDto()
        {
            CreatedDate = DateTime.Now;
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string Photo { get; set; }
        public string PhotoSmall { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public Client.Dtos.UserDto User { get; set; }
    }
}
