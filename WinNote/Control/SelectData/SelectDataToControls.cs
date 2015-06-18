using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinNote.Control.Entity;

namespace WinNote.Control.SelectData
{
    class SelectDataToControls
    {
        EntityData data;
        private SelectDataToControls() 
        {
            data = EntityData.GetInstance;
        }


        private static SelectDataToControls controlsDataSelect { get; set; }
        public static SelectDataToControls GetIndtance
        {
            get
            {
                if (controlsDataSelect == null)
                    controlsDataSelect = new SelectDataToControls();
                return controlsDataSelect;
            }
            set
            {
                controlsDataSelect = value;
            }
        }

        
    }
}
