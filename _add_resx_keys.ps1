$files = @(
    @{ Path = "KanvasProje.Web\Resources\SharedResource.tr.resx"; Lang = "tr" },
    @{ Path = "KanvasProje.Web\Resources\SharedResource.en.resx"; Lang = "en" },
    @{ Path = "KanvasProje.Web\Resources\SharedResource.ar.resx"; Lang = "ar" }
)

$keys = @(
    @{ Name = "StorePickup"; Tr = "Mağazadan Teslim"; En = "Store Pickup"; Ar = "الاستلام من المتجر" }
    @{ Name = "StorePickupDesc"; Tr = "Ürünlerinizi mağazamızdan teslim alın, kargo ücreti yok"; En = "Pick up your order from our store, no shipping fee"; Ar = "استلم طلبك من متجرنا، بدون رسوم شحن" }
    @{ Name = "DeliverToAddress"; Tr = "Adrese Teslim"; En = "Deliver to Address"; Ar = "توصيل إلى العنوان" }
    @{ Name = "DeliverToAddressDesc"; Tr = "Kargo ile adresinize teslim edilsin"; En = "Have it delivered to your address via courier"; Ar = "يتم توصيله إلى عنوانك عن طريق البريد" }
    @{ Name = "StorePickupInfo"; Tr = "Mağazadan Teslim"; En = "Store Pickup"; Ar = "الاستلام من المتجر" }
    @{ Name = "StorePickupAddressInfo"; Tr = "Siparişiniz hazır olduğunda mağazamızdan teslim alabilirsiniz. Size sms ve e-posta ile bildirim göndereceğiz."; En = "You can pick up your order from our store when ready. We will notify you via SMS and email."; Ar = "يمكنك استلام طلبك من متجرنا عندما يكون جاهزًا. سنقوم بإعلامك عبر الرسائل القصيرة والبريد الإلكتروني." }
    @{ Name = "StorePickupCity"; Tr = "Mağaza"; En = "Store"; Ar = "المتجر" }
    @{ Name = "StorePickupDistrict"; Tr = "Teslim Noktası"; En = "Pickup Point"; Ar = "نقطة الاستلام" }
    @{ Name = "StorePickupAddress"; Tr = "Mağazadan Teslim"; En = "Store Pickup"; Ar = "استلام من المتجر" }
)

foreach ($f in $files) {
    [xml]$xml = Get-Content $f.Path -Encoding UTF8
    $root = $xml.documentElement
    
    foreach ($k in $keys) {
        $elem = $root.CreateElement("data")
        $elem.SetAttribute("name", $k.Name)
        $elem.SetAttribute("xml:space", "preserve")
        $val = $root.CreateElement("value")
        
        if ($f.Lang -eq "tr") { $val.InnerText = $k.Tr }
        elseif ($f.Lang -eq "en") { $val.InnerText = $k.En }
        elseif ($f.Lang -eq "ar") { $val.InnerText = $k.Ar }
        
        $elem.AppendChild($val) | Out-Null
        $root.AppendChild($elem) | Out-Null
    }
    
    $xml.Save($f.Path)
    Write-Host "$($f.Path) updated"
}

Write-Host "All resource files updated"
