[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create(Insuree insuree)
{
    if (ModelState.IsValid)
    {
        // Base quote
        decimal quote = 50m;

        // Age calculation
        int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
        if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

        if (age <= 18)
        {
            quote += 100m;
        }
        else if (age >= 19 && age <= 25)
        {
            quote += 50m;
        }
        else
        {
            quote += 25m;
        }

        // Car year
        if (insuree.CarYear < 2000)
        {
            quote += 25m;
        }
        if (insuree.CarYear > 2015)
        {
            quote += 25m;
        }

        // Car make and model
        if (!string.IsNullOrEmpty(insuree.CarMake) && insuree.CarMake.ToLower() == "porsche")
        {
            quote += 25m;
            if (!string.IsNullOrEmpty(insuree.CarModel) && insuree.CarModel.ToLower() == "911 carrera")
            {
                quote += 25m;
            }
        }

        // Speeding tickets
        quote += insuree.SpeedingTickets * 10;

        // DUI
        if (insuree.DUI)
        {
            quote *= 1.25m;
        }

        // Full coverage
        if (insuree.CoverageType)
        {
            quote *= 1.50m;
        }

        insuree.Quote = quote;

        db.Insurees.Add(insuree);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    return View(insuree);
}
