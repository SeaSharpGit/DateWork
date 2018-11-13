using DateWork.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateWork.Windows
{
    public class NoteViewModel : EntityBase
    {
        public Action CloseWindow { get; set; } = null;

    }
}
