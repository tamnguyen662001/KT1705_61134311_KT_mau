using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using THITHUWEB.Models;

namespace THITHUWEB.Controllers
{
    public class TAISAN_61134311Controller : Controller
    {
        private Thi61CNTT_61134311Entities db = new Thi61CNTT_61134311Entities();

        string LayMaTS()
        {
            var maMax = db.TAISANs.ToList().Select(n => n.MaTS).Max();
            int maTS = int.Parse(maMax.Substring(2)) + 1;
            string NV = String.Concat("000", maTS.ToString());
            return "TS" + NV.Substring(maTS.ToString().Length - 1);
        }
        public ActionResult TimKiem()
        {
            var taisan = db.TAISANs.Include(n => n.LOAITAISAN);
            return View(taisan.ToList());
        }
        [HttpPost]
        public ActionResult TimKiem(string maTS)
        {
            var taisan = db.TAISANs.Where(abc => abc.MaTS == maTS);
            return View(taisan.ToList());
        }
        [HttpGet]

        public ActionResult Search(string TenTS = "", string giaMin = "", string giaMax = "")
        {
            string min = giaMin, max = giaMax;
           
          
            ViewBag.TenTS = TenTS;
           
            if (giaMin == "")
            {
                ViewBag.giaMin = "";
                min = "0";
            }
            else
            {
                ViewBag.giaMin = giaMin;
                min = giaMin;
            }
            if (max == "")
            {
                max = Int32.MaxValue.ToString();
                ViewBag.giaMax = "";// Int32.MaxValue.ToString(); 
            }
            else
            {
                ViewBag.giaMax = giaMax;
                max = giaMax;
            }
           
            
            var taisan = db.TAISANs.SqlQuery("TaiSan_TimKiem'" + TenTS +"','" + min + "','" + max  + "'");
            if (taisan.Count() == 0)
                ViewBag.TB = "Không có thông tin tài sản cần tìm kiếm.";
            return View(taisan.ToList());
        }
        public ActionResult Index()
        {

            var taisan = db.TAISANs.Include(n => n.LOAITAISAN);
            return View(taisan.ToList());
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAISAN nhanVien = db.TAISANs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {

            ViewBag.MaTS = LayMaTS();
            ViewBag.MaLTS = new SelectList(db.LOAITAISANs, "MaLTS", "TenLTS");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTS,TenTS,DVT,XuatSu,DonGia,AnhMH,MaLTS,GhiChu")] TAISAN taisan)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var imgTS = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgTS.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/" + postedFileName);
            imgTS.SaveAs(path);

            if (ModelState.IsValid)
            {
                taisan.MaTS = LayMaTS();
                taisan.AnhMH = postedFileName;
                db.TAISANs.Add(taisan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaPB = new SelectList(db.LOAITAISANs, "MaLTS", "TenLTS", taisan.MaLTS);
            return View(taisan);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAISAN taisan = db.TAISANs.Find(id);
            if (taisan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPB = new SelectList(db.LOAITAISANs, "MaLTS", "TenLTS", taisan.MaLTS);
            return View(taisan);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTS,TenTS,DVT,XuatSu,DonGia,AnhMH,MaLTS,GhiChu")] TAISAN taisan)
        {
            var imgTS = Request.Files["Avatar"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgTS.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/" + postedFileName);
                imgTS.SaveAs(path);
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                db.Entry(taisan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPB = new SelectList(db.LOAITAISANs, "MaLTS", "TenLTS", taisan.MaLTS);

            return View(taisan);
        }

        // GET: NhanViens/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAISAN nhanVien = db.TAISANs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TAISAN nhanVien = db.TAISANs.Find(id);
            db.TAISANs.Remove(nhanVien);
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
