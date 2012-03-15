namespace Sedodream.SelfPub.ConfigService.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Models;
    using Sedodream.SelfPub.ConfigService.Models.PageModels;

    public class HomeController : Controller {
        protected IPackageRepository PackageRepository { get; set; }

        public HomeController(IPackageRepository packageRepository) {
            if (packageRepository == null) { throw new ArgumentNullException("packageRepository"); }

            this.PackageRepository = packageRepository;
        }

        public ActionResult Index() {
            // TODO: this should be paged so that we don't return all pacakges at once
            IList<Package> packages = (from p in this.PackageRepository.GetPackages()
                                       select p).ToList();

            // TODO: Use automapper for this instead
            HomePageModel hpm = new HomePageModel(packages);

            

            return View(hpm);
        }

        public ActionResult AddPackage() {
            return View(new Package());
        }

        [HttpPost]
        public ActionResult AddPackage(Package package) {
            if (package == null) { throw new ArgumentNullException("package"); }

            ValidateModel(package);

            this.PackageRepository.AddPackage(package);

            return RedirectToAction("Index");
        }
    }
}
