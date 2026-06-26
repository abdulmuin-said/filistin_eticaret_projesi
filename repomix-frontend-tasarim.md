This file is a merged representation of a subset of the codebase, containing specifically included files and files not matching ignore patterns, combined into a single document by Repomix.

# File Summary

## Purpose
This file contains a packed representation of a subset of the repository's contents that is considered the most important context.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.

## File Format
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Repository files (if enabled)
5. Multiple file entries, each consisting of:
  a. A header with the file path (## File: path/to/file)
  b. The full contents of the file in a code block

## Usage Guidelines
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.

## Notes
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Only files matching these patterns are included: KanvasProje.Web/**/*.js, KanvasProje.Web/**/*.css
- Files matching these patterns are excluded: **/wwwroot/lib/**
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Files are sorted by Git change count (files with more changes are at the bottom)

# Directory Structure
```
KanvasProje.Web/Styles/storefront.css
KanvasProje.Web/tailwind.config.js
KanvasProje.Web/Views/Shared/_Layout.cshtml.css
KanvasProje.Web/wwwroot/css/admin.css
KanvasProje.Web/wwwroot/css/meteor-galeri.css
KanvasProje.Web/wwwroot/css/site.css
KanvasProje.Web/wwwroot/css/storefront.css
KanvasProje.Web/wwwroot/js/admin.js
KanvasProje.Web/wwwroot/js/canvasia-interactions.js
KanvasProje.Web/wwwroot/js/premium-motion.js
KanvasProje.Web/wwwroot/js/site.js
```

# Files

## File: KanvasProje.Web/Styles/storefront.css
```css
@tailwind base;
@tailwind components;
@tailwind utilities;

@layer base {
  :root {
    --sf-ink: #22211f;
    --sf-forest: #26413c;
    --sf-forest-deep: #1a2d29;
    --sf-gold: #af8452;
    --sf-sand: #f4efe7;
    --sf-mist: #e6ddd0;
    --sf-cream: #fbf8f2;
    --sf-border: rgba(34, 33, 31, 0.1);
    --sf-border-strong: rgba(34, 33, 31, 0.18);
    --sf-shadow: 0 18px 45px rgba(34, 33, 31, 0.08);
    --sf-shadow-panel: 0 24px 60px rgba(28, 43, 40, 0.1);
    --sf-header-offset: 12rem;
  }

  html {
    scroll-behavior: smooth;
  }

  body {
    @apply bg-canvasia-cream font-body text-canvasia-ink antialiased;
    background-image:
      radial-gradient(circle at top left, rgba(175, 132, 82, 0.09), transparent 30%),
      linear-gradient(180deg, rgba(255, 255, 255, 0.55), rgba(255, 255, 255, 0));
  }

  main {
    @apply relative;
  }

  h1,
  h2,
  h3,
  h4,
  h5,
  h6 {
    @apply font-heading text-canvasia-ink;
    letter-spacing: -0.02em;
  }

  a {
    @apply transition-colors duration-200;
  }

  img {
    @apply max-w-full;
  }

  input,
  select,
  textarea {
    @apply font-body;
  }
}

@layer components {
  .storefront-shell {
    @apply relative min-h-screen overflow-x-hidden;
  }

  .shell-topbar {
    @apply border-b border-white/10 bg-canvasia-forest-deep text-[0.72rem] uppercase tracking-[0.2em] text-white/85;
  }

  .shell-topbar-inner {
    @apply mx-auto flex max-w-7xl flex-wrap items-center justify-between gap-3 px-4 py-3 sm:px-6 lg:px-8;
  }

  .shell-topbar-copy {
    @apply flex items-center gap-2;
  }

  .shell-topbar-meta {
    @apply hidden items-center gap-6 lg:flex;
  }

  .shell-header {
    @apply sticky top-0 z-40 border-b border-black/5 bg-white/90 backdrop-blur-xl;
  }

  .shell-header-inner {
    @apply mx-auto flex max-w-7xl items-center justify-between gap-4 px-4 py-4 sm:px-6 lg:px-8;
  }

  .shell-brand {
    @apply flex min-w-0 items-center gap-4;
  }

  .shell-brand-mark {
    @apply flex h-12 items-center justify-center overflow-hidden rounded-full border border-black/5 bg-white px-4 shadow-soft;
  }

  .shell-brand-mark img {
    @apply h-8 w-auto object-contain;
  }

  .shell-brand-copy {
    @apply hidden min-w-0 flex-col sm:flex;
  }

  .shell-brand-title {
    @apply truncate font-heading text-2xl font-semibold leading-none text-canvasia-ink;
  }

  .shell-brand-subtitle {
    @apply truncate text-xs uppercase tracking-[0.2em] text-black/55;
  }

  .shell-header-actions {
    @apply flex items-center gap-2 sm:gap-3;
  }

  .shell-action-link {
    @apply hidden items-center gap-2 rounded-full border border-black/10 bg-white px-4 py-3 text-sm font-semibold text-canvasia-ink shadow-soft hover:border-canvasia-gold hover:text-canvasia-forest lg:inline-flex;
  }

  .shell-icon-button {
    @apply inline-flex h-11 w-11 items-center justify-center rounded-full border border-black/10 bg-white text-canvasia-ink shadow-soft hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .header-cart-pill {
    @apply inline-flex items-center gap-3 rounded-full bg-canvasia-forest px-5 py-3 text-sm font-semibold text-white shadow-panel hover:bg-canvasia-forest-deep;
  }

  .header-cart-count {
    @apply inline-flex h-6 min-w-[1.5rem] items-center justify-center rounded-full bg-white px-1 text-xs font-bold text-canvasia-forest;
  }

  .header-search-shell {
    @apply hidden lg:block;
  }

  .header-search-form {
    @apply flex min-w-[18rem] items-center gap-3 rounded-full border border-black/10 bg-white px-4 py-3 shadow-soft;
  }

  .header-search-form input {
    @apply w-full border-0 bg-transparent p-0 text-sm text-canvasia-ink outline-none ring-0;
  }

  .desktop-nav {
    @apply hidden border-t border-black/5 bg-white/80 lg:block;
  }

  .desktop-nav-inner {
    @apply mx-auto flex max-w-7xl items-center justify-between gap-4 px-4 py-4 sm:px-6 lg:px-8;
  }

  .desktop-nav-links {
    @apply flex flex-wrap items-center gap-6;
  }

  .desktop-nav-link {
    @apply text-[0.82rem] font-semibold uppercase tracking-[0.18em] text-black/70 hover:text-canvasia-forest;
  }

  .desktop-nav-link-active {
    @apply text-canvasia-forest;
  }

  .desktop-nav-caption {
    @apply text-xs uppercase tracking-[0.18em] text-black/45;
  }

  .mobile-search-drawer {
    @apply hidden border-t border-black/5 bg-white px-4 py-4 shadow-soft;
  }

  .mobile-search-drawer.active {
    @apply block;
  }

  .mobile-search-form {
    @apply flex items-center gap-3 rounded-[1.5rem] border border-black/10 bg-canvasia-cream px-4 py-3;
  }

  .mobile-search-form input {
    @apply w-full border-0 bg-transparent p-0 text-sm outline-none ring-0;
  }

  .mobile-nav {
    @apply fixed inset-x-0 bottom-0 z-40 border-t border-black/10 bg-white/95 px-4 py-3 shadow-[0_-10px_30px_rgba(34,33,31,0.12)] backdrop-blur-xl lg:hidden;
  }

  .mobile-nav-grid {
    @apply mx-auto grid max-w-lg grid-cols-5 gap-2;
  }

  .mobile-nav-link {
    @apply flex flex-col items-center gap-1 rounded-2xl px-2 py-2 text-[0.68rem] font-semibold uppercase tracking-[0.08em] text-black/55;
  }

  .mobile-nav-link.active {
    @apply bg-canvasia-sand text-canvasia-forest;
  }

  .mobile-drawer-scrim {
    @apply pointer-events-none fixed inset-0 z-40 bg-black/40 opacity-0 transition-opacity duration-200;
  }

  .mobile-drawer-scrim.active {
    @apply pointer-events-auto opacity-100;
  }

  .mobile-category-drawer {
    @apply fixed inset-y-0 left-0 z-50 flex w-full max-w-sm -translate-x-full flex-col bg-white p-6 shadow-panel transition-transform duration-300;
  }

  .mobile-category-drawer.active {
    @apply translate-x-0;
  }

  .drawer-head {
    @apply mb-6 flex items-center justify-between gap-4;
  }

  .drawer-kicker {
    @apply block text-[0.72rem] font-semibold uppercase tracking-[0.22em] text-canvasia-gold;
  }

  .drawer-title {
    @apply text-3xl font-semibold;
  }

  .drawer-grid {
    @apply grid gap-3 overflow-y-auto pr-1;
  }

  .drawer-link {
    @apply flex items-center justify-between rounded-[1.4rem] border border-black/10 bg-canvasia-cream px-4 py-4 text-sm font-semibold text-canvasia-ink shadow-soft hover:border-canvasia-gold hover:bg-white;
  }

  .shell-footer {
    @apply mt-20 border-t border-black/5 bg-white;
  }

  .shell-footer-inner {
    @apply mx-auto max-w-7xl px-4 py-16 sm:px-6 lg:px-8;
  }

  .footer-top {
    @apply grid gap-8 border-b border-black/5 pb-10 lg:grid-cols-[1.5fr_1fr];
  }

  .footer-brand {
    @apply space-y-5;
  }

  .footer-brand-copy {
    @apply max-w-2xl text-sm leading-7 text-black/65;
  }

  .footer-socials {
    @apply flex flex-wrap items-center gap-3;
  }

  .footer-socials a {
    @apply inline-flex h-11 w-11 items-center justify-center rounded-full border border-black/10 text-canvasia-ink hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .footer-trust-list {
    @apply grid gap-3 sm:grid-cols-3 lg:grid-cols-1;
  }

  .footer-trust-card {
    @apply flex items-start gap-3 rounded-[1.5rem] border border-black/10 bg-canvasia-cream px-4 py-4;
  }

  .footer-columns {
    @apply grid gap-8 pt-10 md:grid-cols-2 xl:grid-cols-4;
  }

  .footer-title {
    @apply mb-4 text-2xl font-semibold;
  }

  .footer-list {
    @apply space-y-3 text-sm text-black/65;
  }

  .footer-list a {
    @apply hover:text-canvasia-forest;
  }

  .footer-bottom {
    @apply mt-10 flex flex-col gap-4 border-t border-black/5 pt-6 text-sm text-black/55 sm:flex-row sm:items-center sm:justify-between;
  }

  .storefront-main {
    @apply pb-28 lg:pb-0;
  }

  .section-shell {
    @apply mx-auto max-w-7xl px-4 sm:px-6 lg:px-8;
  }

  .editorial-section {
    @apply py-12 lg:py-16;
  }

  .editorial-section-muted {
    @apply rounded-[2rem] border border-black/5 bg-white shadow-soft;
  }

  .section-kicker {
    @apply mb-3 block text-[0.76rem] font-semibold uppercase tracking-[0.24em] text-canvasia-gold;
  }

  .section-heading {
    @apply text-4xl font-semibold leading-tight sm:text-5xl;
  }

  .section-lead {
    @apply mt-4 max-w-2xl text-sm leading-7 text-black/65 sm:text-base;
  }

  .section-link {
    @apply inline-flex items-center gap-2 text-sm font-semibold uppercase tracking-[0.16em] text-canvasia-forest hover:text-canvasia-gold;
  }

  .hero-shell {
    @apply section-shell pt-6 lg:pt-10;
  }

  .hero-grid {
    @apply grid gap-6 lg:grid-cols-[1.05fr_0.95fr];
  }

  .hero-copy-card {
    @apply relative overflow-hidden rounded-[2rem] border border-black/10 bg-white px-6 py-8 shadow-panel sm:px-8 sm:py-10;
  }

  .hero-copy-card::before {
    content: "";
    @apply absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-canvasia-gold via-canvasia-forest to-canvasia-gold;
  }

  .hero-copy-card::after {
    content: "";
    @apply pointer-events-none absolute -right-16 -top-20 h-52 w-52 rounded-full bg-canvasia-sand/80 blur-3xl;
  }

  .hero-actions {
    @apply mt-8 flex flex-wrap items-center gap-3;
  }

  .storefront-button {
    @apply inline-flex items-center justify-center gap-2 rounded-full bg-canvasia-forest px-6 py-3 text-sm font-semibold uppercase tracking-[0.16em] text-white shadow-panel transition hover:bg-canvasia-forest-deep;
  }

  .storefront-button-ghost {
    @apply inline-flex items-center justify-center gap-2 rounded-full border border-black/10 bg-white px-6 py-3 text-sm font-semibold uppercase tracking-[0.16em] text-canvasia-ink shadow-soft transition hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .hero-stats {
    @apply mt-10 grid gap-4 sm:grid-cols-3;
  }

  .hero-stat {
    @apply rounded-[1.5rem] border border-black/10 bg-canvasia-cream px-4 py-4;
  }

  .hero-stat strong {
    @apply block font-heading text-3xl font-semibold text-canvasia-forest;
  }

  .hero-stat span {
    @apply mt-2 block text-xs uppercase tracking-[0.18em] text-black/55;
  }

  .hero-media-card {
    @apply relative overflow-hidden rounded-[2rem] border border-black/10 bg-[#e7dfd4] shadow-panel;
  }

  .hero-media-frame {
    @apply relative h-[22rem] overflow-hidden sm:h-[28rem] lg:h-full;
  }

  .hero-media-slide {
    @apply absolute inset-0 opacity-0 transition-opacity duration-500;
  }

  .hero-media-slide.active {
    @apply opacity-100;
  }

  .hero-media-slide img {
    @apply h-full w-full object-cover;
  }

  .hero-media-overlay {
    @apply pointer-events-none absolute inset-x-0 bottom-0 bg-gradient-to-t from-black/50 via-black/10 to-transparent px-6 py-6 text-white;
  }

  .section-card-grid {
    @apply grid gap-5 sm:grid-cols-2 xl:grid-cols-4;
  }

  .collection-card {
    @apply flex items-center gap-4 rounded-[1.7rem] border border-black/10 bg-white px-5 py-5 shadow-soft transition hover:-translate-y-1 hover:border-canvasia-gold hover:shadow-panel;
  }

  .collection-card-icon {
    @apply inline-flex h-14 w-14 items-center justify-center rounded-full bg-canvasia-sand text-lg text-canvasia-forest;
  }

  .collection-card-copy {
    @apply min-w-0 flex-1;
  }

  .collection-card-copy strong {
    @apply block truncate text-lg font-semibold;
  }

  .collection-card-copy span {
    @apply block text-xs uppercase tracking-[0.18em] text-black/50;
  }

  .feature-grid {
    @apply grid gap-5 md:grid-cols-2 xl:grid-cols-4;
  }

  .feature-card {
    @apply rounded-[1.7rem] border border-black/10 bg-white px-6 py-7 shadow-soft;
  }

  .feature-icon {
    @apply mb-5 inline-flex h-14 w-14 items-center justify-center rounded-full bg-canvasia-sand text-xl text-canvasia-forest;
  }

  .feature-title {
    @apply text-2xl font-semibold;
  }

  .feature-copy {
    @apply mt-3 text-sm leading-7 text-black/65;
  }

  .product-grid {
    @apply grid gap-5 sm:grid-cols-2 xl:grid-cols-4;
  }

  .pro-product-card {
    @apply relative flex h-full flex-col overflow-hidden rounded-[1.75rem] border border-black/10 bg-white shadow-soft transition duration-300 hover:-translate-y-1 hover:border-canvasia-gold hover:shadow-panel;
    transform-style: preserve-3d;
  }

  .product-badges {
    @apply pointer-events-none absolute left-4 top-4 z-20 flex flex-col gap-2;
  }

  .badge-item {
    @apply inline-flex items-center rounded-full px-3 py-1 text-[0.68rem] font-semibold uppercase tracking-[0.16em];
  }

  .badge-discount {
    @apply bg-rose-100 text-rose-700;
  }

  .badge-new {
    @apply bg-emerald-100 text-emerald-700;
  }

  .badge-bestseller {
    @apply bg-amber-100 text-amber-700;
  }

  .like-btn,
  .zoom-btn {
    @apply absolute z-20 inline-flex h-11 w-11 items-center justify-center rounded-full border border-white/70 bg-white/90 text-canvasia-ink shadow-soft backdrop-blur hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .like-btn {
    @apply right-4 top-4;
  }

  .zoom-btn {
    @apply left-4 top-4;
  }

  .pro-img-container {
    @apply relative block overflow-hidden bg-canvasia-sand;
    aspect-ratio: 4 / 4.45;
  }

  .pro-img {
    @apply h-full w-full object-cover transition duration-500;
  }

  .pro-product-card:hover .pro-img {
    transform: scale(1.05);
  }

  .card-glare {
    @apply pointer-events-none absolute inset-0 z-10 opacity-0 transition-opacity duration-300;
  }

  .card-info {
    @apply flex flex-1 flex-col gap-4 px-5 py-5;
  }

  .product-title {
    @apply line-clamp-2 text-xl font-semibold leading-snug text-canvasia-ink hover:text-canvasia-forest;
  }

  .price-wrapper {
    @apply flex flex-wrap items-end gap-2;
  }

  .old-price {
    @apply text-sm text-black/45 line-through;
  }

  .new-price {
    @apply text-2xl font-bold text-canvasia-forest;
  }

  .neon-btn-wrapper {
    @apply mt-auto inline-flex w-full items-center justify-center rounded-full bg-canvasia-forest px-5 py-3 text-sm font-semibold uppercase tracking-[0.14em] text-white shadow-panel transition hover:bg-canvasia-forest-deep;
  }

  .neon-btn-inner {
    @apply inline-flex items-center justify-center gap-2;
  }

  .catalog-header {
    @apply section-shell pt-8;
  }

  .catalog-intro {
    @apply rounded-[2rem] border border-black/10 bg-white px-6 py-8 shadow-soft sm:px-8;
  }

  .catalog-chip-row {
    @apply mt-6 flex gap-3 overflow-x-auto pb-2;
    scrollbar-width: none;
  }

  .catalog-chip-row::-webkit-scrollbar {
    display: none;
  }

  .cat-chip {
    @apply inline-flex shrink-0 items-center gap-2 rounded-full border border-black/10 bg-white px-4 py-3 text-xs font-semibold uppercase tracking-[0.14em] text-black/60 shadow-soft hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .cat-chip.active {
    @apply border-canvasia-forest bg-canvasia-forest text-white;
  }

  .filter-bar {
    @apply sticky z-30 mt-6 border-y border-black/5 bg-white/90 backdrop-blur-xl;
    top: 4.75rem;
  }

  .filter-bar.scrolled {
    box-shadow: var(--sf-shadow);
  }

  .filter-bar-inner {
    @apply flex flex-col gap-4 py-4 lg:flex-row lg:items-center lg:justify-between;
  }

  .result-count {
    @apply text-sm text-black/55;
  }

  .filter-controls {
    @apply flex flex-wrap items-center gap-2;
  }

  .active-filter-tag {
    @apply inline-flex items-center gap-2 rounded-full bg-canvasia-sand px-3 py-2 text-xs font-semibold uppercase tracking-[0.12em] text-canvasia-forest;
  }

  .remove-filter {
    @apply inline-flex h-5 w-5 items-center justify-center rounded-full bg-white text-[0.65rem] text-canvasia-forest hover:bg-black hover:text-white;
  }

  .custom-dropdown {
    @apply relative;
  }

  .filter-dropdown-btn {
    @apply inline-flex items-center gap-2 rounded-full border border-black/10 bg-white px-4 py-3 text-xs font-semibold uppercase tracking-[0.12em] text-canvasia-ink shadow-soft hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .custom-dropdown-menu {
    @apply pointer-events-none absolute right-0 top-full z-30 mt-3 min-w-[15rem] translate-y-2 rounded-[1.5rem] border border-black/10 bg-white p-3 opacity-0 shadow-panel transition duration-200;
  }

  .custom-dropdown.open .custom-dropdown-menu {
    @apply pointer-events-auto translate-y-0 opacity-100;
  }

  .sort-menu {
    @apply space-y-1;
  }

  .sort-item,
  .filter-option-btn {
    @apply flex w-full items-center justify-between rounded-2xl px-4 py-3 text-sm font-medium text-canvasia-ink hover:bg-canvasia-sand hover:text-canvasia-forest;
  }

  .sort-item.active,
  .filter-option-btn.active {
    @apply bg-canvasia-sand text-canvasia-forest;
  }

  .filter-option-meta {
    @apply text-xs uppercase tracking-[0.12em] text-black/40;
  }

  .price-filter-popup {
    @apply space-y-3;
  }

  .price-input-group {
    @apply flex items-center gap-3;
  }

  .price-input,
  .coupon-input,
  .auth-input,
  .auth-select,
  .checkout-input,
  .checkout-select {
    @apply w-full rounded-2xl border border-black/10 bg-canvasia-cream px-4 py-3 text-sm text-canvasia-ink outline-none transition focus:border-canvasia-gold focus:bg-white;
  }

  .empty-state {
    @apply rounded-[2rem] border border-dashed border-black/10 bg-white px-6 py-16 text-center shadow-soft;
  }

  .empty-state-icon {
    @apply mx-auto mb-5 inline-flex h-16 w-16 items-center justify-center rounded-full bg-canvasia-sand text-xl text-canvasia-forest;
  }

  .load-more-wrapper {
    @apply mt-10 text-center;
  }

  .load-more-btn {
    @apply inline-flex items-center justify-center gap-2 rounded-full border border-black/10 bg-white px-6 py-3 text-xs font-semibold uppercase tracking-[0.16em] text-canvasia-ink shadow-soft hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .page-progress {
    @apply mt-4 text-xs uppercase tracking-[0.16em] text-black/45;
  }

  .page-progress-bar {
    @apply mx-auto mt-3 h-1.5 w-32 overflow-hidden rounded-full bg-black/10;
  }

  .page-progress-fill {
    @apply h-full rounded-full bg-canvasia-forest;
  }

  .product-page-shell {
    @apply section-shell py-8;
  }

  .product-breadcrumb {
    @apply mb-6 text-xs uppercase tracking-[0.14em] text-black/45;
  }

  .product-breadcrumb a {
    @apply hover:text-canvasia-forest;
  }

  .product-detail-grid {
    @apply grid gap-8 lg:grid-cols-[1.05fr_0.95fr];
  }

  .product-gallery-panel,
  .product-copy-panel,
  .storefront-panel,
  .order-summary,
  .auth-card,
  .checkout-section-card,
  .checkout-summary-card {
    @apply rounded-[2rem] border border-black/10 bg-white shadow-soft;
  }

  .product-gallery-panel,
  .product-copy-panel {
    @apply overflow-hidden;
  }

  .product-main-image {
    @apply h-[28rem] w-full bg-canvasia-sand object-contain p-6 sm:h-[36rem];
  }

  .product-gallery {
    @apply relative;
  }

  .breadcrumb-urun {
    @apply product-breadcrumb;
  }

  .breadcrumb-urun .breadcrumb {
    @apply mb-0 flex flex-wrap items-center gap-2 bg-transparent p-0;
  }

  .breadcrumb-urun .breadcrumb-item {
    @apply text-xs uppercase tracking-[0.14em] text-black/45;
  }

  .breadcrumb-urun .breadcrumb-item + .breadcrumb-item::before {
    @apply pr-2 text-black/30;
    content: "/";
  }

  .zoom-btn {
    @apply absolute left-5 top-5 z-10 inline-flex h-12 w-12 items-center justify-center rounded-full border border-black/10 bg-white/90 text-canvasia-forest shadow-soft backdrop-blur hover:border-canvasia-gold hover:text-canvasia-gold;
  }

  .thumbnail-container {
    @apply flex gap-3 overflow-x-auto px-6 py-5;
    scrollbar-width: none;
  }

  .thumbnail-container::-webkit-scrollbar {
    display: none;
  }

  .thumbnail-item {
    @apply shrink-0 overflow-hidden rounded-[1.1rem] border border-black/10 bg-canvasia-cream p-1 transition hover:border-canvasia-gold;
    width: 5.5rem;
    height: 5.5rem;
  }

  .thumbnail-item.active {
    @apply border-canvasia-forest;
  }

  .thumbnail-img {
    @apply h-full w-full rounded-[0.95rem] object-cover;
  }

  .product-copy-panel {
    @apply px-6 py-7 sm:px-8;
  }

  .product-header h1 {
    @apply text-4xl font-semibold leading-tight sm:text-5xl;
  }

  .price-container {
    @apply mt-5 flex flex-wrap items-end gap-3;
  }

  .current-price {
    @apply text-4xl font-bold text-canvasia-forest;
  }

  .discount-badge {
    @apply inline-flex rounded-full bg-canvasia-sand px-3 py-1 text-xs font-semibold uppercase tracking-[0.14em] text-canvasia-gold;
  }

  .variant-selection,
  .quantity-selector,
  .product-description-section {
    @apply mt-8;
  }

  .variant-label,
  .description-title {
    @apply mb-3 text-2xl font-semibold;
  }

  .variant-options {
    @apply flex flex-wrap gap-3;
  }

  .variant-radio {
    @apply sr-only;
  }

  .variant-box {
    @apply flex min-w-[11rem] flex-col gap-1 rounded-[1.3rem] border border-black/10 bg-canvasia-cream px-4 py-4 text-left text-sm font-semibold text-canvasia-ink transition hover:border-canvasia-gold;
  }

  .variant-radio:checked + .variant-box {
    @apply border-canvasia-forest bg-canvasia-sand text-canvasia-forest;
  }

  .variant-option.disabled .variant-box,
  .variant-radio:disabled + .variant-box {
    @apply cursor-not-allowed border-black/5 bg-black/5 text-black/35;
  }

  .variant-meta {
    @apply text-xs font-medium uppercase tracking-[0.12em] text-black/45;
  }

  .quantity-selector .d-flex {
    @apply flex items-center gap-3;
  }

  .quantity-btn {
    @apply inline-flex h-11 w-11 items-center justify-center rounded-full border border-black/10 bg-white text-lg text-canvasia-ink hover:border-canvasia-gold hover:text-canvasia-forest;
  }

  .quantity-input {
    @apply h-11 w-20 rounded-full border border-black/10 bg-canvasia-cream text-center text-sm font-semibold;
  }

  .trust-badges {
    @apply mt-8 grid gap-4 sm:grid-cols-3;
  }

  .trust-badges .badge-item {
    @apply flex flex-col items-start rounded-[1.4rem] border border-black/10 bg-canvasia-cream px-4 py-4 text-left normal-case tracking-normal;
  }

  .badge-icon {
    @apply mb-3 inline-flex h-10 w-10 items-center justify-center rounded-full bg-white text-canvasia-forest;
  }

  .badge-text {
    @apply text-base font-semibold text-canvasia-ink;
  }

  .badge-subtext {
    @apply mt-1 text-sm text-black/55;
  }

  .description-content {
    @apply text-sm leading-7 text-black/65;
  }

  .persuasion-features {
    @apply mt-8 grid gap-4;
  }

  .feature-item {
    @apply flex items-start gap-4 rounded-[1.5rem] border border-black/10 bg-canvasia-cream px-5 py-5;
  }

  .feature-icon {
    @apply inline-flex h-12 w-12 shrink-0 items-center justify-center rounded-full bg-white text-canvasia-forest;
  }

  .feature-content h5 {
    @apply text-lg font-semibold;
  }

  .feature-content p {
    @apply mt-1 text-sm leading-6 text-black/55;
  }

  .mobile-lightbox {
    @apply pointer-events-none fixed inset-0 z-[80] flex flex-col bg-black/95 opacity-0 transition;
  }

  .mobile-lightbox.active {
    @apply pointer-events-auto opacity-100;
  }

  .lightbox-controls,
  .lightbox-controls-desktop {
    @apply flex items-center justify-between gap-3 p-4;
  }

  .lightbox-btn,
  .lightbox-control-btn,
  .zoom-btn-mobile {
    @apply inline-flex h-11 w-11 items-center justify-center rounded-full border border-white/10 bg-white/10 text-white backdrop-blur transition hover:bg-white hover:text-canvasia-forest;
  }

  .lightbox-control-btn.close-btn {
    @apply bg-white text-canvasia-forest;
  }

  .lightbox-img-container {
    @apply relative flex min-h-0 flex-1 items-center justify-center overflow-hidden p-4 sm:p-8;
  }

  .lightbox-image {
    @apply max-h-full w-auto max-w-full object-contain transition;
    transform-origin: center center;
  }

  .zoom-controls {
    @apply flex items-center justify-center gap-4 p-4 text-white;
  }

  .zoom-level {
    @apply min-w-[4rem] text-center text-sm font-semibold uppercase tracking-[0.14em] text-white/70;
  }

  .modal-lightbox .modal-dialog {
    @apply max-w-6xl;
  }

  .modal-lightbox .modal-content {
    @apply overflow-hidden rounded-[2rem] border-0 bg-black;
  }

  .technical-spec-table {
    @apply w-full border-collapse overflow-hidden rounded-[1.5rem] border border-black/10;
  }

  .technical-spec-table th,
  .technical-spec-table td {
    @apply border-b border-black/10 px-4 py-3 text-left text-sm;
  }

  .technical-spec-table th {
    @apply w-[34%] bg-canvasia-cream font-semibold text-canvasia-ink;
  }

  .media-showcase-section,
  .reviews-section {
    @apply mt-12 space-y-8;
  }

  .media-showcase-block {
    @apply rounded-[2rem] border border-black/10 bg-white px-6 py-7 shadow-soft;
  }

  .media-showcase-card {
    @apply overflow-hidden rounded-[1.5rem] border border-black/10 bg-canvasia-cream;
  }

  .media-showcase-frame {
    @apply overflow-hidden bg-white;
    aspect-ratio: 1 / 0.85;
  }

  .media-showcase-frame img,
  .media-showcase-frame video,
  .media-showcase-frame iframe {
    @apply h-full w-full object-cover;
  }

  .media-showcase-body {
    @apply space-y-2 px-4 py-4;
  }

  .media-showcase-body h5 {
    @apply text-xl font-semibold;
  }

  .media-showcase-tags {
    @apply text-xs uppercase tracking-[0.16em] text-black/45;
  }

  .review-avatar {
    @apply mr-4 inline-flex h-12 w-12 shrink-0 items-center justify-center rounded-full bg-canvasia-sand font-semibold text-canvasia-forest;
  }

  .auth-shell {
    @apply section-shell py-10 lg:py-16;
  }

  .auth-card {
    @apply overflow-hidden;
  }

  .auth-card-body,
  .auth-form-panel {
    @apply px-6 py-8 sm:px-8 sm:py-10;
  }

  .auth-card-body {
    @apply bg-canvasia-forest text-white;
  }

  .auth-form-panel {
    @apply bg-white;
  }

  .auth-kicker {
    @apply inline-flex items-center gap-2 text-[0.72rem] font-semibold uppercase tracking-[0.22em] text-canvasia-gold;
  }

  .auth-title {
    @apply mt-5 text-4xl font-semibold leading-tight text-white;
  }

  .auth-copy {
    @apply mt-4 text-sm leading-7 text-white/75;
  }

  .auth-metrics {
    @apply mt-8 space-y-4;
  }

  .auth-metric {
    @apply flex items-start gap-4 rounded-[1.5rem] border border-white/10 bg-white/5 px-4 py-4;
  }

  .auth-metric-icon {
    @apply inline-flex h-11 w-11 shrink-0 items-center justify-center rounded-full bg-white/10 text-canvasia-gold;
  }

  .auth-field {
    @apply space-y-2;
  }

  .auth-field label,
  .checkout-label {
    @apply text-sm font-semibold text-canvasia-ink;
  }

  .auth-input-wrap {
    @apply flex items-center gap-3 rounded-2xl border border-black/10 bg-canvasia-cream px-4 py-3 transition focus-within:border-canvasia-gold focus-within:bg-white;
  }

  .auth-input-wrap i {
    @apply text-black/35;
  }

  .auth-input,
  .auth-select {
    @apply border-0 bg-transparent p-0;
  }

  .auth-actions {
    @apply mt-4 flex flex-wrap items-center justify-between gap-3 text-sm;
  }

  .auth-link {
    @apply font-semibold text-canvasia-forest hover:text-canvasia-gold;
  }

  .btn-premium {
    @apply inline-flex items-center justify-center gap-2 rounded-full bg-canvasia-forest px-6 py-3 text-sm font-semibold uppercase tracking-[0.16em] text-white shadow-panel transition hover:bg-canvasia-forest-deep;
  }

  .auth-footer {
    @apply mt-6 text-sm text-black/55;
  }

  .cart-page {
    @apply section-shell py-10 lg:py-14;
  }

  .cart-breadcrumb {
    @apply mb-3 text-xs uppercase tracking-[0.16em] text-black/45;
  }

  .cart-breadcrumb a {
    @apply hover:text-canvasia-forest;
  }

  .cart-title {
    @apply text-4xl font-semibold;
  }

  .cart-count {
    @apply mt-2 text-sm text-black/55;
  }

  .empty-cart {
    @apply rounded-[2rem] border border-dashed border-black/10 bg-white px-6 py-16 text-center shadow-soft;
  }

  .empty-cart-circle {
    @apply mx-auto mb-5 inline-flex h-20 w-20 items-center justify-center rounded-full bg-canvasia-sand text-2xl text-canvasia-forest;
  }

  .shop-now-btn {
    @apply inline-flex items-center justify-center gap-2 rounded-full bg-canvasia-forest px-6 py-3 text-sm font-semibold uppercase tracking-[0.14em] text-white shadow-panel hover:bg-canvasia-forest-deep;
  }

  .cart-item {
    @apply relative mb-4 flex gap-4 rounded-[1.75rem] border border-black/10 bg-white p-5 shadow-soft;
  }

  .cart-item-img {
    @apply h-24 w-24 shrink-0 rounded-[1.2rem] object-cover;
  }

  .cart-item-body {
    @apply flex min-w-0 flex-1 flex-col gap-3;
  }

  .cart-item-title {
    @apply line-clamp-2 text-lg font-semibold leading-snug;
  }

  .cart-item-variant {
    @apply inline-flex self-start rounded-full bg-canvasia-sand px-3 py-1 text-xs font-semibold uppercase tracking-[0.14em] text-canvasia-forest;
  }

  .cart-item-bottom {
    @apply mt-auto flex flex-wrap items-center justify-between gap-3;
  }

  .cart-item-price {
    @apply text-xl font-bold text-canvasia-forest;
  }

  .qty-control {
    @apply inline-flex items-center overflow-hidden rounded-full border border-black/10 bg-canvasia-cream;
  }

  .qty-btn {
    @apply inline-flex h-10 w-10 items-center justify-center text-sm text-canvasia-ink hover:bg-white hover:text-canvasia-forest;
  }

  .qty-num {
    @apply inline-flex h-10 min-w-[2.5rem] items-center justify-center border-x border-black/10 bg-white px-3 text-sm font-semibold;
  }

  .cart-item-delete {
    @apply inline-flex h-9 w-9 items-center justify-center rounded-full text-black/35 hover:bg-rose-50 hover:text-rose-600;
  }

  .summary-head {
    @apply border-b border-black/5 bg-canvasia-cream px-6 py-5 text-lg font-semibold;
  }

  .order-summary {
    @apply sticky top-28 overflow-hidden;
  }

  .summary-body {
    @apply px-6 py-6;
  }

  .summary-line {
    @apply flex items-center justify-between gap-4 py-2 text-sm;
  }

  .summary-line .label {
    @apply text-black/55;
  }

  .summary-line .value {
    @apply font-semibold text-canvasia-ink;
  }

  .summary-line.discount .value,
  .summary-line.free .value {
    @apply text-emerald-600;
  }

  .summary-line.total {
    @apply mt-4 border-t border-black/10 pt-4;
  }

  .summary-line.total .label,
  .summary-line.total .value {
    @apply text-base;
  }

  .summary-line.total .value {
    @apply text-2xl text-canvasia-forest;
  }

  .coupon-wrap {
    @apply mt-5 border-t border-black/5 pt-5;
  }

  .coupon-row {
    @apply flex gap-3;
  }

  .coupon-apply,
  .checkout-btn,
  .checkout-submit {
    @apply inline-flex items-center justify-center gap-2 rounded-full bg-canvasia-forest px-5 py-3 text-sm font-semibold uppercase tracking-[0.14em] text-white shadow-panel hover:bg-canvasia-forest-deep;
  }

  .checkout-btn,
  .checkout-submit {
    @apply w-full;
  }

  .secure-note {
    @apply mt-4 flex items-center justify-center gap-2 text-xs uppercase tracking-[0.16em] text-black/45;
  }

  .mobile-checkout-bar {
    @apply fixed inset-x-0 bottom-0 z-40 border-t border-black/10 bg-white/95 px-4 py-3 shadow-[0_-10px_30px_rgba(34,33,31,0.12)] backdrop-blur-xl lg:hidden;
  }

  .mobile-checkout-inner {
    @apply mx-auto flex max-w-lg items-center justify-between gap-4;
  }

  .mobile-total-label {
    @apply text-[0.72rem] uppercase tracking-[0.16em] text-black/45;
  }

  .mobile-total-price {
    @apply text-xl font-bold text-canvasia-forest;
  }

  .mobile-checkout-btn {
    @apply inline-flex flex-1 items-center justify-center gap-2 rounded-full bg-canvasia-forest px-5 py-3 text-sm font-semibold uppercase tracking-[0.14em] text-white shadow-panel hover:bg-canvasia-forest-deep;
  }

  .checkout-shell {
    @apply section-shell py-10 lg:py-14;
  }

  .checkout-hero {
    @apply mb-8 overflow-hidden rounded-[2rem] bg-canvasia-forest px-6 py-8 text-white shadow-panel sm:px-8;
  }

  .checkout-hero-grid {
    @apply flex flex-col gap-6 lg:flex-row lg:items-end lg:justify-between;
  }

  .checkout-stepper {
    @apply flex items-center gap-3;
  }

  .checkout-step {
    @apply flex items-center gap-3;
  }

  .checkout-step-circle {
    @apply inline-flex h-10 w-10 items-center justify-center rounded-full border border-white/20 bg-white/10 text-sm font-semibold;
  }

  .checkout-step.active .checkout-step-circle,
  .checkout-step.completed .checkout-step-circle {
    @apply bg-white text-canvasia-forest;
  }

  .checkout-step-label {
    @apply text-xs uppercase tracking-[0.18em] text-white/70;
  }

  .checkout-grid {
    @apply grid gap-6 lg:grid-cols-[1.1fr_0.9fr];
  }

  .checkout-section-card,
  .checkout-summary-card {
    @apply overflow-hidden;
  }

  .checkout-section-head {
    @apply border-b border-black/5 bg-canvasia-cream px-6 py-5 text-lg font-semibold;
  }

  .checkout-section-body {
    @apply px-6 py-6;
  }

  .checkout-field-grid {
    @apply grid gap-4 sm:grid-cols-2;
  }

  .checkout-field-grid .span-2 {
    @apply sm:col-span-2;
  }

  .checkout-address-grid {
    @apply grid gap-4 sm:grid-cols-2;
  }

  .checkout-address-card {
    @apply relative rounded-[1.5rem] border border-black/10 bg-canvasia-cream p-4 transition hover:border-canvasia-gold hover:bg-white;
  }

  .checkout-address-card.selected {
    @apply border-canvasia-forest bg-canvasia-sand;
  }

  .checkout-header {
    @apply mb-8 overflow-hidden rounded-[2rem] bg-canvasia-forest px-6 py-8 text-white shadow-panel sm:px-8;
  }

  .stepper {
    @apply flex items-center gap-3;
  }

  .stepper-item {
    @apply flex items-center gap-3;
  }

  .stepper-circle {
    @apply inline-flex h-10 w-10 items-center justify-center rounded-full border border-white/20 bg-white/10 text-sm font-semibold text-white;
  }

  .stepper-item.completed .stepper-circle,
  .stepper-item.active .stepper-circle {
    @apply bg-white text-canvasia-forest;
  }

  .stepper-label {
    @apply text-xs uppercase tracking-[0.18em] text-white/70;
  }

  .stepper-divider {
    @apply h-px w-8 bg-white/20;
  }

  .address-card {
    @apply relative rounded-[1.5rem] border border-black/10 bg-canvasia-cream p-4 transition hover:border-canvasia-gold hover:bg-white;
  }

  .address-card.selected {
    @apply border-canvasia-forest bg-canvasia-sand;
  }

  .radio-circle {
    @apply inline-flex h-5 w-5 rounded-full border-2 border-black/20 bg-white transition;
  }

  .address-card.selected .radio-circle {
    @apply border-canvasia-forest bg-canvasia-forest shadow-[inset_0_0_0_4px_white];
  }

  .address-selected-label {
    @apply absolute right-4 top-4 rounded-full bg-canvasia-forest px-3 py-1 text-[0.65rem] font-semibold uppercase tracking-[0.14em] text-white opacity-0 transition;
  }

  .address-card.selected .address-selected-label {
    @apply opacity-100;
  }

  .payment-method-card {
    @apply rounded-[1.5rem] border border-black/10 bg-canvasia-cream p-5 text-center transition hover:border-canvasia-gold hover:bg-white;
  }

  .payment-method-card.active {
    @apply border-canvasia-forest bg-canvasia-sand;
  }

  .payment-icon {
    @apply text-canvasia-forest;
  }

  .star-rating {
    @apply flex flex-row-reverse justify-end gap-2;
  }

  .star-rating input {
    @apply hidden;
  }

  .star-rating label {
    @apply cursor-pointer text-2xl text-black/20 transition hover:text-amber-400;
  }

  .star-rating input:checked ~ label,
  .star-rating label:hover,
  .star-rating label:hover ~ label {
    @apply text-amber-400;
  }

  .checkout-radio-card {
    @apply rounded-[1.5rem] border border-black/10 bg-canvasia-cream p-5 transition hover:border-canvasia-gold;
  }

  .checkout-radio-card.active {
    @apply border-canvasia-forest bg-canvasia-sand;
  }

  .toast-container {
    @apply fixed right-4 top-4 z-[90] flex w-full max-w-sm flex-col gap-3;
  }

  .toast-message {
    @apply flex items-start gap-3 rounded-[1.4rem] border border-black/10 bg-white px-4 py-4 shadow-panel opacity-0 transition duration-300;
    transform: translateY(-0.75rem);
  }

  .toast-message.show {
    @apply opacity-100;
    transform: translateY(0);
  }

  .toast-icon {
    @apply inline-flex h-10 w-10 items-center justify-center rounded-full bg-canvasia-sand text-canvasia-forest;
  }

  .toast-title {
    @apply text-sm font-semibold uppercase tracking-[0.14em] text-canvasia-ink;
  }

  .toast-text {
    @apply mt-1 text-sm leading-6 text-black/60;
  }

  .scroll-reveal {
    @apply opacity-0 transition duration-700;
    transform: translateY(1.25rem);
  }

  .scroll-reveal.revealed {
    @apply opacity-100;
    transform: translateY(0);
  }
}

@layer utilities {
  .sf-divider {
    @apply h-px w-full bg-black/10;
  }
}

@media (max-width: 1023px) {
  .filter-bar {
    top: 4.25rem;
  }

  .desktop-summary {
    display: none;
  }
}
```

## File: KanvasProje.Web/tailwind.config.js
```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",
    "./Areas/**/*.cshtml",
    "./wwwroot/js/**/*.js",
    "./wwwroot/js/*.js",
    "./*.cshtml"
  ],
  theme: {
    extend: {
      colors: {
        canvasia: {
          ink: "#22211f",
          forest: "#26413c",
          "forest-deep": "#1a2d29",
          gold: "#af8452",
          sand: "#f4efe7",
          mist: "#e6ddd0",
          cream: "#fbf8f2"
        }
      },
      fontFamily: {
        heading: ["Cormorant Garamond", "serif"],
        body: ["Manrope", "sans-serif"]
      },
      boxShadow: {
        soft: "0 18px 45px rgba(34, 33, 31, 0.08)",
        panel: "0 24px 60px rgba(28, 43, 40, 0.1)"
      }
    }
  },
  plugins: []
};
```

## File: KanvasProje.Web/Views/Shared/_Layout.cshtml.css
```css
/* Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
for details on configuring this project to bundle and minify static web assets. */

a.navbar-brand {
  white-space: normal;
  text-align: center;
  word-break: break-all;
}

a {
  color: #0077cc;
}

.btn-primary {
  color: #fff;
  background-color: #1b6ec2;
  border-color: #1861ac;
}

.nav-pills .nav-link.active, .nav-pills .show > .nav-link {
  color: #fff;
  background-color: #1b6ec2;
  border-color: #1861ac;
}

.border-top {
  border-top: 1px solid #e5e5e5;
}
.border-bottom {
  border-bottom: 1px solid #e5e5e5;
}

.box-shadow {
  box-shadow: 0 .25rem .75rem rgba(0, 0, 0, .05);
}

button.accept-policy {
  font-size: 1rem;
  line-height: inherit;
}

.footer {
  position: absolute;
  bottom: 0;
  width: 100%;
  white-space: nowrap;
  line-height: 60px;
}
```

## File: KanvasProje.Web/wwwroot/css/admin.css
```css
/* ============================================================
   CANVASIA ADMIN — Premium Design System
   ============================================================ */

/* --- CSS Custom Properties (Light Theme) --- */
:root {
    /* Primary (Canvasia Green) */
    --ca-primary: #313511;
    --ca-primary-hover: #1c2001;
    --ca-primary-light: rgba(49, 53, 17, .1);
    --ca-primary-rgb: 49, 53, 17;

    /* Accent (Canvasia Gold) */
    --ca-accent: #b58735;
    --ca-accent-hover: #966d35;
    --ca-accent-light: rgba(181, 135, 53, .1);

    /* Semantic */
    --ca-success: #10b981;
    --ca-success-light: #dcfce7;
    --ca-success-border: #bbf7d0;
    --ca-warning: #f59e0b;
    --ca-warning-light: #fff7ed;
    --ca-warning-border: #ffedd5;
    --ca-danger: #ef4444;
    --ca-danger-light: #fee2e2;
    --ca-danger-border: #fecaca;
    --ca-info: #0ea5e9;
    --ca-info-light: #e0f2fe;
    --ca-info-border: #bae6fd;

    /* Neutrals */
    --ca-bg: #fcf9f3;
    --ca-surface: #ffffff;
    --ca-surface-hover: #f1ede7;
    --ca-border: #e5e2dc;
    --ca-border-light: #f1ede7;
    --ca-text: #1c1c18;
    --ca-text-secondary: #47473d;
    --ca-text-muted: #8a8880;

    /* Sidebar */
    --ca-sidebar-bg: #313511;
    --ca-sidebar-hover: #404515;
    --ca-sidebar-active: rgba(255, 255, 255, .1);
    --ca-sidebar-text: #e5e2dc;
    --ca-sidebar-text-active: #ffffff;
    --ca-sidebar-width: 260px;
    --ca-sidebar-collapsed: 0px;

    /* Topbar */
    --ca-topbar-bg: #ffffff;
    --ca-topbar-shadow: 0 1px 3px rgba(0, 0, 0, .03);

    /* Shadows */
    --ca-shadow-sm: 0 4px 12px rgba(49, 53, 17, .04);
    --ca-shadow: 0 8px 24px rgba(49, 53, 17, .06);
    --ca-shadow-lg: 0 16px 32px rgba(49, 53, 17, .08);
    --ca-shadow-xl: 0 24px 48px rgba(49, 53, 17, .1);

    /* Radius (Square/Sharp for premium feel) */
    --ca-radius-sm: 4px;
    --ca-radius: 6px;
    --ca-radius-lg: 8px;
    --ca-radius-xl: 12px;
    --ca-radius-full: 50px;

    /* Transitions */
    --ca-transition: .25s cubic-bezier(.4, 0, .2, 1);

    /* Font */
    --ca-font: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
    --ca-font-serif: 'EB Garamond', Georgia, serif;
}

/* ============================================================
   TOP NAV ADMIN LAYOUT
   ============================================================ */
.ca-admin-shell {
    min-height: 100vh;
    background: #f7f4ee;
    padding-top: 0;
    overflow-x: hidden;
}

.ca-admin-workspace {
    width: min(100%, 1680px);
    margin: 0 auto;
    min-height: calc(100vh - 62px);
}

.ca-partner-header {
    position: sticky;
    top: 0;
    left: 0;
    right: 0;
    z-index: 10000;
    min-height: 62px;
    padding: 0 22px;
    background: rgba(255, 255, 255, .96);
    border-bottom: 1px solid var(--ca-border);
    box-shadow: 0 1px 0 rgba(49, 53, 17, .04), 0 10px 24px rgba(49, 53, 17, .045);
    backdrop-filter: blur(16px);
    display: grid;
    grid-template-columns: auto minmax(0, 1fr) auto;
    align-items: center;
    gap: 12px;
    overflow: visible;
}

.ca-partner-brand {
    display: flex;
    align-items: center;
    gap: 14px;
    min-width: 176px;
    padding-right: 18px;
    border-right: 1px solid var(--ca-border);
}

.ca-partner-logo {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 118px;
}

.ca-partner-logo img {
    width: 118px;
    height: auto;
    display: block;
    object-fit: contain;
}

.ca-admin-mode {
    min-height: 28px;
    padding: 0 10px;
    border-radius: var(--ca-radius-full);
    display: inline-flex;
    align-items: center;
    background: var(--ca-primary);
    color: #fff;
    font-size: .72rem;
    font-weight: 800;
}

.ca-partner-nav {
    display: flex;
    align-items: center;
    gap: 2px;
    min-width: 0;
    max-width: 100%;
    flex-wrap: nowrap;
    padding: 0;
}

.ca-partner-nav::-webkit-scrollbar {
    display: none;
}

.ca-partner-nav-link {
    min-height: 62px;
    border: 1px solid transparent;
    border-radius: 0;
    background: transparent;
    color: var(--ca-text-secondary);
    padding: 0 8px;
    display: inline-flex;
    align-items: center;
    gap: 7px;
    white-space: nowrap;
    font-size: .8rem;
    font-weight: 700;
    position: relative;
    transition: all var(--ca-transition);
}

.ca-partner-nav-link:hover,
.ca-partner-nav-link:focus {
    color: var(--ca-primary);
    background: rgba(49, 53, 17, .045);
    border-color: transparent;
}

.ca-partner-nav-link.active {
    color: var(--ca-primary);
    background: transparent;
    border-color: transparent;
}

.ca-partner-nav-link.active::after {
    content: '';
    position: absolute;
    left: 10px;
    right: 10px;
    bottom: 0;
    height: 3px;
    border-radius: 3px 3px 0 0;
    background: var(--ca-accent);
}

.ca-partner-nav-link i {
    color: var(--ca-accent);
    font-size: .9rem;
}

.ca-partner-nav .dropdown-toggle::after {
    border-top-color: var(--ca-text);
    margin-left: 1px;
    opacity: .85;
}

.ca-top-badge {
    min-width: 20px;
    height: 20px;
    padding: 0 6px;
    border-radius: 999px;
    background: var(--ca-danger);
    color: #fff;
    font-size: .72rem;
    display: inline-flex;
    align-items: center;
    justify-content: center;
}

.ca-top-badge.warning {
    background: var(--ca-warning);
    color: #1c1c18;
}

.ca-top-menu {
    min-width: 246px;
    z-index: 10001 !important;
    padding: 10px !important;
    border: 1px solid var(--ca-border) !important;
    border-radius: var(--ca-radius) !important;
    box-shadow: 0 18px 38px rgba(28, 28, 24, .12) !important;
}

.ca-top-menu .dropdown-item {
    display: flex;
    align-items: center;
    gap: 10px;
    border-radius: var(--ca-radius);
    padding: 10px 12px;
    color: var(--ca-text-secondary);
    font-size: .88rem;
    font-weight: 600;
}

.ca-top-menu .dropdown-item:hover {
    color: var(--ca-primary);
    background: var(--ca-surface-hover);
}

.ca-top-menu .dropdown-item i {
    width: 18px;
    color: var(--ca-accent);
    text-align: center;
}

.ca-partner-actions {
    display: flex;
    align-items: center;
    gap: 8px;
    min-width: 0;
    flex-shrink: 0;
}

.ca-partner-search {
    position: relative;
    width: clamp(170px, 14vw, 230px);
    flex-shrink: 1;
}

.ca-partner-search i {
    position: absolute;
    left: 13px;
    top: 50%;
    transform: translateY(-50%);
    color: var(--ca-text-muted);
    font-size: .82rem;
}

.ca-partner-search input {
    width: 100%;
    min-height: 36px;
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius);
    background: #f9f7f2;
    color: var(--ca-text);
    padding: 8px 14px 8px 36px;
    font-size: .84rem;
    outline: none;
}

.ca-partner-search input:focus {
    background: #fff;
    border-color: var(--ca-primary);
    box-shadow: 0 0 0 3px var(--ca-primary-light);
}

.ca-icon-action,
.ca-user-trigger {
    border: 1px solid var(--ca-border);
    background: #fff;
    color: var(--ca-text-secondary);
    min-height: 36px;
    border-radius: var(--ca-radius);
    display: inline-flex;
    align-items: center;
    justify-content: center;
    transition: all var(--ca-transition);
}

.ca-icon-action {
    width: 36px;
}

.ca-icon-action:hover,
.ca-user-trigger:hover {
    color: var(--ca-primary);
    background: var(--ca-surface-hover);
}

.ca-user-trigger {
    gap: 10px;
    padding: 3px 5px 3px 10px;
}

.ca-user-meta {
    display: flex;
    flex-direction: column;
    line-height: 1.2;
}

.ca-user-meta strong {
    color: var(--ca-text);
    font-size: .82rem;
}

.ca-user-meta small {
    color: var(--ca-text-muted);
    font-size: .72rem;
}

.ca-readonly-chip {
    min-height: 34px;
    padding: 0 10px;
    border-radius: var(--ca-radius-full);
    background: var(--ca-warning-light);
    color: #92400e;
    border: 1px solid var(--ca-warning-border);
    display: inline-flex;
    align-items: center;
    font-size: .78rem;
    font-weight: 700;
}

.ca-admin-pagebar {
    padding: 22px 28px 0;
    display: flex;
    align-items: flex-end;
    justify-content: space-between;
    gap: 16px;
}

.ca-admin-pagebar h1 {
    margin: 2px 0 0;
    font-size: 1.45rem;
    color: var(--ca-text);
    font-weight: 800;
}

.ca-page-kicker {
    color: var(--ca-accent);
    font-size: .72rem;
    font-weight: 800;
    letter-spacing: .12em;
    text-transform: uppercase;
}

.ca-pagebar-status {
    min-height: 36px;
    padding: 0 12px;
    border-radius: var(--ca-radius-full);
    background: var(--ca-success-light);
    color: #047857;
    border: 1px solid var(--ca-success-border);
    display: inline-flex;
    align-items: center;
    gap: 8px;
    font-size: .82rem;
    font-weight: 700;
}

.ca-topnav-content {
    padding-top: 22px;
    position: relative;
    z-index: 1;
}

.ca-topnav-footer {
    margin-left: 0;
}

@media (max-width: 1700px) {
    .ca-partner-search {
        display: none;
    }
}

@media (max-width: 1320px) {
    .ca-partner-header {
        grid-template-columns: auto minmax(0, 1fr) auto;
        align-items: center;
        padding: 0 14px;
        min-height: 58px;
        gap: 8px;
    }

    .ca-partner-nav {
        width: 100%;
        min-height: 58px;
    }

    .ca-partner-nav-link {
        min-height: 58px;
        border-radius: 0;
        padding: 0 7px;
        font-size: .76rem;
    }

    .ca-partner-nav-link.active::after {
        left: 8px;
        right: 8px;
    }

    .ca-partner-search {
        display: none;
    }
}

@media (max-width: 768px) {
    .ca-partner-header {
        padding: 0 10px;
        gap: 8px;
    }

    .ca-partner-brand {
        min-width: 0;
        border-right: none;
        padding-right: 0;
    }

    .ca-partner-logo img {
        width: 98px;
    }

    .ca-admin-mode {
        display: none;
    }

    .ca-partner-nav-link .ca-nav-label {
        display: none;
    }

    .ca-partner-nav-link {
        width: 42px;
        justify-content: center;
        padding: 0;
        gap: 0;
    }

    .ca-top-badge {
        position: absolute;
        top: 8px;
        right: 3px;
        min-width: 16px;
        height: 16px;
        font-size: .62rem;
    }

    .ca-admin-pagebar {
        padding: 18px 16px 0;
        align-items: flex-start;
        flex-direction: column;
    }

    .ca-pagebar-status {
        display: none;
    }
}

/* --- Dark Theme --- */
[data-theme="dark"] {
    --ca-bg: #0f172a;
    --ca-surface: #1e293b;
    --ca-surface-hover: #334155;
    --ca-border: #334155;
    --ca-border-light: #1e293b;
    --ca-text: #f1f5f9;
    --ca-text-secondary: #94a3b8;
    --ca-text-muted: #64748b;
    --ca-sidebar-bg: #020617;
    --ca-sidebar-hover: #0f172a;
    --ca-topbar-bg: #1e293b;
    --ca-topbar-shadow: 0 1px 3px rgba(0, 0, 0, .3);
    --ca-shadow-sm: 0 1px 3px rgba(0, 0, 0, .2);
    --ca-shadow: 0 4px 12px rgba(0, 0, 0, .25);
    --ca-shadow-lg: 0 10px 30px rgba(0, 0, 0, .3);
    --ca-success-light: rgba(16, 185, 129, .15);
    --ca-warning-light: rgba(245, 158, 11, .15);
    --ca-danger-light: rgba(239, 68, 68, .15);
    --ca-info-light: rgba(14, 165, 233, .15);
    --ca-primary-light: rgba(99, 102, 241, .2);
}

/* --- Reset & Base --- */
*,
*::before,
*::after {
    box-sizing: border-box;
}

body {
    font-family: var(--ca-font);
    background: var(--ca-bg);
    color: var(--ca-text);
    overflow-x: hidden;
    margin: 0;
    -webkit-font-smoothing: antialiased;
}

a {
    text-decoration: none;
    color: inherit;
}

/* --- Layout Wrapper --- */
.ca-wrapper {
    display: flex;
    min-height: 100vh;
}

/* ============================================================
   SIDEBAR
   ============================================================ */
.ca-sidebar {
    position: fixed;
    top: 0;
    left: 0;
    bottom: 0;
    width: var(--ca-sidebar-width);
    background: var(--ca-sidebar-bg);
    z-index: 1040;
    display: flex;
    flex-direction: column;
    transition: transform var(--ca-transition), width var(--ca-transition);
    overflow-y: auto;
    overflow-x: hidden;
    scrollbar-width: thin;
    scrollbar-color: rgba(255, 255, 255, .1) transparent;
}

.ca-sidebar::-webkit-scrollbar {
    width: 4px;
}

.ca-sidebar::-webkit-scrollbar-thumb {
    background: rgba(255, 255, 255, .1);
    border-radius: 4px;
}

.ca-sidebar-header {
    padding: 24px 20px 18px;
    border-bottom: 1px solid rgba(255, 255, 255, .06);
    flex-shrink: 0;
}

.ca-brand-stack {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.ca-brand {
    font-family: var(--ca-font-serif);
    font-size: 1.5rem;
    font-weight: 500;
    color: #fff;
    letter-spacing: .5px;
    display: flex;
    align-items: center;
    gap: 3px;
}

.ca-brand-accent {
    color: var(--ca-accent);
}

.ca-brand-badge {
    font-size: .55rem;
    font-weight: 500;
    background: rgba(255, 255, 255, .08);
    color: var(--ca-sidebar-text);
    padding: 2px 8px;
    border-radius: var(--ca-radius-full);
    margin-left: 8px;
    letter-spacing: 1.5px;
    text-transform: uppercase;
}

.ca-brand-subtitle {
    color: rgba(255, 255, 255, .58);
    font-size: .76rem;
    line-height: 1.55;
}

/* Menu */
.ca-menu {
    list-style: none;
    padding: 12px 0;
    margin: 0;
    flex: 1;
}

.ca-menu-label {
    padding: 20px 20px 8px;
    font-size: .68rem;
    font-weight: 700;
    color: rgba(255, 255, 255, .25);
    letter-spacing: 1.2px;
    text-transform: uppercase;
}

.ca-menu-item a {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 10px 20px;
    color: var(--ca-sidebar-text);
    font-size: .875rem;
    font-weight: 500;
    border-radius: 0;
    margin: 1px 0;
    transition: all var(--ca-transition);
    border-left: 3px solid transparent;
    position: relative;
}

.ca-menu-item a:hover {
    color: #fff;
    background: var(--ca-sidebar-hover);
}

.ca-menu-item.active a {
    color: var(--ca-sidebar-text-active);
    background: var(--ca-sidebar-active);
    border-left-color: var(--ca-primary);
}

.ca-menu-item a i {
    width: 20px;
    text-align: center;
    font-size: 1rem;
    opacity: .7;
    flex-shrink: 0;
}

.ca-menu-item.active a i {
    opacity: 1;
    color: var(--ca-primary);
}

.ca-menu-badge {
    margin-left: auto;
    font-size: .7rem;
    font-weight: 600;
    padding: 2px 8px;
    border-radius: var(--ca-radius-full);
    line-height: 1.4;
}

.ca-sidebar-footer {
    padding: 16px 20px;
    border-top: 1px solid rgba(255, 255, 255, .06);
    flex-shrink: 0;
}

/* Collapsed state */
.ca-sidebar.collapsed {
    transform: translateX(-100%);
}

/* ============================================================
   MAIN CONTENT AREA
   ============================================================ */
.ca-main {
    margin-left: var(--ca-sidebar-width);
    flex: 1;
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    transition: margin-left var(--ca-transition);
}

.ca-sidebar.collapsed~.ca-main {
    margin-left: 0;
}

/* ============================================================
   TOPBAR
   ============================================================ */
.ca-topbar {
    position: sticky;
    top: 0;
    z-index: 1030;
    background: rgba(255, 255, 255, .82);
    backdrop-filter: blur(18px);
    box-shadow: 0 16px 34px rgba(20, 33, 47, .06);
    padding: 0 24px;
    height: 72px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 16px;
}

.ca-topbar-left {
    display: flex;
    align-items: center;
    gap: 16px;
}

.ca-topbar-right {
    display: flex;
    align-items: center;
    gap: 12px;
}

.ca-toggle-btn {
    width: 38px;
    height: 38px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius-sm);
    color: var(--ca-text-secondary);
    cursor: pointer;
    transition: all var(--ca-transition);
    font-size: 1rem;
}

.ca-toggle-btn:hover {
    background: var(--ca-primary-light);
    color: var(--ca-primary);
    border-color: var(--ca-primary);
}

/* Search */
.ca-search {
    position: relative;
    width: 280px;
}

.ca-search input {
    width: 100%;
    background: rgba(255, 255, 255, .92);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius-full);
    padding: 10px 16px 10px 40px;
    font-size: .875rem;
    color: var(--ca-text);
    outline: none;
    transition: all var(--ca-transition);
    font-family: var(--ca-font);
    box-shadow: inset 0 1px 0 rgba(255, 255, 255, .7);
}

.ca-search input::placeholder {
    color: var(--ca-text-muted);
}

.ca-search input:focus {
    border-color: var(--ca-primary);
    background: var(--ca-surface);
    box-shadow: 0 0 0 3px var(--ca-primary-light), 0 12px 26px rgba(20, 33, 47, .08);
}

.ca-context-chip {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    min-height: 40px;
    padding: 0 14px;
    border-radius: var(--ca-radius-full);
    background: rgba(24, 59, 91, .06);
    border: 1px solid rgba(24, 59, 91, .08);
    color: var(--ca-primary);
    font-size: .8rem;
    font-weight: 700;
}

.ca-search-icon {
    position: absolute;
    left: 14px;
    top: 50%;
    transform: translateY(-50%);
    color: var(--ca-text-muted);
    font-size: .85rem;
}

/* Theme toggle */
.ca-theme-toggle {
    width: 38px;
    height: 38px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: transparent;
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius-sm);
    color: var(--ca-text-secondary);
    cursor: pointer;
    transition: all var(--ca-transition);
    font-size: 1rem;
}

.ca-theme-toggle:hover {
    background: var(--ca-primary-light);
    color: var(--ca-primary);
}

/* Notification bell */
.ca-notification {
    position: relative;
    width: 38px;
    height: 38px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: transparent;
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius-sm);
    color: var(--ca-text-secondary);
    cursor: pointer;
    transition: all var(--ca-transition);
    font-size: 1rem;
}

.ca-notification:hover {
    background: var(--ca-primary-light);
    color: var(--ca-primary);
}

.ca-notification-dot {
    position: absolute;
    top: 8px;
    right: 8px;
    width: 8px;
    height: 8px;
    background: var(--ca-danger);
    border: 2px solid var(--ca-topbar-bg);
    border-radius: 50%;
}

/* User dropdown */
.ca-user-info {
    display: flex;
    align-items: center;
    gap: 10px;
    cursor: pointer;
    padding: 4px 8px;
    border-radius: var(--ca-radius);
    transition: background var(--ca-transition);
}

.ca-user-info:hover {
    background: var(--ca-surface-hover);
}

.ca-user-name {
    font-size: .875rem;
    font-weight: 600;
    color: var(--ca-text);
    line-height: 1.2;
}

.ca-user-role {
    font-size: .7rem;
    color: var(--ca-text-muted);
}

.ca-avatar {
    width: 36px;
    height: 36px;
    background: linear-gradient(135deg, var(--ca-primary), #2c557c);
    color: #fff;
    border-radius: var(--ca-radius-sm);
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 700;
    font-size: .875rem;
}

/* Dropdown menu */
.ca-dropdown-menu {
    border: 1px solid var(--ca-border) !important;
    background: var(--ca-surface) !important;
    box-shadow: var(--ca-shadow-lg) !important;
    border-radius: var(--ca-radius) !important;
    padding: 8px !important;
    min-width: 200px;
}

.ca-dropdown-menu .dropdown-item {
    border-radius: var(--ca-radius-sm);
    padding: 8px 12px;
    font-size: .875rem;
    color: var(--ca-text);
    transition: background var(--ca-transition);
}

.ca-dropdown-menu .dropdown-item:hover {
    background: var(--ca-surface-hover);
}

.ca-dropdown-menu .dropdown-item.text-danger {
    color: var(--ca-danger) !important;
}

.ca-dropdown-menu .dropdown-divider {
    border-color: var(--ca-border);
    margin: 4px 0;
}

/* ============================================================
   BREADCRUMB
   ============================================================ */
.ca-breadcrumb {
    padding: 16px 24px 12px;
    background: transparent;
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: .8rem;
    color: var(--ca-text-muted);
}

.ca-breadcrumb a {
    color: var(--ca-text-secondary);
    transition: color var(--ca-transition);
}

.ca-breadcrumb a:hover {
    color: var(--ca-primary);
}

.ca-breadcrumb-sep {
    opacity: .4;
}

.ca-breadcrumb-current {
    color: var(--ca-text);
    font-weight: 600;
}

/* ============================================================
   CONTENT AREA
   ============================================================ */
.ca-content {
    padding: 0 28px 30px;
    flex: 1;
}

/* Page header */
.ca-page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 18px;
    gap: 16px;
    flex-wrap: wrap;
    padding: 6px 0 18px;
    background: transparent;
    border: none;
    border-bottom: 1px solid var(--ca-border);
    border-radius: 0;
    box-shadow: none;
}

.ca-page-title {
    font-family: var(--ca-font);
    font-size: 1.28rem;
    font-weight: 700;
    color: var(--ca-text);
    margin: 0 0 5px;
    letter-spacing: 0;
}

.ca-page-subtitle {
    color: var(--ca-text-muted);
    margin: 0;
    font-size: .86rem;
}

/* ============================================================
   CARDS
   ============================================================ */
.ca-card {
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius);
    box-shadow: 0 1px 2px rgba(49, 53, 17, .035);
    overflow: hidden;
    transition: box-shadow var(--ca-transition), border-color var(--ca-transition);
}

.ca-card:hover {
    box-shadow: 0 8px 24px rgba(49, 53, 17, .055);
    border-color: rgba(var(--ca-primary-rgb), .12);
}

.ca-card-header {
    padding: 16px 20px;
    border-bottom: 1px solid var(--ca-border);
    display: flex;
    align-items: center;
    justify-content: space-between;
    background: #fff;
}

.ca-card-title {
    font-size: .92rem;
    font-weight: 700;
    color: var(--ca-text);
    margin: 0;
}

.ca-card-body {
    padding: 20px;
}

.ca-card-footer {
    padding: 14px 20px;
    border-top: 1px solid var(--ca-border);
    background: var(--ca-surface);
}

/* ============================================================
   KPI / STAT WIDGETS
   ============================================================ */
.ca-stat {
    background: #fff;
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius);
    padding: 18px;
    position: relative;
    overflow: hidden;
    transition: box-shadow var(--ca-transition), border-color var(--ca-transition);
}

.ca-stat:hover {
    box-shadow: 0 8px 24px rgba(49, 53, 17, .055);
    border-color: rgba(var(--ca-primary-rgb), .12);
}

.ca-stat-icon {
    width: 48px;
    height: 48px;
    border-radius: var(--ca-radius);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.25rem;
    color: #fff;
}

