# Performance Cache Notes

- `SiteSettingsService` caches site settings with key `site-settings` for 5 minutes and removes it immediately after admin settings save.
- `CategoryMenu` caches public menu categories with keys `category-menu:v1:{culture}` for 10 minutes. Admin category create/update/delete removes all `category-menu:v1:` keys.
- `HomePageSectionService` caches active homepage sections with key `home-page-sections` for 5 minutes and clears it after section create/update/delete/product assignment/toggle.
- Do not cache user-specific data such as cart contents, authenticated prices, permissions, checkout totals, or session state in these global keys.
- Current cache is `IMemoryCache`, so invalidation is per application instance. In multi-instance production, use a distributed cache or publish invalidation messages between instances before relying on cross-node freshness.
