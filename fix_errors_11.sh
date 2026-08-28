sed -i -E 's/_localizer\["Admin_Bank_RequiredFields"\]\.Value/"Admin_Bank_RequiredFields"/g' FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs
sed -i -E 's/string\.Format\(_localizer\["Admin_Bank_Added"\]\.Value, model\.BankaAdi\)/$"Admin_Bank_Added {model.BankaAdi}"/g' FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs
sed -i -E 's/_localizer\["Admin_Bank_NotFound"\]\.Value/"Admin_Bank_NotFound"/g' FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs
sed -i -E 's/string\.Format\(_localizer\["Admin_Bank_Updated"\]\.Value, model\.BankaAdi\)/$"Admin_Bank_Updated {model.BankaAdi}"/g' FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs
sed -i -E 's/_localizer\["Admin_Error"\]\.Value \+ ex\.Message/"Admin_Error " + ex.Message/g' FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs
sed -i -E 's/string\.Format\(_localizer\["Admin_Bank_Deleted"\]\.Value, hesap\.BankaAdi\)/$"Admin_Bank_Deleted {hesap.BankaAdi}"/g' FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs

sed -i -E 's/string\.Format\(_localizer\["Admin_Coupon_Created"\]\.Value, model\.Kod\)/$"Admin_Coupon_Created {model.Kod}"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/string\.Format\(_localizer\["Admin_Coupon_Updated"\]\.Value, kupon\.Kod\)/$"Admin_Coupon_Updated {kupon.Kod}"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/string\.Format\(_localizer\["Admin_Coupon_Archived"\]\.Value, kupon\.Kod\)/$"Admin_Coupon_Archived {kupon.Kod}"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/_localizer\["Admin_Coupon_InvalidType"\]\.Value/"Admin_Coupon_InvalidType"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/_localizer\["Admin_Coupon_ValueGreaterThanZero"\]\.Value/"Admin_Coupon_ValueGreaterThanZero"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/_localizer\["Admin_Coupon_ValueMax100"\]\.Value/"Admin_Coupon_ValueMax100"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/_localizer\["Admin_Coupon_MinCartNotNegative"\]\.Value/"Admin_Coupon_MinCartNotNegative"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/_localizer\["Admin_Coupon_LimitNotNegative"\]\.Value/"Admin_Coupon_LimitNotNegative"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/_localizer\["Admin_Coupon_InvalidDate"\]\.Value/"Admin_Coupon_InvalidDate"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
sed -i -E 's/_localizer\["Admin_Coupon_CodeInUse"\]\.Value/"Admin_Coupon_CodeInUse"/g' FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs
