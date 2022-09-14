using ArbolAVL;
using Lab02_EDII.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Huffman;
using Formatting = Newtonsoft.Json.Formatting;

namespace Lab02_EDII
{
    internal class Program
    {
        public static AVLTree<Ingreso> solicitante = new AVLTree<Ingreso>();
        static void Main(string[] args)
        {
            string ruta = "";
            Console.WriteLine("Ingrese la direccion de archvio");
            ruta = Console.ReadLine();
            var reader = new StreamReader(File.OpenRead(ruta));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var value = line.Split(';');
                if (value[0] == "INSERT")
                {
                    var data = JsonConvert.DeserializeObject<Solicitante>(value[1]);
                    Solicitante trabajar = data;
                    List<string> dupli = trabajar.companies.Distinct().ToList();
                    trabajar.companies = dupli;
                    List<Compania> companias = new List<Compania>();
                    for (int i = 0; i < trabajar.companies.Count; i++)
                    {
                        Compania comp = new Compania();
                        comp.Name = trabajar.companies[i];
                        comp.Libreria.Build(comp.Name + "/" + trabajar.dpi);
                        comp.dpicod = comp.Libreria.Encode(comp.Name +"/"+ trabajar.dpi);
                        companias.Add(comp);
                    }
                    Ingreso ingreso = new Ingreso();
                    ingreso.name = trabajar.name;
                    ingreso.dpi = trabajar.dpi;
                    ingreso.address = trabajar.address;
                    ingreso.dateBirth = trabajar.dateBirth;
                    ingreso.companies = companias;
                    solicitante.insert(ingreso, ComparacioDPI);

                }
                else if (value[0] == "PATCH")
                {
                    var data = JsonConvert.DeserializeObject<Solicitante>(value[1]);
                    Solicitante trabajar = data;
                    Ingreso busqueda = new Ingreso();
                    busqueda.name = trabajar.name;
                    busqueda.dpi = trabajar.dpi;
                    if (solicitante.Search(busqueda, ComparacioDPI).name == trabajar.name)
                    {
                        if (trabajar.dateBirth != null)
                        {
                            solicitante.Search(busqueda, ComparacioDPI).dateBirth = trabajar.dateBirth;
                        }
                        if (trabajar.address != null)
                        {
                            solicitante.Search(busqueda, ComparacioDPI).address = trabajar.address;
                        }
                        if(trabajar.companies != null) 
                        {
                            List<string> dupli = trabajar.companies.Distinct().ToList();
                            List<Compania> sindupli = new List<Compania>();
                            for (int i = 0; i < dupli.Count; i++)
                            {
                                Compania comp = new Compania();
                                comp.Name = dupli[i];
                                comp.Libreria.Build(comp.Name +"/"+ trabajar.dpi);
                                comp.dpicod = comp.Libreria.Encode(comp.Name +"/"+ trabajar.dpi);
                                sindupli.Add(comp);
                            }
                            solicitante.Search(busqueda, ComparacioDPI).companies = sindupli;
                        }

                    }
                }
                else if (value[0] == "DELETE")
                {
                    var data = JsonConvert.DeserializeObject<Solicitante>(value[1]);
                    Solicitante trabajar = data;
                    Ingreso ingreso = new Ingreso();
                    ingreso.dpi = trabajar.dpi;
                    List<Ingreso> trabajo = solicitante.getAll();
                    int cant = trabajo.Count();
                    for (int i = 0; i < trabajo.Count; i++)
                    {
                        if (trabajo[i].dpi == ingreso.dpi)
                        {
                            trabajo.RemoveAt(i);
                        }
                    }
                    solicitante = new AVLTree<Ingreso>();
                    int cant2 = trabajo.Count();
                    for (int j = 0; j < trabajo.Count; j++)
                    {
                        solicitante.insert(trabajo[j], ComparacioDPI);
                    }
                }
            }
            string dpi;
            string rutaguardar;
            Console.WriteLine("Escriba el DPI que desea buscar");
            dpi = Console.ReadLine();
            Ingreso solicitantebus = new Ingreso();
            Ingreso solicitantefin = new Ingreso();
            solicitantebus.dpi = dpi;
            solicitantefin=solicitante.Search(solicitantebus, ComparacioDPI);
            Console.WriteLine("Escriba donde guardar el archivo");
            rutaguardar = Console.ReadLine();
            List<Ingreso> solicitantelist = new List<Ingreso>();
            solicitantelist.Add(solicitantefin);
            Serializacion(solicitantelist, rutaguardar);
            string rutacompanias = "";
            Console.WriteLine("Ingrese la direccion de archvio en donde estan las compañias");
            rutacompanias = Console.ReadLine();
            var reader2 = new StreamReader(File.OpenRead(rutacompanias));
            List<string> Companiasingre = new List<string>();
            while (!reader2.EndOfStream) 
            {
                var line2 = reader2.ReadLine();
                var data = JsonConvert.DeserializeObject<List<string>>(line2);
                Companiasingre = data;

            }
            for(int i = 0; i< Companiasingre.Count(); i++) 
            {
                Console.WriteLine(Companiasingre[i]);
            }
            string seleccionado = "";
            Console.WriteLine("Seleccione una compania");
            seleccionado = Console.ReadLine();
            List <Mostrar> finale= new List<Mostrar>();
            for(int j = 0; j < Companiasingre.Count(); j++) 
            {
                if(seleccionado == Companiasingre[j]) 
                {
                    for(int k = 0; k < solicitante.getAll().Count(); k++) 
                    {
                        for (int l = 0; l < solicitante.getAll()[k].companies.Count(); l++) 
                        {
                            if (solicitante.getAll()[k].companies[l].Name == seleccionado) 
                            {
                                
                                Mostrar empleado = new Mostrar();
                                empleado.name = solicitante.getAll()[k].name;
                                empleado.dpi = solicitante.getAll()[k].dpi;
                                empleado.dateBirth = solicitante.getAll()[k].dateBirth;
                                empleado.address = solicitante.getAll()[k].address;
                                empleado.companie = solicitante.getAll()[k].companies[l];
                                string[] split = solicitante.getAll()[k].companies[l].Libreria.Decode(solicitante.getAll()[k].companies[l].dpicod).Split('/');
                                if (split[1].Length > 13) 
                                {
                                    while (split[1].Length > 13)
                                        split[1] = split[1].Remove(split[1].Length - 1);
                                }
                                empleado.decode = split[1];
                                finale.Add(empleado);
                            }
                        }
                    }
                }
            }
            string rutaguardar2;
            Console.WriteLine("Escriba donde guardar el archivo");
            rutaguardar2 = Console.ReadLine();
            Serializacion2(finale, rutaguardar2);
        }
        public static void Serializacion(List<Ingreso> Lista, string path)
        {
            string solictanteJson = JsonConvert.SerializeObject(Lista.ToArray(), Formatting.Indented);
            File.WriteAllText(path, solictanteJson);
        }
        public static void Serializacion2(List<Mostrar> Lista, string path)
        {
            string solictanteJson = JsonConvert.SerializeObject(Lista.ToArray(), Formatting.Indented);
            File.WriteAllText(path, solictanteJson);
        }
        public static bool ComparacioDPI(Ingreso paciente, string operador, Ingreso paciente2)
        {
            int Comparacion = string.Compare(paciente.dpi,paciente2.dpi);
            if (operador == "<")
            {
                return Comparacion < 0;
            }
            else if (operador == ">")
            {
                return Comparacion > 0;
            }
            else if (operador == "==")
            {
                return Comparacion == 0;
            }
            else return false;
        }
    }
}
