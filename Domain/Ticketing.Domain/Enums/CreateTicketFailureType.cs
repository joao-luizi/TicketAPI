using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Domain.Enums
{
    public enum CreateTicketFailureType
    {
        None,
        UserNotFound,
        UserInactive,
        DuplicateTicket
    }
}
