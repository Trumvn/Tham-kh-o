using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging.Dtos
{
    public class GetMessageDto
    {
        public int Total { get; set; }
        public List<Dtos.AutoMessagingMessageDto> Messages { get; set; }
    }
}
