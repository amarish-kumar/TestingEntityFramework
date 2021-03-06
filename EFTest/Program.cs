﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using EFTest.Entities;

namespace EFTest
{
    class Program
    {

        public async Task Run()
        {

            //Non lazy service examples
            using (var scope = IOCManager.Instance.Container.BeginLifetimeScope())
            {
                using (var uow = scope.Resolve<ISachaContext>())
                {
                    var someService = scope.Resolve<ISomeService>();
                    await someService.InsertAsync(string.Format("EFTest Non Lazy version {0}", 
                            DateTime.Now.ToLongTimeString()));
                    var posts = await someService.GetAllAsync();
                    var postLast = await someService.FindByIdAsync(posts.Last().Id);
                    await uow.SaveChangesAsync();
                }
                Console.WriteLine("DONE NON LAZY");

 
            }


            //Lazy service examples
            using (var scope = IOCManager.Instance.Container.BeginLifetimeScope())
            {
                using (var uow = scope.Resolve<ISachaLazyContext>())
                {
                    var someServiceLazy = scope.Resolve<ISomeServiceLazy>();
                    await someServiceLazy.InsertAsync(string.Format("EFTest Lazy version {0}",
                            DateTime.Now.ToLongTimeString()));
                    var posts = await someServiceLazy.GetAllAsync();
                    var postLast = await someServiceLazy.FindByIdAsync(posts.Last().Id);
                    await uow.SaveChangesAsync();
                }
            }
            Console.WriteLine("DONE LAZY");


            Console.WriteLine("ALL DONE");
            Console.ReadLine();
        }

       


        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run().Wait();
        }
    }
}
