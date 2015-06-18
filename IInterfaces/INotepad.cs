using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IInterfaces
{
    public interface INotepadCategories
    {
        Int32 UserId { get; set; }
        String CategoryName { get; set; }
        Int32 ServerId { get; set; }
        DateTime Created { get; set; }
        Int32 CategoryId { get; set; }
    }

    public interface INotepadTextDocuments
    {
        String CategoryName { get; set; }
        Int32 UserId { get; set; }
        Int32 CategoryId { get; set; }
        String Data { get; set; }
        Int32 ServerId { get; set; }
        DateTime Created { get; set; }
    }
}
