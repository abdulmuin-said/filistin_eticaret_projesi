sed -i 's/_logger\.LogError(ex, "Kargo hesaplama hatası");/_logger.LogError(ex, "Shipping calculation error.");/g' FilistinProje.Web/Controllers/SiparisController.cs
sed -i 's/_logger\.LogError(ex, "Kimlik fotografi yuklenirken hata olustu");/_logger.LogError(ex, "Error uploading identity photo.");/g' FilistinProje.Web/Controllers/SiparisController.cs
sed -i 's/_logger\.LogError(ex, "Reçete yüklenirken hata oluştu");/_logger.LogError(ex, "Error uploading prescription.");/g' FilistinProje.Web/Controllers/SiparisController.cs
sed -i 's/_logger\.LogError(ex, "PDF fatura olusturma hatasi. SiparisId={SiparisId}", id);/_logger.LogError(ex, "Error generating PDF invoice. OrderId={OrderId}", id);/g' FilistinProje.Web/Areas/Admin/Controllers/SiparisController.cs
