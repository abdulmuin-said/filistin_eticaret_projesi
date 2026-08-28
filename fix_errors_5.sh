sed -i 's/TempData\["Mesaj"\] = "?? ????? ??????? ????? ?????.";/TempData["Mesaj"] = _localizer["Admin_Order_UnknownError"].Value;/g' FilistinProje.Web/Areas/Admin/Controllers/SiparisController.cs
sed -i 's/TempData\["Hata"\] = "لم يتم العثور على الطلب.";/TempData["Hata"] = _localizer["Admin_Order_NotFound"].Value;/g' FilistinProje.Web/Areas/Admin/Controllers/SiparisController.cs