.ca-stat-icon.primary {
    background: linear-gradient(135deg, #6366f1, #818cf8);
}

.ca-stat-icon.success {
    background: linear-gradient(135deg, #10b981, #34d399);
}

.ca-stat-icon.warning {
    background: linear-gradient(135deg, #f59e0b, #fbbf24);
}

.ca-stat-icon.info {
    background: linear-gradient(135deg, #0ea5e9, #38bdf8);
}

.ca-stat-icon.danger {
    background: linear-gradient(135deg, #ef4444, #f87171);
}

.ca-stat-value {
    font-size: 1.75rem;
    font-weight: 800;
    color: var(--ca-text);
    margin: 12px 0 4px;
    line-height: 1;
}

.ca-stat-label {
    font-size: .75rem;
    font-weight: 600;
    color: var(--ca-text-muted);
    text-transform: uppercase;
    letter-spacing: .5px;
}

.ca-stat-footer {
    font-size: .8rem;
    color: var(--ca-text-muted);
    margin-top: 12px;
}

.ca-stat-bg-icon {
    position: absolute;
    right: -10px;
    bottom: -10px;
    font-size: 5rem;
    opacity: .04;
    transform: rotate(-15deg);
    color: var(--ca-text);
    pointer-events: none;
}

/* ============================================================
   TABLES
   ============================================================ */
.ca-table-wrapper {
    overflow-x: auto;
}

.ca-table {
    width: 100%;
    border-collapse: collapse;
    font-size: .875rem;
}

.ca-table thead th {
    background: var(--ca-bg);
    color: var(--ca-text-secondary);
    font-size: .7rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: .5px;
    padding: 12px 20px;
    border: none;
    white-space: nowrap;
}

.ca-table tbody td {
    padding: 14px 20px;
    vertical-align: middle;
    border-bottom: 1px solid var(--ca-border-light);
    color: var(--ca-text);
}

.ca-table tbody tr {
    transition: background var(--ca-transition);
}

.ca-table tbody tr:hover {
    background: var(--ca-surface-hover);
}

.ca-table tbody tr:last-child td {
    border-bottom: none;
}

/* Table thumbnail */
.ca-thumb {
    width: 48px;
    height: 48px;
    border-radius: var(--ca-radius-sm);
    object-fit: cover;
    border: 1px solid var(--ca-border);
    transition: transform var(--ca-transition);
}

.ca-table tbody tr:hover .ca-thumb {
    transform: scale(1.08);
}

/* Empty state */
.ca-empty {
    text-align: center;
    padding: 48px 24px;
    color: var(--ca-text-muted);
}

.ca-empty i {
    font-size: 3rem;
    opacity: .3;
    margin-bottom: 12px;
    display: block;
}

.ca-empty p {
    font-weight: 600;
    margin: 0;
}

/* ============================================================
   BADGES
   ============================================================ */
.ca-badge {
    display: inline-flex;
    align-items: center;
    gap: 5px;
    padding: 5px 12px;
    border-radius: var(--ca-radius-sm);
    font-size: .75rem;
    font-weight: 600;
    line-height: 1.4;
}

.ca-badge-success {
    background: var(--ca-success-light);
    color: var(--ca-success);
    border: 1px solid var(--ca-success-border);
}

.ca-badge-warning {
    background: var(--ca-warning-light);
    color: #ea580c;
    border: 1px solid var(--ca-warning-border);
}

.ca-badge-danger {
    background: var(--ca-danger-light);
    color: var(--ca-danger);
    border: 1px solid var(--ca-danger-border);
}

.ca-badge-info {
    background: var(--ca-info-light);
    color: var(--ca-info);
    border: 1px solid var(--ca-info-border);
}

.ca-badge-primary {
    background: var(--ca-primary-light);
    color: var(--ca-primary);
    border: 1px solid rgba(var(--ca-primary-rgb), .2);
}

.ca-badge-neutral {
    background: var(--ca-bg);
    color: var(--ca-text-secondary);
    border: 1px solid var(--ca-border);
}

/* ============================================================
   BUTTONS
   ============================================================ */
.ca-btn {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 10px 20px;
    border-radius: var(--ca-radius);
    font-size: .875rem;
    font-weight: 600;
    border: 1px solid transparent;
    cursor: pointer;
    transition: all var(--ca-transition);
    font-family: var(--ca-font);
    text-decoration: none;
    line-height: 1.4;
}

.ca-btn-primary {
    background: var(--ca-primary);
    color: #fff;
}

.ca-btn-primary:hover {
    background: var(--ca-primary-hover);
    color: #fff;
    transform: translateY(-1px);
    box-shadow: 0 4px 12px rgba(var(--ca-primary-rgb), .3);
}

.ca-btn-success {
    background: var(--ca-success);
    color: #fff;
}

.ca-btn-success:hover {
    background: #059669;
    color: #fff;
    transform: translateY(-1px);
}

.ca-btn-danger {
    background: var(--ca-danger);
    color: #fff;
}

.ca-btn-danger:hover {
    background: #dc2626;
    color: #fff;
}

.ca-btn-outline {
    background: var(--ca-surface);
    border-color: var(--ca-border);
    color: var(--ca-text-secondary);
}

.ca-btn-outline:hover {
    border-color: var(--ca-primary);
    color: var(--ca-primary);
    background: var(--ca-primary-light);
}

.ca-btn-ghost {
    background: transparent;
    color: var(--ca-text-secondary);
}

.ca-btn-ghost:hover {
    background: var(--ca-surface-hover);
    color: var(--ca-text);
}

.ca-btn-sm {
    padding: 6px 14px;
    font-size: .8rem;
}

.ca-btn-lg {
    padding: 12px 28px;
    font-size: 1rem;
}

.ca-btn-icon {
    width: 36px;
    height: 36px;
    padding: 0;
    border-radius: var(--ca-radius-sm);
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    color: var(--ca-text-secondary);
    cursor: pointer;
    transition: all var(--ca-transition);
}

.ca-btn-icon:hover {
    transform: translateY(-2px);
}

.ca-btn-icon.edit:hover {
    color: var(--ca-warning);
    border-color: var(--ca-warning);
    background: var(--ca-warning-light);
}

.ca-btn-icon.view:hover {
    color: var(--ca-info);
    border-color: var(--ca-info);
    background: var(--ca-info-light);
}

.ca-btn-icon.delete:hover {
    color: var(--ca-danger);
    border-color: var(--ca-danger);
    background: var(--ca-danger-light);
}

.ca-btn-excel {
    background: #107c41;
    color: #fff;
}

.ca-btn-excel:hover {
    background: #0a5c2f;
    color: #fff;
    transform: translateY(-1px);
}

/* ============================================================
   FORMS
   ============================================================ */
.ca-form-group {
    margin-bottom: 20px;
}

.ca-label {
    display: block;
    font-size: .85rem;
    font-weight: 600;
    color: var(--ca-text-secondary);
    margin-bottom: 6px;
}

.ca-label .required {
    color: var(--ca-danger);
}

.ca-input,
.ca-select,
.ca-textarea {
    width: 100%;
    background: var(--ca-bg);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius);
    padding: 10px 14px;
    font-size: .9rem;
    color: var(--ca-text);
    outline: none;
    transition: all var(--ca-transition);
    font-family: var(--ca-font);
}

.ca-input:focus,
.ca-select:focus,
.ca-textarea:focus {
    background: var(--ca-surface);
    border-color: var(--ca-primary);
    box-shadow: 0 0 0 3px var(--ca-primary-light);
}

.ca-input::placeholder,
.ca-textarea::placeholder {
    color: var(--ca-text-muted);
}

.ca-input-icon-group {
    position: relative;
}

.ca-input-icon-group .ca-icon-left {
    position: absolute;
    left: 14px;
    top: 50%;
    transform: translateY(-50%);
    color: var(--ca-text-muted);
    font-size: .85rem;
    transition: color var(--ca-transition);
}

.ca-input-icon-group .ca-input {
    padding-left: 40px;
}

.ca-input-icon-group:focus-within .ca-icon-left {
    color: var(--ca-primary);
}

/* Validation */
.ca-validation-error {
    color: var(--ca-danger);
    font-size: .8rem;
    margin-top: 4px;
    display: block;
}

.field-validation-error {
    color: var(--ca-danger);
    font-size: .8rem;
    margin-top: 4px;
    display: block;
}

/* Switch */
.ca-switch {
    display: flex;
    align-items: center;
    gap: 12px;
    cursor: pointer;
}

.ca-switch input[type="checkbox"] {
    width: 44px;
    height: 24px;
    appearance: none;
    -webkit-appearance: none;
    background: var(--ca-border);
    border-radius: var(--ca-radius-full);
    position: relative;
    cursor: pointer;
    transition: background var(--ca-transition);
    outline: none;
    border: none;
    flex-shrink: 0;
}

.ca-switch input[type="checkbox"]::after {
    content: '';
    width: 18px;
    height: 18px;
    background: #fff;
    border-radius: 50%;
    position: absolute;
    top: 3px;
    left: 3px;
    transition: transform var(--ca-transition);
    box-shadow: 0 1px 3px rgba(0, 0, 0, .2);
}

.ca-switch input[type="checkbox"]:checked {
    background: var(--ca-primary);
}

.ca-switch input[type="checkbox"]:checked::after {
    transform: translateX(20px);
}

.ca-switch-label {
    font-size: .875rem;
    font-weight: 500;
    color: var(--ca-text-secondary);
}

/* Upload zone */
.ca-upload-zone {
    border: 2px dashed var(--ca-border);
    border-radius: var(--ca-radius-lg);
    background: var(--ca-bg);
    padding: 32px;
    text-align: center;
    cursor: pointer;
    transition: all var(--ca-transition);
}

.ca-upload-zone:hover {
    border-color: var(--ca-primary);
    background: var(--ca-primary-light);
}

.ca-upload-icon {
    font-size: 2.5rem;
    color: var(--ca-text-muted);
    margin-bottom: 12px;
}

/* Form card header */
.ca-form-header {
    background: linear-gradient(135deg, var(--ca-primary), #818cf8);
    padding: 28px 24px;
    color: #fff;
    position: relative;
    overflow: hidden;
}

.ca-form-header-icon {
    position: absolute;
    right: -15px;
    bottom: -20px;
    font-size: 6rem;
    opacity: .1;
    transform: rotate(-15deg);
}

/* ============================================================
   QUICK ACTIONS
   ============================================================ */
.ca-quick-action {
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius);
    padding: 20px;
    text-align: center;
    transition: all var(--ca-transition);
    cursor: pointer;
    height: 100%;
}

.ca-quick-action:hover {
    border-color: var(--ca-primary);
    background: var(--ca-primary-light);
    transform: translateY(-2px);
    box-shadow: var(--ca-shadow);
}

.ca-quick-icon {
    font-size: 1.75rem;
    margin-bottom: 8px;
}

.ca-quick-label {
    font-size: .85rem;
    font-weight: 600;
    color: var(--ca-text);
}

/* ============================================================
   SYSTEM ALERT (Dashboard)
   ============================================================ */
.ca-system-alert {
    background: linear-gradient(135deg, #0f172a, #1e3a5f);
    color: #fff;
    border-radius: var(--ca-radius-lg);
    padding: 24px;
    position: relative;
    overflow: hidden;
}

.ca-system-alert::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: url("data:image/svg+xml,%3Csvg width='40' height='40' viewBox='0 0 40 40' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M0 0h20v20H0V0zm20 20h20v20H20V20z' fill='%23fff' fill-opacity='.03'/%3E%3C/svg%3E");
    pointer-events: none;
}

/* ============================================================
   ACTIVITY FEED
   ============================================================ */
.ca-activity-item {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 14px 20px;
    border-bottom: 1px solid var(--ca-border-light);
    transition: background var(--ca-transition);
}

.ca-activity-item:last-child {
    border-bottom: none;
}

.ca-activity-item:hover {
    background: var(--ca-surface-hover);
}

.ca-activity-icon {
    width: 36px;
    height: 36px;
    border-radius: var(--ca-radius-sm);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: .85rem;
    flex-shrink: 0;
}

.ca-activity-title {
    font-size: .85rem;
    font-weight: 600;
    color: var(--ca-text);
}

.ca-activity-desc {
    font-size: .75rem;
    color: var(--ca-text-muted);
}

.ca-activity-time {
    font-size: .7rem;
    color: var(--ca-text-muted);
    margin-left: auto;
    white-space: nowrap;
}

/* ============================================================
   STATUS TABS
   ============================================================ */
.ca-tabs {
    display: flex;
    gap: 8px;
    flex-wrap: wrap;
    margin-bottom: 20px;
}

.ca-tab {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 8px 18px;
    border-radius: var(--ca-radius-full);
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    color: var(--ca-text-secondary);
    font-size: .85rem;
    font-weight: 600;
    transition: all var(--ca-transition);
    text-decoration: none;
}

.ca-tab:hover,
.ca-tab.active {
    background: var(--ca-primary);
    color: #fff;
    border-color: var(--ca-primary);
    box-shadow: 0 4px 12px rgba(var(--ca-primary-rgb), .3);
}

.ca-tab-count {
    font-size: .7rem;
    background: rgba(255, 255, 255, .2);
    padding: 1px 8px;
    border-radius: var(--ca-radius-full);
}

/* ============================================================
   ALERTS / SUCCESS MESSAGES
   ============================================================ */
.ca-alert {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 14px 20px;
    border-radius: var(--ca-radius);
    font-size: .875rem;
    font-weight: 500;
    margin-bottom: 20px;
    animation: caFadeIn .4s ease;
}

.ca-alert-success {
    background: var(--ca-success-light);
    color: var(--ca-success);
    border: 1px solid var(--ca-success-border);
}

.ca-alert-danger {
    background: var(--ca-danger-light);
    color: var(--ca-danger);
    border: 1px solid var(--ca-danger-border);
}

.ca-alert-warning {
    background: var(--ca-warning-light);
    color: #b45309;
    border: 1px solid var(--ca-warning-border);
}

.ca-alert-info {
    background: var(--ca-info-light);
    color: var(--ca-info);
    border: 1px solid var(--ca-info-border);
}

.ca-alert .btn-close {
    filter: none;
    opacity: .6;
}

.ca-alert .btn-close:hover {
    opacity: 1;
}

/* ============================================================
   TOAST NOTIFICATION
   ============================================================ */
.ca-toast-container {
    position: fixed;
    top: 80px;
    right: 24px;
    z-index: 9999;
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.ca-toast {
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius);
    padding: 14px 20px;
    box-shadow: var(--ca-shadow-lg);
    display: flex;
    align-items: center;
    gap: 12px;
    min-width: 300px;
    animation: caSlideIn .4s ease;
    font-size: .875rem;
    font-weight: 500;
}

.ca-toast.success {
    border-left: 4px solid var(--ca-success);
}

.ca-toast.error {
    border-left: 4px solid var(--ca-danger);
}

.ca-toast.warning {
    border-left: 4px solid var(--ca-warning);
}

.ca-toast.info {
    border-left: 4px solid var(--ca-info);
}

/* ============================================================
   FILTER BAR
   ============================================================ */
.ca-filter-bar {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 16px 20px;
    flex-wrap: wrap;
}

.ca-filter-bar .ca-input,
.ca-filter-bar .ca-select {
    max-width: 280px;
}

.ca-filter-count {
    font-size: .8rem;
    color: var(--ca-text-muted);
    margin-left: auto;
}

/* ============================================================
   FOOTER
   ============================================================ */
.ca-footer {
    text-align: center;
    padding: 18px 24px 28px;
    font-size: .8rem;
    color: var(--ca-text-muted);
    margin-top: auto;
    border-top: 1px solid var(--ca-border);
}

/* ============================================================
   MOBILE OVERLAY
   ============================================================ */
.ca-overlay {
    display: none;
    position: fixed;
    inset: 0;
    background: rgba(0, 0, 0, .5);
    z-index: 1035;
    backdrop-filter: blur(2px);
}

.ca-overlay.active {
    display: block;
}

/* ============================================================
   LOADING SPINNER
   ============================================================ */
.ca-spinner {
    display: inline-block;
    width: 20px;
    height: 20px;
    border: 2px solid var(--ca-border);
    border-top-color: var(--ca-primary);
    border-radius: 50%;
    animation: caSpin .6s linear infinite;
}

/* ============================================================
   UTILITY CLASSES
   ============================================================ */
.ca-text-primary {
    color: var(--ca-primary) !important;
}

.ca-text-success {
    color: var(--ca-success) !important;
}

.ca-text-warning {
    color: var(--ca-warning) !important;
}

.ca-text-danger {
    color: var(--ca-danger) !important;
}

.ca-text-info {
    color: var(--ca-info) !important;
}

.ca-text-muted {
    color: var(--ca-text-muted) !important;
}

.ca-fw-bold {
    font-weight: 700;
}

.ca-fw-semi {
    font-weight: 600;
}

/* ============================================================
   ANIMATIONS
   ============================================================ */
@keyframes caFadeIn {
    from {
        opacity: 0;
        transform: translateY(-8px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

@keyframes caSlideIn {
    from {
        opacity: 0;
        transform: translateX(40px);
    }

    to {
        opacity: 1;
        transform: translateX(0);
    }
}

@keyframes caSpin {
    to {
        transform: rotate(360deg);
    }
}

.ca-fade-in {
    animation: caFadeIn .4s ease;
}

/* ============================================================
   RESPONSIVE
   ============================================================ */
@media (max-width: 1024px) {
    .ca-sidebar {
        transform: translateX(-100%);
    }

    .ca-sidebar.mobile-open {
        transform: translateX(0);
    }

    .ca-main {
        margin-left: 0;
    }

    .ca-search {
        width: 200px;
    }
}

@media (max-width: 768px) {
    .ca-search {
        display: none;
    }

    .ca-topbar {
        padding: 0 16px;
    }

    .ca-content {
        padding: 0 16px 16px;
    }

    .ca-user-meta {
        display: none;
    }

    .ca-page-header {
        flex-direction: column;
    }

    .ca-stat {
        padding: 16px;
    }

    .ca-stat-value {
        font-size: 1.35rem;
    }
}

/* ============================================================
   CHART CONTAINER
   ============================================================ */
.ca-chart-container {
    position: relative;
    height: 300px;
    width: 100%;
}

/* ============================================================
   ORDER ID STYLING
   ============================================================ */
.ca-order-id {
    font-weight: 700;
    color: var(--ca-primary);
    font-family: 'JetBrains Mono', monospace;
}

/* Customer avatar in tables */
.ca-customer-avatar {
    width: 32px;
    height: 32px;
    background: var(--ca-bg);
    color: var(--ca-text-secondary);
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 700;
    font-size: .8rem;
    flex-shrink: 0;
}

/* Pagination */
.ca-pagination {
    display: flex;
    align-items: center;
    gap: 4px;
    justify-content: center;
    padding: 16px 0;
}

.ca-pagination a,
.ca-pagination span {
    width: 36px;
    height: 36px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    border-radius: var(--ca-radius-sm);
    font-size: .85rem;
    font-weight: 600;
    color: var(--ca-text-secondary);
    transition: all var(--ca-transition);
    border: 1px solid transparent;
}

.ca-pagination a:hover {
    background: var(--ca-primary-light);
    color: var(--ca-primary);
}

.ca-pagination .active {
    background: var(--ca-primary);
    color: #fff;
}

/* Image preview within forms */
.ca-image-preview {
    max-width: 100%;
    max-height: 250px;
    border-radius: var(--ca-radius);
    margin-top: 12px;
    box-shadow: var(--ca-shadow);
    object-fit: contain;
    display: none;
}

/* Date pill */
.ca-date-pill {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    padding: 6px 16px;
    border-radius: var(--ca-radius-full);
    font-size: .8rem;
    color: var(--ca-text-muted);
}

/* ============================================================
   KPI CARDS
   ============================================================ */
.ca-kpi-card {
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius-lg);
    padding: 20px 24px;
    display: flex;
    align-items: center;
    gap: 16px;
    transition: transform var(--ca-transition), box-shadow var(--ca-transition);
}

.ca-kpi-card:hover {
    transform: translateY(-4px);
    box-shadow: var(--ca-shadow-lg);
}

.ca-kpi-icon {
    width: 56px;
    height: 56px;
    border-radius: var(--ca-radius);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.35rem;
    color: #fff;
    flex-shrink: 0;
}

.ca-kpi-content {
    display: flex;
    flex-direction: column;
}

.ca-kpi-label {
    font-size: .75rem;
    font-weight: 700;
    color: var(--ca-text-muted);
    text-transform: uppercase;
    letter-spacing: .5px;
    margin-bottom: 4px;
}

.ca-kpi-value {
    font-size: 1.65rem;
    font-weight: 800;
    color: var(--ca-text);
    line-height: 1;
}

/* ============================================================
   PROGRESS BAR
   ============================================================ */
.ca-progress-bar {
    width: 100%;
    height: 6px;
    background: var(--ca-bg);
    border-radius: var(--ca-radius-full);
    overflow: hidden;
}

.ca-progress-fill {
    height: 100%;
    background: linear-gradient(90deg, var(--ca-primary), #818cf8);
    border-radius: var(--ca-radius-full);
    transition: width .6s ease;
}

/* ============================================================
   SETTINGS PAGE
   ============================================================ */
.ca-settings-tabs {
    display: flex;
    gap: 6px;
    flex-wrap: wrap;
    margin-bottom: 24px;
    padding: 6px;
    background: var(--ca-surface);
    border: 1px solid var(--ca-border);
    border-radius: var(--ca-radius-lg);
}

.ca-settings-tab {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 10px 18px;
    border-radius: var(--ca-radius);
    background: transparent;
    border: none;
    color: var(--ca-text-secondary);
    font-size: .85rem;
    font-weight: 600;
    cursor: pointer;
    transition: all var(--ca-transition);
    font-family: var(--ca-font);
}

.ca-settings-tab:hover {
    background: var(--ca-surface-hover);
    color: var(--ca-text);
}

.ca-settings-tab.active {
    background: var(--ca-primary);
    color: #fff;
    box-shadow: 0 4px 12px rgba(var(--ca-primary-rgb), .3);
}

.ca-settings-panel {
    display: none;
    animation: caFadeIn .3s ease;
}

.ca-settings-panel.active {
    display: block;
}

.ca-settings-save-bar {
    position: sticky;
    bottom: 0;
    background: var(--ca-surface);
    border-top: 1px solid var(--ca-border);
    padding: 16px 24px;
    margin: 24px -24px -24px;
    border-radius: 0 0 var(--ca-radius-lg) var(--ca-radius-lg);
    box-shadow: 0 -4px 12px rgba(0, 0, 0, .06);
    z-index: 10;
}
```

## File: KanvasProje.Web/wwwroot/css/meteor-galeri.css
```css
/* ============================================================
   METEOR GALERİ — Premium Tablo & Dekorasyon E-Ticaret
   Design System CSS
   ============================================================
   Renk Paleti:
   - Zemin: kırık beyaz #FAF8F5, sıcak beyaz #FFFFFF
   - Bej/Taş: #EDE8E0, #D9D2C7
   - Koyu Antrasit: #2C2C2C
   - Zeytin Yeşili (Vurgu): #5C6B55
   - Altınsı Ton: #B8956A
   - Açık Gri: #F5F3F0

   Tipografi:
   - Başlıklar: Playfair Display (serif)
   - Gövde: Inter (sans-serif)
   ============================================================ */

/* --- RESET & BASE --- */
*,
*::before,
*::after {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
}

:root {
    /* Color tokens */
    --mg-bg: #FAF8F5;
    --mg-bg-white: #FFFFFF;
    --mg-bg-soft: #F5F3F0;
    --mg-bg-cream: #EDE8E0;
    --mg-bg-stone: #D9D2C7;
    --mg-ink: #2C2C2C;
    --mg-ink-light: #5A5A5A;
    --mg-ink-muted: #8A8A8A;
    --mg-ink-faint: #B0B0B0;
    --mg-olive: #5C6B55;
    --mg-olive-dark: #495743;
    --mg-olive-light: #7A8B73;
    --mg-gold: #B8956A;
    --mg-gold-light: #D4B896;
    --mg-gold-dark: #9A7850;
    --mg-border: rgba(44, 44, 44, 0.08);
    --mg-border-strong: rgba(44, 44, 44, 0.15);
    --mg-shadow-sm: 0 2px 8px rgba(44, 44, 44, 0.04);
    --mg-shadow: 0 4px 20px rgba(44, 44, 44, 0.06);
    --mg-shadow-lg: 0 8px 40px rgba(44, 44, 44, 0.08);
    --mg-shadow-xl: 0 16px 60px rgba(44, 44, 44, 0.10);
    --mg-radius-sm: 8px;
    --mg-radius: 12px;
    --mg-radius-lg: 16px;
    --mg-radius-xl: 24px;
    --mg-radius-full: 9999px;
    --mg-transition: 0.25s cubic-bezier(0.4, 0, 0.2, 1);
    --mg-transition-slow: 0.4s cubic-bezier(0.4, 0, 0.2, 1);

    /* Spacing scale */
    --mg-space-xs: 4px;
    --mg-space-sm: 8px;
    --mg-space-md: 16px;
    --mg-space-lg: 24px;
    --mg-space-xl: 32px;
    --mg-space-2xl: 48px;
    --mg-space-3xl: 64px;
    --mg-space-4xl: 96px;

    /* Max width */
    --mg-container: 1280px;
    --mg-container-narrow: 960px;
}

html {
    scroll-behavior: smooth;
    -webkit-text-size-adjust: 100%;
}

body {
    font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
    font-size: 15px;
    line-height: 1.6;
    color: var(--mg-ink);
    background-color: var(--mg-bg);
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    overflow-x: hidden;
}

h1, h2, h3, h4, h5, h6 {
    font-family: 'Playfair Display', 'Georgia', serif;
    color: var(--mg-ink);
    line-height: 1.2;
    letter-spacing: -0.01em;
}

a {
    color: inherit;
    text-decoration: none;
    transition: color var(--mg-transition);
}

img {
    max-width: 100%;
    height: auto;
    display: block;
}

button {
    font-family: inherit;
    cursor: pointer;
    border: none;
    background: none;
    color: inherit;
}

input, select, textarea {
    font-family: 'Inter', sans-serif;
}

ul, ol {
    list-style: none;
}

.no-scroll {
    overflow: hidden;
}


/* --- CONTAINER --- */
.mg-container {
    width: 100%;
    max-width: var(--mg-container);
    margin: 0 auto;
    padding: 0 var(--mg-space-md);
}

@media (min-width: 640px) {
    .mg-container { padding: 0 var(--mg-space-lg); }
}

@media (min-width: 1024px) {
    .mg-container { padding: 0 var(--mg-space-xl); }
}


/* ============================================================
   TOP BAR
   ============================================================ */
.mg-topbar {
    background-color: var(--mg-ink);
    color: rgba(255, 255, 255, 0.85);
    font-size: 0.72rem;
    letter-spacing: 0.14em;
    text-transform: uppercase;
    border-bottom: 1px solid rgba(255, 255, 255, 0.08);
}

.mg-topbar__inner {
    max-width: var(--mg-container);
    margin: 0 auto;
    padding: 10px var(--mg-space-md);
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-wrap: wrap;
    gap: 8px;
}

@media (min-width: 640px) {
    .mg-topbar__inner { padding-left: var(--mg-space-lg); padding-right: var(--mg-space-lg); }
}

@media (min-width: 1024px) {
    .mg-topbar__inner { padding-left: var(--mg-space-xl); padding-right: var(--mg-space-xl); }
}

.mg-topbar__primary {
    display: flex;
    align-items: center;
    gap: 8px;
}

.mg-topbar__primary i {
    color: var(--mg-gold-light);
    font-size: 0.65rem;
}

.mg-topbar__meta {
    display: none;
    align-items: center;
    gap: 20px;
}

.mg-topbar__meta span {
    display: flex;
    align-items: center;
    gap: 6px;
}

.mg-topbar__meta i {
    color: var(--mg-gold-light);
    font-size: 0.65rem;
}

@media (min-width: 1024px) {
    .mg-topbar__meta { display: flex; }
}


/* ============================================================
   HEADER
   ============================================================ */
.mg-header {
    position: sticky;
    top: 0;
    z-index: 100;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(20px);
    -webkit-backdrop-filter: blur(20px);
    border-bottom: 1px solid var(--mg-border);
    transition: box-shadow var(--mg-transition);
}

.mg-header.scrolled {
    box-shadow: var(--mg-shadow);
}

.mg-header__inner {
    max-width: var(--mg-container);
    margin: 0 auto;
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 16px;
    padding: 12px var(--mg-space-md);
}

@media (min-width: 640px) {
    .mg-header__inner { padding-left: var(--mg-space-lg); padding-right: var(--mg-space-lg); }
}

@media (min-width: 1024px) {
    .mg-header__inner { padding: 14px var(--mg-space-xl); }
}

/* Logo / Brand */
.mg-header__brand {
    display: flex;
    align-items: center;
    gap: 12px;
    text-decoration: none;
    min-width: 0;
    flex-shrink: 0;
}

.mg-header__logo {
    height: 40px;
    width: auto;
    object-fit: contain;
}

@media (min-width: 640px) {
    .mg-header__logo { height: 44px; }
}

.mg-header__brand-text {
    display: none;
    flex-direction: column;
    min-width: 0;
}

@media (min-width: 640px) {
    .mg-header__brand-text { display: flex; }
}

.mg-header__brand-name {
    font-family: 'Playfair Display', serif;
    font-size: 1.35rem;
    font-weight: 700;
    color: var(--mg-ink);
    line-height: 1.1;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.mg-header__brand-desc {
    font-size: 0.68rem;
    color: var(--mg-ink-muted);
    letter-spacing: 0.08em;
    text-transform: uppercase;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin-top: 2px;
}

/* Search */
.mg-header__search {
    display: none;
    flex: 1;
    max-width: 420px;
}

@media (min-width: 1024px) {
    .mg-header__search { display: block; }
}

.mg-search-form {
    display: flex;
    align-items: center;
    gap: 10px;
    background: var(--mg-bg-soft);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-full);
    padding: 10px 16px;
    transition: border-color var(--mg-transition), box-shadow var(--mg-transition);
}

.mg-search-form:focus-within {
    border-color: var(--mg-gold);
    box-shadow: 0 0 0 3px rgba(184, 149, 106, 0.1);
    background: var(--mg-bg-white);
}

.mg-search-form i {
    color: var(--mg-ink-muted);
    font-size: 0.85rem;
    flex-shrink: 0;
}

.mg-search-form input {
    flex: 1;
    border: none;
    background: transparent;
    font-size: 0.85rem;
    color: var(--mg-ink);
    outline: none;
}

.mg-search-form input::placeholder {
    color: var(--mg-ink-faint);
}

.mg-search-form__btn {
    font-size: 0.68rem;
    font-weight: 600;
    letter-spacing: 0.14em;
    text-transform: uppercase;
    color: var(--mg-olive);
    padding: 4px 12px;
    border-radius: var(--mg-radius-full);
    transition: all var(--mg-transition);
    white-space: nowrap;
}

.mg-search-form__btn:hover {
    background: var(--mg-olive);
    color: #fff;
}

/* Header Actions */
.mg-header__actions {
    display: flex;
    align-items: center;
    gap: 6px;
}

@media (min-width: 640px) {
    .mg-header__actions { gap: 8px; }
}

.mg-header__action-link {
    display: none;
    align-items: center;
    gap: 8px;
    padding: 8px 14px;
    border-radius: var(--mg-radius-full);
    border: 1px solid var(--mg-border);
    font-size: 0.8rem;
    font-weight: 600;
    color: var(--mg-ink);
    transition: all var(--mg-transition);
    white-space: nowrap;
}

.mg-header__action-link:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
}

@media (min-width: 1024px) {
    .mg-header__action-link { display: inline-flex; }
}

.mg-icon-btn {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    width: 42px;
    height: 42px;
    border-radius: 50%;
    border: 1px solid var(--mg-border);
    background: var(--mg-bg-white);
    color: var(--mg-ink);
    font-size: 1rem;
    transition: all var(--mg-transition);
}

.mg-icon-btn:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
    box-shadow: var(--mg-shadow-sm);
}

.mg-icon-btn--menu {
    display: inline-flex;
}

@media (min-width: 1024px) {
    .mg-icon-btn--menu { display: none; }
}

.mg-icon-btn--search-mobile {
    display: inline-flex;
}

@media (min-width: 1024px) {
    .mg-icon-btn--search-mobile { display: none; }
}

/* Cart pill */
.mg-cart-pill {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 8px 16px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-olive);
    color: #fff;
    font-size: 0.8rem;
    font-weight: 600;
    box-shadow: 0 4px 16px rgba(92, 107, 85, 0.2);
    transition: all var(--mg-transition);
}

.mg-cart-pill:hover {
    background: var(--mg-olive-dark);
    color: #fff;
}

.mg-cart-pill__count {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 22px;
    height: 22px;
    border-radius: 50%;
    background: #fff;
    color: var(--mg-olive);
    font-size: 0.7rem;
    font-weight: 700;
    padding: 0 4px;
}

.mg-cart-pill__text {
    display: none;
}

@media (min-width: 640px) {
    .mg-cart-pill__text { display: inline; }
}


/* ============================================================
   MOBILE SEARCH DRAWER
   ============================================================ */
.mg-mobile-search {
    display: none;
    padding: 12px var(--mg-space-md);
    background: var(--mg-bg-white);
    border-bottom: 1px solid var(--mg-border);
}

.mg-mobile-search.active {
    display: block;
}

.mg-mobile-search__form {
    display: flex;
    align-items: center;
    gap: 10px;
    background: var(--mg-bg-soft);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-full);
    padding: 10px 14px;
}

.mg-mobile-search__form input {
    flex: 1;
    border: none;
    background: transparent;
    font-size: 0.875rem;
    color: var(--mg-ink);
    outline: none;
}

.mg-mobile-search__form input::placeholder {
    color: var(--mg-ink-faint);
}


/* ============================================================
   DESKTOP NAV
   ============================================================ */
.mg-nav {
    display: none;
    background: rgba(255, 255, 255, 0.85);
    backdrop-filter: blur(12px);
    border-bottom: 1px solid var(--mg-border);
}

@media (min-width: 1024px) {
    .mg-nav { display: block; }
}

.mg-nav__inner {
    max-width: var(--mg-container);
    margin: 0 auto;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 var(--mg-space-xl);
}

.mg-nav__links {
    display: flex;
    align-items: center;
    gap: 0;
}

.mg-nav__link {
    display: block;
    padding: 14px 18px;
    font-size: 0.78rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    color: var(--mg-ink-light);
    position: relative;
    transition: color var(--mg-transition);
}

.mg-nav__link::after {
    content: '';
    position: absolute;
    bottom: 0;
    left: 18px;
    right: 18px;
    height: 2px;
    background: var(--mg-gold);
    transform: scaleX(0);
    transition: transform var(--mg-transition);
}

.mg-nav__link:hover {
    color: var(--mg-ink);
}

.mg-nav__link:hover::after {
    transform: scaleX(1);
}

.mg-nav__caption {
    font-size: 0.7rem;
    letter-spacing: 0.12em;
    text-transform: uppercase;
    color: var(--mg-ink-faint);
    padding: 14px 0;
}


/* ============================================================
   MOBILE DRAWER (Side Menu)
   ============================================================ */
.mg-drawer-overlay {
    position: fixed;
    inset: 0;
    z-index: 110;
    background: rgba(0, 0, 0, 0.4);
    opacity: 0;
    pointer-events: none;
    transition: opacity var(--mg-transition);
}

.mg-drawer-overlay.active {
    opacity: 1;
    pointer-events: auto;
}

.mg-drawer {
    position: fixed;
    top: 0;
    left: 0;
    bottom: 0;
    z-index: 120;
    width: 100%;
    max-width: 340px;
    background: var(--mg-bg-white);
    transform: translateX(-100%);
    transition: transform 0.35s cubic-bezier(0.4, 0, 0.2, 1);
    display: flex;
    flex-direction: column;
    box-shadow: var(--mg-shadow-xl);
}

.mg-drawer.active {
    transform: translateX(0);
}

.mg-drawer__head {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 20px 20px 16px;
    border-bottom: 1px solid var(--mg-border);
}

.mg-drawer__kicker {
    font-size: 0.65rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.18em;
    color: var(--mg-gold);
    display: block;
    margin-bottom: 4px;
}

.mg-drawer__title {
    font-family: 'Playfair Display', serif;
    font-size: 1.5rem;
    font-weight: 700;
    color: var(--mg-ink);
}

.mg-drawer__body {
    flex: 1;
    overflow-y: auto;
    padding: 16px;
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.mg-drawer__link {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 14px 16px;
    border-radius: var(--mg-radius);
    background: var(--mg-bg-soft);
    border: 1px solid transparent;
    font-size: 0.875rem;
    font-weight: 600;
    color: var(--mg-ink);
    transition: all var(--mg-transition);
}

.mg-drawer__link:hover {
    border-color: var(--mg-gold);
    background: var(--mg-bg-white);
}

.mg-drawer__link-info {
    display: flex;
    align-items: center;
    gap: 12px;
}

.mg-drawer__link-icon {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.85rem;
}

.mg-drawer__link i.fa-arrow-right {
    font-size: 0.7rem;
    color: var(--mg-ink-faint);
    transition: color var(--mg-transition), transform var(--mg-transition);
}

.mg-drawer__link:hover i.fa-arrow-right {
    color: var(--mg-gold);
    transform: translateX(3px);
}

.mg-drawer__footer {
    padding: 16px 20px;
    border-top: 1px solid var(--mg-border);
}

.mg-drawer__social {
    display: flex;
    gap: 8px;
}

.mg-drawer__social a {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    border: 1px solid var(--mg-border);
    display: flex;
    align-items: center;
    justify-content: center;
    color: var(--mg-ink-muted);
    font-size: 0.85rem;
    transition: all var(--mg-transition);
}

.mg-drawer__social a:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
}


/* ============================================================
   MOBILE BOTTOM NAVIGATION
   ============================================================ */
.mg-mobile-nav {
    position: fixed;
    bottom: 0;
    left: 0;
    width: 100%;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(12px);
    -webkit-backdrop-filter: blur(12px);
    border-top: 1px solid var(--mg-border);
    z-index: 100;
    padding-bottom: env(safe-area-inset-bottom);
}

/* HIDDEN ON DESKTOP */
@media (min-width: 1024px) {
    .mg-mobile-nav {
        display: none !important;
    }
}

.mg-mobile-nav__grid {
    max-width: 420px;
    margin: 0 auto;
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 4px;
}

.mg-mobile-nav__link {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 3px;
    padding: 6px 4px;
    border-radius: var(--mg-radius);
    font-size: 0.62rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.06em;
    color: var(--mg-ink-muted);
    transition: all var(--mg-transition);
}

.mg-mobile-nav__link i {
    font-size: 1.05rem;
}

.mg-mobile-nav__link.active {
    color: var(--mg-olive);
    background: rgba(92, 107, 85, 0.06);
}


/* ============================================================
   MAIN CONTENT
   ============================================================ */
.mg-main {
    min-height: 60vh;
    padding-bottom: 80px;
}

@media (min-width: 1024px) {
    .mg-main { padding-bottom: 0; }
}


/* ============================================================
   SECTION UTILITIES
   ============================================================ */
.mg-section {
    padding: var(--mg-space-2xl) 0;
}

@media (min-width: 768px) {
    .mg-section { padding: var(--mg-space-3xl) 0; }
}

@media (min-width: 1024px) {
    .mg-section { padding: var(--mg-space-4xl) 0; }
}

.mg-section__kicker {
    display: block;
    font-size: 0.72rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.2em;
    color: var(--mg-gold);
    margin-bottom: 10px;
}

.mg-section__title {
    font-family: 'Playfair Display', serif;
    font-size: clamp(1.75rem, 4vw, 2.75rem);
    font-weight: 700;
    line-height: 1.15;
    color: var(--mg-ink);
    margin-bottom: 12px;
}

.mg-section__desc {
    font-size: 0.9rem;
    line-height: 1.7;
    color: var(--mg-ink-light);
    max-width: 560px;
}

.mg-section__header {
    display: flex;
    flex-direction: column;
    gap: 6px;
    margin-bottom: var(--mg-space-xl);
}

@media (min-width: 768px) {
    .mg-section__header {
        flex-direction: row;
        align-items: flex-end;
        justify-content: space-between;
        gap: 24px;
    }
}

.mg-section__link {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    font-size: 0.8rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    color: var(--mg-olive);
    transition: color var(--mg-transition);
    white-space: nowrap;
    flex-shrink: 0;
}

.mg-section__link:hover {
    color: var(--mg-gold);
}

.mg-section__link i {
    font-size: 0.7rem;
    transition: transform var(--mg-transition);
}

.mg-section__link:hover i {
    transform: translateX(4px);
}


/* ============================================================
   HERO SECTION
   ============================================================ */
.mg-hero {
    padding: var(--mg-space-md);
}

@media (min-width: 640px) {
    .mg-hero { padding: var(--mg-space-lg); }
}

@media (min-width: 1024px) {
    .mg-hero { padding: var(--mg-space-xl); }
}

.mg-hero__inner {
    max-width: var(--mg-container);
    margin: 0 auto;
}

.mg-hero__grid {
    display: grid;
    gap: var(--mg-space-lg);
}

@media (min-width: 1024px) {
    .mg-hero__grid {
        grid-template-columns: 1.1fr 0.9fr;
        gap: var(--mg-space-xl);
    }
}

/* Hero text card */
.mg-hero__content {
    background: var(--mg-bg-white);
    border-radius: var(--mg-radius-xl);
    border: 1px solid var(--mg-border);
    padding: var(--mg-space-xl) var(--mg-space-lg);
    position: relative;
    overflow: hidden;
    box-shadow: var(--mg-shadow);
}

@media (min-width: 640px) {
    .mg-hero__content { padding: var(--mg-space-2xl); }
}

.mg-hero__content::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 3px;
    background: linear-gradient(90deg, var(--mg-gold), var(--mg-olive), var(--mg-gold));
}

.mg-hero__kicker {
    font-size: 0.7rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.22em;
    color: var(--mg-gold);
    margin-bottom: 16px;
}

.mg-hero__title {
    font-family: 'Playfair Display', serif;
    font-size: clamp(1.8rem, 4.5vw, 3rem);
    font-weight: 700;
    line-height: 1.15;
    color: var(--mg-ink);
    margin-bottom: 16px;
}

.mg-hero__subtitle {
    font-size: 0.9rem;
    line-height: 1.7;
    color: var(--mg-ink-light);
    max-width: 480px;
    margin-bottom: 28px;
}

.mg-hero__actions {
    display: flex;
    flex-wrap: wrap;
    gap: 12px;
    margin-bottom: 32px;
}

.mg-hero__stats {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 12px;
}

@media (max-width: 480px) {
    .mg-hero__stats { grid-template-columns: 1fr; gap: 8px; }
}

.mg-hero__stat {
    background: var(--mg-bg-soft);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius);
    padding: 14px 16px;
    text-align: center;
}

.mg-hero__stat-value {
    display: block;
    font-family: 'Playfair Display', serif;
    font-size: 1.5rem;
    font-weight: 700;
    color: var(--mg-olive);
    line-height: 1.2;
}

.mg-hero__stat-label {
    display: block;
    font-size: 0.65rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.14em;
    color: var(--mg-ink-muted);
    margin-top: 4px;
}

/* Hero media card */
.mg-hero__media {
    border-radius: var(--mg-radius-xl);
    overflow: hidden;
    border: 1px solid var(--mg-border);
    box-shadow: var(--mg-shadow-lg);
    position: relative;
    background: var(--mg-bg-cream);
}

.mg-hero__slider {
    position: relative;
    height: 300px;
    overflow: hidden;
}

@media (min-width: 640px) {
    .mg-hero__slider { height: 380px; }
}

@media (min-width: 1024px) {
    .mg-hero__slider { height: 100%; min-height: 420px; }
}

.mg-hero__slide {
    position: absolute;
    inset: 0;
    opacity: 0;
    transition: opacity 0.6s ease;
}

.mg-hero__slide.active {
    opacity: 1;
}

.mg-hero__slide img {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.mg-hero__overlay {
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    padding: 24px;
    background: linear-gradient(to top, rgba(0,0,0,0.5), rgba(0,0,0,0.1), transparent);
    color: #fff;
    pointer-events: none;
}

.mg-hero__overlay-kicker {
    font-size: 0.68rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.18em;
    color: rgba(255,255,255,0.7);
    margin-bottom: 6px;
}

.mg-hero__overlay-text {
    font-size: 0.85rem;
    line-height: 1.6;
    color: rgba(255,255,255,0.8);
    max-width: 420px;
}

.mg-hero__dots {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 14px 20px;
}

.mg-hero__dot {
    width: 10px;
    height: 10px;
    border-radius: 50%;
    border: 1px solid rgba(44, 44, 44, 0.2);
    background: rgba(44, 44, 44, 0.1);
    cursor: pointer;
    transition: all var(--mg-transition);
}

.mg-hero__dot.active {
    background: var(--mg-olive);
    border-color: var(--mg-olive);
}


/* ============================================================
   BUTTONS
   ============================================================ */
.mg-btn {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    border-radius: var(--mg-radius-full);
    font-size: 0.8rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    padding: 12px 24px;
    transition: all var(--mg-transition);
    white-space: nowrap;
    cursor: pointer;
    border: none;
}

.mg-btn--primary {
    background: var(--mg-olive);
    color: #fff;
    box-shadow: 0 4px 16px rgba(92, 107, 85, 0.2);
}

.mg-btn--primary:hover {
    background: var(--mg-olive-dark);
    box-shadow: 0 6px 20px rgba(92, 107, 85, 0.3);
    transform: translateY(-1px);
}

.mg-btn--ghost {
    background: var(--mg-bg-white);
    color: var(--mg-ink);
    border: 1px solid var(--mg-border-strong);
    box-shadow: var(--mg-shadow-sm);
}

.mg-btn--ghost:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
    box-shadow: var(--mg-shadow);
}

.mg-btn--large {
    padding: 14px 32px;
    font-size: 0.85rem;
}

.mg-btn--full {
    width: 100%;
}


/* ============================================================
   CATEGORY CARDS
   ============================================================ */
.mg-categories__grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
}

