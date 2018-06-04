using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.Model
{
    class EditedEvent
    {
        public static Events editedEvent = new Events();

        public static void SetEditedEvent(Events selectedEvent)
        {
            editedEvent.title = selectedEvent.title;
            editedEvent.date = selectedEvent.date;
            editedEvent.days = selectedEvent.days;
            editedEvent.check = selectedEvent.check;
        }

        public static Events getEditedEvent()
        {
            return editedEvent;
        }
    }
}
