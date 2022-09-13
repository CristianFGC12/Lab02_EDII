using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab02_EDII.Model;

namespace Lab02_EDII.Model
{
    public class Solicitante
    {
        public string name { get; set; }
        public string dpi { get; set; }
        public string dateBirth { get; set; }
        public string address { get; set; }
        public List<string> companies { get; set; }    
    }
}
