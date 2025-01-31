using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polomka.DB
{
    internal class DBConnection
    {
        public static PolomkaEntities polomka = new PolomkaEntities();

        public static void DisposeAndCreate()
        {
            //polomka.Dispose();
            //polomka = new PolomkaEntities();


            foreach (DbEntityEntry dbEntityEntry in DBConnection.polomka.ChangeTracker.Entries().ToArray())
            {

                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }

            
        }

    
}
