using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrunListem.Models;

namespace UrunListem.Data
{
    public class UrunListemContext : DbContext
    {
        public UrunListemContext (DbContextOptions<UrunListemContext> options)
            : base(options)
        {
        }

        public DbSet<UrunListem.Models.Urun> Urunler { get; set; }
        public DbSet<UrunListem.Models.Resim> Resimler { get; set; }
    }
}
