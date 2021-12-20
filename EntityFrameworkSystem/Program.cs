using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<CsharpDBContext> optionsBuilder =
                new DbContextOptionsBuilder<CsharpDBContext>();

            optionsBuilder.UseSqlServer("Server=DESKTOP-U1F6TFI; Database=CsharpDB; Trusted_Connection=true;");

            bool again = true;
            int op = 0;

            do
            {
                ShowMenu();
                Console.WriteLine("Elige una opcion: ");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1:
                        Show(optionsBuilder);
                        break;
                    case 2:
                        Add(optionsBuilder);
                        break;
                    case 3:
                        Edit(optionsBuilder);
                        break;
                    case 4:
                        Delete(optionsBuilder);
                        break;
                    case 5:
                        again = false;
                        break;
                }
            } while (again);
        }

        public static void Add(DbContextOptionsBuilder<CsharpDBContext> optionsBuilder)
        {
            Console.Clear();
            Console.WriteLine("---------- Agregar Cervezas ----------");
            Console.Write("Escribe el nombre de la cerveza: ");
            string name = Console.ReadLine();
            Console.Write("Escribe el ID de marca: ");
            int brandId = int.Parse(Console.ReadLine());

            using (var context = new CsharpDBContext(optionsBuilder.Options))
            {
                Beer beer = new Beer()
                {
                    Name = name,
                    BrandId = brandId
                };

                context.Add(beer);
                context.SaveChanges();
            }
        }

        public static void Edit(DbContextOptionsBuilder<CsharpDBContext> optionsBuilder)
        {
            Console.Clear();
            Show(optionsBuilder);
            Console.WriteLine("---------- Editar Cervezas ----------");
            Console.Write("Ingresa el ID de tu Cerveza: ");
            int id = int.Parse(Console.ReadLine());

            using (var context = new CsharpDBContext(optionsBuilder.Options))
            {
                Beer beer = context.Beers.Find(id);
                if (beer != null)
                {
                    Console.Write("Escribe el nombre de la cerveza: ");
                    string name = Console.ReadLine();
                    Console.Write("Escribe el ID de la marca: ");
                    int brandId = int.Parse(Console.ReadLine());

                    beer.Name = name;
                    beer.BrandId = brandId;

                    context.Entry(beer).State = EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Cerveza no existente");
                }
            }
        }

        public static void Delete(DbContextOptionsBuilder<CsharpDBContext> optionsBuilder)
        {
            Console.Clear();
            Show(optionsBuilder);
            Console.WriteLine("---------- Borrar Cervezas ----------");
            Console.Write("Escirbe el ID de la cerveza a eliminar: ");
            int id = int.Parse(Console.ReadLine());

            using (var context = new CsharpDBContext(optionsBuilder.Options))
            {
                Beer beer = context.Beers.Find(id);

                if (beer != null)
                {
                    context.Beers.Remove(beer);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("La cerveza no existe");
                }
            }
        }

        public static void Show(DbContextOptionsBuilder<CsharpDBContext> optionsBuilder)
        {
            Console.Clear();
            Console.WriteLine("---------- Cervezas en la Base de Datos ----------");
            using (var context = new CsharpDBContext(optionsBuilder.Options))
            {
                List<Beer> beers = (from b in context.Beers
                                    select b).Include(b => b.Brand).ToList();

                foreach (var beer in beers)
                {
                    Console.WriteLine($"{beer.BeerId} - {beer.Name} de {beer.Brand.Name}");
                }
            }
        }

        public static void ShowMenu()
        {
            Console.WriteLine("\n---------- Menú ----------");
            Console.WriteLine("1.- Mostrar | 2.- Agregar");
            Console.WriteLine("3.- Editar  | 4.- Eliminar");
            Console.WriteLine("5.- Salir");
        }
    }
}
