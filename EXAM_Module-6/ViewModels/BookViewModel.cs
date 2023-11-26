using EXAM_Module_6.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EXAM_Module_6.ViewModels
{
    public class BookViewModel
    {
        public List<Books> Books { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string Category { get; set; }
    }
}
