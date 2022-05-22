using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UrunListem.Data;
using UrunListem.Models;

namespace UrunListem.Controllers
{
    public class UrunIslemleri : Controller
    {
        private readonly UrunListemContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _dosyaYolu;

        public UrunIslemleri(UrunListemContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: UrunIslemleri
        public async Task<IActionResult> Index()
        {
            return View(await _context.Urunler.Include(x=>x.Resimler).ToListAsync());
        }

        // GET: UrunIslemleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler.Include(x=> x.Resimler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }

        // GET: UrunIslemleri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UrunIslemleri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adi,Tur,Renk,Aciklama,Fiyat,Dosyalar")] Urun urun)
        {
            if (ModelState.IsValid)
            {
                 var DosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "Resimler");
                 if(!Directory.Exists(DosyaYolu))
                 {
                     Directory.CreateDirectory(DosyaYolu);
                 }

                 foreach (var item in urun.Dosyalar)
                 {
                    var fullDosyaAdi = Path.Combine(DosyaYolu, item.FileName);

                    using(var DosyaAkisi= new FileStream(fullDosyaAdi, FileMode.Create))
                    {
                        await item.CopyToAsync(DosyaAkisi);
                    }

                    urun.Resimler.Add(new Resim{ResimAdi=item.FileName});
                 }

                 _context.Add(urun);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
            return View(urun);
        }

        // GET: UrunIslemleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var urun = await _context.Urunler.Include(x => x.Resimler).SingleOrDefaultAsync(x => x.Id == id);
            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }

        // POST: UrunIslemleri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adi,Tur,Renk,Aciklama,Fiyat,Dosyalar")] Urun urun)
        {
            if (id != urun.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(urun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunExists(urun.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
            
        }

        // GET: UrunIslemleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }

        // POST: UrunIslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            _context.Urunler.Remove(urun);
            await _context.SaveChangesAsync();

            foreach (var item in urun.Resimler)
            {
                System.IO.File.Delete(Path.Combine(_dosyaYolu,item.ResimAdi));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ResimSil(int id)
        {
            var resim = await _context.Resimler.FindAsync(id);

            _context.Resimler.Remove(resim);

            await _context.SaveChangesAsync();

            System.IO.File.Delete(Path.Combine(_dosyaYolu,resim.ResimAdi));
            return RedirectToAction(nameof(Edit), new {id=resim.UrunuId});
        }

        private bool UrunExists(int id)
        {
            return _context.Urunler.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult>ResimEkle(Urun urun)
        {
            var resimEklenecekUrun = await _context.Urunler.FindAsync(urun.Id);
            var DosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "Resimler");
            if(!Directory.Exists(DosyaYolu))
            {
                Directory.CreateDirectory(DosyaYolu);
            }
            foreach (var item in urun.Dosyalar)
            {
                using (var DosyaAkisi = new FileStream(Path.Combine(DosyaYolu,item.FileName), FileMode.Create))
                {
                    await item.CopyToAsync(DosyaAkisi);
                }
                resimEklenecekUrun.Resimler.Add(new Resim {ResimAdi = item.FileName});
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new {id = urun.Id});
        }

    }
}