@media (min-width: 640px) {
    .mg-categories__grid { gap: 16px; }
}

@media (min-width: 768px) {
    .mg-categories__grid { grid-template-columns: repeat(3, 1fr); }
}

@media (min-width: 1024px) {
    .mg-categories__grid { grid-template-columns: repeat(4, 1fr); }
}

.mg-category-card {
    display: flex;
    align-items: center;
    gap: 14px;
    padding: 18px 16px;
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-lg);
    box-shadow: var(--mg-shadow-sm);
    transition: all var(--mg-transition);
    text-decoration: none;
    color: var(--mg-ink);
}

.mg-category-card:hover {
    border-color: var(--mg-gold-light);
    box-shadow: var(--mg-shadow);
    transform: translateY(-3px);
}

.mg-category-card__icon {
    width: 48px;
    height: 48px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.1rem;
    flex-shrink: 0;
    transition: transform var(--mg-transition);
}

.mg-category-card:hover .mg-category-card__icon {
    transform: scale(1.08);
}

.mg-category-card__info {
    min-width: 0;
    flex: 1;
}

.mg-category-card__name {
    font-size: 0.95rem;
    font-weight: 700;
    color: var(--mg-ink);
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    display: block;
}

.mg-category-card__label {
    font-size: 0.68rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    color: var(--mg-ink-faint);
    margin-top: 2px;
}

.mg-category-card__arrow {
    color: var(--mg-ink-faint);
    font-size: 0.7rem;
    transition: all var(--mg-transition);
    flex-shrink: 0;
}

.mg-category-card:hover .mg-category-card__arrow {
    color: var(--mg-gold);
    transform: translateX(4px);
}

/* Category section muted background */
.mg-categories__wrap {
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-xl);
    padding: var(--mg-space-lg);
    box-shadow: var(--mg-shadow-sm);
}

@media (min-width: 640px) {
    .mg-categories__wrap { padding: var(--mg-space-xl); }
}


/* ============================================================
   PRODUCT CARDS
   ============================================================ */
.mg-products__grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
}

@media (min-width: 640px) {
    .mg-products__grid { gap: 16px; }
}

@media (min-width: 768px) {
    .mg-products__grid { grid-template-columns: repeat(3, 1fr); }
}

@media (min-width: 1280px) {
    .mg-products__grid { grid-template-columns: repeat(4, 1fr); }
}

.mg-product-card {
    position: relative;
    display: flex;
    flex-direction: column;
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-lg);
    overflow: hidden;
    box-shadow: var(--mg-shadow-sm);
    transition: all 0.35s cubic-bezier(0.4, 0, 0.2, 1);
}

.mg-product-card:hover {
    box-shadow: var(--mg-shadow-lg);
    border-color: var(--mg-border-strong);
    transform: translateY(-4px);
}

/* Product badges */
.mg-product-card__badges {
    position: absolute;
    top: 12px;
    left: 12px;
    z-index: 5;
    display: flex;
    flex-direction: column;
    gap: 6px;
    pointer-events: none;
}

.mg-badge {
    display: inline-flex;
    align-items: center;
    padding: 4px 10px;
    border-radius: var(--mg-radius-full);
    font-size: 0.62rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.1em;
}

.mg-badge--new {
    background: rgba(92, 107, 85, 0.1);
    color: var(--mg-olive);
}

.mg-badge--bestseller {
    background: rgba(184, 149, 106, 0.15);
    color: var(--mg-gold-dark);
}

.mg-badge--discount {
    background: #FEE2E2;
    color: #B91C1C;
}

/* Product action buttons */
.mg-product-card__like,
.mg-product-card__zoom {
    position: absolute;
    z-index: 5;
    width: 38px;
    height: 38px;
    border-radius: 50%;
    background: rgba(255, 255, 255, 0.92);
    backdrop-filter: blur(8px);
    border: 1px solid rgba(255, 255, 255, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.9rem;
    color: var(--mg-ink);
    box-shadow: 0 2px 8px rgba(0,0,0,0.08);
    opacity: 0;
    transition: all var(--mg-transition);
    cursor: pointer;
}

.mg-product-card:hover .mg-product-card__like,
.mg-product-card:hover .mg-product-card__zoom {
    opacity: 1;
}

.mg-product-card__like {
    top: 12px;
    right: 12px;
}

.mg-product-card__zoom {
    top: 56px;
    right: 12px;
}

.mg-product-card__like:hover,
.mg-product-card__zoom:hover {
    background: #fff;
    border-color: var(--mg-gold);
    color: var(--mg-olive);
    transform: scale(1.1);
}

.mg-product-card__like.liked {
    opacity: 1;
    color: #E11D48;
}

.mg-product-card__like.liked i {
    animation: mg-heart-pop 0.3s ease;
}

@keyframes mg-heart-pop {
    0% { transform: scale(1); }
    50% { transform: scale(1.3); }
    100% { transform: scale(1); }
}

/* Product image */
.mg-product-card__img-wrap {
    position: relative;
    display: block;
    background: var(--mg-bg-soft);
    aspect-ratio: 4 / 4.5;
    overflow: hidden;
}

.mg-product-card__img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 0.5s cubic-bezier(0.4, 0, 0.2, 1);
}

.mg-product-card:hover .mg-product-card__img {
    transform: scale(1.05);
}

/* Product info */
.mg-product-card__body {
    display: flex;
    flex-direction: column;
    flex: 1;
    padding: 16px;
    gap: 10px;
}

.mg-product-card__title {
    font-family: 'Playfair Display', serif;
    font-size: 1rem;
    font-weight: 600;
    line-height: 1.35;
    color: var(--mg-ink);
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
    overflow: hidden;
    transition: color var(--mg-transition);
}

@media (min-width: 640px) {
    .mg-product-card__title { font-size: 1.05rem; }
}

.mg-product-card__title:hover {
    color: var(--mg-olive);
}

.mg-product-card__price {
    display: flex;
    flex-wrap: wrap;
    align-items: baseline;
    gap: 8px;
}

.mg-product-card__price-old {
    font-size: 0.8rem;
    color: var(--mg-ink-faint);
    text-decoration: line-through;
}

.mg-product-card__price-current {
    font-size: 1.2rem;
    font-weight: 700;
    color: var(--mg-olive);
}

@media (min-width: 640px) {
    .mg-product-card__price-current { font-size: 1.3rem; }
}

/* Add to cart button */
.mg-product-card__cart-btn {
    margin-top: auto;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    width: 100%;
    padding: 11px 16px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-olive);
    color: #fff;
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    border: none;
    cursor: pointer;
    box-shadow: 0 3px 12px rgba(92, 107, 85, 0.15);
    transition: all var(--mg-transition);
}

.mg-product-card__cart-btn:hover {
    background: var(--mg-olive-dark);
    box-shadow: 0 4px 16px rgba(92, 107, 85, 0.25);
}

.mg-product-card__cart-btn .neon-btn-inner {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
}

.mg-product-card__out-of-stock {
    font-size: 0.72rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    color: #DC2626;
    text-align: center;
    padding: 8px 0;
}


/* ============================================================
   TRUST / FEATURES SECTION
   ============================================================ */
.mg-trust__grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
}

@media (min-width: 768px) {
    .mg-trust__grid { grid-template-columns: repeat(4, 1fr); }
}

.mg-trust-card {
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-lg);
    padding: 24px 20px;
    text-align: center;
    box-shadow: var(--mg-shadow-sm);
    transition: all var(--mg-transition);
}

.mg-trust-card:hover {
    box-shadow: var(--mg-shadow);
    transform: translateY(-2px);
}

.mg-trust-card__icon {
    width: 48px;
    height: 48px;
    margin: 0 auto 14px;
    border-radius: 50%;
    background: rgba(92, 107, 85, 0.08);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.1rem;
    color: var(--mg-olive);
}

.mg-trust-card__title {
    font-size: 0.85rem;
    font-weight: 700;
    color: var(--mg-ink);
    margin-bottom: 6px;
}

.mg-trust-card__desc {
    font-size: 0.78rem;
    color: var(--mg-ink-light);
    line-height: 1.5;
}


/* ============================================================
   NEWSLETTER
   ============================================================ */
.mg-newsletter {
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-xl);
    padding: var(--mg-space-xl) var(--mg-space-lg);
    box-shadow: var(--mg-shadow);
    position: relative;
    overflow: hidden;
}

@media (min-width: 640px) {
    .mg-newsletter { padding: var(--mg-space-2xl); }
}

.mg-newsletter::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 3px;
    background: linear-gradient(90deg, var(--mg-gold), var(--mg-olive), var(--mg-gold));
}

.mg-newsletter__body {
    display: grid;
    gap: 20px;
}

@media (min-width: 768px) {
    .mg-newsletter__body {
        grid-template-columns: 1fr auto;
        align-items: end;
    }
}

.mg-newsletter__form {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

@media (min-width: 480px) {
    .mg-newsletter__form { flex-direction: row; }
}

.mg-newsletter__input {
    flex: 1;
    min-width: 260px;
    padding: 12px 18px;
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-full);
    background: var(--mg-bg-soft);
    font-size: 0.875rem;
    color: var(--mg-ink);
    outline: none;
    transition: all var(--mg-transition);
}

.mg-newsletter__input:focus {
    border-color: var(--mg-gold);
    background: #fff;
    box-shadow: 0 0 0 3px rgba(184, 149, 106, 0.1);
}

.mg-newsletter__input::placeholder {
    color: var(--mg-ink-faint);
}


/* ============================================================
   FOOTER
   ============================================================ */
.mg-footer {
    margin-top: var(--mg-space-3xl);
    background: var(--mg-bg-white);
    border-top: 1px solid var(--mg-border);
}

.mg-footer__inner {
    max-width: var(--mg-container);
    margin: 0 auto;
    padding: var(--mg-space-2xl) var(--mg-space-md);
}

@media (min-width: 640px) {
    .mg-footer__inner { padding-left: var(--mg-space-lg); padding-right: var(--mg-space-lg); }
}

@media (min-width: 1024px) {
    .mg-footer__inner { padding-left: var(--mg-space-xl); padding-right: var(--mg-space-xl); }
}

/* Footer top */
.mg-footer__top {
    display: grid;
    gap: 32px;
    padding-bottom: 32px;
    border-bottom: 1px solid var(--mg-border);
}

@media (min-width: 1024px) {
    .mg-footer__top { grid-template-columns: 1.5fr 1fr; }
}

.mg-footer__brand {
    display: flex;
    align-items: flex-start;
    gap: 16px;
}

.mg-footer__brand-logo {
    height: 40px;
    width: auto;
    object-fit: contain;
    flex-shrink: 0;
}

.mg-footer__brand-name {
    font-family: 'Playfair Display', serif;
    font-size: 1.3rem;
    font-weight: 700;
    color: var(--mg-ink);
    margin-bottom: 4px;
}

.mg-footer__brand-desc {
    font-size: 0.85rem;
    line-height: 1.7;
    color: var(--mg-ink-light);
    max-width: 400px;
    margin-top: 8px;
}

.mg-footer__socials {
    display: flex;
    gap: 10px;
    margin-top: 16px;
}

.mg-footer__socials a {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    border: 1px solid var(--mg-border);
    display: flex;
    align-items: center;
    justify-content: center;
    color: var(--mg-ink-muted);
    font-size: 0.9rem;
    transition: all var(--mg-transition);
}

.mg-footer__socials a:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
}

/* Footer trust badges */
.mg-footer__trust {
    display: grid;
    gap: 10px;
}

@media (min-width: 640px) and (max-width: 1023px) {
    .mg-footer__trust { grid-template-columns: repeat(3, 1fr); }
}

.mg-footer__trust-item {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 14px 16px;
    background: var(--mg-bg-soft);
    border-radius: var(--mg-radius);
    border: 1px solid var(--mg-border);
}

.mg-footer__trust-item i {
    color: var(--mg-olive);
    font-size: 0.9rem;
    margin-top: 2px;
    flex-shrink: 0;
}

.mg-footer__trust-item strong {
    display: block;
    font-size: 0.78rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    color: var(--mg-ink);
    margin-bottom: 2px;
}

.mg-footer__trust-item span {
    font-size: 0.78rem;
    color: var(--mg-ink-light);
    line-height: 1.4;
}

/* Footer columns */
.mg-footer__columns {
    display: grid;
    gap: 28px;
    padding-top: 32px;
}

@media (min-width: 768px) {
    .mg-footer__columns { grid-template-columns: repeat(2, 1fr); }
}

@media (min-width: 1280px) {
    .mg-footer__columns { grid-template-columns: repeat(4, 1fr); }
}

.mg-footer__col-title {
    font-family: 'Playfair Display', serif;
    font-size: 1.15rem;
    font-weight: 700;
    color: var(--mg-ink);
    margin-bottom: 16px;
}

.mg-footer__col-list {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.mg-footer__col-list li {
    font-size: 0.85rem;
    color: var(--mg-ink-light);
    line-height: 1.5;
}

.mg-footer__col-list a:hover {
    color: var(--mg-olive);
}

.mg-footer__col-list i {
    color: var(--mg-olive);
    margin-right: 8px;
    font-size: 0.8rem;
}

/* Footer bottom */
.mg-footer__bottom {
    margin-top: 32px;
    padding-top: 20px;
    border-top: 1px solid var(--mg-border);
    display: flex;
    flex-direction: column;
    gap: 12px;
    align-items: center;
}

@media (min-width: 640px) {
    .mg-footer__bottom { flex-direction: row; justify-content: space-between; }
}

.mg-footer__copyright {
    font-size: 0.8rem;
    color: var(--mg-ink-muted);
}

.mg-footer__payments {
    display: flex;
    align-items: center;
    gap: 12px;
    font-size: 1.3rem;
    color: var(--mg-ink-faint);
}


/* ============================================================
   LIGHTBOX
   ============================================================ */
.mg-lightbox {
    position: fixed;
    inset: 0;
    z-index: 200;
    background: rgba(0, 0, 0, 0.92);
    display: flex;
    align-items: center;
    justify-content: center;
    opacity: 0;
    pointer-events: none;
    transition: opacity var(--mg-transition);
}

.mg-lightbox.active {
    opacity: 1;
    pointer-events: auto;
}

.mg-lightbox__close {
    position: absolute;
    top: 20px;
    right: 20px;
    width: 48px;
    height: 48px;
    border-radius: 50%;
    background: rgba(255,255,255,0.1);
    backdrop-filter: blur(8px);
    border: 1px solid rgba(255,255,255,0.15);
    color: #fff;
    font-size: 1.2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    transition: all var(--mg-transition);
}

.mg-lightbox__close:hover {
    background: #fff;
    color: var(--mg-ink);
}

.mg-lightbox img {
    max-width: 90%;
    max-height: 85vh;
    object-fit: contain;
    border-radius: var(--mg-radius);
}


/* ============================================================
   UTILITY CLASSES (bakc-compat with existing code)
   ============================================================ */

/* Backward compatibility with existing JS/Razor references */
.storefront-shell { position: relative; min-height: 100vh; overflow-x: hidden; }
.storefront-main { min-height: 60vh; }

/* Existing neon-btn-wrapper compatibility */
.neon-btn-wrapper {
    margin-top: auto;
    display: inline-flex;
    width: 100%;
    align-items: center;
    justify-content: center;
    border-radius: var(--mg-radius-full);
    background: var(--mg-olive);
    padding: 11px 16px;
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    color: #fff;
    box-shadow: 0 3px 12px rgba(92, 107, 85, 0.15);
    transition: all var(--mg-transition);
    border: none;
    cursor: pointer;
}

.neon-btn-wrapper:hover {
    background: var(--mg-olive-dark);
}

.neon-btn-inner {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
}

/* Like / Zoom btn compatibility */
.like-btn, .zoom-btn {
    position: absolute;
    z-index: 5;
    width: 38px;
    height: 38px;
    border-radius: 50%;
    background: rgba(255,255,255,0.92);
    backdrop-filter: blur(8px);
    border: 1px solid rgba(255,255,255,0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.9rem;
    color: var(--mg-ink);
    box-shadow: 0 2px 8px rgba(0,0,0,0.08);
    cursor: pointer;
    transition: all var(--mg-transition);
}

.like-btn { top: 12px; right: 12px; }
.zoom-btn { top: 12px; left: 12px; }

.like-btn:hover, .zoom-btn:hover {
    background: #fff;
    border-color: var(--mg-gold);
    color: var(--mg-olive);
}

.like-btn.liked { color: #E11D48; }

/* Pro-product-card compatibility */
.pro-product-card {
    position: relative;
    display: flex;
    flex-direction: column;
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-lg);
    overflow: hidden;
    box-shadow: var(--mg-shadow-sm);
    transition: all 0.35s cubic-bezier(0.4, 0, 0.2, 1);
    height: 100%;
}

.pro-product-card:hover {
    box-shadow: var(--mg-shadow-lg);
    border-color: var(--mg-border-strong);
    transform: translateY(-4px);
}

.product-badges {
    position: absolute;
    top: 12px;
    left: 12px;
    z-index: 5;
    display: flex;
    flex-direction: column;
    gap: 6px;
    pointer-events: none;
}

.badge-item {
    display: inline-flex;
    align-items: center;
    padding: 4px 10px;
    border-radius: var(--mg-radius-full);
    font-size: 0.62rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.1em;
}

.badge-bestseller { background: rgba(184,149,106,0.15); color: var(--mg-gold-dark); }
.badge-new { background: rgba(92,107,85,0.1); color: var(--mg-olive); }
.badge-discount { background: #FEE2E2; color: #B91C1C; }

.pro-img-container {
    position: relative;
    display: block;
    background: var(--mg-bg-soft);
    aspect-ratio: 4 / 4.5;
    overflow: hidden;
}

.pro-img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 0.5s cubic-bezier(0.4, 0, 0.2, 1);
}

.pro-product-card:hover .pro-img {
    transform: scale(1.05);
}

.card-info {
    display: flex;
    flex-direction: column;
    flex: 1;
    padding: 16px;
    gap: 10px;
}

.product-title {
    font-family: 'Playfair Display', serif;
    font-size: 1rem;
    font-weight: 600;
    line-height: 1.35;
    color: var(--mg-ink);
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
    overflow: hidden;
    text-decoration: none;
}

.product-title:hover { color: var(--mg-olive); }

.price-wrapper {
    display: flex;
    flex-wrap: wrap;
    align-items: baseline;
    gap: 8px;
}

.old-price {
    font-size: 0.8rem;
    color: var(--mg-ink-faint);
    text-decoration: line-through;
}

.new-price {
    font-size: 1.25rem;
    font-weight: 700;
    color: var(--mg-olive);
}

/* Card glare */
.card-glare {
    pointer-events: none;
    position: absolute;
    inset: 0;
    z-index: 10;
    opacity: 0;
    transition: opacity 0.3s;
}

/* Product grid */
.product-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
}

@media (min-width: 640px) {
    .product-grid { gap: 16px; }
}

@media (min-width: 768px) {
    .product-grid { grid-template-columns: repeat(3, 1fr); }
}

@media (min-width: 1280px) {
    .product-grid { grid-template-columns: repeat(4, 1fr); }
}

/* FlyToCart animation */
.cart-bounce {
    animation: mg-cart-bounce 0.5s ease;
}

@keyframes mg-cart-bounce {
    0%, 100% { transform: scale(1); }
    50% { transform: scale(1.3); }
}

/* Toast container compat */
.toast-container {
    position: fixed;
    right: 16px;
    top: 16px;
    z-index: 300;
    width: 100%;
    max-width: 360px;
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.toast-message {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 14px;
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius);
    box-shadow: var(--mg-shadow-lg);
    opacity: 0;
    transform: translateY(-10px);
    transition: all 0.3s ease;
}

.toast-message.show {
    opacity: 1;
    transform: translateY(0);
}

.toast-icon {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    background: rgba(92, 107, 85, 0.1);
    display: flex;
    align-items: center;
    justify-content: center;
    color: var(--mg-olive);
    flex-shrink: 0;
}

.toast-title {
    font-size: 0.8rem;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    color: var(--mg-ink);
}

.toast-text {
    font-size: 0.82rem;
    color: var(--mg-ink-light);
    margin-top: 2px;
    line-height: 1.4;
}


/* ============================================================
   SCROLL REVEAL
   ============================================================ */
.mg-reveal {
    opacity: 0;
    transform: translateY(24px);
    transition: opacity 0.6s ease, transform 0.6s ease;
}

.mg-reveal.revealed {
    opacity: 1;
    transform: translateY(0);
}

/* Legacy scroll-reveal classes */
.scroll-reveal {
    opacity: 0;
    transform: translateY(20px);
    transition: all 0.7s cubic-bezier(0.4, 0, 0.2, 1);
}

.scroll-reveal.revealed {
    opacity: 1;
    transform: translateY(0);
}


/* ============================================================
   SECTION LINK COMPATIBILITY
   ============================================================ */
.section-kicker {
    display: block;
    font-size: 0.72rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.2em;
    color: var(--mg-gold);
    margin-bottom: 10px;
}

.section-heading {
    font-family: 'Playfair Display', serif;
    font-size: clamp(1.75rem, 4vw, 2.75rem);
    font-weight: 700;
    line-height: 1.15;
}

.section-lead {
    margin-top: 10px;
    font-size: 0.9rem;
    line-height: 1.7;
    color: var(--mg-ink-light);
    max-width: 560px;
}

.section-link {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    font-size: 0.8rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    color: var(--mg-olive);
}

.section-link:hover { color: var(--mg-gold); }

/* Section shell compat */
.section-shell {
    max-width: var(--mg-container);
    margin: 0 auto;
    padding: 0 var(--mg-space-md);
}

@media (min-width: 640px) {
    .section-shell { padding: 0 var(--mg-space-lg); }
}

@media (min-width: 1024px) {
    .section-shell { padding: 0 var(--mg-space-xl); }
}

.editorial-section {
    padding: var(--mg-space-2xl) 0;
}

@media (min-width: 1024px) {
    .editorial-section { padding: var(--mg-space-3xl) 0; }
}

.editorial-section-muted {
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-xl);
    padding: var(--mg-space-lg);
    box-shadow: var(--mg-shadow-sm);
}

@media (min-width: 640px) {
    .editorial-section-muted { padding: var(--mg-space-xl); }
}


/* ============================================================
   COLLECTION CARD COMPAT
   ============================================================ */
.collection-card {
    display: flex;
    align-items: center;
    gap: 14px;
    padding: 18px 16px;
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-lg);
    box-shadow: var(--mg-shadow-sm);
    transition: all var(--mg-transition);
    text-decoration: none;
    color: var(--mg-ink);
}

.collection-card:hover {
    border-color: var(--mg-gold-light);
    box-shadow: var(--mg-shadow);
    transform: translateY(-3px);
}

.collection-card-icon {
    width: 48px;
    height: 48px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.1rem;
    flex-shrink: 0;
    background: var(--mg-bg-soft);
    color: var(--mg-olive);
}

.collection-card-copy {
    min-width: 0;
    flex: 1;
}

.collection-card-copy strong {
    display: block;
    font-size: 0.95rem;
    font-weight: 700;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.collection-card-copy span {
    display: block;
    font-size: 0.68rem;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    color: var(--mg-ink-faint);
    margin-top: 2px;
}

.section-card-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
}

@media (min-width: 640px) {
    .section-card-grid { gap: 16px; }
}

@media (min-width: 1024px) {
    .section-card-grid { grid-template-columns: repeat(4, 1fr); }
}


/* ============================================================
   FEATURE GRID COMPAT
   ============================================================ */
.feature-grid {
    display: grid;
    gap: 12px;
}

@media (min-width: 768px) {
    .feature-grid { grid-template-columns: repeat(2, 1fr); }
}

@media (min-width: 1280px) {
    .feature-grid { grid-template-columns: repeat(4, 1fr); }
}

.feature-card {
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-lg);
    padding: 24px 20px;
    box-shadow: var(--mg-shadow-sm);
}

.feature-icon {
    width: 48px;
    height: 48px;
    border-radius: 50%;
    background: rgba(92, 107, 85, 0.08);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.1rem;
    color: var(--mg-olive);
    margin-bottom: 16px;
}

.feature-title {
    font-family: 'Playfair Display', serif;
    font-size: 1.15rem;
    font-weight: 700;
    margin-bottom: 8px;
}

.feature-copy {
    font-size: 0.82rem;
    line-height: 1.6;
    color: var(--mg-ink-light);
}


/* ============================================================
   HERO SHELL COMPAT
   ============================================================ */
.hero-shell {
    padding: var(--mg-space-md);
}

@media (min-width: 640px) { .hero-shell { padding: var(--mg-space-lg); } }
@media (min-width: 1024px) { .hero-shell { padding: var(--mg-space-xl); } }

.hero-shell { max-width: var(--mg-container); margin: 0 auto; }

.hero-grid {
    display: grid;
    gap: var(--mg-space-lg);
}

@media (min-width: 1024px) {
    .hero-grid { grid-template-columns: 1.1fr 0.9fr; }
}

.hero-copy-card {
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-xl);
    padding: var(--mg-space-xl) var(--mg-space-lg);
    position: relative;
    overflow: hidden;
    box-shadow: var(--mg-shadow);
}

@media (min-width: 640px) {
    .hero-copy-card { padding: var(--mg-space-2xl); }
}

.hero-copy-card::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 3px;
    background: linear-gradient(90deg, var(--mg-gold), var(--mg-olive), var(--mg-gold));
}

.hero-actions {
    display: flex;
    flex-wrap: wrap;
    gap: 12px;
    margin-top: 28px;
}

.storefront-button {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-olive);
    color: #fff;
    padding: 12px 24px;
    font-size: 0.8rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    box-shadow: 0 4px 16px rgba(92, 107, 85, 0.2);
    transition: all var(--mg-transition);
    border: none;
    cursor: pointer;
}

.storefront-button:hover {
    background: var(--mg-olive-dark);
    box-shadow: 0 6px 20px rgba(92, 107, 85, 0.3);
}

.storefront-button-ghost {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-bg-white);
    color: var(--mg-ink);
    border: 1px solid var(--mg-border-strong);
    padding: 12px 24px;
    font-size: 0.8rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.12em;
    box-shadow: var(--mg-shadow-sm);
    transition: all var(--mg-transition);
    cursor: pointer;
}

.storefront-button-ghost:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
}

.hero-stats {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 12px;
    margin-top: 32px;
}

@media (max-width: 480px) {
    .hero-stats { grid-template-columns: 1fr; gap: 8px; }
}

.hero-stat {
    background: var(--mg-bg-soft);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius);
    padding: 14px 16px;
    text-align: center;
}

.hero-stat strong {
    display: block;
    font-family: 'Playfair Display', serif;
    font-size: 1.5rem;
    font-weight: 700;
    color: var(--mg-olive);
}

.hero-stat span {
    display: block;
    font-size: 0.65rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.14em;
    color: var(--mg-ink-muted);
    margin-top: 4px;
}

.hero-media-card {
    border-radius: var(--mg-radius-xl);
    overflow: hidden;
    border: 1px solid var(--mg-border);
    box-shadow: var(--mg-shadow-lg);
    background: var(--mg-bg-cream);
    position: relative;
}

.hero-media-frame {
    position: relative;
    height: 300px;
    overflow: hidden;
}

@media (min-width: 640px) { .hero-media-frame { height: 380px; } }
@media (min-width: 1024px) { .hero-media-frame { height: 100%; min-height: 420px; } }

.hero-media-slide {
    position: absolute;
    inset: 0;
    opacity: 0;
    transition: opacity 0.6s ease;
}

.hero-media-slide.active { opacity: 1; }

.hero-media-slide img {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.hero-media-overlay {
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    padding: 24px;
    background: linear-gradient(to top, rgba(0,0,0,0.5), rgba(0,0,0,0.1), transparent);
    color: #fff;
    pointer-events: none;
}


/* ============================================================
   PRICE INPUT / AUTH / CHECKOUT COMPAT
   ============================================================ */
.price-input, .auth-input, .checkout-input, .coupon-input {
    width: 100%;
    padding: 12px 16px;
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius);
    background: var(--mg-bg-soft);
    font-size: 0.875rem;
    color: var(--mg-ink);
    outline: none;
    transition: all var(--mg-transition);
}

.price-input:focus, .auth-input:focus, .checkout-input:focus, .coupon-input:focus {
    border-color: var(--mg-gold);
    background: #fff;
}


/* ============================================================
   CUSTOM LIGHTBOX COMPAT
   ============================================================ */
.custom-lightbox {
    position: fixed;
    inset: 0;
    z-index: 200;
    background: rgba(0, 0, 0, 0.92);
    display: flex;
    align-items: center;
    justify-content: center;
    opacity: 0;
    pointer-events: none;
    transition: opacity var(--mg-transition);
}

.custom-lightbox.active {
    opacity: 1;
    pointer-events: auto;
}

.custom-lightbox-close {
    position: absolute;
    top: 20px;
    right: 20px;
    width: 48px;
    height: 48px;
    border-radius: 50%;
    background: rgba(255,255,255,0.1);
    border: 1px solid rgba(255,255,255,0.15);
    color: #fff;
    font-size: 1.2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    transition: all var(--mg-transition);
}

.custom-lightbox-close:hover { background: #fff; color: var(--mg-ink); }

.custom-lightbox img {
    max-width: 90%;
    max-height: 85vh;
    object-fit: contain;
}


/* ============================================================
   LIKE-POP / HEART-PARTICLE ANIMATIONS
   ============================================================ */
.like-pop {
    animation: mg-like-pop 0.3s ease;
}

@keyframes mg-like-pop {
    0% { transform: scale(1); }
    50% { transform: scale(1.2); }
    100% { transform: scale(1); }
}

.heart-particle {
    position: absolute;
    font-size: 0.6rem;
    pointer-events: none;
    animation: mg-particle-burst 0.6s ease-out forwards;
    animation-delay: var(--delay, 0s);
}

@keyframes mg-particle-burst {
    0% { opacity: 1; transform: translateY(0) rotate(0deg); }
    100% { opacity: 0; transform: translateY(-20px) rotate(var(--angle, 60deg)) scale(0.5); }
}


/* ============================================================
   FLEX/GRID HELPER COMPATIBILITY
   ============================================================ */
.flex { display: flex; }
.flex-col { flex-direction: column; }
.flex-wrap { flex-wrap: wrap; }
.items-center { align-items: center; }
.items-start { align-items: flex-start; }
.items-end { align-items: flex-end; }
.justify-center { justify-content: center; }
.justify-between { justify-content: space-between; }
.gap-1 { gap: 4px; }
.gap-2 { gap: 8px; }
.gap-3 { gap: 12px; }
.gap-4 { gap: 16px; }
.gap-6 { gap: 24px; }
.gap-8 { gap: 32px; }
.hidden { display: none; }
.block { display: block; }
.inline-flex { display: inline-flex; }
.w-full { width: 100%; }
.text-center { text-align: center; }
.mt-1 { margin-top: 4px; }
.mt-2 { margin-top: 8px; }
.mt-3 { margin-top: 12px; }
.mt-4 { margin-top: 16px; }
.mt-6 { margin-top: 24px; }
.mt-8 { margin-top: 32px; }
.mb-0 { margin-bottom: 0; }
.mb-2 { margin-bottom: 8px; }
.mb-4 { margin-bottom: 16px; }
.mb-8 { margin-bottom: 32px; }
.me-2 { margin-right: 8px; }
.p-4 { padding: 16px; }
.p-6 { padding: 24px; }
.px-6 { padding-left: 24px; padding-right: 24px; }
.py-5 { padding-top: 20px; padding-bottom: 20px; }
.py-8 { padding-top: 32px; padding-bottom: 32px; }
.rounded-full { border-radius: 9999px; }
.overflow-hidden { overflow: hidden; }
.text-sm { font-size: 0.875rem; }
.text-xs { font-size: 0.75rem; }
.text-lg { font-size: 1.125rem; }
.text-xl { font-size: 1.25rem; }
.text-3xl { font-size: 1.875rem; }
.font-semibold { font-weight: 600; }
.uppercase { text-transform: uppercase; }
.leading-7 { line-height: 1.75rem; }
.max-w-md { max-width: 28rem; }

@media (min-width: 640px) {
    .sm\:inline { display: inline; }
    .sm\:flex-row { flex-direction: row; }
    .sm\:px-8 { padding-left: 32px; padding-right: 32px; }
}

@media (min-width: 1024px) {
    .lg\:hidden { display: none; }
    .lg\:flex-row { flex-direction: row; }
    .lg\:items-end { align-items: flex-end; }
    .lg\:justify-between { justify-content: space-between; }
    .lg\:grid-cols-\[1fr_auto\] { grid-template-columns: 1fr auto; }
}

/* Grid compatibility */
.grid { display: grid; }

@media (min-width: 640px) {
    .sm\:grid-cols-3 { grid-template-columns: repeat(3, 1fr); }
}

@media (min-width: 768px) {
    .md\:grid-cols-2 { grid-template-columns: repeat(2, 1fr); }
}


/* ============================================================
   LEGACY SITE.CSS OVERRIDES
   These override the old fixed-position layout from site.css 
   since the new layout uses sticky positioning instead.
   ============================================================ */
body {
    padding-top: 0 !important;
    padding-bottom: 0 !important;
}

/* Hide old fixed elements from site.css that clash with new layout */
.announcement-bar,
.site-header,
.desktop-category-nav,
.mobile-bottom-nav,
.site-footer,
.mobile-category-sheet,
.menu-overlay {
    display: none !important;
}


/* ============================================================
   CATALOG PAGE (Urun/Index)
   ============================================================ */
.catalog-header {
    padding: var(--mg-space-xl) var(--mg-space-md) var(--mg-space-lg);
    max-width: var(--mg-container);
    margin: 0 auto;
}

@media (min-width: 640px) {
    .catalog-header { padding-left: var(--mg-space-lg); padding-right: var(--mg-space-lg); }
}

@media (min-width: 1024px) {
    .catalog-header { padding-left: var(--mg-space-xl); padding-right: var(--mg-space-xl); }
}

.catalog-intro {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.product-breadcrumb {
    display: flex;
    align-items: center;
    gap: 6px;
    font-size: 0.82rem;
    color: var(--mg-ink-muted);
}

.product-breadcrumb a {
    color: var(--mg-ink-muted);
    transition: color var(--mg-transition);
}

.product-breadcrumb a:hover { color: var(--mg-olive); }

.catalog-chip-row {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
    margin-top: 12px;
}

.cat-chip {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 8px 16px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    font-size: 0.78rem;
    font-weight: 600;
    color: var(--mg-ink-light);
    text-transform: uppercase;
    letter-spacing: 0.08em;
    transition: all var(--mg-transition);
    white-space: nowrap;
}

.cat-chip:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
}

.cat-chip.active {
    background: var(--mg-olive);
    color: #fff;
    border-color: var(--mg-olive);
}

.cat-chip i {
    font-size: 0.75rem;
}


/* Filter Bar */
.filter-bar {
    background: var(--mg-bg-white);
    border-top: 1px solid var(--mg-border);
    border-bottom: 1px solid var(--mg-border);
    position: sticky;
    top: 0;
    z-index: 50;
    transition: box-shadow var(--mg-transition);
}

.filter-bar.scrolled {
    box-shadow: var(--mg-shadow);
}

.filter-bar-inner {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    justify-content: space-between;
    gap: 12px;
    padding: 12px 0;
}

.result-count {
    font-size: 0.82rem;
    color: var(--mg-ink-light);
}

.result-count strong {
    color: var(--mg-ink);
    font-weight: 700;
}

.active-filter-tag {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 5px 12px;
    border-radius: var(--mg-radius-full);
    background: rgba(92, 107, 85, 0.08);
    color: var(--mg-olive);
    font-size: 0.75rem;
    font-weight: 600;
}

.remove-filter {
    color: var(--mg-ink-muted);
    font-size: 0.65rem;
    transition: color var(--mg-transition);
}

.remove-filter:hover { color: #DC2626; }

.filter-controls {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 8px;
}

.custom-dropdown {
    position: relative;
}

.filter-dropdown-btn {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 8px 14px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-bg-soft);
    border: 1px solid var(--mg-border);
    font-size: 0.78rem;
    font-weight: 600;
    color: var(--mg-ink);
    transition: all var(--mg-transition);
    white-space: nowrap;
    cursor: pointer;
}

.filter-dropdown-btn:hover {
    border-color: var(--mg-gold);
}

.custom-dropdown-menu {
    position: absolute;
    top: 100%;
    right: 0;
    z-index: 60;
    min-width: 200px;
    margin-top: 8px;
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border);
    border-radius: var(--mg-radius-lg);
    box-shadow: var(--mg-shadow-lg);
    padding: 8px;
    display: none;
}

.custom-dropdown.open .custom-dropdown-menu {
    display: block;
}

.price-filter-popup {
    min-width: 260px;
}

.price-input-group {
    display: flex;
    align-items: center;
    gap: 8px;
}

.sort-menu {
    display: flex;
    flex-direction: column;
    gap: 4px;
}

.sort-item {
    display: block;
    padding: 10px 14px;
    border-radius: var(--mg-radius);
    font-size: 0.82rem;
    color: var(--mg-ink-light);
    transition: all var(--mg-transition);
}

.sort-item:hover {
    background: var(--mg-bg-soft);
    color: var(--mg-ink);
}

.sort-item.active {
    background: rgba(92, 107, 85, 0.08);
    color: var(--mg-olive);
    font-weight: 700;
}

.filter-option-btn {
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    padding: 10px 14px;
    border-radius: var(--mg-radius);
    font-size: 0.82rem;
    color: var(--mg-ink-light);
    background: none;
    border: none;
    cursor: pointer;
    transition: all var(--mg-transition);
}

.filter-option-btn:hover {
    background: var(--mg-bg-soft);
    color: var(--mg-ink);
}

.filter-option-btn.active {
    background: rgba(92, 107, 85, 0.08);
    color: var(--mg-olive);
    font-weight: 700;
}

.filter-option-meta {
    font-size: 0.72rem;
    color: var(--mg-ink-faint);
}


/* Load More */
.load-more-wrapper {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 12px;
    margin-top: var(--mg-space-xl);
    padding: var(--mg-space-lg) 0;
}

.load-more-btn {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    padding: 12px 32px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-bg-white);
    border: 1px solid var(--mg-border-strong);
    color: var(--mg-ink);
    font-size: 0.82rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    cursor: pointer;
    transition: all var(--mg-transition);
}

.load-more-btn:hover {
    border-color: var(--mg-gold);
    color: var(--mg-olive);
    box-shadow: var(--mg-shadow);
}

.load-more-btn:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.page-progress {
    font-size: 0.75rem;
    color: var(--mg-ink-muted);
}

