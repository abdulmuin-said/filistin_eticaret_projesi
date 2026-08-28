import os
import re

def fix_file(path):
    with open(path, 'r', encoding='utf-8') as f:
        content = f.read()

    # Add missing _localizer field if needed, but the errors are about _localizer not existing.
    # Looking at the code, it's a Controller so we should just inject it or replace it with plain strings or use the existing Localizer if available.
    # The user wanted to remove Turkish logs/warnings and errors. So let's just replace them with English strings instead of _localizer if _localizer is not injected.
    
    # We'll just replace the missing _localizer usage with English strings directly for these specific errors.
    
    if 'BankalarController' in path:
        content = content.replace('_localizer["Admin_Bank_RequiredFields"].Value', '"Bank name, account holder and IBAN are required."')
        content = content.replace('$"Admin_Bank_Added {model.BankaAdi}"', 'string.Format("Bank account {0} added.", model.BankaAdi)')
        content = content.replace('_localizer["Admin_Bank_NotFound"].Value', '"Bank account not found."')
        content = content.replace('$"Admin_Bank_Updated {model.BankaAdi}"', 'string.Format("Bank account {0} updated.", model.BankaAdi)')
        content = content.replace('"Admin_Error " + ex.Message', '"Error: " + ex.Message')
        content = content.replace('$"Admin_Bank_Deleted {hesap.BankaAdi}"', 'string.Format("Bank account {0} deleted.", hesap.BankaAdi)')

    if 'KuponController' in path:
        content = content.replace('_localizer["Admin_Coupon_InvalidType"].Value', '"Please select a valid discount type."')
        content = content.replace('_localizer["Admin_Coupon_ValueGreaterThanZero"].Value', '"Discount value must be greater than zero."')
        content = content.replace('_localizer["Admin_Coupon_ValueMax100"].Value', '"Percentage discount cannot exceed 100%."')
        content = content.replace('_localizer["Admin_Coupon_MinCartNotNegative"].Value', '"Minimum cart amount cannot be negative."')
        content = content.replace('_localizer["Admin_Coupon_LimitNotNegative"].Value', '"Usage limit cannot be negative. Enter 0 for unlimited."')
        content = content.replace('_localizer["Admin_Coupon_InvalidDate"].Value', '"Please select a valid expiration date."')
        content = content.replace('_localizer["Admin_Coupon_CodeInUse"].Value', '"This discount code is already in use."')
        content = content.replace('$"Admin_Coupon_Created {model.Kod}"', 'string.Format("Coupon {0} created.", model.Kod)')
        content = content.replace('$"Admin_Coupon_Updated {kupon.Kod}"', 'string.Format("Coupon {0} updated.", kupon.Kod)')
        content = content.replace('$"Admin_Coupon_Archived {kupon.Kod}"', 'string.Format("Coupon {0} archived.", kupon.Kod)')

    with open(path, 'w', encoding='utf-8') as f:
        f.write(content)

fix_file('FilistinProje.Web/Areas/Admin/Controllers/BankalarController.cs')
fix_file('FilistinProje.Web/Areas/Admin/Controllers/KuponController.cs')
