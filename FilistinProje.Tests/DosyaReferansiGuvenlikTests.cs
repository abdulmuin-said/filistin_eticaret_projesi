using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;

namespace FilistinProje.Tests;

public sealed class DosyaReferansiGuvenlikTests
{
    [Fact]
    public void TemporaryReference_ValidSessionBoundReference_IsParsed()
    {
        var storageKey = new string('a', 32);
        var reference = $"temporary://receteler/{storageKey}/{Guid.NewGuid():N}.pdf";

        var valid = DosyaServisi.TryParseTemporaryReference(
            reference,
            out var category,
            out var parsedStorageKey,
            out var fileName);

        Assert.True(valid);
        Assert.Equal(HassasBelgeKategorisi.Recete, category);
        Assert.Equal(storageKey, parsedStorageKey);
        Assert.EndsWith(".pdf", fileName);
    }

    [Theory]
    [InlineData("temporary://receteler/../../secret.pdf")]
    [InlineData("temporary://receteler/not-a-session/file.pdf")]
    [InlineData("temporary://unknown/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/file.pdf")]
    [InlineData("temporary://kimlikler/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/../file.png")]
    public void TemporaryReference_PathTraversalOrInvalidShape_IsRejected(string reference)
    {
        Assert.False(DosyaServisi.TryParseTemporaryReference(reference, out _, out _, out _));
    }

    [Theory]
    [InlineData("private://faturalar/00000000000000000000000000000000.pdf", HassasBelgeKategorisi.Fatura)]
    [InlineData("private://kimlikler/00000000000000000000000000000000.png", HassasBelgeKategorisi.Kimlik)]
    public void PrivateReference_ValidReference_IsParsed(string reference, HassasBelgeKategorisi expected)
    {
        Assert.True(DosyaServisi.TryParsePrivateReference(reference, out var category, out _));
        Assert.Equal(expected, category);
    }

    [Theory]
    [InlineData("private://faturalar/../../secret.pdf")]
    [InlineData("private://faturalar/file.exe")]
    [InlineData("/uploads/invoices/invoice.pdf")]
    public void PrivateReference_UnsafeReference_IsRejected(string reference)
    {
        Assert.False(DosyaServisi.TryParsePrivateReference(reference, out _, out _));
    }
}
