using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.DTOs
{
    public class ConversacionDto
    {
        public record ConversacionGet
        {
            public int Id { get; set; }
            public string CreadorId { get; set; }
            public string UserId { get; set; }
            public string UsernameCreator { get; set; }
            public string UsernameUser { get; set; }
            public int MensageId { get; set; }
            public Boolean Read { get; set;  }
            public string Body { get; set; }
            public string FechaHora { get; set; }
            public DateTime FechaHoraOrden { get; set;  }
            public string UserSender { get; set; }
        }

        public record MensajesGet
        {
            public int MensajeId { get; set; }
            public int ConversacionId { get; set; }
            public string UserSender { get; set; }
            public string Username { get; set; }
            public string Body { get; set; }
            public string DateTimeSent { get; set; }
            public Boolean Read { get; set; }

        }

        public record MensajeAdd
        {
            public int ConversacionId { get; set; }
            public string CreadorId { get; set; }
            public string Body { get; set; }
        }

        public record ConnectionChat
        {
            public string Id { get; set; }
        }

    }
}