.page-progress-bar {
    width: 200px;
    height: 3px;
    border-radius: 3px;
    background: var(--mg-border);
    overflow: hidden;
}

.page-progress-fill {
    height: 100%;
    border-radius: 3px;
    background: var(--mg-olive);
    transition: width var(--mg-transition-slow);
}


/* Empty State */
.empty-state {
    text-align: center;
    padding: var(--mg-space-3xl) var(--mg-space-lg);
}

.empty-state-icon {
    width: 80px;
    height: 80px;
    margin: 0 auto 20px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    background: var(--mg-bg-soft);
    color: var(--mg-ink-faint);
    font-size: 2rem;
}


/* Quick View Modal compat */
.quick-view-modal .modal-content {
    border: none;
    border-radius: var(--mg-radius-xl);
    overflow: hidden;
    box-shadow: var(--mg-shadow-xl);
}

.quick-view-modal .modal-body {
    padding: 0;
}

.bg-canvasia-sand {
    background: var(--mg-bg-cream);
}


/* Checkout compat */
.checkout-submit {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    width: 100%;
    padding: 10px 20px;
    border-radius: var(--mg-radius-full);
    background: var(--mg-olive);
    color: #fff;
    font-size: 0.78rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    border: none;
    cursor: pointer;
    transition: all var(--mg-transition);
}

.checkout-submit:hover {
    background: var(--mg-olive-dark);
}

/* Cart page compat */
.desktop-cart-count {
    display: inline-flex;
    align-items: center;
    justify-content: center;
}


/* ============================================================
   STOREFRONT.CSS FULL RE-THEME
   Override all storefront.css color/typography tokens to match
   MeteorGaleri design system (olive/gold/cream palette).
   These rules MUST come last to override storefront.css.
   ============================================================ */

/* --- Root color re-mapping --- */
:root {
    --sf-ink: var(--mg-ink);
    --sf-forest: var(--mg-olive);
    --sf-forest-deep: var(--mg-olive-dark);
    --sf-gold: var(--mg-gold);
    --sf-sand: var(--mg-bg-soft);
    --sf-mist: var(--mg-bg-cream);
    --sf-cream: var(--mg-bg);
    --sf-border: var(--mg-border);
    --sf-border-strong: var(--mg-border-strong);
    --sf-shadow: var(--mg-shadow);
    --sf-shadow-panel: var(--mg-shadow-lg);
}

/* --- Typography override --- */
body {
    font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif !important;
    background-color: var(--mg-bg) !important;
    color: var(--mg-ink) !important;
    background-image: none !important;
}

h1, h2, h3, h4, h5, h6 {
    font-family: 'Playfair Display', 'Georgia', serif !important;
    color: var(--mg-ink) !important;
}

/* --- Buttons → Olive --- */
.storefront-button,
.neon-btn-wrapper,
.checkout-btn,
.checkout-submit,
.coupon-apply,
.shop-now-btn,
.btn-premium,
.mobile-checkout-btn,
.header-cart-pill {
    background-color: var(--mg-olive) !important;
    box-shadow: 0 4px 16px rgba(92, 107, 85, 0.2) !important;
}

.storefront-button:hover,
.neon-btn-wrapper:hover,
.checkout-btn:hover,
.checkout-submit:hover,
.coupon-apply:hover,
.shop-now-btn:hover,
.btn-premium:hover,
.mobile-checkout-btn:hover,
.header-cart-pill:hover {
    background-color: var(--mg-olive-dark) !important;
}

.storefront-button-ghost:hover,
.cat-chip:hover,
.filter-dropdown-btn:hover,
.collection-card:hover,
.thumbnail-item:hover,
.variant-box:hover,
.drawer-link:hover,
.footer-socials a:hover,
.shell-action-link:hover,
.shell-icon-button:hover,
.desktop-nav-link:hover,
.load-more-btn:hover {
    border-color: var(--mg-gold) !important;
}

/* Active category chip */
.cat-chip.active {
    background-color: var(--mg-olive) !important;
    border-color: var(--mg-olive) !important;
    color: #fff !important;
}

/* --- Forest green → Olive text --- */
.section-link,
.section-link:hover,
.auth-link,
.auth-link:hover,
.desktop-nav-link:hover,
.product-title:hover,
.cart-breadcrumb a:hover,
.product-breadcrumb a:hover,
.breadcrumb-urun a:hover,
.header-cart-count,
.footer-socials a:hover,
.shell-action-link:hover,
.shell-icon-button:hover,
.text-canvasia-forest,
.payment-icon {
    color: var(--mg-olive) !important;
}

/* --- Price colors --- */
.current-price,
.new-price,
.cart-item-price,
.mobile-total-price,
.summary-line.total .value {
    color: var(--mg-olive) !important;
}

/* --- Gold accent kicker --- */
.section-kicker,
.auth-kicker,
.drawer-kicker,
.discount-badge {
    color: var(--mg-gold) !important;
}

.auth-metric-icon {
    color: var(--mg-gold) !important;
}

/* --- Cards & panels → new background + radius --- */
.pro-product-card,
.product-gallery-panel,
.product-copy-panel,
.order-summary,
.checkout-section-card,
.checkout-summary-card,
.auth-card,
.storefront-panel,
.media-showcase-block,
.hero-copy-card,
.hero-media-card,
.collection-card,
.feature-card,
.cart-item,
.empty-cart,
.empty-state,
.catalog-intro {
    border-color: var(--mg-border) !important;
    box-shadow: var(--mg-shadow) !important;
}

.pro-product-card:hover,
.collection-card:hover {
    border-color: var(--mg-gold) !important;
    box-shadow: var(--mg-shadow-lg) !important;
}

/* Card background to soft white */
.bg-canvasia-sand,
.bg-canvasia-cream {
    background-color: var(--mg-bg-soft) !important;
}

/* --- Product gallery → cream bg --- */
.pro-img-container,
.product-main-image,
.hero-media-card {
    background-color: var(--mg-bg-cream) !important;
}

/* --- Variant selected state --- */
.variant-radio:checked + .variant-box {
    border-color: var(--mg-olive) !important;
    background-color: rgba(92, 107, 85, 0.06) !important;
    color: var(--mg-olive) !important;
}

/* --- Hero copy card gradient bar --- */
.hero-copy-card::before {
    background-image: linear-gradient(to right, var(--mg-gold), var(--mg-olive), var(--mg-gold)) !important;
}

/* --- Hero stat → olive accent --- */
.hero-stat strong {
    color: var(--mg-olive) !important;
}

/* --- Feature icon / collection icon → cream bg --- */
.collection-card-icon,
.feature-icon,
.badge-icon,
.empty-cart-circle,
.empty-state-icon,
.review-avatar,
.toast-icon {
    background-color: var(--mg-bg-cream) !important;
    color: var(--mg-olive) !important;
}

/* --- Auth card body → olive bg --- */
.auth-card-body {
    background-color: var(--mg-olive) !important;
}

.auth-title {
    color: #fff !important;
}

/* --- Sort/filter active items --- */
.sort-item.active,
.filter-option-btn.active {
    background-color: rgba(92, 107, 85, 0.06) !important;
    color: var(--mg-olive) !important;
}

/* --- Progress fill → olive --- */
.page-progress-fill {
    background-color: var(--mg-olive) !important;
}

/* --- Checkout address selected --- */
.checkout-address-card.selected,
.address-card.selected,
.payment-method-card.active,
.checkout-radio-card.active {
    border-color: var(--mg-olive) !important;
    background-color: rgba(92, 107, 85, 0.04) !important;
}

.address-card.selected .radio-circle,
.address-selected-label {
    background-color: var(--mg-olive) !important;
    border-color: var(--mg-olive) !important;
}

/* --- Checkout hero → olive bg --- */
.checkout-hero {
    background-color: var(--mg-olive) !important;
}

.checkout-step.active .checkout-step-circle,
.checkout-step.completed .checkout-step-circle,
.stepper-item.active .stepper-circle,
.stepper-item.completed .stepper-circle {
    background-color: #fff !important;
    color: var(--mg-olive) !important;
}

/* --- Summary head → cream bg --- */
.summary-head,
.checkout-section-head {
    background-color: var(--mg-bg-soft) !important;
}

/* --- Toast success → olive --- */
.toast-success .toast-icon {
    background-color: rgba(92, 107, 85, 0.1) !important;
    color: var(--mg-olive) !important;
}

/* --- Star rating gold fix --- */
.star-rating input:checked ~ label,
.star-rating label:hover,
.star-rating label:hover ~ label,
.text-warning {
    color: var(--mg-gold) !important;
}

/* --- Mobile bottom navigation → hidden (new mg-mobile-nav used) --- */
.mobile-nav,
.mobile-checkout-bar {
    /* Keep mobile checkout bar visible on cart page */
}

/* --- Shell elements hidden (replaced by new mg- classes) --- */
.shell-topbar,
.shell-header,
.desktop-nav,
.mobile-nav,
.shell-footer,
.mobile-drawer-scrim,
.mobile-category-drawer,
.mobile-search-drawer {
    display: none !important;
}

/* --- Ensure new layout elements show --- */
.mg-topbar,
.mg-header,
.mg-nav,
.mg-drawer-overlay,
.mg-drawer,
.mg-mobile-nav,
.mg-footer {
    display: revert;
}

/* --- Input focus → gold border --- */
.auth-input:focus,
.auth-select:focus,
.checkout-input:focus,
.checkout-select:focus,
.coupon-input:focus,
.price-input:focus,
.auth-input-wrap:focus-within {
    border-color: var(--mg-gold) !important;
}

/* --- Form primary button → olive --- */
.btn-primary,
.btn.btn-primary {
    background-color: var(--mg-olive) !important;
    border-color: var(--mg-olive) !important;
    color: #fff !important;
    border-radius: var(--mg-radius-full) !important;
}

.btn-primary:hover,
.btn.btn-primary:hover {
    background-color: var(--mg-olive-dark) !important;
    border-color: var(--mg-olive-dark) !important;
}

.btn-outline-primary {
    color: var(--mg-olive) !important;
    border-color: var(--mg-olive) !important;
    border-radius: var(--mg-radius-full) !important;
}

.btn-outline-primary:hover {
    background-color: var(--mg-olive) !important;
    border-color: var(--mg-olive) !important;
    color: #fff !important;
}

.btn-outline-dark {
    color: var(--mg-ink) !important;
    border-radius: var(--mg-radius-full) !important;
}

.btn-outline-dark:hover {
    background-color: var(--mg-olive) !important;
    border-color: var(--mg-olive) !important;
    color: #fff !important;
}

/* --- Product detail trust badges → cream bg --- */
.trust-badges .badge-item {
    background-color: var(--mg-bg-soft) !important;
    border-color: var(--mg-border) !important;
}

/* --- Technical spec table --- */
.technical-spec-table {
    border-color: var(--mg-border) !important;
}

.technical-spec-table th {
    background-color: var(--mg-bg-soft) !important;
    color: var(--mg-ink) !important;
}

.technical-spec-table td,
.technical-spec-table th {
    border-color: var(--mg-border) !important;
}

/* --- Scroll reveal → softer motion --- */
.scroll-reveal {
    transform: translateY(16px);
    transition-duration: 0.5s;
}

/* --- Cart page mobile checkout bar (keep on cart page) --- */
.mobile-checkout-bar {
    display: flex !important;
    background: rgba(255, 255, 255, 0.97) !important;
    border-color: var(--mg-border) !important;
}

@media (min-width: 1024px) {
    .mobile-checkout-bar {
        display: none !important;
    }
}

/* --- site.css account/auth page overrides --- */
.auth-shell,
.account-shell {
    background: var(--mg-bg) !important;
}

.btn-premium,
.btn-ghost-premium,
.btn-danger-soft {
    border-radius: var(--mg-radius-full) !important;
}

.account-menu-link.active {
    background: rgba(92, 107, 85, 0.08) !important;
}

.account-menu-link.active i:first-child,
.account-menu-link.active i:last-child {
    color: var(--mg-olive) !important;
}

.account-profile-badge {
    background: linear-gradient(135deg, var(--mg-olive), var(--mg-olive-dark)) !important;
}

.account-modal .modal-header {
    background: linear-gradient(135deg, var(--mg-olive), var(--mg-olive-dark)) !important;
}

.timeline-step.active .timeline-step-icon {
    background: linear-gradient(135deg, var(--mg-olive), var(--mg-olive-dark)) !important;
}

.account-order-id {
    color: var(--mg-olive) !important;
}

.account-info-icon {
    color: var(--mg-olive) !important;
}

.account-pill {
    background: rgba(92, 107, 85, 0.1) !important;
    color: var(--mg-olive) !important;
}

.status-badge.success {
    background: rgba(92, 107, 85, 0.1) !important;
    color: var(--mg-olive) !important;
}

.address-card::before {
    background: linear-gradient(90deg, var(--mg-olive), var(--mg-gold)) !important;
}
```

## File: KanvasProje.Web/wwwroot/css/site.css
```css
:root {
  --primary-color: #2c3e2d;
  --primary-dark: #1a2b1b;
  --primary-soft: #eef2ee;
  --primary-glow: rgba(44, 62, 45, 0.14);
  --accent-color: #b8860b;
  --accent-soft: #faf5eb;
  --text-dark: #1a1a1a;
  --text-muted: #6b6b6b;
  --surface: #ffffff;
  --surface-alt: #f9f7f4;
  --surface-soft: #fdfcfa;
  --border-color: #e8e4de;
  --border-strong: #d4cfc7;
  --success-color: #2d7a4f;
  --danger-color: #b24646;
  --warning-color: #b8860b;
  --shadow-xs: 0 1px 3px rgba(0, 0, 0, 0.04);
  --shadow-sm: 0 4px 12px rgba(0, 0, 0, 0.06);
  --shadow-md: 0 8px 24px rgba(0, 0, 0, 0.08);
  --shadow-lg: 0 16px 48px rgba(0, 0, 0, 0.1);
  --radius-sm: 4px;
  --radius-md: 8px;
  --radius-lg: 12px;
  --radius-pill: 999px;
  --transition-fast: 0.2s ease;
  --transition-base: 0.3s ease;
  --font-body: "Inter", -apple-system, "Segoe UI", sans-serif;
  --font-heading: "Playfair Display", Georgia, serif;
  --announcement-height: 36px;
  --mobile-header-height: 60px;
  --desktop-header-height: 72px;
  --mobile-bottom-nav-height: 64px;
}

*,
*::before,
*::after {
  box-sizing: border-box;
}

html {
  font-size: 15px;
  scroll-behavior: smooth;
}

@media (min-width: 992px) {
  html {
    font-size: 16px;
  }
}

body {
  margin: 0;
  font-family: var(--font-body);
  color: var(--text-dark);
  background: #ffffff;
  overflow-x: hidden;
  padding-top: calc(var(--announcement-height) + var(--mobile-header-height) + 8px);
  padding-bottom: calc(var(--mobile-bottom-nav-height) + 12px);
  -webkit-font-smoothing: antialiased;
  line-height: 1.6;
  letter-spacing: -0.01em;
}

@media (min-width: 992px) {
  body {
    padding-top: calc(var(--announcement-height) + var(--desktop-header-height) + 18px);
    padding-bottom: 0;
  }
}

body.no-scroll {
  overflow: hidden;
}

img {
  max-width: 100%;
  display: block;
}

a {
  color: inherit;
  text-decoration: none;
}

button,
input,
select,
textarea {
  font: inherit;
}

.container {
  max-width: 1260px;
}

.btn:focus,
.btn:active:focus,
.btn-link.nav-link:focus,
.form-control:focus,
.form-select:focus,
.form-check-input:focus {
  box-shadow: 0 0 0 0.18rem rgba(24, 59, 91, 0.12);
}

.form-control,
.form-select,
.form-check-input,
.form-floating>.form-control,
.form-floating>.form-select {
  border-color: var(--border-color);
  border-radius: 6px;
}

.form-control,
.form-select {
  min-height: 48px;
  padding: 0.78rem 1rem;
  background: rgba(255, 255, 255, 0.88);
}

.form-control::placeholder,
.form-select::placeholder {
  color: #8a96a3;
}

.form-control:focus,
.form-select:focus {
  border-color: rgba(24, 59, 91, 0.4);
  background: #fff;
}

.table {
  --bs-table-bg: transparent;
  --bs-table-striped-bg: rgba(24, 59, 91, 0.02);
}

.alert {
  border: 1px solid transparent;
  border-radius: 6px;
  box-shadow: var(--shadow-xs);
}

.announcement-bar {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: var(--announcement-height);
  z-index: 1200;
  background: var(--primary-dark);
  border-bottom: none;
}

.announcement-inner {
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
}

.announcement-copy,
.announcement-meta {
  display: flex;
  align-items: center;
  gap: 12px;
  min-width: 0;
}

.announcement-meta span,
.announcement-text {
  color: rgba(255, 255, 255, 0.9);
  font-size: 0.72rem;
  font-weight: 500;
  white-space: nowrap;
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.announcement-meta i,
.announcement-text i {
  color: var(--accent-color);
}

.announcement-divider {
  width: 1px;
  height: 14px;
  background: rgba(255, 255, 255, 0.2);
}

@media (max-width: 767px) {
  .announcement-inner {
    justify-content: center;
  }

  .announcement-text {
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: calc(100vw - 132px);
  }
}

.site-header {
  position: fixed;
  top: var(--announcement-height);
  left: 0;
  right: 0;
  z-index: 1190;
  background: rgba(255, 255, 255, 0.98);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid var(--border-color);
  box-shadow: none;
}

.site-header-main {
  min-height: var(--mobile-header-height);
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 8px;
}

@media (min-width: 992px) {
  .site-header-main {
    min-height: var(--desktop-header-height);
    grid-template-columns: minmax(220px, 280px) minmax(280px, 1fr) auto;
    gap: 12px;
  }
}

.brand-lockup {
  display: inline-flex;
  align-items: center;
  gap: 0;
  min-width: 0;
}

.brand-mark {
  width: auto;
  height: 50px;
  border-radius: 0;
  background: transparent;
  border: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  box-shadow: none;
  overflow: hidden;
  flex-shrink: 0;
}

.brand-mark img {
  max-width: 180px;
  max-height: 50px;
  object-fit: contain;
}

.brand-copy {
  display: none;
}

.header-square,
.header-icon-link {
  width: 40px;
  height: 40px;
  border-radius: 0;
  border: none;
  background: transparent;
  color: var(--text-dark);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  transition: color var(--transition-fast);
  box-shadow: none;
  cursor: pointer;
  font-size: 1.1rem;
}

.header-square:hover,
.header-icon-link:hover {
  color: var(--accent-color);
}

.header-search {
  display: flex;
  align-items: center;
  gap: 8px;
  min-height: 42px;
  padding: 0 6px 0 14px;
  border-radius: 0;
  border: 1px solid var(--border-color);
  background: #fff;
  box-shadow: none;
}

.header-search i {
  color: var(--text-muted);
  flex-shrink: 0;
}

.header-search input {
  width: 100%;
  border: 0;
  background: transparent;
  padding: 0;
  color: var(--text-dark);
  outline: 0;
}

.header-search button,
.mobile-search-form button {
  min-width: 66px;
  border: 0;
  border-radius: 0;
  background: var(--primary-dark);
  color: #fff;
  padding: 0.5rem 0.82rem;
  font-weight: 600;
  box-shadow: none;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  font-size: 0.75rem;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 8px;
}

.header-action {
  min-height: 36px;
  padding: 0 12px;
  border-radius: 0;
  border: none;
  background: transparent;
  color: var(--text-dark);
  display: inline-flex;
  align-items: center;
  gap: 8px;
  font-weight: 500;
  font-size: 0.85rem;
  transition: color var(--transition-fast);
}

.header-action:hover {
  color: var(--accent-color);
}

.header-support {
  min-height: 44px;
  min-width: 176px;
  padding: 0 12px;
  border-radius: 14px;
  background: linear-gradient(145deg, rgba(255, 255, 255, 0.98), rgba(247, 244, 239, 0.92));
  border: 1px solid rgba(24, 59, 91, 0.08);
  box-shadow: var(--shadow-xs);
  display: inline-flex;
  align-items: center;
  gap: 12px;
}

.header-support-icon {
  width: 30px;
  height: 30px;
  border-radius: 9px;
  background: var(--primary-soft);
  color: var(--primary-color);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.header-support strong {
  display: block;
  font-size: 0.74rem;
  color: var(--primary-dark);
}

.header-support small {
  display: block;
  color: var(--text-muted);
  font-size: 0.64rem;
}

.header-cart {
  position: relative;
  min-height: 36px;
  padding: 0 16px;
  border-radius: 0;
  background: var(--primary-dark);
  color: #fff;
  display: inline-flex;
  align-items: center;
  gap: 10px;
  font-weight: 500;
  font-size: 0.8rem;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  box-shadow: none;
  transition: background var(--transition-fast);
}

.header-cart:hover {
  color: #fff;
  background: var(--primary-color);
}

.header-cart-count {
  min-width: 24px;
  height: 24px;
  padding: 0 7px;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.18);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 0.75rem;
  font-weight: 800;
}

.site-mobile-search {
  display: none;
  padding-bottom: 12px;
}

.site-mobile-search.active {
  display: block;
}

.mobile-search-form {
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 10px;
  min-height: 50px;
  padding: 0 8px 0 14px;
  border-radius: 16px;
  border: 1px solid rgba(24, 59, 91, 0.08);
  background: rgba(255, 255, 255, 0.95);
  box-shadow: var(--shadow-xs);
}

.mobile-search-form i {
  color: var(--text-muted);
}

.mobile-search-form input {
  border: 0;
  background: transparent;
  outline: 0;
}

.desktop-category-nav {
  margin-top: 8px;
}

.category-ribbon {
  display: flex;
  align-items: center;
  gap: 0;
  padding: 0;
  border-radius: 0;
  background: transparent;
  border: none;
  border-bottom: 1px solid var(--border-color);
  box-shadow: none;
  overflow-x: auto;
  scrollbar-width: none;
}

.category-ribbon::-webkit-scrollbar {
  display: none;
}

.category-ribbon-label {
  display: none;
}

[data-desktop-nav] {
  scrollbar-width: none;
  -ms-overflow-style: none;
}

[data-desktop-nav]::-webkit-scrollbar {
  display: none;
}

.category-dropdown {
  backface-visibility: hidden;
  will-change: transform, opacity;
}

.desktop-category-link {
  display: inline-flex;
  align-items: center;
  flex-shrink: 0;
  min-height: 40px;
  padding: 0 18px;
  border-radius: 0;
  background: transparent;
  border: none;
  color: var(--text-dark);
  font-weight: 500;
  font-size: 0.82rem;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  transition: color var(--transition-fast);
}

.desktop-category-link:hover {
  color: var(--accent-color);
}

.desktop-category-link.desktop-category-link-all {
  background: var(--primary-dark);
  border-color: transparent;
  color: #fff;
}

.site-main {
  min-height: calc(100vh - 280px);
  padding-bottom: 48px;
}

.mobile-bottom-nav {
  position: fixed;
  left: 12px;
  right: 12px;
  bottom: 12px;
  z-index: 1150;
  display: grid;
  grid-template-columns: repeat(5, minmax(0, 1fr));
  gap: 4px;
  padding: 6px;
  border-radius: 0;
  background: #fff;
  border: 1px solid var(--border-color);
  backdrop-filter: blur(10px);
  box-shadow: 0 -4px 12px rgba(0, 0, 0, 0.08);
}

.mobile-bottom-link {
  min-height: 48px;
  border: 0;
  border-radius: 4px;
  background: transparent;
  color: var(--text-muted);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 3px;
  font-size: 0.68rem;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.04em;
}

.mobile-bottom-link i {
  font-size: 1rem;
}

.mobile-bottom-link.active,
.mobile-bottom-link:hover {
  background: var(--primary-soft);
  color: var(--primary-color);
}

.menu-overlay {
  position: fixed;
  inset: 0;
  background: rgba(20, 33, 47, 0.34);
  opacity: 0;
  visibility: hidden;
  transition: opacity var(--transition-base), visibility var(--transition-base);
  z-index: 1180;
}

.menu-overlay.active {
  opacity: 1;
  visibility: visible;
}

.mobile-category-sheet {
  position: fixed;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 1195;
  max-height: calc(100vh - 110px);
  padding: 24px 18px 28px;
  border-radius: 0;
  background: #fff;
  box-shadow: 0 -8px 24px rgba(0, 0, 0, 0.12);
  transform: translateY(105%);
  transition: transform var(--transition-base);
  overflow-y: auto;
}

.mobile-category-sheet.active {
  transform: translateY(0);
}

.mobile-sheet-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 16px;
  margin-bottom: 18px;
}

.mobile-sheet-kicker {
  display: inline-block;
  margin-bottom: 6px;
  color: var(--text-muted);
  font-size: 0.74rem;
  font-weight: 800;
  letter-spacing: 0.08em;
  text-transform: uppercase;
}

.mobile-sheet-header h5 {
  margin: 0;
  font-family: var(--font-heading);
  font-size: 1.35rem;
  color: var(--primary-dark);
}

.mobile-sheet-grid {
  display: grid;
  gap: 12px;
}

.mobile-sheet-item {
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 12px;
  align-items: center;
  padding: 14px;
  border-radius: 20px;
  background: rgba(255, 255, 255, 0.92);
  border: 1px solid rgba(24, 59, 91, 0.08);
  box-shadow: var(--shadow-xs);
}

.mobile-sheet-item-wide {
  grid-template-columns: 1fr auto;
}

.mobile-sheet-icon {
  width: 48px;
  height: 48px;
  border-radius: 16px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 1.05rem;
}

.mobile-sheet-copy {
  display: flex;
  flex-direction: column;
  gap: 3px;
  min-width: 0;
}

.mobile-sheet-copy strong {
  color: var(--primary-dark);
}

.mobile-sheet-copy small {
  color: var(--text-muted);
}

.site-footer {
  padding: 48px 0 calc(24px + env(safe-area-inset-bottom, 0px));
  background: var(--primary-dark);
  color: rgba(255, 255, 255, 0.88);
}

@media (min-width: 992px) {
  .site-footer {
    padding: 68px 0 28px;
  }
}

.site-footer-top {
  display: grid;
  gap: 24px;
  padding-bottom: 28px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
}

@media (min-width: 992px) {
  .site-footer-top {
    grid-template-columns: minmax(0, 1.1fr) minmax(0, 1fr);
    align-items: start;
  }
}

