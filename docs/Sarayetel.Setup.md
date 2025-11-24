# Sarayetel Infrastructure & Localization Setup

This addendum captures the working conventions for the Sarayetel deployment. Follow these steps before adding RTL theming, Gemini-powered support tooling, or other customizations.

## 1. Container stack

1. Copy `.env.example` to `.env` and replace secrets.
2. Start the stack: `docker compose up -d --build`.
3. The services run locally on the following ports:
   - Storefront: http://localhost:8080
   - SQL Server Express 2022: port 1433
   - PostgreSQL 16 (optional datastore): port 5432
   - Redis 7.4: port 6379
4. Persistent volumes keep `App_Data`, logs, plugins, wwwroot assets, SQL data, PostgreSQL data, and Redis snapshots between restarts.

### Required environment variables

| Variable | Purpose |
| --- | --- |
| `NOP_SQL_SA_PASSWORD` | Strong SA password for the SQL container. |
| `NOP_DB_CONNECTION_STRING` | Default SQL Server connection string for the installer/background jobs. |
| `NOP_PG_USER` / `NOP_PG_PASSWORD` / `NOP_PG_DATABASE` | Credentials for the optional PostgreSQL container. |
| `NOP_PG_CONNECTION_STRING` | Pre-built PostgreSQL connection string (switch to this during installation if you choose PostgreSQL). |
| `NOP_DISTRIBUTED_CACHE_TYPE` | Leave as `redis` for a pure distributed cache, or `redissynchronizedmemory` if you want local caching plus Redis event propagation. |
| `NOP_REDIS_CONNECTION_STRING` | Redis host definition (defaults to the bundled container). |
| `NOP_REDIS_INSTANCE_NAME` | Logical partition name so multiple apps can share a Redis server. |

## 2. First-time store install

1. Browse to `http://localhost:8080/install` after the containers start.
2. Use the connection string stored in `NOP_DB_CONNECTION_STRING` (or paste the same text) when the wizard asks for database details. SQL authentication *must* match the SA password in the `.env` file. If you prefer PostgreSQL, flip the data provider to **PostgreSQL** in the installer and paste the `NOP_PG_CONNECTION_STRING` value instead.
3. After finishing the wizard, `App_Data/dataSettings.json` will be created inside the `app_data` Docker volume.
4. Keep the Redis cache enabled by leaving the environment variables in place; sarayetel will auto-detect them.

## 3. Persian and Arabic localization

1. Place the provided `language_pack.fa-IR.xml` inside the repository (recommended path: `src/Localization/language_pack.fa-IR.xml`). This keeps the file versioned and makes CI/CD artifacting easier.
2. From the Admin area → **Configuration → Languages**, click **Add language** → **Import resources**, select the Persian XML file, and mark the language as RTL.
3. Repeat for Arabic (once its XML is available).
4. Update the theme fonts to include families such as Vazirmatn (Persian) and Tajawal (Arabic) and verify all resource keys.

## 4. RTL theming roadmap

1. Duplicate `Themes/DefaultClean` into `Themes/Sarayetel`.
2. Set `"supportRtl": true` in the new theme's `theme.json`.
3. Create `wwwroot/css/rtl.css` (or equivalent bundle) that flips layout primitives, slider directions, and iconography.
4. Update storefront assets (logo, hero banners) with localized art.
5. Capture screenshots of key flows (catalog, cart, checkout) in both fa-IR and ar-SA for QA sign-off.

## 5. Gemini-powered support tooling

1. Create a secure secret store for the Google Gemini API key (e.g., environment variable or Azure Key Vault reference consumed at runtime).
2. Build a custom sarayetel plugin that exposes an admin widget for support agents.
3. Sanitize the payload (order/customer context) before calling Gemini via the official REST API.
4. Log every request/response pair for auditing.

## 6. Login policy hardening

1. Enforce multi-factor authentication for administrators (`Nop.Plugin.MultiFactorAuth.GoogleAuthenticator`).
2. Configure lockout thresholds and CAPTCHA for customer logins.
3. Audit sign-in attempts (success and failure) via a scheduled task that exports to centralized logging.

## 7. Git workflow

1. Branch from `main` using a descriptive name, e.g., `feature/rtl-l10n`.
2. Commit infrastructure and localization assets separately from UI changes to keep reviews small.
3. Push to the client repository `git@github.com:shokry110/sarayetel.net.git` and open a PR for review.
4. Include deployment notes referencing this file so operations can reproduce the environment.

Keep this document updated as we add Gemini integration, new plugins, or additional locales.
