using IInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinNote.Model.Entity.Notepad
{
    public class NotepadCategory: INotepadCategories
    {
        public Int32 UserId { get; set; }
        public String CategoryName { get; set; }
        public Int32 ServerId { get; set; }
        public DateTime Created { get; set; }
        public int CategoryId { get; set; }
    }

    public class NotepadTextDocument: INotepadTextDocuments
    {
        public Int32 UserId { get; set; }
        public Int32 CategoryId { get; set; }
        public String Data { get; set; }
        public Int32 ServerId { get; set; }
        public DateTime Created { get; set; }
        public String CategoryName { get; set; }
    }
}
