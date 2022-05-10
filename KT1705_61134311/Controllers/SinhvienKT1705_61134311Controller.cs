using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KT1705_61134311.Models;

namespace KT1705_61134311.Controllers
{
    public class SinhvienKT1705_61134311Controller : Controller
    {
        private KT1705_61134311Entities db = new KT1705_61134311Entities();
        // GET: NhanViens_61134311
        public ActionResult Index()
        {
            var nhanViens = db.Sinhviens.Include(n => n.Lop);
            return View(nhanViens.ToList());
        }

        // GET: NhanViens_61134311/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien nhanVien = db.Sinhviens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens_61134311/Create
        //public ActionResult Create()
        //{
        //    ViewBag.MaPB = new SelectList(db.PhongBans, "MaPB", "TenPB");
        //    return View();
        //}

        // POST: NhanViens_61134311/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MaNV,HoNV,TenNV,GioiTinh,NgaySinh,Luong,AnhNV,DiaChi,MaPB")] NhanVien nhanVien)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.NhanViens.Add(nhanVien);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.MaPB = new SelectList(db.PhongBans, "MaPB", "TenPB", nhanVien.MaPB);
        //    return View(nhanVien);
        //}
        string LayMaSV()
        {
            var maMax = db.Sinhviens.ToList().Select(n => n.MaSV).Max();
            int maSV = int.Parse(maMax.Substring(2)) + 1;
            string SV = String.Concat("00", maSV.ToString());
            return "SV" + SV.Substring(maSV.ToString().Length - 1);
        }

        public ActionResult TimKiem()
        {
            var sinhviens = db.Sinhviens.Include(n => n.Lop);
            return View(sinhviens.ToList());
        }
        [HttpPost]
        public ActionResult TimKiem_61134311(string maSV)
        {

            //var nhanViens = db.NhanViens.SqlQuery("exec NhanVien_DS '"+maNV+"' ");
            /// var nhanViens = db.NhanViens.SqlQuery("SELECT * FROM NhanVien WHERE MaNV='" + maNV + "'");
            var sinhviens = db.Sinhviens.Where(abc => abc.MaSV == maSV);
            return View(sinhviens.ToList());
        }
        [HttpGet]

        public ActionResult TimKiemNC_61134311(string maSV = "", string hoTen = "", string gioiTinh = "", string diaChi = "", string maLop = "")
        {
            
            ViewBag.diaChi = diaChi;
            ViewBag.MaLop = new SelectList(db.Sinhviens, "MaLop", "TenLop");
            var nhanViens = db.Sinhviens.SqlQuery("NhanVien_TimKiem'" + maSV + "','" + hoTen + "','" + gioiTinh + "','" + "',N'" + diaChi + "','" + maLop + "'");
            if (nhanViens.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            return View(nhanViens.ToList());
        }
        public ActionResult Create()
        {

            ViewBag.MaSV = LayMaSV();
            ViewBag.MaLop = new SelectList(db.Lops, "MaLop", "TenLop");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSV,HoSV,TenSV,NgaySinh,GioiTinh,AnhNV,DiaChi,MaLop")] Sinhvien sinhvien)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var imgSV = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgSV.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/" + postedFileName);
            imgSV.SaveAs(path);

            if (ModelState.IsValid)
            {
                sinhvien.MaSV = LayMaSV();
                sinhvien.AnhNV = postedFileName;
                db.Sinhviens.Add(sinhvien);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception e)
                {

                }
               
                return RedirectToAction("Index");
            }

            ViewBag.MaLop = new SelectList(db.Lops, "MaLop", "TenLop", sinhvien.MaSV);
            return View(sinhvien);
        }

        // GET: NhanViens_61134311/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLop = new SelectList(db.Lops, "MaLop", "TenLop", sinhvien.MaSV);
            return View(sinhvien);
        }

        // POST: NhanViens_61134311/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSV,HoSV,TenSV,NgaySinh,GioiTinh,AnhNV,DiaChi,MaLop")] Sinhvien sinhvien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLop = new SelectList(db.Lops, "Malop", "TenLop",sinhvien.MaLop);
            return View(sinhvien);
        }

        // GET: NhanViens_61134311/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            return View(sinhvien);
        }

        // POST: NhanViens_61134311/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            db.Sinhviens.Remove(sinhvien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
