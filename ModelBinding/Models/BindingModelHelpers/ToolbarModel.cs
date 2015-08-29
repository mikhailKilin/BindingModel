using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBinding.Models.BindingModelHelpers
{
    public class ToolbarModel
    {
        public ToolbarModel()
        {
            Buttons = new List<ToolbarButton>();
        }

        public List<ToolbarButton> Buttons { get; set; }
        public SearchInput SearchInput { get; set; }
    }
    public class ToolbarButton
    {
        public ToolbarButton()
        {
            Enable = true;
            Render = true;
        }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public string Click { get; set; }

        public bool Enable { get; set; }
        public bool Render { get; set; }
        public string Style { get; set; }
    }
    public class SearchInput
    {
        public SearchInput()
        {
            Render = true;
        }

        public bool Render { get; set; }
    }
}