.footer-intro {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.footer-brand-lockup {
  display: flex;
  align-items: center;
  gap: 14px;
}

.footer-brand-lockup h4 {
  margin: 0 0 6px;
  font-family: var(--font-heading);
  font-size: 1.25rem;
  color: #fff;
}

.footer-brand-lockup p {
  margin: 0;
  color: rgba(255, 255, 255, 0.72);
  max-width: 520px;
  line-height: 1.7;
}

.footer-socials {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.footer-socials a {
  width: 42px;
  height: 42px;
  border-radius: 14px;
  background: rgba(255, 255, 255, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.08);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  color: #fff;
}

.footer-trust-grid {
  display: grid;
  gap: 12px;
}

@media (min-width: 768px) {
  .footer-trust-grid {
    grid-template-columns: repeat(3, minmax(0, 1fr));
  }
}

.footer-trust-card {
  display: flex;
  gap: 12px;
  padding: 16px;
  border-radius: 20px;
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid rgba(255, 255, 255, 0.08);
}

.footer-trust-card i {
  color: #fff;
  font-size: 1.1rem;
  margin-top: 2px;
}

.footer-trust-card strong,
.footer-trust-card span {
  display: block;
}

.footer-trust-card strong {
  color: #fff;
  margin-bottom: 4px;
}

.footer-trust-card span {
  color: rgba(255, 255, 255, 0.66);
  font-size: 0.88rem;
}

.footer-grid {
  display: grid;
  gap: 24px;
  padding: 28px 0;
}

@media (min-width: 768px) {
  .footer-grid {
    grid-template-columns: repeat(2, minmax(0, 1fr));
  }
}

@media (min-width: 1200px) {
  .footer-grid {
    grid-template-columns: 1.1fr 1fr 1fr 1.2fr;
  }
}

.footer-grid h5 {
  margin: 0 0 16px;
  font-size: 0.82rem;
  font-weight: 800;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: rgba(255, 255, 255, 0.76);
}

.footer-links,
.footer-contact-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: grid;
  gap: 10px;
}

.footer-links a {
  color: rgba(255, 255, 255, 0.86);
}

.footer-links a:hover {
  color: #fff;
}

.footer-contact-list li {
  display: flex;
  gap: 10px;
  align-items: flex-start;
  color: rgba(255, 255, 255, 0.82);
}

.footer-contact-list i {
  color: rgba(255, 255, 255, 0.62);
  margin-top: 4px;
}

.site-footer-bottom {
  padding-top: 22px;
  border-top: 1px solid rgba(255, 255, 255, 0.08);
  display: flex;
  flex-direction: column;
  gap: 14px;
}

@media (min-width: 768px) {
  .site-footer-bottom {
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
  }
}

.site-footer-bottom p {
  margin: 0;
  color: rgba(255, 255, 255, 0.72);
}

.footer-payment-icons {
  display: flex;
  gap: 14px;
  color: rgba(255, 255, 255, 0.82);
  font-size: 1.35rem;
}

.custom-lightbox {
  position: fixed;
  inset: 0;
  z-index: 2100;
  display: grid;
  place-items: center;
  padding: 24px;
  background: rgba(8, 15, 24, 0.86);
  opacity: 0;
  visibility: hidden;
  transition: opacity var(--transition-base), visibility var(--transition-base);
}

.custom-lightbox.active {
  opacity: 1;
  visibility: visible;
}

.custom-lightbox img {
  max-width: min(92vw, 1200px);
  max-height: 88vh;
  border-radius: 22px;
  box-shadow: var(--shadow-lg);
}

.custom-lightbox-close {
  position: absolute;
  top: 24px;
  right: 24px;
  width: 48px;
  height: 48px;
  border-radius: 16px;
  border: 1px solid rgba(255, 255, 255, 0.12);
  background: rgba(255, 255, 255, 0.1);
  color: #fff;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.page-shell,
.surface-panel,
.soft-card {
  background: rgba(255, 255, 255, 0.9);
  border: 1px solid rgba(24, 59, 91, 0.08);
  box-shadow: none;
  border-radius: 4px;
}

.page-shell {
  padding: clamp(20px, 3vw, 36px);
}

.section-kicker {
  display: inline-flex;
  align-items: center;
  min-height: 28px;
  padding: 0 12px;
  border-radius: 0;
  background: transparent;
  color: var(--accent-color);
  font-size: 0.72rem;
  font-weight: 600;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  border-left: 3px solid var(--accent-color);
}

.section-title {
  margin: 12px 0 0;
  font-family: var(--font-heading);
  font-size: clamp(1.8rem, 2.5vw, 2.8rem);
  font-weight: 400;
  color: var(--primary-dark);
}

.section-copy {
  margin: 12px 0 0;
  color: var(--text-muted);
  line-height: 1.8;
}

.breadcrumb,
.breadcrumb-nav,
.cart-breadcrumb {
  color: var(--text-muted);
}

.breadcrumb a,
.breadcrumb-nav a,
.cart-breadcrumb a {
  color: var(--text-muted);
}

.breadcrumb a:hover,
.breadcrumb-nav a:hover,
.cart-breadcrumb a:hover {
  color: var(--primary-color);
}

.scroll-reveal {
  opacity: 0;
  transform: translateY(22px);
  transition: opacity 0.45s ease, transform 0.45s ease;
}

.scroll-reveal.revealed {
  opacity: 1;
  transform: translateY(0);
}

.motion-reveal,
.motion-stagger-item {
  opacity: 0;
  transform: translate3d(0, 20px, 0);
  transition:
    opacity 0.58s cubic-bezier(0.22, 1, 0.36, 1),
    transform 0.58s cubic-bezier(0.22, 1, 0.36, 1);
}

.motion-stagger-item {
  transition-delay: var(--motion-delay, 0ms);
}

.motion-reveal.is-visible,
.motion-stagger-item.is-visible {
  opacity: 1;
  transform: translate3d(0, 0, 0);
}

.motion-reveal.is-animating,
.motion-stagger-item.is-animating,
.motion-hero-prep {
  will-change: opacity, transform;
}

.motion-hero-prep {
  opacity: 0;
  transform: translate3d(0, 14px, 0) scale(0.985);
  transition:
    opacity 0.66s cubic-bezier(0.22, 1, 0.36, 1),
    transform 0.66s cubic-bezier(0.22, 1, 0.36, 1);
}

.motion-hero-prep.is-visible {
  opacity: 1;
  transform: translate3d(0, 0, 0) scale(1);
}

.mobile-nav-backdrop {
  opacity: 0;
  transition: opacity 0.32s cubic-bezier(0.22, 1, 0.36, 1);
}

.mobile-nav-backdrop.is-open {
  opacity: 1;
}

.mobile-nav-panel {
  opacity: 0;
  transform: translate3d(-100%, 0, 0);
  transition:
    opacity 0.38s cubic-bezier(0.22, 1, 0.36, 1),
    transform 0.42s cubic-bezier(0.22, 1, 0.36, 1);
  will-change: opacity, transform;
}

.mobile-nav-panel.is-open {
  opacity: 1;
  transform: translate3d(0, 0, 0);
}

.mobile-nav-panel [data-mobile-nav-item] {
  opacity: 0;
  transform: translate3d(0, 10px, 0);
  transition:
    opacity 0.36s cubic-bezier(0.22, 1, 0.36, 1),
    transform 0.36s cubic-bezier(0.22, 1, 0.36, 1);
  transition-delay: var(--mobile-nav-delay, 0ms);
}

.mobile-nav-panel.is-open [data-mobile-nav-item] {
  opacity: 1;
  transform: translate3d(0, 0, 0);
}

[data-mobile-category] [data-mobile-subitem] {
  opacity: 0;
  transform: translate3d(0, 8px, 0);
  transition:
    opacity 0.28s cubic-bezier(0.22, 1, 0.36, 1),
    transform 0.28s cubic-bezier(0.22, 1, 0.36, 1);
  transition-delay: var(--mobile-sub-delay, 0ms);
}

[data-mobile-category][open] [data-mobile-subitem] {
  opacity: 1;
  transform: translate3d(0, 0, 0);
}

.category-preview-panel {
  opacity: 1;
  transform: translate3d(0, 0, 0);
  transition:
    opacity 0.24s cubic-bezier(0.22, 1, 0.36, 1),
    transform 0.24s cubic-bezier(0.22, 1, 0.36, 1);
}

.category-preview-panel.is-preview-changing {
  opacity: 0;
  transform: translate3d(0, 8px, 0);
}

.category-preview-media img {
  opacity: 1;
  transform: scale(1);
  transition:
    opacity 0.34s cubic-bezier(0.22, 1, 0.36, 1),
    transform 0.46s cubic-bezier(0.22, 1, 0.36, 1);
}

.category-preview-panel.is-preview-changing .category-preview-media img {
  opacity: 0.72;
  transform: scale(1.015);
}

@media (prefers-reduced-motion: reduce) {
  .motion-reveal,
  .motion-stagger-item,
  .motion-hero-prep,
  .mobile-nav-backdrop,
  .mobile-nav-panel,
  .mobile-nav-panel [data-mobile-nav-item],
  [data-mobile-category] [data-mobile-subitem],
  .category-preview-panel,
  .category-preview-media img {
    transition-duration: 0.01ms !important;
    transition-delay: 0ms !important;
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
  }

  .motion-reveal,
  .motion-stagger-item,
  .motion-hero-prep,
  .motion-reveal.is-visible,
  .motion-stagger-item.is-visible,
  .motion-hero-prep.is-visible {
    opacity: 1 !important;
    transform: none !important;
  }
}

.card-glare {
  position: absolute;
  inset: 0;
  pointer-events: none;
  opacity: 0;
  transition: opacity var(--transition-fast);
  border-radius: inherit;
}

.cart-bounce {
  animation: cartBounce 0.55s ease;
}

@keyframes cartBounce {

  0%,
  100% {
    transform: scale(1);
  }

  30% {
    transform: scale(1.16);
  }

  60% {
    transform: scale(0.92);
  }
}

.toast-container {
  position: fixed;
  top: calc(var(--announcement-height) + var(--mobile-header-height) + 12px);
  right: 16px;
  z-index: 2200;
  display: grid;
  gap: 10px;
  width: min(360px, calc(100vw - 24px));
}

@media (min-width: 992px) {
  .toast-container {
    top: calc(var(--announcement-height) + var(--desktop-header-height) + 16px);
  }
}

.toast-message {
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 12px;
  padding: 16px;
  border-radius: 18px;
  background: rgba(255, 255, 255, 0.96);
  border: 1px solid rgba(24, 59, 91, 0.08);
  box-shadow: var(--shadow-md);
}

.toast-icon {
  width: 42px;
  height: 42px;
  border-radius: 14px;
  background: var(--primary-soft);
  color: var(--primary-color);
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.toast-success .toast-icon {
  background: rgba(39, 130, 92, 0.12);
  color: var(--success-color);
}

.toast-error .toast-icon {
  background: rgba(178, 70, 70, 0.12);
  color: var(--danger-color);
}

.toast-warning .toast-icon {
  background: rgba(185, 123, 26, 0.12);
  color: var(--warning-color);
}

.toast-title {
  font-weight: 800;
  color: var(--primary-dark);
}

.toast-text {
  color: var(--text-muted);
  margin-top: 3px;
  font-size: 0.92rem;
  line-height: 1.6;
}

.like-btn {
  position: relative;
  overflow: visible;
}

.like-pop {
  animation: likePop 0.5s ease;
}

@keyframes likePop {

  0%,
  100% {
    transform: scale(1);
  }

  45% {
    transform: scale(1.12);
  }
}

.heart-particle {
  position: absolute;
  top: 50%;
  left: 50%;
  font-size: 0.7rem;
  color: #d26a6a;
  transform: translate(-50%, -50%) rotate(var(--angle)) translateY(-18px);
  opacity: 0;
  animation: heartBurst 0.6s ease forwards;
  animation-delay: var(--delay);
}

@keyframes heartBurst {
  0% {
    opacity: 0;
    transform: translate(-50%, -50%) rotate(var(--angle)) translateY(0);
  }

  30% {
    opacity: 1;
  }

  100% {
    opacity: 0;
    transform: translate(-50%, -50%) rotate(var(--angle)) translateY(-28px);
  }
}

@media (max-width: 991px) {
  .brand-copy small {
    display: none;
  }

  .header-cart span:not(.header-cart-count) {
    display: none;
  }

  .desktop-category-nav {
    display: none !important;
  }
}

.auth-shell,
.account-shell {
  padding: clamp(28px, 4vw, 56px) 0 72px;
}

.auth-shell {
  min-height: calc(100vh - var(--announcement-height) - var(--desktop-header-height));
  display: flex;
  align-items: center;
  background: var(--surface-alt);
}

.auth-card,
.account-card,
.account-table-card,
.account-empty-state,
.account-danger-card,
.account-timeline-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: 4px;
  box-shadow: var(--shadow-sm);
}

.auth-card {
  overflow: hidden;
}

.auth-card-body {
  padding: clamp(28px, 4vw, 42px);
}

.auth-kicker,
.account-kicker {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  min-height: 32px;
  padding: 0 0 0 12px;
  border-radius: 0;
  background: transparent;
  border-left: 3px solid var(--accent-color);
  color: var(--accent-color);
  font-size: 0.72rem;
  font-weight: 600;
  letter-spacing: 0.1em;
  text-transform: uppercase;
}

.auth-title,
.account-title {
  margin: 16px 0 8px;
  font-family: var(--font-heading);
  font-size: clamp(1.8rem, 3vw, 2.4rem);
  font-weight: 400;
  color: var(--primary-dark);
}

.auth-copy,
.account-copy {
  color: var(--text-muted);
  line-height: 1.75;
  margin: 0;
}

.auth-metrics {
  display: grid;
  gap: 12px;
  margin-top: 24px;
}

.auth-metric {
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 14px;
  align-items: center;
  padding: 16px 18px;
  border-radius: 18px;
  background: rgba(24, 59, 91, 0.04);
  border: 1px solid rgba(24, 59, 91, 0.08);
}

.auth-metric-icon {
  width: 46px;
  height: 46px;
  border-radius: 16px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  background: rgba(183, 136, 70, 0.16);
  color: var(--primary-dark);
}

.auth-metric strong,
.account-stat strong {
  display: block;
  color: var(--primary-dark);
  font-size: 1rem;
}

.auth-metric span,
.account-stat span {
  display: block;
  color: var(--text-muted);
  font-size: 0.9rem;
  margin-top: 3px;
}

.auth-form-panel {
  padding: clamp(28px, 4vw, 42px);
  background: linear-gradient(180deg, rgba(255, 255, 255, 0.98) 0%, rgba(247, 243, 235, 0.98) 100%);
}

.auth-form {
  display: grid;
  gap: 18px;
}

.auth-field label {
  display: block;
  margin-bottom: 8px;
  color: var(--primary-dark);
  font-size: 0.86rem;
  font-weight: 700;
}

.auth-input-wrap {
  position: relative;
}

.auth-input-wrap i {
  position: absolute;
  left: 16px;
  top: 50%;
  transform: translateY(-50%);
  color: var(--text-muted);
}

.auth-input,
.auth-select,
.account-input,
.account-select {
  width: 100%;
  min-height: 50px;
  border-radius: 4px;
  border: 1px solid var(--border-color);
  background: #fff;
  color: var(--primary-dark);
  padding: 0 18px 0 46px;
  transition: border-color var(--transition-fast), box-shadow var(--transition-fast);
}

.auth-select,
.account-select {
  padding-right: 18px;
}

.auth-input:focus,
.auth-select:focus,
.account-input:focus,
.account-select:focus {
  outline: none;
  border-color: rgba(24, 59, 91, 0.28);
  box-shadow: 0 0 0 4px rgba(24, 59, 91, 0.08);
}

.auth-actions {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 14px;
  flex-wrap: wrap;
}

.auth-link,
.account-link {
  color: var(--primary-color);
  font-weight: 700;
  text-decoration: none;
}

.auth-link:hover,
.account-link:hover {
  color: var(--primary-dark);
}

.btn-premium,
.btn-ghost-premium,
.btn-danger-soft {
  min-height: 48px;
  border-radius: 0;
  border: 1px solid transparent;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  padding: 0 28px;
  font-weight: 600;
  font-size: 0.82rem;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  transition: background var(--transition-fast), color var(--transition-fast), border-color var(--transition-fast);
}

.btn-premium {
  background: var(--primary-dark);
  color: #fff;
  box-shadow: none;
}

.btn-premium:hover {
  color: #fff;
  background: var(--primary-color);
}

.btn-ghost-premium {
  background: #fff;
  color: var(--primary-dark);
  border-color: var(--border-color);
}

.btn-ghost-premium:hover {
  color: var(--primary-dark);
  background: var(--surface-alt);
}

.btn-danger-soft {
  background: var(--danger-color);
  color: #fff;
  box-shadow: none;
}

.btn-danger-soft:hover {
  color: #fff;

  .auth-footer {
    margin-top: 22px;
    padding-top: 18px;
    border-top: 1px solid rgba(24, 59, 91, 0.08);
    color: var(--text-muted);
    font-size: 0.95rem;
  }

  .account-breadcrumb {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 10px;
    margin-bottom: 18px;
    color: var(--text-muted);
    font-size: 0.92rem;
  }

  .account-breadcrumb a {
    color: var(--text-muted);
    text-decoration: none;
  }

  .account-breadcrumb a:hover {
    color: var(--primary-color);
  }

  .account-sidebar-card {
    position: sticky;
    top: calc(var(--announcement-height) + var(--desktop-header-height) + 24px);
    padding: 16px;
  }

  .account-menu {
    display: grid;
    gap: 8px;
  }

  .account-menu-link {
    display: grid;
    grid-template-columns: auto 1fr auto;
    gap: 12px;
    align-items: center;
    min-height: 58px;
    padding: 0 16px;
    border-radius: 18px;
    color: var(--primary-dark);
    text-decoration: none;
    transition: background var(--transition-fast), color var(--transition-fast), transform var(--transition-fast), box-shadow var(--transition-fast);
  }

  .account-menu-link i:first-child {
    width: 18px;
    text-align: center;
    color: var(--text-muted);
  }

  .account-menu-link:hover {
    background: rgba(24, 59, 91, 0.05);
    color: var(--primary-dark);
  }

  .account-menu-link.active {
    background: linear-gradient(135deg, rgba(24, 59, 91, 0.12), rgba(183, 136, 70, 0.12));
    box-shadow: inset 0 0 0 1px rgba(24, 59, 91, 0.08);
  }

  .account-menu-link.active i:first-child,
  .account-menu-link.active i:last-child {
    color: var(--primary-color);
  }

  .account-menu-link.danger {
    color: #922f2f;
  }

  .account-menu-link.danger:hover {
    background: rgba(146, 47, 47, 0.08);
  }

  .account-header {
    padding: clamp(24px, 3vw, 36px);
  }

  .account-header-grid {
    display: grid;
    grid-template-columns: minmax(0, 1fr) auto;
    gap: 18px;
    align-items: center;
  }

  .account-header-copy {
    display: grid;
    gap: 12px;
  }

  .account-header-actions {
    display: flex;
    flex-wrap: wrap;
    gap: 12px;
    justify-content: flex-end;
  }

  .account-profile-badge {
    display: inline-grid;
    place-items: center;
    width: 74px;
    height: 74px;
    border-radius: 24px;
    background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
    color: #fff;
    font-family: var(--font-heading);
    font-size: 1.8rem;
    font-weight: 800;
    box-shadow: 0 18px 30px rgba(24, 59, 91, 0.2);
  }

  .account-stat-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 18px;
  }

  .account-stat {
    padding: 24px;
  }

  .account-stat-value {
    margin-top: 18px;
    font-family: var(--font-heading);
    font-size: clamp(1.8rem, 3vw, 2.4rem);
    color: var(--primary-dark);
    letter-spacing: -0.04em;
  }

  .account-stat-meta {
    margin-top: 8px;
    color: var(--text-muted);
    font-size: 0.92rem;
  }

  .account-info-list {
    display: grid;
    gap: 12px;
  }

  .account-info-item {
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 14px;
    align-items: start;
    padding: 18px 0;
    border-bottom: 1px solid rgba(24, 59, 91, 0.08);
  }

  .account-info-item:last-child {
    border-bottom: 0;
    padding-bottom: 0;
  }

  .account-info-item:first-child {
    padding-top: 0;
  }

  .account-info-icon {
    width: 44px;
    height: 44px;
    border-radius: 16px;
    background: rgba(24, 59, 91, 0.06);
    color: var(--primary-color);
    display: inline-flex;
    align-items: center;
    justify-content: center;
  }

  .account-info-label {
    font-size: 0.76rem;
    font-weight: 800;
    letter-spacing: 0.08em;
    text-transform: uppercase;
    color: var(--text-muted);
  }

  .account-info-value {
    margin-top: 4px;
    font-size: 1rem;
    font-weight: 700;
    color: var(--primary-dark);
  }

  .account-pill {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    min-height: 34px;
    padding: 0 14px;
    border-radius: var(--radius-pill);
    background: rgba(39, 130, 92, 0.12);
    color: var(--success-color);
    font-weight: 700;
    font-size: 0.84rem;
  }

  .account-table-card {
    overflow: hidden;
  }

  .account-table-head {
    padding: 20px 24px;
    border-bottom: 1px solid rgba(24, 59, 91, 0.08);
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 12px;
  }

  .account-table-head h3,
  .account-table-head h4,
  .account-table-head h5 {
    margin: 0;
    font-family: var(--font-heading);
    color: var(--primary-dark);
  }

  .account-table {
    width: 100%;
    margin: 0;
  }

  .account-table thead th {
    background: rgba(24, 59, 91, 0.04);
    color: var(--primary-dark);
    font-size: 0.82rem;
    font-weight: 800;
    letter-spacing: 0.08em;
    text-transform: uppercase;
    border: 0;
    padding: 18px 20px;
  }

  .account-table tbody td {
    padding: 18px 20px;
    border-color: rgba(24, 59, 91, 0.08);
    vertical-align: middle;
  }

  .account-table tbody tr:hover td {
    background: rgba(24, 59, 91, 0.025);
  }

  .account-order-id {
    font-family: var(--font-heading);
    font-weight: 700;
    color: var(--primary-color);
  }

  .account-price {
    font-weight: 800;
    color: var(--primary-dark);
  }

  .status-badge {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    min-height: 36px;
    padding: 0 14px;
    border-radius: 12px;
    font-size: 0.82rem;
    font-weight: 700;
  }

  .status-badge.pending {
    background: rgba(185, 123, 26, 0.12);
    color: #916000;
  }

  .status-badge.info {
    background: rgba(56, 120, 184, 0.12);
    color: #255f93;
  }

  .status-badge.progress {
    background: rgba(183, 136, 70, 0.14);
    color: #8a6127;
  }

  .status-badge.success {
    background: rgba(39, 130, 92, 0.12);
    color: var(--success-color);
  }

  .status-badge.danger {
    background: rgba(178, 70, 70, 0.12);
    color: var(--danger-color);
  }

  .status-badge.soft {
    background: rgba(24, 59, 91, 0.08);
    color: var(--primary-color);
  }

  .account-chip {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    min-height: 34px;
    padding: 0 14px;
    border-radius: 12px;
    background: rgba(24, 59, 91, 0.06);
    color: var(--primary-dark);
    font-size: 0.84rem;
  }

  .account-empty-state {
    padding: 42px 28px;
    text-align: center;
  }

  .account-empty-icon {
    width: 88px;
    height: 88px;
    margin: 0 auto 18px;
    border-radius: 28px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, rgba(24, 59, 91, 0.1), rgba(183, 136, 70, 0.14));
    color: var(--primary-color);
    font-size: 2rem;
  }

  .address-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 18px;
  }

  .address-card {
    padding: 24px;
    position: relative;
    overflow: hidden;
  }

  .address-card::before {
    content: "";
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 4px;
    background: linear-gradient(90deg, var(--primary-color), var(--accent-color));
  }

  .address-card-head {
    display: flex;
    align-items: start;
    justify-content: space-between;
    gap: 14px;
    margin-bottom: 16px;
  }

  .address-card-meta {
    display: grid;
    gap: 12px;
  }

  .address-card-row {
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 12px;
    align-items: start;
    color: var(--text-muted);
  }

  .address-card-row i {
    width: 16px;
    color: var(--primary-color);
    margin-top: 4px;
  }

  .account-modal .modal-content {
    border: 1px solid rgba(24, 59, 91, 0.08);
    border-radius: 26px;
    box-shadow: var(--shadow-lg);
    overflow: hidden;
  }

  .account-modal .modal-header {
    padding: 22px 24px;
    background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
    color: #fff;
    border: 0;
  }

  .account-modal .modal-body {
    padding: 24px;
  }

  .account-modal .modal-footer {
    padding: 0 24px 24px;
    border: 0;
  }

  .account-modal .account-input,
  .account-modal .account-select {
    padding-left: 18px;
  }

  .account-modal textarea.account-input {
    min-height: 132px;
  }

  .danger-note {
    padding: 18px 20px;
    border-radius: 18px;
    background: rgba(178, 70, 70, 0.08);
    border: 1px solid rgba(178, 70, 70, 0.12);
    color: #7e3535;
  }

  .danger-list {
    list-style: none;
    margin: 0;
    padding: 0;
    display: grid;
    gap: 10px;
  }

  .danger-list li {
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 10px;
    align-items: start;
    color: var(--text-muted);
  }

  .danger-list i {
    color: #9a3838;
    margin-top: 4px;
  }

  .access-shell {
    min-height: calc(100vh - var(--announcement-height) - var(--desktop-header-height));
    display: flex;
    align-items: center;
    background:
      radial-gradient(circle at top center, rgba(146, 47, 47, 0.08), transparent 22%),
      linear-gradient(180deg, #fbfaf8 0%, #f2ede6 100%);
    position: relative;
    overflow: hidden;
  }

  .access-background-code {
    position: absolute;
    inset: 0;
    display: grid;
    place-items: center;
    font-family: var(--font-heading);
    font-size: clamp(8rem, 18vw, 18rem);
    color: rgba(146, 47, 47, 0.04);
    letter-spacing: -0.08em;
    pointer-events: none;
  }

  .access-card {
    position: relative;
    z-index: 1;
    max-width: 720px;
    margin: 0 auto;
    padding: clamp(30px, 4vw, 46px);
    text-align: center;
  }

  .access-icon {
    width: 96px;
    height: 96px;
    margin: 0 auto 20px;
    border-radius: 30px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: rgba(146, 47, 47, 0.1);
    color: #8f3636;
    font-size: 2.4rem;
    box-shadow: 0 18px 30px rgba(146, 47, 47, 0.08);
  }

  .timeline {
    position: relative;
    display: grid;
    grid-template-columns: repeat(4, minmax(0, 1fr));
    gap: 18px;
    padding: 34px 24px;
  }

  .timeline::before {
    content: "";
    position: absolute;
    top: 58px;
    left: 12%;
    right: 12%;
    height: 3px;
    border-radius: 999px;
    background: rgba(24, 59, 91, 0.09);
  }

  .timeline-step {
    position: relative;
    z-index: 1;
    text-align: center;
  }

  .timeline-step-icon {
    width: 56px;
    height: 56px;
    margin: 0 auto 14px;
    border-radius: 20px;
    background: #fff;
    border: 1px solid rgba(24, 59, 91, 0.1);
    color: var(--text-muted);
    display: inline-flex;
    align-items: center;
    justify-content: center;
    box-shadow: var(--shadow-xs);
  }

  .timeline-step.active .timeline-step-icon {
    background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
    color: #fff;
    border-color: transparent;
    box-shadow: 0 18px 30px rgba(24, 59, 91, 0.16);
  }

  .timeline-step-title {
    color: var(--primary-dark);
    font-size: 0.88rem;
    font-weight: 700;
  }

  .timeline-step-copy {
    margin-top: 6px;
    color: var(--text-muted);
    font-size: 0.8rem;
    line-height: 1.5;
  }

  @media (max-width: 991px) {

    .auth-shell,
    .access-shell {
      min-height: auto;
      padding-top: 28px;
      padding-bottom: 48px;
    }

    .account-sidebar-card {
      position: static;
    }

    .account-header-grid {
      grid-template-columns: 1fr;
    }

    .account-header-actions {
      justify-content: flex-start;
    }

    .address-grid,
    .account-stat-grid {
      grid-template-columns: 1fr;
    }

    .timeline {
      grid-template-columns: repeat(2, minmax(0, 1fr));
    }

    .timeline::before {
      display: none;
    }
  }

  @media (max-width: 767px) {

    .auth-card-body,
    .auth-form-panel,
    .account-header,
    .account-card,
    .account-table-head,
    .address-card,
    .account-empty-state,
    .account-danger-card,
    .account-timeline-card {
      padding-left: 20px;
      padding-right: 20px;
    }

    .account-menu {
      grid-auto-flow: column;
      grid-auto-columns: max-content;
      overflow-x: auto;
      padding-bottom: 6px;
    }

    .account-table thead {
      display: none;
    }

    .account-table,
    .account-table tbody,
    .account-table tr,
    .account-table td {
      display: block;
      width: 100%;
    }

    .account-table tbody tr {
      padding: 18px;
      border-bottom: 1px solid rgba(24, 59, 91, 0.08);
    }

    .account-table tbody td {
      padding: 8px 0;
      border: 0;
    }

    .timeline {
      grid-template-columns: 1fr;
    }
  }
}
```

## File: KanvasProje.Web/wwwroot/css/storefront.css
```css
*,:after,:before{--tw-border-spacing-x:0;--tw-border-spacing-y:0;--tw-translate-x:0;--tw-translate-y:0;--tw-rotate:0;--tw-skew-x:0;--tw-skew-y:0;--tw-scale-x:1;--tw-scale-y:1;--tw-pan-x: ;--tw-pan-y: ;--tw-pinch-zoom: ;--tw-scroll-snap-strictness:proximity;--tw-gradient-from-position: ;--tw-gradient-via-position: ;--tw-gradient-to-position: ;--tw-ordinal: ;--tw-slashed-zero: ;--tw-numeric-figure: ;--tw-numeric-spacing: ;--tw-numeric-fraction: ;--tw-ring-inset: ;--tw-ring-offset-width:0px;--tw-ring-offset-color:#fff;--tw-ring-color:rgba(59,130,246,.5);--tw-ring-offset-shadow:0 0 #0000;--tw-ring-shadow:0 0 #0000;--tw-shadow:0 0 #0000;--tw-shadow-colored:0 0 #0000;--tw-blur: ;--tw-brightness: ;--tw-contrast: ;--tw-grayscale: ;--tw-hue-rotate: ;--tw-invert: ;--tw-saturate: ;--tw-sepia: ;--tw-drop-shadow: ;--tw-backdrop-blur: ;--tw-backdrop-brightness: ;--tw-backdrop-contrast: ;--tw-backdrop-grayscale: ;--tw-backdrop-hue-rotate: ;--tw-backdrop-invert: ;--tw-backdrop-opacity: ;--tw-backdrop-saturate: ;--tw-backdrop-sepia: ;--tw-contain-size: ;--tw-contain-layout: ;--tw-contain-paint: ;--tw-contain-style: }::backdrop{--tw-border-spacing-x:0;--tw-border-spacing-y:0;--tw-translate-x:0;--tw-translate-y:0;--tw-rotate:0;--tw-skew-x:0;--tw-skew-y:0;--tw-scale-x:1;--tw-scale-y:1;--tw-pan-x: ;--tw-pan-y: ;--tw-pinch-zoom: ;--tw-scroll-snap-strictness:proximity;--tw-gradient-from-position: ;--tw-gradient-via-position: ;--tw-gradient-to-position: ;--tw-ordinal: ;--tw-slashed-zero: ;--tw-numeric-figure: ;--tw-numeric-spacing: ;--tw-numeric-fraction: ;--tw-ring-inset: ;--tw-ring-offset-width:0px;--tw-ring-offset-color:#fff;--tw-ring-color:rgba(59,130,246,.5);--tw-ring-offset-shadow:0 0 #0000;--tw-ring-shadow:0 0 #0000;--tw-shadow:0 0 #0000;--tw-shadow-colored:0 0 #0000;--tw-blur: ;--tw-brightness: ;--tw-contrast: ;--tw-grayscale: ;--tw-hue-rotate: ;--tw-invert: ;--tw-saturate: ;--tw-sepia: ;--tw-drop-shadow: ;--tw-backdrop-blur: ;--tw-backdrop-brightness: ;--tw-backdrop-contrast: ;--tw-backdrop-grayscale: ;--tw-backdrop-hue-rotate: ;--tw-backdrop-invert: ;--tw-backdrop-opacity: ;--tw-backdrop-saturate: ;--tw-backdrop-sepia: ;--tw-contain-size: ;--tw-contain-layout: ;--tw-contain-paint: ;--tw-contain-style: }/*! tailwindcss v3.4.19 | MIT License | https://tailwindcss.com*/*,:after,:before{box-sizing:border-box;border:0 solid #e5e7eb}:after,:before{--tw-content:""}:host,html{line-height:1.5;-webkit-text-size-adjust:100%;-moz-tab-size:4;-o-tab-size:4;tab-size:4;font-family:ui-sans-serif,system-ui,sans-serif,Apple Color Emoji,Segoe UI Emoji,Segoe UI Symbol,Noto Color Emoji;font-feature-settings:normal;font-variation-settings:normal;-webkit-tap-highlight-color:transparent}body{margin:0;line-height:inherit}hr{height:0;color:inherit;border-top-width:1px}abbr:where([title]){-webkit-text-decoration:underline dotted;text-decoration:underline dotted}h1,h2,h3,h4,h5,h6{font-size:inherit;font-weight:inherit}a{color:inherit;text-decoration:inherit}b,strong{font-weight:bolder}code,kbd,pre,samp{font-family:ui-monospace,SFMono-Regular,Menlo,Monaco,Consolas,Liberation Mono,Courier New,monospace;font-feature-settings:normal;font-variation-settings:normal;font-size:1em}small{font-size:80%}sub,sup{font-size:75%;line-height:0;position:relative;vertical-align:baseline}sub{bottom:-.25em}sup{top:-.5em}table{text-indent:0;border-color:inherit;border-collapse:collapse}button,input,optgroup,select,textarea{font-family:inherit;font-feature-settings:inherit;font-variation-settings:inherit;font-size:100%;font-weight:inherit;line-height:inherit;letter-spacing:inherit;color:inherit;margin:0;padding:0}button,select{text-transform:none}button,input:where([type=button]),input:where([type=reset]),input:where([type=submit]){-webkit-appearance:button;background-color:transparent;background-image:none}:-moz-focusring{outline:auto}:-moz-ui-invalid{box-shadow:none}progress{vertical-align:baseline}::-webkit-inner-spin-button,::-webkit-outer-spin-button{height:auto}[type=search]{-webkit-appearance:textfield;outline-offset:-2px}::-webkit-search-decoration{-webkit-appearance:none}::-webkit-file-upload-button{-webkit-appearance:button;font:inherit}summary{display:list-item}blockquote,dd,dl,figure,h1,h2,h3,h4,h5,h6,hr,p,pre{margin:0}fieldset{margin:0}fieldset,legend{padding:0}menu,ol,ul{list-style:none;margin:0;padding:0}dialog{padding:0}textarea{resize:vertical}input::-moz-placeholder,textarea::-moz-placeholder{opacity:1;color:#9ca3af}input::placeholder,textarea::placeholder{opacity:1;color:#9ca3af}[role=button],button{cursor:pointer}:disabled{cursor:default}audio,canvas,embed,iframe,img,object,svg,video{display:block;vertical-align:middle}img,video{max-width:100%;height:auto}[hidden]:where(:not([hidden=until-found])){display:none}:root{--sf-ink:#22211f;--sf-forest:#26413c;--sf-forest-deep:#1a2d29;--sf-gold:#af8452;--sf-sand:#f4efe7;--sf-mist:#e6ddd0;--sf-cream:#fbf8f2;--sf-border:rgba(34,33,31,.1);--sf-border-strong:rgba(34,33,31,.18);--sf-shadow:0 18px 45px rgba(34,33,31,.08);--sf-shadow-panel:0 24px 60px rgba(28,43,40,.1);--sf-header-offset:12rem}html{scroll-behavior:smooth}body{--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));font-family:Manrope,sans-serif;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));-webkit-font-smoothing:antialiased;-moz-osx-font-smoothing:grayscale;background-image:radial-gradient(circle at top left,rgba(175,132,82,.09),transparent 30%),linear-gradient(180deg,hsla(0,0%,100%,.55),hsla(0,0%,100%,0))}main{position:relative}h1,h2,h3,h4,h5,h6{font-family:Cormorant Garamond,serif;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));letter-spacing:-.02em}a{transition-property:color,background-color,border-color,text-decoration-color,fill,stroke;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.2s}img{max-width:100%}input,select,textarea{font-family:Manrope,sans-serif}.\!container{width:100%!important}.container{width:100%}@media (min-width:640px){.\!container{max-width:640px!important}.container{max-width:640px}}@media (min-width:768px){.\!container{max-width:768px!important}.container{max-width:768px}}@media (min-width:1024px){.\!container{max-width:1024px!important}.container{max-width:1024px}}@media (min-width:1280px){.\!container{max-width:1280px!important}.container{max-width:1280px}}@media (min-width:1536px){.\!container{max-width:1536px!important}.container{max-width:1536px}}.storefront-shell{position:relative;min-height:100vh;overflow-x:hidden}.shell-topbar{border-bottom-width:1px;border-color:hsla(0,0%,100%,.1);--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1));font-size:.72rem;text-transform:uppercase;letter-spacing:.2em;color:hsla(0,0%,100%,.85)}.shell-topbar-inner{margin-left:auto;margin-right:auto;display:flex;max-width:80rem;flex-wrap:wrap;align-items:center;justify-content:space-between;gap:.75rem;padding:.75rem 1rem}@media (min-width:640px){.shell-topbar-inner{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.shell-topbar-inner{padding-left:2rem;padding-right:2rem}}.shell-topbar-copy{display:flex;align-items:center;gap:.5rem}.shell-topbar-meta{display:none;align-items:center;gap:1.5rem}@media (min-width:1024px){.shell-topbar-meta{display:flex}}.shell-header{position:sticky;top:0;z-index:40;border-bottom-width:1px;border-color:rgba(0,0,0,.05);background-color:hsla(0,0%,100%,.9);--tw-backdrop-blur:blur(24px);-webkit-backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia)}.shell-header-inner{margin-left:auto;margin-right:auto;display:flex;max-width:80rem;align-items:center;justify-content:space-between;gap:1rem;padding:1rem}@media (min-width:640px){.shell-header-inner{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.shell-header-inner{padding-left:2rem;padding-right:2rem}}.shell-brand{display:flex;min-width:0;align-items:center;gap:1rem}.shell-brand-mark{display:flex;height:3rem;align-items:center;justify-content:center;overflow:hidden;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.05);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding-left:1rem;padding-right:1rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.shell-brand-mark img{height:2rem;width:auto;-o-object-fit:contain;object-fit:contain}.shell-brand-copy{display:none;min-width:0;flex-direction:column}@media (min-width:640px){.shell-brand-copy{display:flex}}.shell-brand-title{font-family:Cormorant Garamond,serif;font-size:1.5rem;line-height:2rem;font-weight:600;line-height:1;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.shell-brand-subtitle,.shell-brand-title{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.shell-brand-subtitle{font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.2em;color:rgba(0,0,0,.55)}.shell-header-actions{display:flex;align-items:center;gap:.5rem}@media (min-width:640px){.shell-header-actions{gap:.75rem}}.shell-action-link{display:none;align-items:center;gap:.5rem;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:.75rem 1rem;font-size:.875rem;line-height:1.25rem;font-weight:600;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.shell-action-link:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}@media (min-width:1024px){.shell-action-link{display:inline-flex}}.shell-icon-button{display:inline-flex;height:2.75rem;width:2.75rem;align-items:center;justify-content:center;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.shell-icon-button:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.header-cart-pill{display:inline-flex;align-items:center;gap:.75rem;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.75rem 1.25rem;font-size:.875rem;line-height:1.25rem;font-weight:600;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.header-cart-pill:hover{--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1))}.header-cart-count{display:inline-flex;height:1.5rem;min-width:1.5rem;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding-left:.25rem;padding-right:.25rem;font-size:.75rem;line-height:1rem;font-weight:700;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.header-search-shell{display:none}@media (min-width:1024px){.header-search-shell{display:block}}.header-search-form{display:flex;min-width:18rem;align-items:center;gap:.75rem;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:.75rem 1rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.header-search-form input{width:100%;border-width:0;background-color:transparent;padding:0;font-size:.875rem;line-height:1.25rem;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));outline:2px solid transparent;outline-offset:2px;--tw-ring-offset-shadow:var(--tw-ring-inset) 0 0 0 var(--tw-ring-offset-width) var(--tw-ring-offset-color);--tw-ring-shadow:var(--tw-ring-inset) 0 0 0 calc(var(--tw-ring-offset-width)) var(--tw-ring-color);box-shadow:var(--tw-ring-offset-shadow),var(--tw-ring-shadow),var(--tw-shadow,0 0 #0000)}.desktop-nav{display:none;border-top-width:1px;border-color:rgba(0,0,0,.05);background-color:hsla(0,0%,100%,.8)}@media (min-width:1024px){.desktop-nav{display:block}}.desktop-nav-inner{margin-left:auto;margin-right:auto;display:flex;max-width:80rem;align-items:center;justify-content:space-between;gap:1rem;padding:1rem}@media (min-width:640px){.desktop-nav-inner{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.desktop-nav-inner{padding-left:2rem;padding-right:2rem}}.desktop-nav-links{display:flex;flex-wrap:wrap;align-items:center;gap:1.5rem}.desktop-nav-link{font-size:.82rem;font-weight:600;text-transform:uppercase;letter-spacing:.18em;color:rgba(0,0,0,.7)}.desktop-nav-link:hover{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.desktop-nav-caption{font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.18em;color:rgba(0,0,0,.45)}.mobile-search-drawer{display:none;border-top-width:1px;border-color:rgba(0,0,0,.05);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:1rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.mobile-search-drawer.active{display:block}.mobile-search-form{display:flex;align-items:center;gap:.75rem;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:.75rem 1rem}.mobile-search-form input{width:100%;border-width:0;background-color:transparent;padding:0;font-size:.875rem;line-height:1.25rem;outline:2px solid transparent;outline-offset:2px;--tw-ring-offset-shadow:var(--tw-ring-inset) 0 0 0 var(--tw-ring-offset-width) var(--tw-ring-offset-color);--tw-ring-shadow:var(--tw-ring-inset) 0 0 0 calc(var(--tw-ring-offset-width)) var(--tw-ring-color);box-shadow:var(--tw-ring-offset-shadow),var(--tw-ring-shadow),var(--tw-shadow,0 0 #0000)}.mobile-nav{position:fixed;left:0;right:0;bottom:0;z-index:40;border-top-width:1px;border-color:rgba(0,0,0,.1);background-color:hsla(0,0%,100%,.95);padding:.75rem 1rem;--tw-shadow:0 -10px 30px rgba(34,33,31,.12);--tw-shadow-colored:0 -10px 30px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);--tw-backdrop-blur:blur(24px);-webkit-backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia)}@media (min-width:1024px){.mobile-nav{display:none}}.mobile-nav-grid{margin-left:auto;margin-right:auto;display:grid;max-width:32rem;grid-template-columns:repeat(5,minmax(0,1fr));gap:.5rem}.mobile-nav-link{display:flex;flex-direction:column;align-items:center;gap:.25rem;border-radius:1rem;padding:.5rem;font-size:.68rem;font-weight:600;text-transform:uppercase;letter-spacing:.08em;color:rgba(0,0,0,.55)}.mobile-nav-link.active{--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.mobile-drawer-scrim{pointer-events:none;position:fixed;inset:0;z-index:40;background-color:rgba(0,0,0,.4);opacity:0;transition-property:opacity;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.2s}.mobile-drawer-scrim.active{pointer-events:auto;opacity:1}.mobile-category-drawer{position:fixed;top:0;bottom:0;left:0;z-index:50;display:flex;width:100%;max-width:24rem;--tw-translate-x:-100%;flex-direction:column;--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:1.5rem;--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);transition-property:transform;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.3s}.mobile-category-drawer,.mobile-category-drawer.active{transform:translate(var(--tw-translate-x),var(--tw-translate-y)) rotate(var(--tw-rotate)) skewX(var(--tw-skew-x)) skewY(var(--tw-skew-y)) scaleX(var(--tw-scale-x)) scaleY(var(--tw-scale-y))}.mobile-category-drawer.active{--tw-translate-x:0px}.drawer-head{margin-bottom:1.5rem;display:flex;align-items:center;justify-content:space-between;gap:1rem}.drawer-kicker{display:block;font-size:.72rem;font-weight:600;text-transform:uppercase;letter-spacing:.22em;--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.drawer-title{font-size:1.875rem;line-height:2.25rem;font-weight:600}.drawer-grid{display:grid;gap:.75rem;overflow-y:auto;padding-right:.25rem}.drawer-link{display:flex;align-items:center;justify-content:space-between;border-radius:1.4rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1rem;font-size:.875rem;line-height:1.25rem;font-weight:600;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.drawer-link:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1))}.drawer-link:hover,.shell-footer{--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.shell-footer{margin-top:5rem;border-top-width:1px;border-color:rgba(0,0,0,.05)}.shell-footer-inner{margin-left:auto;margin-right:auto;max-width:80rem;padding:4rem 1rem}@media (min-width:640px){.shell-footer-inner{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.shell-footer-inner{padding-left:2rem;padding-right:2rem}}.footer-top{display:grid;gap:2rem;border-bottom-width:1px;border-color:rgba(0,0,0,.05);padding-bottom:2.5rem}@media (min-width:1024px){.footer-top{grid-template-columns:1.5fr 1fr}}.footer-brand>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(1.25rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(1.25rem*var(--tw-space-y-reverse))}.footer-brand-copy{max-width:42rem;font-size:.875rem;line-height:1.75rem;color:rgba(0,0,0,.65)}.footer-socials{display:flex;flex-wrap:wrap;align-items:center;gap:.75rem}.footer-socials a{display:inline-flex;height:2.75rem;width:2.75rem;align-items:center;justify-content:center;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.footer-socials a:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.footer-trust-list{display:grid;gap:.75rem}@media (min-width:640px){.footer-trust-list{grid-template-columns:repeat(3,minmax(0,1fr))}}@media (min-width:1024px){.footer-trust-list{grid-template-columns:repeat(1,minmax(0,1fr))}}.footer-trust-card{display:flex;align-items:flex-start;gap:.75rem;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1rem}.footer-columns{display:grid;gap:2rem;padding-top:2.5rem}@media (min-width:768px){.footer-columns{grid-template-columns:repeat(2,minmax(0,1fr))}}@media (min-width:1280px){.footer-columns{grid-template-columns:repeat(4,minmax(0,1fr))}}.footer-title{margin-bottom:1rem;font-size:1.5rem;line-height:2rem;font-weight:600}.footer-list>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(.75rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(.75rem*var(--tw-space-y-reverse))}.footer-list{font-size:.875rem;line-height:1.25rem;color:rgba(0,0,0,.65)}.footer-list a:hover{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.footer-bottom{margin-top:2.5rem;display:flex;flex-direction:column;gap:1rem;border-top-width:1px;border-color:rgba(0,0,0,.05);padding-top:1.5rem;font-size:.875rem;line-height:1.25rem;color:rgba(0,0,0,.55)}@media (min-width:640px){.footer-bottom{flex-direction:row;align-items:center;justify-content:space-between}}.storefront-main{padding-bottom:7rem}@media (min-width:1024px){.storefront-main{padding-bottom:0}}.section-shell{margin-left:auto;margin-right:auto;max-width:80rem;padding-left:1rem;padding-right:1rem}@media (min-width:640px){.section-shell{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.section-shell{padding-left:2rem;padding-right:2rem}}.editorial-section{padding-top:3rem;padding-bottom:3rem}@media (min-width:1024px){.editorial-section{padding-top:4rem;padding-bottom:4rem}}.editorial-section-muted{border-radius:2rem;border-width:1px;border-color:rgba(0,0,0,.05);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.section-kicker{margin-bottom:.75rem;display:block;font-size:.76rem;font-weight:600;text-transform:uppercase;letter-spacing:.24em;--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.section-heading{font-size:2.25rem;line-height:2.5rem;font-weight:600;line-height:1.25}@media (min-width:640px){.section-heading{font-size:3rem;line-height:1}}.section-lead{margin-top:1rem;max-width:42rem;font-size:.875rem;line-height:1.75rem;color:rgba(0,0,0,.65)}@media (min-width:640px){.section-lead{font-size:1rem;line-height:1.5rem}}.section-link{display:inline-flex;align-items:center;gap:.5rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.16em;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.section-link:hover{--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.hero-shell{margin-left:auto;margin-right:auto;max-width:80rem;padding-left:1rem;padding-right:1rem}@media (min-width:640px){.hero-shell{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.hero-shell{padding-left:2rem;padding-right:2rem}}.hero-shell{padding-top:1.5rem}@media (min-width:1024px){.hero-shell{padding-top:2.5rem}}.hero-grid{display:grid;gap:1.5rem}@media (min-width:1024px){.hero-grid{grid-template-columns:1.05fr .95fr}}.hero-copy-card{position:relative;overflow:hidden;border-radius:2rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:2rem 1.5rem;--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}@media (min-width:640px){.hero-copy-card{padding:2.5rem 2rem}}.hero-copy-card:before{content:"";position:absolute;left:0;right:0;top:0;height:.25rem;background-image:linear-gradient(to right,var(--tw-gradient-stops));--tw-gradient-from:#af8452 var(--tw-gradient-from-position);--tw-gradient-to:rgba(175,132,82,0) var(--tw-gradient-to-position);--tw-gradient-stops:var(--tw-gradient-from),var(--tw-gradient-to);--tw-gradient-to:rgba(38,65,60,0) var(--tw-gradient-to-position);--tw-gradient-stops:var(--tw-gradient-from),#26413c var(--tw-gradient-via-position),var(--tw-gradient-to);--tw-gradient-to:#af8452 var(--tw-gradient-to-position)}.hero-copy-card:after{content:"";pointer-events:none;position:absolute;right:-4rem;top:-5rem;height:13rem;width:13rem;border-radius:9999px;background-color:hsla(37,37%,93%,.8);--tw-blur:blur(64px);filter:var(--tw-blur) var(--tw-brightness) var(--tw-contrast) var(--tw-grayscale) var(--tw-hue-rotate) var(--tw-invert) var(--tw-saturate) var(--tw-sepia) var(--tw-drop-shadow)}.hero-actions{margin-top:2rem;display:flex;flex-wrap:wrap;align-items:center;gap:.75rem}.storefront-button{display:inline-flex;align-items:center;justify-content:center;gap:.5rem;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.75rem 1.5rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.16em;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.storefront-button:hover{--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1))}.storefront-button-ghost{display:inline-flex;align-items:center;justify-content:center;gap:.5rem;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:.75rem 1.5rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.16em;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.storefront-button-ghost:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.hero-stats{margin-top:2.5rem;display:grid;gap:1rem}@media (min-width:640px){.hero-stats{grid-template-columns:repeat(3,minmax(0,1fr))}}.hero-stat{border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1rem}.hero-stat strong{display:block;font-family:Cormorant Garamond,serif;font-size:1.875rem;line-height:2.25rem;font-weight:600;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.hero-stat span{margin-top:.5rem;display:block;font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.18em;color:rgba(0,0,0,.55)}.hero-media-card{position:relative;overflow:hidden;border-radius:2rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(231 223 212/var(--tw-bg-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.hero-media-frame{position:relative;height:22rem;overflow:hidden}@media (min-width:640px){.hero-media-frame{height:28rem}}@media (min-width:1024px){.hero-media-frame{height:100%}}.hero-media-slide{position:absolute;inset:0;opacity:0;transition-property:opacity;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.5s}.hero-media-slide.active{opacity:1}.hero-media-slide img{height:100%;width:100%;-o-object-fit:cover;object-fit:cover}.hero-media-overlay{pointer-events:none;position:absolute;left:0;right:0;bottom:0;background-image:linear-gradient(to top,var(--tw-gradient-stops));--tw-gradient-from:rgba(0,0,0,.5) var(--tw-gradient-from-position);--tw-gradient-stops:var(--tw-gradient-from),var(--tw-gradient-to);--tw-gradient-stops:var(--tw-gradient-from),rgba(0,0,0,.1) var(--tw-gradient-via-position),var(--tw-gradient-to);--tw-gradient-to:transparent var(--tw-gradient-to-position);padding:1.5rem;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1))}.section-card-grid{display:grid;gap:1.25rem}@media (min-width:640px){.section-card-grid{grid-template-columns:repeat(2,minmax(0,1fr))}}@media (min-width:1280px){.section-card-grid{grid-template-columns:repeat(4,minmax(0,1fr))}}.collection-card{display:flex;align-items:center;gap:1rem;border-radius:1.7rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:1.25rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.collection-card,.collection-card:hover{box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.collection-card:hover{--tw-translate-y:-0.25rem;transform:translate(var(--tw-translate-x),var(--tw-translate-y)) rotate(var(--tw-rotate)) skewX(var(--tw-skew-x)) skewY(var(--tw-skew-y)) scaleX(var(--tw-scale-x)) scaleY(var(--tw-scale-y));--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color)}.collection-card-icon{display:inline-flex;height:3.5rem;width:3.5rem;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));font-size:1.125rem;line-height:1.75rem;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.collection-card-copy{min-width:0;flex:1 1 0%}.collection-card-copy strong{display:block;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;font-size:1.125rem;line-height:1.75rem;font-weight:600}.collection-card-copy span{display:block;font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.18em;color:rgba(0,0,0,.5)}.feature-grid{display:grid;gap:1.25rem}@media (min-width:768px){.feature-grid{grid-template-columns:repeat(2,minmax(0,1fr))}}@media (min-width:1280px){.feature-grid{grid-template-columns:repeat(4,minmax(0,1fr))}}.feature-card{border-radius:1.7rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:1.75rem 1.5rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.feature-icon{margin-bottom:1.25rem;height:3.5rem;width:3.5rem;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));font-size:1.25rem;line-height:1.75rem}.feature-title{font-size:1.5rem;line-height:2rem;font-weight:600}.feature-copy{margin-top:.75rem;font-size:.875rem;line-height:1.75rem;color:rgba(0,0,0,.65)}.product-grid{display:grid;gap:1.25rem}@media (min-width:640px){.product-grid{grid-template-columns:repeat(2,minmax(0,1fr))}}@media (min-width:1280px){.product-grid{grid-template-columns:repeat(4,minmax(0,1fr))}}.pro-product-card{position:relative;display:flex;height:100%;flex-direction:column;overflow:hidden;border-radius:1.75rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.3s}.pro-product-card,.pro-product-card:hover{box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.pro-product-card:hover{--tw-translate-y:-0.25rem;transform:translate(var(--tw-translate-x),var(--tw-translate-y)) rotate(var(--tw-rotate)) skewX(var(--tw-skew-x)) skewY(var(--tw-skew-y)) scaleX(var(--tw-scale-x)) scaleY(var(--tw-scale-y));--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color)}.pro-product-card{transform-style:preserve-3d}.product-badges{pointer-events:none;position:absolute;left:1rem;top:1rem;z-index:20;display:flex;flex-direction:column;gap:.5rem}.badge-item{display:inline-flex;align-items:center;border-radius:9999px;padding:.25rem .75rem;font-size:.68rem;font-weight:600;text-transform:uppercase;letter-spacing:.16em}.badge-discount{background-color:rgb(255 228 230/var(--tw-bg-opacity,1));color:rgb(190 18 60/var(--tw-text-opacity,1))}.badge-discount,.badge-new{--tw-bg-opacity:1;--tw-text-opacity:1}.badge-new{background-color:rgb(209 250 229/var(--tw-bg-opacity,1));color:rgb(4 120 87/var(--tw-text-opacity,1))}.badge-bestseller{--tw-bg-opacity:1;background-color:rgb(254 243 199/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(180 83 9/var(--tw-text-opacity,1))}.like-btn,.zoom-btn{position:absolute;z-index:20;display:inline-flex;height:2.75rem;width:2.75rem;align-items:center;justify-content:center;border-radius:9999px;border-width:1px;border-color:hsla(0,0%,100%,.7);background-color:hsla(0,0%,100%,.9);--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);--tw-backdrop-blur:blur(8px);-webkit-backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia)}.like-btn:hover,.zoom-btn:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.like-btn{right:1rem;top:1rem}.zoom-btn{left:1rem;top:1rem}.pro-img-container{position:relative;display:block;overflow:hidden;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));aspect-ratio:4/4.45}.pro-img{height:100%;width:100%;-o-object-fit:cover;object-fit:cover;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.5s}.pro-product-card:hover .pro-img{transform:scale(1.05)}.card-glare{pointer-events:none;position:absolute;inset:0;z-index:10;opacity:0;transition-property:opacity;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.3s}.card-info{display:flex;flex:1 1 0%;flex-direction:column;gap:1rem;padding:1.25rem}.product-title{overflow:hidden;display:-webkit-box;-webkit-box-orient:vertical;-webkit-line-clamp:2;font-size:1.25rem;line-height:1.75rem;font-weight:600;line-height:1.375;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.product-title:hover{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.price-wrapper{display:flex;flex-wrap:wrap;align-items:flex-end;gap:.5rem}.old-price{font-size:.875rem;line-height:1.25rem;color:rgba(0,0,0,.45);text-decoration-line:line-through}.new-price{font-size:1.5rem;line-height:2rem;font-weight:700;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.neon-btn-wrapper{margin-top:auto;display:inline-flex;width:100%;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.75rem 1.25rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.neon-btn-wrapper:hover{--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1))}.neon-btn-inner{display:inline-flex;align-items:center;justify-content:center;gap:.5rem}.catalog-header{margin-left:auto;margin-right:auto;max-width:80rem;padding-left:1rem;padding-right:1rem}@media (min-width:640px){.catalog-header{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.catalog-header{padding-left:2rem;padding-right:2rem}}.catalog-header{padding-top:2rem}.catalog-intro{border-radius:2rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:2rem 1.5rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}@media (min-width:640px){.catalog-intro{padding-left:2rem;padding-right:2rem}}.catalog-chip-row{margin-top:1.5rem;display:flex;gap:.75rem;overflow-x:auto;padding-bottom:.5rem;scrollbar-width:none}.catalog-chip-row::-webkit-scrollbar{display:none}.cat-chip{display:inline-flex;flex-shrink:0;align-items:center;gap:.5rem;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:.75rem 1rem;font-size:.75rem;line-height:1rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;color:rgba(0,0,0,.6);--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.cat-chip:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.cat-chip.active{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1))}.filter-bar{position:sticky;z-index:30;margin-top:1.5rem;border-top-width:1px;border-bottom-width:1px;border-color:rgba(0,0,0,.05);background-color:hsla(0,0%,100%,.9);--tw-backdrop-blur:blur(24px);-webkit-backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);top:4.75rem}.filter-bar.scrolled{box-shadow:var(--sf-shadow)}.filter-bar-inner{display:flex;flex-direction:column;gap:1rem;padding-top:1rem;padding-bottom:1rem}@media (min-width:1024px){.filter-bar-inner{flex-direction:row;align-items:center;justify-content:space-between}}.result-count{font-size:.875rem;line-height:1.25rem;color:rgba(0,0,0,.55)}.filter-controls{display:flex;flex-wrap:wrap;align-items:center;gap:.5rem}.active-filter-tag{gap:.5rem;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));padding:.5rem .75rem;font-size:.75rem;line-height:1rem;font-weight:600;text-transform:uppercase;letter-spacing:.12em}.active-filter-tag,.remove-filter{display:inline-flex;align-items:center;border-radius:9999px;--tw-bg-opacity:1;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.remove-filter{height:1.25rem;width:1.25rem;justify-content:center;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));font-size:.65rem}.remove-filter:hover{--tw-bg-opacity:1;background-color:rgb(0 0 0/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1))}.custom-dropdown{position:relative}.filter-dropdown-btn{display:inline-flex;align-items:center;gap:.5rem;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:.75rem 1rem;font-size:.75rem;line-height:1rem;font-weight:600;text-transform:uppercase;letter-spacing:.12em;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.filter-dropdown-btn:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.custom-dropdown-menu{pointer-events:none;position:absolute;right:0;top:100%;z-index:30;margin-top:.75rem;min-width:15rem;--tw-translate-y:0.5rem;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:.75rem;opacity:0;--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.2s}.custom-dropdown-menu,.custom-dropdown.open .custom-dropdown-menu{transform:translate(var(--tw-translate-x),var(--tw-translate-y)) rotate(var(--tw-rotate)) skewX(var(--tw-skew-x)) skewY(var(--tw-skew-y)) scaleX(var(--tw-scale-x)) scaleY(var(--tw-scale-y))}.custom-dropdown.open .custom-dropdown-menu{pointer-events:auto;--tw-translate-y:0px;opacity:1}.sort-menu>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(.25rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(.25rem*var(--tw-space-y-reverse))}.filter-option-btn,.sort-item{display:flex;width:100%;align-items:center;justify-content:space-between;border-radius:1rem;padding:.75rem 1rem;font-size:.875rem;line-height:1.25rem;font-weight:500;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.filter-option-btn.active,.filter-option-btn:hover,.sort-item.active,.sort-item:hover{--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.filter-option-meta{font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.12em;color:rgba(0,0,0,.4)}.price-filter-popup>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(.75rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(.75rem*var(--tw-space-y-reverse))}.price-input-group{display:flex;align-items:center;gap:.75rem}.auth-input,.auth-select,.checkout-input,.checkout-select,.coupon-input,.price-input{width:100%;border-radius:1rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:.75rem 1rem;font-size:.875rem;line-height:1.25rem;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));outline:2px solid transparent;outline-offset:2px;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.auth-input:focus,.auth-select:focus,.checkout-input:focus,.checkout-select:focus,.coupon-input:focus,.price-input:focus{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.empty-state{border-radius:2rem;border:1px dashed rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:4rem 1.5rem;text-align:center;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.empty-state-icon{margin-left:auto;margin-right:auto;margin-bottom:1.25rem;display:inline-flex;height:4rem;width:4rem;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));font-size:1.25rem;line-height:1.75rem;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.load-more-wrapper{margin-top:2.5rem;text-align:center}.load-more-btn{display:inline-flex;align-items:center;justify-content:center;gap:.5rem;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:.75rem 1.5rem;font-size:.75rem;line-height:1rem;font-weight:600;text-transform:uppercase;letter-spacing:.16em;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.load-more-btn:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.page-progress{margin-top:1rem;font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.16em;color:rgba(0,0,0,.45)}.page-progress-bar{margin-left:auto;margin-right:auto;margin-top:.75rem;height:.375rem;width:8rem;overflow:hidden;border-radius:9999px;background-color:rgba(0,0,0,.1)}.page-progress-fill{height:100%;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1))}.product-page-shell{margin-left:auto;margin-right:auto;max-width:80rem;padding-left:1rem;padding-right:1rem}@media (min-width:640px){.product-page-shell{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.product-page-shell{padding-left:2rem;padding-right:2rem}}.product-page-shell{padding-top:2rem;padding-bottom:2rem}.product-breadcrumb{margin-bottom:1.5rem;font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.14em;color:rgba(0,0,0,.45)}.product-breadcrumb a:hover{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.product-detail-grid{display:grid;gap:2rem}@media (min-width:1024px){.product-detail-grid{grid-template-columns:1.05fr .95fr}}.auth-card,.checkout-section-card,.checkout-summary-card,.order-summary,.product-copy-panel,.product-gallery-panel,.storefront-panel{border-radius:2rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.product-copy-panel,.product-gallery-panel{overflow:hidden}.product-main-image{height:28rem;width:100%;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));-o-object-fit:contain;object-fit:contain;padding:1.5rem}@media (min-width:640px){.product-main-image{height:36rem}}.product-gallery{position:relative}.breadcrumb-urun{margin-bottom:1.5rem;font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.14em;color:rgba(0,0,0,.45)}.breadcrumb-urun a:hover{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.breadcrumb-urun .breadcrumb{margin-bottom:0;display:flex;flex-wrap:wrap;align-items:center;gap:.5rem;background-color:transparent;padding:0}.breadcrumb-urun .breadcrumb-item{font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.14em;color:rgba(0,0,0,.45)}.breadcrumb-urun .breadcrumb-item+.breadcrumb-item:before{padding-right:.5rem;color:rgba(0,0,0,.3);content:"/"}.zoom-btn{position:absolute;left:1.25rem;top:1.25rem;z-index:10;display:inline-flex;height:3rem;width:3rem;align-items:center;justify-content:center;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);background-color:hsla(0,0%,100%,.9);--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1));--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);--tw-backdrop-blur:blur(8px);-webkit-backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia)}.zoom-btn:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.thumbnail-container{display:flex;gap:.75rem;overflow-x:auto;padding:1.25rem 1.5rem;scrollbar-width:none}.thumbnail-container::-webkit-scrollbar{display:none}.thumbnail-item{flex-shrink:0;overflow:hidden;border-radius:1.1rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:.25rem;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.thumbnail-item:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1))}.thumbnail-item{width:5.5rem;height:5.5rem}.thumbnail-item.active{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1))}.thumbnail-img{height:100%;width:100%;border-radius:.95rem;-o-object-fit:cover;object-fit:cover}.product-copy-panel{padding:1.75rem 1.5rem}@media (min-width:640px){.product-copy-panel{padding-left:2rem;padding-right:2rem}}.product-header h1{font-size:2.25rem;line-height:2.5rem;font-weight:600;line-height:1.25}@media (min-width:640px){.product-header h1{font-size:3rem;line-height:1}}.price-container{margin-top:1.25rem;display:flex;flex-wrap:wrap;align-items:flex-end;gap:.75rem}.current-price{font-size:2.25rem;line-height:2.5rem;font-weight:700;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.discount-badge{display:inline-flex;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));padding:.25rem .75rem;font-size:.75rem;line-height:1rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.product-description-section,.quantity-selector,.variant-selection{margin-top:2rem}.description-title,.variant-label{margin-bottom:.75rem;font-size:1.5rem;line-height:2rem;font-weight:600}.variant-options{display:flex;flex-wrap:wrap;gap:.75rem}.variant-radio{position:absolute;width:1px;height:1px;padding:0;margin:-1px;overflow:hidden;clip:rect(0,0,0,0);white-space:nowrap;border-width:0}.variant-box{display:flex;min-width:11rem;flex-direction:column;gap:.25rem;border-radius:1.3rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1rem;text-align:left;font-size:.875rem;line-height:1.25rem;font-weight:600;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1));transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.variant-box:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1))}.variant-radio:checked+.variant-box{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.variant-option.disabled .variant-box,.variant-radio:disabled+.variant-box{cursor:not-allowed;border-color:rgba(0,0,0,.05);background-color:rgba(0,0,0,.05);color:rgba(0,0,0,.35)}.variant-meta{font-size:.75rem;line-height:1rem;font-weight:500;text-transform:uppercase;letter-spacing:.12em;color:rgba(0,0,0,.45)}.quantity-selector .d-flex{display:flex;align-items:center;gap:.75rem}.quantity-btn{display:inline-flex;height:2.75rem;width:2.75rem;align-items:center;justify-content:center;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));font-size:1.125rem;line-height:1.75rem;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.quantity-btn:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.quantity-input{height:2.75rem;width:5rem;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));text-align:center;font-size:.875rem;line-height:1.25rem;font-weight:600}.trust-badges{margin-top:2rem;display:grid;gap:1rem}@media (min-width:640px){.trust-badges{grid-template-columns:repeat(3,minmax(0,1fr))}}.trust-badges .badge-item{display:flex;flex-direction:column;align-items:flex-start;border-radius:1.4rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1rem;text-align:left;text-transform:none;letter-spacing:0}.badge-icon{margin-bottom:.75rem;display:inline-flex;height:2.5rem;width:2.5rem;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.badge-text{font-size:1rem;line-height:1.5rem;font-weight:600;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.badge-subtext{margin-top:.25rem;font-size:.875rem;line-height:1.25rem;color:rgba(0,0,0,.55)}.description-content{font-size:.875rem;line-height:1.75rem;color:rgba(0,0,0,.65)}.persuasion-features{margin-top:2rem;display:grid;gap:1rem}.feature-item{display:flex;align-items:flex-start;gap:1rem;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1.25rem}.feature-icon{display:inline-flex;height:3rem;width:3rem;flex-shrink:0;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.feature-content h5{font-size:1.125rem;line-height:1.75rem;font-weight:600}.feature-content p{margin-top:.25rem;font-size:.875rem;line-height:1.5rem;color:rgba(0,0,0,.55)}.mobile-lightbox{pointer-events:none;position:fixed;inset:0;z-index:80;display:flex;flex-direction:column;background-color:rgba(0,0,0,.95);opacity:0;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.mobile-lightbox.active{pointer-events:auto;opacity:1}.lightbox-controls,.lightbox-controls-desktop{display:flex;align-items:center;justify-content:space-between;gap:.75rem;padding:1rem}.lightbox-btn,.lightbox-control-btn,.zoom-btn-mobile{display:inline-flex;height:2.75rem;width:2.75rem;align-items:center;justify-content:center;border-radius:9999px;border-width:1px;border-color:hsla(0,0%,100%,.1);background-color:hsla(0,0%,100%,.1);--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-backdrop-blur:blur(8px);-webkit-backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.lightbox-btn:hover,.lightbox-control-btn.close-btn,.lightbox-control-btn:hover,.zoom-btn-mobile:hover{--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.lightbox-img-container{position:relative;display:flex;min-height:0;flex:1 1 0%;align-items:center;justify-content:center;overflow:hidden;padding:1rem}@media (min-width:640px){.lightbox-img-container{padding:2rem}}.lightbox-image{max-height:100%;width:auto;max-width:100%;-o-object-fit:contain;object-fit:contain;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s;transform-origin:center center}.zoom-controls{display:flex;align-items:center;justify-content:center;gap:1rem;padding:1rem;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1))}.zoom-level{min-width:4rem;text-align:center;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;color:hsla(0,0%,100%,.7)}.modal-lightbox .modal-dialog{max-width:72rem}.modal-lightbox .modal-content{overflow:hidden;border-radius:2rem;border-width:0;--tw-bg-opacity:1;background-color:rgb(0 0 0/var(--tw-bg-opacity,1))}.technical-spec-table{width:100%;border-collapse:collapse;overflow:hidden;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1)}.technical-spec-table td,.technical-spec-table th{border-bottom-width:1px;border-color:rgba(0,0,0,.1);padding:.75rem 1rem;text-align:left;font-size:.875rem;line-height:1.25rem}.technical-spec-table th{width:34%;--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));font-weight:600;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.media-showcase-section,.reviews-section{margin-top:3rem}.media-showcase-section>:not([hidden])~:not([hidden]),.reviews-section>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(2rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(2rem*var(--tw-space-y-reverse))}.media-showcase-block{border-radius:2rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:1.75rem 1.5rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.media-showcase-card{overflow:hidden;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1))}.media-showcase-frame{overflow:hidden;--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));aspect-ratio:1/.85}.media-showcase-frame iframe,.media-showcase-frame img,.media-showcase-frame video{height:100%;width:100%;-o-object-fit:cover;object-fit:cover}.media-showcase-body>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(.5rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(.5rem*var(--tw-space-y-reverse))}.media-showcase-body{padding:1rem}.media-showcase-body h5{font-size:1.25rem;line-height:1.75rem;font-weight:600}.media-showcase-tags{font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.16em;color:rgba(0,0,0,.45)}.review-avatar{margin-right:1rem;display:inline-flex;height:3rem;width:3rem;flex-shrink:0;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));font-weight:600;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.auth-shell{margin-left:auto;margin-right:auto;max-width:80rem;padding-left:1rem;padding-right:1rem}@media (min-width:640px){.auth-shell{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.auth-shell{padding-left:2rem;padding-right:2rem}}.auth-shell{padding-top:2.5rem;padding-bottom:2.5rem}@media (min-width:1024px){.auth-shell{padding-top:4rem;padding-bottom:4rem}}.auth-card{overflow:hidden}.auth-card-body,.auth-form-panel{padding:2rem 1.5rem}@media (min-width:640px){.auth-card-body,.auth-form-panel{padding:2.5rem 2rem}}.auth-card-body{--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1))}.auth-form-panel{--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.auth-kicker{display:inline-flex;align-items:center;gap:.5rem;font-size:.72rem;font-weight:600;text-transform:uppercase;letter-spacing:.22em;--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.auth-title{margin-top:1.25rem;font-size:2.25rem;line-height:2.5rem;font-weight:600;line-height:1.25;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1))}.auth-copy{margin-top:1rem;font-size:.875rem;line-height:1.75rem;color:hsla(0,0%,100%,.75)}.auth-metrics{margin-top:2rem}.auth-metrics>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(1rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(1rem*var(--tw-space-y-reverse))}.auth-metric{display:flex;align-items:flex-start;gap:1rem;border-radius:1.5rem;border-width:1px;border-color:hsla(0,0%,100%,.1);background-color:hsla(0,0%,100%,.05);padding:1rem}.auth-metric-icon{display:inline-flex;height:2.75rem;width:2.75rem;flex-shrink:0;align-items:center;justify-content:center;border-radius:9999px;background-color:hsla(0,0%,100%,.1);--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.auth-field>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(.5rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(.5rem*var(--tw-space-y-reverse))}.auth-field label,.checkout-label{font-size:.875rem;line-height:1.25rem;font-weight:600;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.auth-input-wrap{display:flex;align-items:center;gap:.75rem;border-radius:1rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:.75rem 1rem;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.auth-input-wrap:focus-within{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.auth-input-wrap i{color:rgba(0,0,0,.35)}.auth-input,.auth-select{border-width:0;background-color:transparent;padding:0}.auth-actions{margin-top:1rem;display:flex;flex-wrap:wrap;align-items:center;justify-content:space-between;gap:.75rem;font-size:.875rem;line-height:1.25rem}.auth-link{font-weight:600;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.auth-link:hover{--tw-text-opacity:1;color:rgb(175 132 82/var(--tw-text-opacity,1))}.btn-premium{display:inline-flex;align-items:center;justify-content:center;gap:.5rem;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.75rem 1.5rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.16em;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.btn-premium:hover{--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1))}.auth-footer{margin-top:1.5rem;font-size:.875rem;line-height:1.25rem;color:rgba(0,0,0,.55)}.cart-page{margin-left:auto;margin-right:auto;max-width:80rem;padding-left:1rem;padding-right:1rem}@media (min-width:640px){.cart-page{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.cart-page{padding-left:2rem;padding-right:2rem}}.cart-page{padding-top:2.5rem;padding-bottom:2.5rem}@media (min-width:1024px){.cart-page{padding-top:3.5rem;padding-bottom:3.5rem}}.cart-breadcrumb{margin-bottom:.75rem;font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.16em;color:rgba(0,0,0,.45)}.cart-breadcrumb a:hover{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.cart-title{font-size:2.25rem;line-height:2.5rem;font-weight:600}.cart-count{margin-top:.5rem;font-size:.875rem;line-height:1.25rem;color:rgba(0,0,0,.55)}.empty-cart{border-radius:2rem;border:1px dashed rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:4rem 1.5rem;text-align:center;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.empty-cart-circle{margin-left:auto;margin-right:auto;margin-bottom:1.25rem;height:5rem;width:5rem;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));font-size:1.5rem;line-height:2rem;color:rgb(38 65 60/var(--tw-text-opacity,1))}.empty-cart-circle,.shop-now-btn{display:inline-flex;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;--tw-text-opacity:1}.shop-now-btn{gap:.5rem;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.75rem 1.5rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.shop-now-btn:hover{--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1))}.cart-item{position:relative;margin-bottom:1rem;display:flex;gap:1rem;border-radius:1.75rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:1.25rem;--tw-shadow:0 18px 45px rgba(34,33,31,.08);--tw-shadow-colored:0 18px 45px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.cart-item-img{height:6rem;width:6rem;flex-shrink:0;border-radius:1.2rem;-o-object-fit:cover;object-fit:cover}.cart-item-body{display:flex;min-width:0;flex:1 1 0%;flex-direction:column;gap:.75rem}.cart-item-title{overflow:hidden;display:-webkit-box;-webkit-box-orient:vertical;-webkit-line-clamp:2;font-size:1.125rem;line-height:1.75rem;font-weight:600;line-height:1.375}.cart-item-variant{display:inline-flex;align-self:flex-start;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));padding:.25rem .75rem;font-size:.75rem;line-height:1rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.cart-item-bottom{margin-top:auto;display:flex;flex-wrap:wrap;align-items:center;justify-content:space-between;gap:.75rem}.cart-item-price{font-size:1.25rem;line-height:1.75rem;font-weight:700;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.qty-control{display:inline-flex;align-items:center;overflow:hidden;border-radius:9999px;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1))}.qty-btn{display:inline-flex;height:2.5rem;width:2.5rem;align-items:center;justify-content:center;font-size:.875rem;line-height:1.25rem;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.qty-btn:hover{--tw-bg-opacity:1;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.qty-btn:hover,.qty-num{background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.qty-num{height:2.5rem;min-width:2.5rem;border-left-width:1px;border-right-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;padding-left:.75rem;padding-right:.75rem;font-size:.875rem;line-height:1.25rem;font-weight:600}.cart-item-delete,.qty-num{display:inline-flex;align-items:center;justify-content:center}.cart-item-delete{height:2.25rem;width:2.25rem;border-radius:9999px;color:rgba(0,0,0,.35)}.cart-item-delete:hover{--tw-bg-opacity:1;background-color:rgb(255 241 242/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(225 29 72/var(--tw-text-opacity,1))}.summary-head{border-bottom-width:1px;border-color:rgba(0,0,0,.05);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1.25rem 1.5rem;font-size:1.125rem;line-height:1.75rem;font-weight:600}.order-summary{position:sticky;top:7rem;overflow:hidden}.summary-body{padding:1.5rem}.summary-line{display:flex;align-items:center;justify-content:space-between;gap:1rem;padding-top:.5rem;padding-bottom:.5rem;font-size:.875rem;line-height:1.25rem}.summary-line .label{color:rgba(0,0,0,.55)}.summary-line .value{font-weight:600;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.summary-line.discount .value,.summary-line.free .value{--tw-text-opacity:1;color:rgb(5 150 105/var(--tw-text-opacity,1))}.summary-line.total{margin-top:1rem;border-top-width:1px;border-color:rgba(0,0,0,.1);padding-top:1rem}.summary-line.total .label,.summary-line.total .value{font-size:1rem;line-height:1.5rem}.summary-line.total .value{font-size:1.5rem;line-height:2rem;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.coupon-wrap{margin-top:1.25rem;border-top-width:1px;border-color:rgba(0,0,0,.05);padding-top:1.25rem}.coupon-row{display:flex;gap:.75rem}.checkout-btn,.checkout-submit,.coupon-apply{display:inline-flex;align-items:center;justify-content:center;gap:.5rem;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.75rem 1.25rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.checkout-btn:hover,.checkout-submit:hover,.coupon-apply:hover{--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1))}.checkout-btn,.checkout-submit{width:100%}.secure-note{margin-top:1rem;display:flex;align-items:center;justify-content:center;gap:.5rem;font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.16em;color:rgba(0,0,0,.45)}.mobile-checkout-bar{position:fixed;left:0;right:0;bottom:0;z-index:40;border-top-width:1px;border-color:rgba(0,0,0,.1);background-color:hsla(0,0%,100%,.95);padding:.75rem 1rem;--tw-shadow:0 -10px 30px rgba(34,33,31,.12);--tw-shadow-colored:0 -10px 30px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);--tw-backdrop-blur:blur(24px);-webkit-backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia);backdrop-filter:var(--tw-backdrop-blur) var(--tw-backdrop-brightness) var(--tw-backdrop-contrast) var(--tw-backdrop-grayscale) var(--tw-backdrop-hue-rotate) var(--tw-backdrop-invert) var(--tw-backdrop-opacity) var(--tw-backdrop-saturate) var(--tw-backdrop-sepia)}@media (min-width:1024px){.mobile-checkout-bar{display:none}}.mobile-checkout-inner{margin-left:auto;margin-right:auto;display:flex;max-width:32rem;align-items:center;justify-content:space-between;gap:1rem}.mobile-total-label{font-size:.72rem;text-transform:uppercase;letter-spacing:.16em;color:rgba(0,0,0,.45)}.mobile-total-price{font-size:1.25rem;line-height:1.75rem;font-weight:700;--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.mobile-checkout-btn{display:inline-flex;flex:1 1 0%;align-items:center;justify-content:center;gap:.5rem;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.75rem 1.25rem;font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.mobile-checkout-btn:hover{--tw-bg-opacity:1;background-color:rgb(26 45 41/var(--tw-bg-opacity,1))}.checkout-shell{margin-left:auto;margin-right:auto;max-width:80rem;padding-left:1rem;padding-right:1rem}@media (min-width:640px){.checkout-shell{padding-left:1.5rem;padding-right:1.5rem}}@media (min-width:1024px){.checkout-shell{padding-left:2rem;padding-right:2rem}}.checkout-shell{padding-top:2.5rem;padding-bottom:2.5rem}@media (min-width:1024px){.checkout-shell{padding-top:3.5rem;padding-bottom:3.5rem}}.checkout-hero{margin-bottom:2rem;overflow:hidden;border-radius:2rem;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:2rem 1.5rem;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}@media (min-width:640px){.checkout-hero{padding-left:2rem;padding-right:2rem}}.checkout-hero-grid{display:flex;flex-direction:column;gap:1.5rem}@media (min-width:1024px){.checkout-hero-grid{flex-direction:row;align-items:flex-end;justify-content:space-between}}.checkout-step,.checkout-stepper{display:flex;align-items:center;gap:.75rem}.checkout-step-circle{display:inline-flex;height:2.5rem;width:2.5rem;align-items:center;justify-content:center;border-radius:9999px;border-width:1px;border-color:hsla(0,0%,100%,.2);background-color:hsla(0,0%,100%,.1);font-size:.875rem;line-height:1.25rem;font-weight:600}.checkout-step.active .checkout-step-circle,.checkout-step.completed .checkout-step-circle{--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.checkout-step-label{font-size:.75rem;line-height:1rem;text-transform:uppercase;letter-spacing:.18em;color:hsla(0,0%,100%,.7)}.checkout-grid{display:grid;gap:1.5rem}@media (min-width:1024px){.checkout-grid{grid-template-columns:1.1fr .9fr}}.checkout-section-card,.checkout-summary-card{overflow:hidden}.checkout-section-head{border-bottom-width:1px;border-color:rgba(0,0,0,.05);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1.25rem 1.5rem;font-size:1.125rem;line-height:1.75rem;font-weight:600}.checkout-section-body{padding:1.5rem}.checkout-field-grid{display:grid;gap:1rem}@media (min-width:640px){.checkout-field-grid{grid-template-columns:repeat(2,minmax(0,1fr))}.checkout-field-grid .span-2{grid-column:span 2/span 2}}.checkout-address-grid{display:grid;gap:1rem}@media (min-width:640px){.checkout-address-grid{grid-template-columns:repeat(2,minmax(0,1fr))}}.checkout-address-card{position:relative;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1rem;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.checkout-address-card:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.checkout-address-card.selected{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1))}.stepper-item.active .stepper-circle,.stepper-item.completed .stepper-circle{--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.address-card{position:relative;border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1rem;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.address-card:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.address-card.selected{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1))}.radio-circle{display:inline-flex;height:1.25rem;width:1.25rem;border-radius:9999px;border-width:2px;border-color:rgba(0,0,0,.2);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.address-card.selected .radio-circle{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));--tw-shadow:inset 0 0 0 4px #fff;--tw-shadow-colored:inset 0 0 0 4px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.address-selected-label{position:absolute;right:1rem;top:1rem;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(38 65 60/var(--tw-bg-opacity,1));padding:.25rem .75rem;font-size:.65rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1));opacity:0;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.address-card.selected .address-selected-label{opacity:1}.payment-method-card{border-radius:1.5rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1));padding:1.25rem;text-align:center;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.payment-method-card:hover{--tw-border-opacity:1;border-color:rgb(175 132 82/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.payment-method-card.active{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1))}.payment-icon{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.star-rating{display:flex;flex-direction:row-reverse;justify-content:flex-end;gap:.5rem}.star-rating input{display:none}.star-rating label{cursor:pointer;font-size:1.5rem;line-height:2rem;color:rgba(0,0,0,.2);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}.star-rating input:checked~label,.star-rating label:hover,.star-rating label:hover~label{--tw-text-opacity:1;color:rgb(251 191 36/var(--tw-text-opacity,1))}.checkout-radio-card.active{--tw-border-opacity:1;border-color:rgb(38 65 60/var(--tw-border-opacity,1));--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1))}.toast-container{position:fixed;right:1rem;top:1rem;z-index:90;width:100%;max-width:24rem;flex-direction:column}.toast-container,.toast-message{display:flex;gap:.75rem}.toast-message{align-items:flex-start;border-radius:1.4rem;border-width:1px;border-color:rgba(0,0,0,.1);--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1));padding:1rem;opacity:0;--tw-shadow:0 24px 60px rgba(28,43,40,.1);--tw-shadow-colored:0 24px 60px var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow);transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.3s;transform:translateY(-.75rem)}.toast-message.show{opacity:1;transform:translateY(0)}.toast-icon{display:inline-flex;height:2.5rem;width:2.5rem;align-items:center;justify-content:center;border-radius:9999px;--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1));--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.toast-title{font-size:.875rem;line-height:1.25rem;font-weight:600;text-transform:uppercase;letter-spacing:.14em;--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.toast-text{margin-top:.25rem;font-size:.875rem;line-height:1.5rem;color:rgba(0,0,0,.6)}.scroll-reveal{opacity:0;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.7s;transform:translateY(1.25rem)}.scroll-reveal.revealed{opacity:1;transform:translateY(0)}.collapse{visibility:collapse}.static{position:static}.absolute{position:absolute}.relative{position:relative}.end-0{inset-inline-end:0}.right-5{right:1.25rem}.top-0{top:0}.top-5{top:1.25rem}.z-10{z-index:10}.m-0{margin:0}.m-2{margin:.5rem}.m-4{margin:1rem}.mx-1{margin-left:.25rem;margin-right:.25rem}.mx-2{margin-left:.5rem;margin-right:.5rem}.mx-auto{margin-left:auto;margin-right:auto}.\!mb-0{margin-bottom:0!important}.\!mb-2{margin-bottom:.5rem!important}.\!mt-0{margin-top:0!important}.\!mt-3{margin-top:.75rem!important}.mb-0{margin-bottom:0}.mb-1{margin-bottom:.25rem}.mb-2{margin-bottom:.5rem}.mb-3{margin-bottom:.75rem}.mb-4{margin-bottom:1rem}.mb-5{margin-bottom:1.25rem}.mb-8{margin-bottom:2rem}.me-1{margin-inline-end:.25rem}.me-2{margin-inline-end:.5rem}.me-3{margin-inline-end:.75rem}.ms-2{margin-inline-start:.5rem}.ms-auto{margin-inline-start:auto}.mt-0{margin-top:0}.mt-1{margin-top:.25rem}.mt-2{margin-top:.5rem}.mt-3{margin-top:.75rem}.mt-4{margin-top:1rem}.mt-5{margin-top:1.25rem}.mt-6{margin-top:1.5rem}.mt-8{margin-top:2rem}.mt-auto{margin-top:auto}.block{display:block}.inline-block{display:inline-block}.inline{display:inline}.flex{display:flex}.inline-flex{display:inline-flex}.table{display:table}.grid{display:grid}.hidden{display:none}.h-10{height:2.5rem}.h-3{height:.75rem}.h-full{height:100%}.min-h-\[9rem\]{min-height:9rem}.w-10{width:2.5rem}.w-3{width:.75rem}.w-full{width:100%}.min-w-0{min-width:0}.min-w-\[18rem\]{min-width:18rem}.max-w-2xl{max-width:42rem}.max-w-md{max-width:28rem}.max-w-xl{max-width:36rem}.flex-1{flex:1 1 0%}.transform{transform:translate(var(--tw-translate-x),var(--tw-translate-y)) rotate(var(--tw-rotate)) skewX(var(--tw-skew-x)) skewY(var(--tw-skew-y)) scaleX(var(--tw-scale-x)) scaleY(var(--tw-scale-y))}.flex-col{flex-direction:column}.flex-wrap{flex-wrap:wrap}.items-start{align-items:flex-start}.items-center{align-items:center}.justify-center{justify-content:center}.justify-between{justify-content:space-between}.gap-0{gap:0}.gap-1{gap:.25rem}.gap-2{gap:.5rem}.gap-3{gap:.75rem}.gap-4{gap:1rem}.gap-6{gap:1.5rem}.space-y-3>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(.75rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(.75rem*var(--tw-space-y-reverse))}.space-y-5>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(1.25rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(1.25rem*var(--tw-space-y-reverse))}.space-y-6>:not([hidden])~:not([hidden]){--tw-space-y-reverse:0;margin-top:calc(1.5rem*(1 - var(--tw-space-y-reverse)));margin-bottom:calc(1.5rem*var(--tw-space-y-reverse))}.overflow-hidden{overflow:hidden}.rounded{border-radius:.25rem}.rounded-\[1\.25rem\]{border-radius:1.25rem}.rounded-\[1\.5rem\]{border-radius:1.5rem}.rounded-\[1\.75rem\]{border-radius:1.75rem}.rounded-\[2rem\]{border-radius:2rem}.rounded-full{border-radius:9999px}.border{border-width:1px}.border-0{border-width:0}.border-black\/10{border-color:rgba(0,0,0,.1)}.border-black\/20{border-color:rgba(0,0,0,.2)}.border-emerald-100{--tw-border-opacity:1;border-color:rgb(209 250 229/var(--tw-border-opacity,1))}.border-white\/30{border-color:hsla(0,0%,100%,.3)}.bg-black\/5{background-color:rgba(0,0,0,.05)}.bg-canvasia-cream{--tw-bg-opacity:1;background-color:rgb(251 248 242/var(--tw-bg-opacity,1))}.bg-canvasia-sand{--tw-bg-opacity:1;background-color:rgb(244 239 231/var(--tw-bg-opacity,1))}.bg-emerald-50{--tw-bg-opacity:1;background-color:rgb(236 253 245/var(--tw-bg-opacity,1))}.bg-transparent{background-color:transparent}.bg-white{--tw-bg-opacity:1;background-color:rgb(255 255 255/var(--tw-bg-opacity,1))}.bg-white\/30{background-color:hsla(0,0%,100%,.3)}.bg-opacity-10{--tw-bg-opacity:0.1}.bg-opacity-25{--tw-bg-opacity:0.25}.object-cover{-o-object-fit:cover;object-fit:cover}.p-0{padding:0}.p-3{padding:.75rem}.p-4{padding:1rem}.p-5{padding:1.25rem}.p-6{padding:1.5rem}.p-8{padding:2rem}.\!px-4{padding-left:1rem!important;padding-right:1rem!important}.\!py-2{padding-top:.5rem!important;padding-bottom:.5rem!important}.\!py-3{padding-top:.75rem!important;padding-bottom:.75rem!important}.px-3{padding-left:.75rem;padding-right:.75rem}.px-4{padding-left:1rem;padding-right:1rem}.px-5{padding-left:1.25rem;padding-right:1.25rem}.px-6{padding-left:1.5rem;padding-right:1.5rem}.py-2{padding-top:.5rem;padding-bottom:.5rem}.py-3{padding-top:.75rem;padding-bottom:.75rem}.py-4{padding-top:1rem;padding-bottom:1rem}.py-5{padding-top:1.25rem;padding-bottom:1.25rem}.py-8{padding-top:2rem;padding-bottom:2rem}.pb-0{padding-bottom:0}.pb-3{padding-bottom:.75rem}.pb-4{padding-bottom:1rem}.pe-4{padding-inline-end:1rem}.ps-0{padding-inline-start:0}.ps-3{padding-inline-start:.75rem}.ps-4{padding-inline-start:1rem}.pt-3{padding-top:.75rem}.pt-4{padding-top:1rem}.text-center{text-align:center}.text-start{text-align:start}.text-end{text-align:end}.align-middle{vertical-align:middle}.\!text-4xl{font-size:2.25rem!important;line-height:2.5rem!important}.\!text-\[0\.7rem\]{font-size:.7rem!important}.text-3xl{font-size:1.875rem;line-height:2.25rem}.text-4xl{font-size:2.25rem;line-height:2.5rem}.text-\[0\.65rem\]{font-size:.65rem}.text-base{font-size:1rem;line-height:1.5rem}.text-lg{font-size:1.125rem;line-height:1.75rem}.text-sm{font-size:.875rem;line-height:1.25rem}.text-xl{font-size:1.25rem;line-height:1.75rem}.text-xs{font-size:.75rem;line-height:1rem}.font-semibold{font-weight:600}.uppercase{text-transform:uppercase}.leading-6{line-height:1.5rem}.leading-7{line-height:1.75rem}.\!tracking-\[0\.16em\]{letter-spacing:.16em!important}.tracking-\[0\.12em\]{letter-spacing:.12em}.tracking-\[0\.14em\]{letter-spacing:.14em}.\!text-white\/70{color:hsla(0,0%,100%,.7)!important}.text-black\/30{color:rgba(0,0,0,.3)}.text-black\/35{color:rgba(0,0,0,.35)}.text-black\/40{color:rgba(0,0,0,.4)}.text-black\/45{color:rgba(0,0,0,.45)}.text-black\/55{color:rgba(0,0,0,.55)}.text-black\/60{color:rgba(0,0,0,.6)}.text-canvasia-forest{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}.text-canvasia-ink{--tw-text-opacity:1;color:rgb(34 33 31/var(--tw-text-opacity,1))}.text-emerald-700{--tw-text-opacity:1;color:rgb(4 120 87/var(--tw-text-opacity,1))}.text-emerald-900{--tw-text-opacity:1;color:rgb(6 78 59/var(--tw-text-opacity,1))}.text-rose-500{--tw-text-opacity:1;color:rgb(244 63 94/var(--tw-text-opacity,1))}.text-rose-600{--tw-text-opacity:1;color:rgb(225 29 72/var(--tw-text-opacity,1))}.text-white{--tw-text-opacity:1;color:rgb(255 255 255/var(--tw-text-opacity,1))}.text-white\/70{color:hsla(0,0%,100%,.7)}.text-white\/75{color:hsla(0,0%,100%,.75)}.text-white\/80{color:hsla(0,0%,100%,.8)}.opacity-25{opacity:.25}.shadow{--tw-shadow:0 1px 3px 0 rgba(0,0,0,.1),0 1px 2px -1px rgba(0,0,0,.1);--tw-shadow-colored:0 1px 3px 0 var(--tw-shadow-color),0 1px 2px -1px var(--tw-shadow-color)}.shadow,.shadow-lg{box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.shadow-lg{--tw-shadow:0 10px 15px -3px rgba(0,0,0,.1),0 4px 6px -4px rgba(0,0,0,.1);--tw-shadow-colored:0 10px 15px -3px var(--tw-shadow-color),0 4px 6px -4px var(--tw-shadow-color)}.shadow-sm{--tw-shadow:0 1px 2px 0 rgba(0,0,0,.05);--tw-shadow-colored:0 1px 2px 0 var(--tw-shadow-color);box-shadow:var(--tw-ring-offset-shadow,0 0 #0000),var(--tw-ring-shadow,0 0 #0000),var(--tw-shadow)}.outline{outline-style:solid}.filter{filter:var(--tw-blur) var(--tw-brightness) var(--tw-contrast) var(--tw-grayscale) var(--tw-hue-rotate) var(--tw-invert) var(--tw-saturate) var(--tw-sepia) var(--tw-drop-shadow)}.transition{transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,-webkit-backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter;transition-property:color,background-color,border-color,text-decoration-color,fill,stroke,opacity,box-shadow,transform,filter,backdrop-filter,-webkit-backdrop-filter;transition-timing-function:cubic-bezier(.4,0,.2,1);transition-duration:.15s}@media (max-width:1023px){.filter-bar{top:4.25rem}.desktop-summary{display:none}}.hover\:text-rose-600:hover{--tw-text-opacity:1;color:rgb(225 29 72/var(--tw-text-opacity,1))}.group:hover .group-hover\:text-canvasia-forest{--tw-text-opacity:1;color:rgb(38 65 60/var(--tw-text-opacity,1))}@media (min-width:640px){.sm\:col-span-2{grid-column:span 2/span 2}.sm\:inline{display:inline}.sm\:flex-row{flex-direction:row}.sm\:px-8{padding-left:2rem;padding-right:2rem}.sm\:text-5xl{font-size:3rem;line-height:1}}@media (min-width:768px){.md\:grid-cols-2{grid-template-columns:repeat(2,minmax(0,1fr))}}@media (min-width:1024px){.lg\:hidden{display:none}.lg\:grid-cols-\[1fr_auto\]{grid-template-columns:1fr auto}.lg\:flex-row{flex-direction:row}.lg\:items-end{align-items:flex-end}.lg\:justify-between{justify-content:space-between}}@media (min-width:1280px){.xl\:col-span-4{grid-column:span 4/span 4}}
```

## File: KanvasProje.Web/wwwroot/js/admin.js
```javascript
/* ============================================================
   CANVASIA ADMIN — JavaScript Module
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // --- Elements ---
    const sidebar = document.getElementById('caSidebar');
    const toggleBtn = document.getElementById('caSidebarToggle');
    const overlay = document.getElementById('caOverlay');
    const themeToggle = document.getElementById('caThemeToggle');

    // =============================================
    // SIDEBAR TOGGLE
    // =============================================
    if (toggleBtn && sidebar) {
        toggleBtn.addEventListener('click', function () {
            if (window.innerWidth <= 1024) {
                sidebar.classList.toggle('mobile-open');
                if (overlay) overlay.classList.toggle('active');
            } else {
                sidebar.classList.toggle('collapsed');
                localStorage.setItem('ca-sidebar', sidebar.classList.contains('collapsed') ? 'collapsed' : 'open');
            }
        });

        // Overlay click closes sidebar on mobile
        if (overlay) {
            overlay.addEventListener('click', function () {
                sidebar.classList.remove('mobile-open');
                overlay.classList.remove('active');
            });
        }

        // Restore sidebar state from localStorage
        if (window.innerWidth > 1024) {
            var saved = localStorage.getItem('ca-sidebar');
            if (saved === 'collapsed') {
                sidebar.classList.add('collapsed');
            }
        }
    }

    // =============================================
    // LIGHT-ONLY ADMIN EXPERIENCE
    // =============================================
    document.documentElement.removeAttribute('data-theme');
    localStorage.removeItem('ca-theme');

    if (themeToggle) {
        updateThemeIcon('light');
        themeToggle.addEventListener('click', function (event) {
            event.preventDefault();
            document.documentElement.removeAttribute('data-theme');
            localStorage.removeItem('ca-theme');
            updateThemeIcon('light');
        });
    }

    function updateThemeIcon(theme) {
        if (!themeToggle) return;
        var icon = themeToggle.querySelector('i');
        if (icon) {
            icon.className = theme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
        }
    }

    // =============================================
    // AUTO-DISMISS SUCCESS ALERTS
    // =============================================
    document.querySelectorAll('.ca-alert-success[data-auto-dismiss]').forEach(function (alert) {
        setTimeout(function () {
            alert.style.opacity = '0';
            alert.style.transform = 'translateY(-10px)';
            setTimeout(function () { alert.remove(); }, 400);
        }, 4000);
    });

    // =============================================
    // CONTENT FADE-IN
    // =============================================
    var content = document.querySelector('.ca-content');
    if (content) {
        content.classList.add('ca-fade-in');
    }

    // =============================================
    // TOOLTIPS (Bootstrap)
    // =============================================
    var tooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltips.forEach(function (el) {
        new bootstrap.Tooltip(el);
    });

}); // end DOMContentLoaded


// =============================================
// GLOBAL: TOAST NOTIFICATION
// =============================================
function showToast(message, type) {
    type = type || 'info';
    var container = document.getElementById('caToastContainer');
    if (!container) {
        container = document.createElement('div');
        container.id = 'caToastContainer';
        container.className = 'ca-toast-container';
        document.body.appendChild(container);
    }

    var iconMap = {
        success: 'fas fa-check-circle',
        error: 'fas fa-exclamation-circle',
        warning: 'fas fa-exclamation-triangle',
        info: 'fas fa-info-circle'
    };

    var toast = document.createElement('div');
    toast.className = 'ca-toast ' + type;
    toast.innerHTML = '<i class="' + (iconMap[type] || iconMap.info) + '"></i><span>' + message + '</span>';
    container.appendChild(toast);

    setTimeout(function () {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(40px)';
        toast.style.transition = 'all .3s ease';
        setTimeout(function () { toast.remove(); }, 300);
    }, 3500);
}


// =============================================
// GLOBAL: IMAGE PREVIEW (for upload forms)
// =============================================
function previewImage(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('imagePreview');
        var content = document.getElementById('uploadContent');
        if (output) {
            output.src = reader.result;
            output.style.display = 'block';
        }
        if (content) {
            content.style.display = 'none';
        }
    };
    if (event.target.files && event.target.files[0]) {
        reader.readAsDataURL(event.target.files[0]);
    }
}


// =============================================
// GLOBAL: CONFIRM DELETE (SweetAlert fallback)
// =============================================
function confirmDelete(url, itemName) {
    if (typeof Swal !== 'undefined') {
        Swal.fire({
            title: 'Emin misiniz?',
            text: (itemName || 'Bu öğe') + ' silinecektir. Bu işlem geri alınamaz.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#ef4444',
            cancelButtonColor: '#64748b',
            confirmButtonText: 'Evet, Sil',
            cancelButtonText: 'İptal'
        }).then(function (result) {
            if (result.isConfirmed) {
                window.location.href = url;
            }
        });
    } else {
        if (confirm('Bu öğeyi silmek istediğinize emin misiniz?')) {
            window.location.href = url;
        }
    }
    return false;
}
```

## File: KanvasProje.Web/wwwroot/js/canvasia-interactions.js
```javascript
/**
 * Canvasia Premium Interactions
 * 3D Tilt, Fly-to-Cart, Like Bounce, Gallery Mode
 */

(function () {
    'use strict';

    // ==========================================
    // 1. 3D TILT EFFECT (Product Cards)
    // ==========================================
    function initTiltEffect() {
        // Touch cihazlarda tilt yok
        if ('ontouchstart' in window) return;

        document.addEventListener('mousemove', function (e) {
            const cards = document.querySelectorAll('.pro-product-card');
            cards.forEach(function (card) {
                const rect = card.getBoundingClientRect();
                const isHovering =
                    e.clientX >= rect.left &&
                    e.clientX <= rect.right &&
                    e.clientY >= rect.top &&
                    e.clientY <= rect.bottom;

                if (isHovering) {
                    const x = e.clientX - rect.left;
                    const y = e.clientY - rect.top;
                    const centerX = rect.width / 2;
                    const centerY = rect.height / 2;

                    const rotateX = ((y - centerY) / centerY) * -6; // Max ±6°
                    const rotateY = ((x - centerX) / centerX) * 6;

                    card.style.transform = 'perspective(800px) rotateX(' + rotateX + 'deg) rotateY(' + rotateY + 'deg) scale3d(1.02, 1.02, 1.02)';

                    // Dynamic light reflection
                    const glare = card.querySelector('.card-glare');
                    if (glare) {
                        const percentX = (x / rect.width) * 100;
                        const percentY = (y / rect.height) * 100;
                        glare.style.background = 'radial-gradient(circle at ' + percentX + '% ' + percentY + '%, rgba(255,255,255,0.15) 0%, transparent 60%)';
                        glare.style.opacity = '1';
                    }
                }
            });
        });

        // Reset on mouse leave
        document.addEventListener('mouseover', function (e) {
            const card = e.target.closest('.pro-product-card');
            if (!card) {
                document.querySelectorAll('.pro-product-card').forEach(function (c) {
                    c.style.transform = '';
                    const glare = c.querySelector('.card-glare');
                    if (glare) glare.style.opacity = '0';
                });
            }
        });

        // Inject glare overlay div into each product card
        document.querySelectorAll('.pro-product-card').forEach(function (card) {
            if (!card.querySelector('.card-glare')) {
                var glare = document.createElement('div');
                glare.className = 'card-glare';
                card.appendChild(glare);
            }
        });
    }

    // ==========================================
    // 2. FLY-TO-CART ANIMATION
    // ==========================================
    window.flyToCart = function (btnElement) {
        var card = btnElement.closest('.pro-product-card');
        if (!card) return;

        var img = card.querySelector('.pro-img');
        if (!img) return;

        // Find cart icon in header
        var cartIcon = document.querySelector('.header-cart-count') ||
            document.querySelector('.header-cart .fa-bag-shopping') ||
            document.querySelector('.mobile-bottom-link .fa-bag-shopping') ||
            document.querySelector('.desktop-cart-count') ||
            document.querySelector('.desktop-nav-item .fa-shopping-bag') ||
            document.querySelector('.cart-indicator') ||
            document.querySelector('.fa-shopping-bag');

        if (!cartIcon) return;

        var imgRect = img.getBoundingClientRect();
        var cartRect = cartIcon.getBoundingClientRect();

        // Create flying clone
        var flyingImg = document.createElement('img');
        flyingImg.src = img.src;
        flyingImg.className = 'flying-cart-item';
        flyingImg.style.cssText =
            'position:fixed;' +
            'top:' + imgRect.top + 'px;' +
            'left:' + imgRect.left + 'px;' +
            'width:' + imgRect.width + 'px;' +
            'height:' + imgRect.height + 'px;' +
            'object-fit:contain;' +
            'z-index:9999;' +
            'pointer-events:none;' +
            'border-radius:10px;' +
            'box-shadow:0 5px 20px rgba(0,0,0,0.3);' +
            'transition: all 0.75s cubic-bezier(0.2, 1, 0.3, 1);';

        document.body.appendChild(flyingImg);

        // Force reflow before starting animation
        flyingImg.offsetHeight;

        // Animate to cart
        var targetX = cartRect.left + cartRect.width / 2 - 20;
        var targetY = cartRect.top + cartRect.height / 2 - 20;

        flyingImg.style.top = targetY + 'px';
        flyingImg.style.left = targetX + 'px';
        flyingImg.style.width = '40px';
        flyingImg.style.height = '40px';
        flyingImg.style.opacity = '0.3';
        flyingImg.style.borderRadius = '50%';

        // Cart icon bounce on arrival
        setTimeout(function () {
            if (cartIcon) {
                cartIcon.classList.add('cart-bounce');
                setTimeout(function () {
                    cartIcon.classList.remove('cart-bounce');
                }, 600);
            }
            flyingImg.remove();
        }, 750);
    };

    // ==========================================
    // 3. LIKE BUTTON BOUNCE
    // ==========================================
    function initLikeBounce() {
        document.addEventListener('click', function (e) {
            var likeBtn = e.target.closest('.like-btn');
            if (!likeBtn) return;

            likeBtn.classList.add('like-pop');

            // Particle burst
            for (var i = 0; i < 6; i++) {
                var particle = document.createElement('span');
                particle.className = 'heart-particle';
                particle.innerHTML = '❤';
                particle.style.cssText =
                    '--angle:' + (i * 60) + 'deg;' +
                    '--delay:' + (i * 0.05) + 's;';
                likeBtn.appendChild(particle);
            }

            setTimeout(function () {
                likeBtn.classList.remove('like-pop');
                likeBtn.querySelectorAll('.heart-particle').forEach(function (p) { p.remove(); });
            }, 700);
        });
    }

    // ==========================================
    // 4. LEGACY THEME CLEANUP
    // ==========================================
    window.toggleGalleryMode = function () {
        document.documentElement.removeAttribute('data-theme');
        localStorage.removeItem('canvasia-theme');
    };

    function applySavedTheme() {
        document.documentElement.removeAttribute('data-theme');
        localStorage.removeItem('canvasia-theme');
    }

    // ==========================================
    // 5. SCROLL REVEAL (Bonus micro-interaction)
    // ==========================================
    function initScrollReveal() {
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('revealed');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.1, rootMargin: '0px 0px -50px 0px' });

        document.querySelectorAll('.product-item').forEach(function (item) {
            item.classList.add('scroll-reveal');
            observer.observe(item);
        });
    }

    // ==========================================
    // 6. TOAST NOTIFICATION SYSTEM
    // ==========================================
    window.showToast = function (title, message, type = 'success') {
        // Container var mı kontrol et, yoksa oluştur
        let container = document.querySelector('.toast-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'toast-container';
            document.body.appendChild(container);
        }

        // İkon belirle
        let iconClass = 'fas fa-check-circle';
        if (type === 'error') iconClass = 'fas fa-times-circle';
        if (type === 'info') iconClass = 'fas fa-info-circle';
        if (type === 'warning') iconClass = 'fas fa-exclamation-triangle';

        // Toast elementini oluştur
        const toast = document.createElement('div');
        toast.className = `toast-message toast-${type}`;
        toast.innerHTML = `
            <div class="toast-icon"><i class="${iconClass}"></i></div>
            <div class="toast-content">
                <div class="toast-title">${title}</div>
                <div class="toast-text">${message}</div>
            </div>
        `;

        // Container'a ekle (En üste)
        container.appendChild(toast);

        // Animasyon için frame bekle
        requestAnimationFrame(() => {
            toast.classList.add('show');
        });

        // Ses efekti (Opsiyonel, kısa ve hoş bir ses)
        // const audio = new Audio('/sounds/pop.mp3');
        // audio.volume = 0.2;
        // audio.play().catch(e => {}); 

        // 4 saniye sonra kaldır
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => {
                toast.remove();
            }, 400); // Transition süresi kadar bekle
        }, 4000);
    };

    // ==========================================
    // 7. WELCOME NOTIFICATION
    // ==========================================
    function initWelcomeMessage() {
        const hasVisited = localStorage.getItem('storefront-visited');

        if (!hasVisited) {
            setTimeout(() => {
                window.showToast('Hos Geldiniz', 'Yeni urunleri ve kampanyalari kesfetmeye baslayin.', 'info');
                localStorage.setItem('storefront-visited', 'true');
            }, 2000); // 2 saniye sonra göster
        } else {
            // Belki geri dönen kullanıcıya "Tekrar hoşgeldin" diyebiliriz (Session bazlı)
            const sessionVisited = sessionStorage.getItem('session-welcome');
            if (!sessionVisited) {
                // Sadece çok şık ve rahatsız etmeyen bir hatırlatma
                // window.showToast('Tekrar Hoşgeldiniz', 'Sizi tekrar görmek güzel!', 'success');
                sessionStorage.setItem('session-welcome', 'true');
            }
        }
    }

    // ==========================================
    // 8. GLOBAL ADD TO CART & FAVORITES
    // ==========================================

    // Favori İşlemleri
    window.toggleLike = window.toggleFavorite = function (urunId, btn) {
        // UI Update
        const icon = btn.querySelector('i');
        const isLiked = btn.classList.contains('liked');

        if (isLiked) {
            btn.classList.remove('liked');
            icon.classList.remove('fas');
            icon.classList.add('far');
            window.showToast('Favorilerden Çıkarıldı', 'Ürün favorilerinizden kaldırıldı.', 'info');
        } else {
            btn.classList.add('liked');
            icon.classList.remove('far');
            icon.classList.add('fas');

            // Like animation
            btn.classList.add('like-pop');
            setTimeout(() => btn.classList.remove('like-pop'), 300);

            window.showToast('Favorilere Eklendi', 'Ürün favorilerinize eklendi.', 'success');
        }

        // Backend'e favori toggle isteği
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        fetch('/Favori/Toggle', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token || ''
            },
            body: 'urunId=' + urunId
        })
            .then(r => r.json())
            .then(data => {
                if (!data.success) {
                    // Giriş yapılmamışsa UI'ı geri al ve uyar
                    if (isLiked) {
                        btn.classList.add('liked');
                        icon.classList.remove('far');
                        icon.classList.add('fas');
                    } else {
                        btn.classList.remove('liked');
                        icon.classList.remove('fas');
                        icon.classList.add('far');
                    }
                    window.showToast('Uyarı', data.message || 'Lütfen giriş yapınız.', 'warning');
                }
            })
            .catch(() => {
                // Hata durumunda UI'ı geri al
                if (isLiked) {
                    btn.classList.add('liked');
                    icon.classList.remove('far');
                    icon.classList.add('fas');
                } else {
                    btn.classList.remove('liked');
                    icon.classList.remove('fas');
                    icon.classList.add('far');
                }
            });
    };

    // Global Sepete Ekle Fonksiyonu
    window.addToCart = function (btnElement, urunId, isDetail = false) {
        // Button ve Icon Seçimi
        let btnWrapper = btnElement.closest('.neon-btn-wrapper') || btnElement;
        let btnInner = btnWrapper.querySelector('.neon-btn-inner') || btnElement;
        let icon = btnInner.querySelector('i');
        const originalHtml = btnInner.innerHTML;

        // Veri Hazırlığı
        let payload = { UrunId: urunId, Adet: 1 };
        let url = '/Sepet/Ekle?UrunId=' + urunId + '&Adet=1';

        // Detay sayfasından geliyorsa varyant ve adet al
        if (isDetail) {
            const form = document.getElementById('addToCartForm');
            if (form) {
                const formData = new FormData(form);
                payload.SecenekId = formData.get('SecenekId');
                payload.Adet = formData.get('Adet');
                url = `/Sepet/Ekle?UrunId=${payload.UrunId}&Adet=${payload.Adet}`;
                if (payload.SecenekId) url += `&SecenekId=${payload.SecenekId}`;
            }
        }

        // 1. Loading Durumu
        btnInner.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Ekleniyor...';
        btnWrapper.style.pointerEvents = 'none';

        // 2. İstek Gönder - CSRF token ekle
        const token = document.querySelector('#addToCartForm input[name="__RequestVerificationToken"]')?.value
            || document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            }
        })
            .then(response => {
                if (!response.ok) throw new Error('Sunucu hatası');
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    // BAŞARILI
                    btnInner.innerHTML = '<i class="fas fa-check"></i> Eklendi';
                    btnInner.style.background = '#4CAF50';
                    btnInner.style.color = '#fff'; // Yazı rengi beyaz

                    // Toast
                    window.showToast('Sepete Eklendi 🛒', 'Ürün başarıyla sepetinize eklendi.', 'success');

                    // Uçma Animasyonu (Listeleme sayfalarındaysa)
                    if (!isDetail && window.flyToCart) {
                        window.flyToCart(btnWrapper);
                    }

                    // Sepet Sayacını Güncelle
                    const indicators = document.querySelectorAll('.cart-indicator, .desktop-cart-count');
                    indicators.forEach(ind => {
                        let count = parseInt(ind.textContent) || 0;
                        ind.textContent = count + parseInt(payload.Adet);
                    });

                    // 2 Saniye sonra reset
                    setTimeout(() => {
                        btnInner.innerHTML = originalHtml;
                        btnInner.style.background = '';
                        btnInner.style.color = '';
                        btnWrapper.style.pointerEvents = 'auto';
                    }, 2000);

                } else {
                    // HATA (Stok yok vb.)
                    window.showToast('Hata', data.message, 'error');
                    btnInner.innerHTML = originalHtml;
                    btnWrapper.style.pointerEvents = 'auto';
                }
            })
            .catch(error => {
                console.error(error);
                window.showToast('Hata', 'Bir sorun oluştu.', 'error');
                btnInner.innerHTML = '<i class="fas fa-times"></i> Hata';
                btnInner.style.background = '#F44336';
                setTimeout(() => {
                    btnInner.innerHTML = originalHtml;
                    btnInner.style.background = '';
                    btnWrapper.style.pointerEvents = 'auto';
                }, 2000);
            });
    };

    // ==========================================
    // INIT
    // ==========================================
    applySavedTheme();

    document.addEventListener('DOMContentLoaded', function () {
        initTiltEffect();
        initLikeBounce();
        initScrollReveal();
        initWelcomeMessage();

        // Update gallery mode button state
        var saved = localStorage.getItem('canvasia-theme');
        if (saved === 'gallery') {
            var btn = document.getElementById('galleryModeBtn');
            if (btn) {
                var icon = btn.querySelector('i');
                if (icon) icon.className = 'fas fa-sun';
                var label = btn.querySelector('.gallery-mode-label');
                if (label) label.textContent = 'Aydınlık Mod';
            }
            // Mobile toggle icon
            var mobileBtn = document.querySelector('.mobile-gallery-toggle');
            if (mobileBtn) {
                var mobileIcon = mobileBtn.querySelector('i');
                if (mobileIcon) mobileIcon.className = 'fas fa-sun';
            }
        }

        // Detay Sayfası Butonu Bağlama
        const detailBtn = document.getElementById('ajaxSepetBtn');
        if (detailBtn) {
            detailBtn.addEventListener('click', function (e) {
                e.preventDefault(); // Form submit engelle
                const urunId = document.querySelector('input[name="UrunId"]').value;
                window.addToCart(this, urunId, true);
            });
        }
    });
})();
```

## File: KanvasProje.Web/wwwroot/js/premium-motion.js
```javascript
(function () {
    'use strict';

    var reduceMotion = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    var mobileQuery = window.matchMedia ? window.matchMedia('(max-width: 767px)') : null;

    function ready(callback) {
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', callback, { once: true });
        } else {
            callback();
        }
    }

    function nextFrame(callback) {
        requestAnimationFrame(function () {
            requestAnimationFrame(callback);
        });
    }

    function revealNow(element) {
        element.classList.add('is-visible');
    }

    function initHeroMotion() {
        var hero = document.querySelector('[data-motion-hero]');
        if (!hero) return;

        hero.classList.add('motion-hero-prep');
        if (reduceMotion) {
            revealNow(hero);
            return;
        }

        nextFrame(function () {
            hero.classList.add('is-visible');
            window.setTimeout(function () {
                hero.classList.remove('motion-hero-prep', 'is-visible');
            }, 760);
        });
    }

    function initScrollReveal() {
        var sections = Array.prototype.slice.call(document.querySelectorAll('[data-motion-section]'));
        var staggerGroups = Array.prototype.slice.call(document.querySelectorAll('[data-motion-stagger]'));
        var items = [];

        sections.forEach(function (section) {
            section.classList.add('motion-reveal');
        });

        staggerGroups.forEach(function (group) {
            var groupItems = Array.prototype.slice.call(group.querySelectorAll('[data-motion-item]'));
            var delayStep = mobileQuery && mobileQuery.matches ? 28 : 55;

            groupItems.forEach(function (item, index) {
                item.classList.add('motion-stagger-item');
                item.style.setProperty('--motion-delay', Math.min(index * delayStep, 420) + 'ms');
                items.push(item);
            });
        });

        var targets = sections.concat(items);
        if (!targets.length) return;

        if (reduceMotion || !('IntersectionObserver' in window)) {
            targets.forEach(revealNow);
            return;
        }

        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (!entry.isIntersecting) return;

                entry.target.classList.add('is-animating');
                revealNow(entry.target);
                observer.unobserve(entry.target);

                window.setTimeout(function () {
                    entry.target.classList.remove('is-animating');
                }, 900);
            });
        }, {
            threshold: 0.12,
            rootMargin: '0px 0px -8% 0px'
        });

        targets.forEach(function (target) {
            observer.observe(target);
        });
    }

    function setMobileItemDelays(drawer) {
        var items = Array.prototype.slice.call(drawer.querySelectorAll('[data-mobile-nav-item]'));
        items.forEach(function (item, index) {
            item.style.setProperty('--mobile-nav-delay', Math.min(70 + (index * 28), 360) + 'ms');
        });

        Array.prototype.slice.call(drawer.querySelectorAll('[data-mobile-category]')).forEach(function (details) {
            Array.prototype.slice.call(details.querySelectorAll('[data-mobile-subitem]')).forEach(function (item, index) {
                item.style.setProperty('--mobile-sub-delay', Math.min(index * 22, 260) + 'ms');
            });
        });
    }

    function initMobileNavigation() {
        var drawer = document.getElementById('mobileNav');
        var backdrop = document.getElementById('mobileNavBackdrop');
        var closeTimer = null;

        if (!drawer || !backdrop) return;

        setMobileItemDelays(drawer);

        window.toggleMobileNav = function (open) {
            var shouldOpen = typeof open === 'boolean' ? open : drawer.classList.contains('hidden');
            window.clearTimeout(closeTimer);

            if (shouldOpen) {
                drawer.classList.remove('hidden');
                backdrop.classList.remove('hidden');
                document.documentElement.classList.add('overflow-hidden');
                document.body.classList.add('overflow-hidden');

                nextFrame(function () {
                    drawer.classList.add('is-open');
                    backdrop.classList.add('is-open');
            });
            // Adjust dropdown position to stay within viewport
            var parentLi = dropdown.closest('[data-category-menu-item]');
            if (parentLi) {
                parentLi.addEventListener('mouseenter', function () {
                    requestAnimationFrame(() => {
                        var vw = window.innerWidth;
                        var rect = dropdown.getBoundingClientRect();
                        var overflowLeft = rect.left < 0;
                        var overflowRight = rect.right > vw;
                        if (overflowLeft) {
                            dropdown.style.left = '0';
                            dropdown.style.right = '';
                            dropdown.style.transform = 'translateX(0)';
                        } else if (overflowRight) {
                            dropdown.style.left = '';
                            dropdown.style.right = '0';
                            dropdown.style.transform = 'translateX(0)';
                        } else {
                            dropdown.style.left = '50%';
                            dropdown.style.right = '';
                            dropdown.style.transform = 'translateX(-50%)';
                        }
                    });
                });
                parentLi.addEventListener('focusin', function () {
                    var vw = window.innerWidth;
                    var rect = dropdown.getBoundingClientRect();
                    var overflowLeft = rect.left < 0;
                    var overflowRight = rect.right > vw;
                    if (overflowLeft) {
                        dropdown.style.left = '0';
                        dropdown.style.right = '';
                        dropdown.style.transform = 'translateX(0)';
                    } else if (overflowRight) {
                        dropdown.style.left = '';
                        dropdown.style.right = '0';
                        dropdown.style.transform = 'translateX(0)';
                    } else {
                        dropdown.style.left = '50%';
                        dropdown.style.right = '';
                        dropdown.style.transform = 'translateX(-50%)';
                    }
                });
            }
                return;
            }

            drawer.classList.remove('is-open');
            backdrop.classList.remove('is-open');
            document.documentElement.classList.remove('overflow-hidden');
            document.body.classList.remove('overflow-hidden');

            closeTimer = window.setTimeout(function () {
                drawer.classList.add('hidden');
                backdrop.classList.add('hidden');
            }, reduceMotion ? 0 : 430);
        };
    }

    function initCategoryPreview() {
        var dropdowns = Array.prototype.slice.call(document.querySelectorAll('[data-category-dropdown]'));
        if (!dropdowns.length) return;

        dropdowns.forEach(function (dropdown) {
            var panel = dropdown.querySelector('[data-category-preview-panel]');
            if (!panel) return;

            var image = panel.querySelector('[data-category-preview-image]');
            var title = panel.querySelector('[data-category-preview-title]');
            var text = panel.querySelector('[data-category-preview-text]');
            var links = Array.prototype.slice.call(dropdown.querySelectorAll('[data-category-preview-title][href]'));
            var changeTimer = null;

            function ensureImage(src, alt) {
                if (!src) return;

                var media = panel.querySelector('.category-preview-media');
                if (!image && media) {
                    image = document.createElement('img');
                    image.setAttribute('data-category-preview-image', '');
                    image.className = 'h-full w-full object-cover';
                    image.loading = 'lazy';
                    image.decoding = 'async';
                    media.innerHTML = '';
                    media.appendChild(image);
                }

                if (image) {
                    image.src = src;
                    image.alt = alt || '';
                }
            }

            function updatePreview(link) {
                var src = link.getAttribute('data-category-preview-src') || '';
                var nextTitle = link.getAttribute('data-category-preview-title') || link.textContent.trim();
                var nextText = link.getAttribute('data-category-preview-text') || 'Bu koleksiyondaki tasarımları keşfedin.';

                window.clearTimeout(changeTimer);
                panel.classList.add('is-preview-changing');

                changeTimer = window.setTimeout(function () {
                    ensureImage(src, nextTitle);
                    if (title) title.textContent = nextTitle;
                    if (text) text.textContent = nextText;
                    panel.classList.remove('is-preview-changing');
                }, reduceMotion ? 0 : 120);
            }

            links.forEach(function (link) {
                link.addEventListener('mouseenter', function () { updatePreview(link); });
                link.addEventListener('focus', function () { updatePreview(link); });
            });
            // Adjust dropdown position to stay within viewport using fixed positioning
            var parentLi = dropdown.closest('[data-category-menu-item]');
            if (parentLi) {
                var originalLeft = dropdown.style.left;
                var originalTransform = dropdown.style.transform;
                function positionDropdown() {
                    requestAnimationFrame(() => {
                        var vw = window.innerWidth;
                        var rect = dropdown.getBoundingClientRect();
                        // Switch to fixed to escape hero video layer
                        dropdown.style.position = 'fixed';
                        dropdown.style.top = rect.top + 'px';
                        // Determine horizontal overflow
                        if (rect.left < 0) {
                            dropdown.style.left = '0px';
                            dropdown.style.right = '';
                            dropdown.style.transform = 'translateX(0)';
                        } else if (rect.right > vw) {
                            dropdown.style.left = '';
                            dropdown.style.right = '0px';
                            dropdown.style.transform = 'translateX(0)';
                        } else {
                            dropdown.style.left = rect.left + 'px';
                            dropdown.style.right = '';
                            dropdown.style.transform = 'translateX(0)';
                        }
                    });
                }
                function resetPosition() {
                    dropdown.style.position = '';
                    dropdown.style.top = '';
                    dropdown.style.left = originalLeft;
                    dropdown.style.right = '';
                    dropdown.style.transform = originalTransform;
                }
                parentLi.addEventListener('mouseenter', positionDropdown);
                parentLi.addEventListener('focusin', positionDropdown);
                parentLi.addEventListener('mouseleave', resetPosition);
                parentLi.addEventListener('focusout', resetPosition);
            }
        });
    }

    function initDesktopNavOverflow() {
        var nav = document.querySelector('[data-desktop-nav]');
        var list = document.querySelector('[data-desktop-nav-list]');
        if (!nav || !list) return;

        function update() {
            var items = list.children;
            var lastItem = items[items.length - 1];
            var firstItem = items[0];
            var parent = nav.parentElement;
            var parentRect = parent.getBoundingClientRect();
            var lastRect = lastItem.getBoundingClientRect();
            var firstRect = firstItem.getBoundingClientRect();
            
            var overflowsLeft = firstRect.left < parentRect.left;
            var overflowsRight = lastRect.right > parentRect.right;

            if (overflowsLeft || overflowsRight) {
                list.classList.remove('justify-center');
                list.classList.add('justify-start');
            } else {
                list.classList.remove('justify-start');
                list.classList.add('justify-center');
            }
        }

        update();
        var timer = null;
        window.addEventListener('resize', function () {
            window.clearTimeout(timer);
            timer = window.setTimeout(update, 80);
        });
    }

    ready(function () {
        initHeroMotion();
        initScrollReveal();
        initMobileNavigation();
        initCategoryPreview();
        initDesktopNavOverflow();
    });
})();
```

## File: KanvasProje.Web/wwwroot/js/site.js
```javascript
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
```
