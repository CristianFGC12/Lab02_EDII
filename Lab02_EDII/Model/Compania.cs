using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Huffman;
using Newtonsoft.Json;

namespace Lab02_EDII.Model
{
    public class Compania
    {
        public string Name { get; set; }
        public byte[] dpicod { get; set; }
        [JsonIgnore]
        public HuffmanTree Libreria = new HuffmanTree();
    }
}
