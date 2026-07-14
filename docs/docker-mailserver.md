# Docker Mailserver SMTP Setup

The application sends email through standard authenticated SMTP. It does not depend on a specific email provider.

## Mail Domain

Use a mail subdomain for the active site domain. For the current site this would normally be:

```text
mail.7anrps48.com
```

Create a mailbox before configuring the application. For example:

```text
info@7anrps48.com
```

With Docker Mailserver, create the mailbox using its setup command from the directory that contains the mailserver configuration:

```powershell
docker exec -it mailserver setup email add info@7anrps48.com <strong-password>
```

Do not put the password in a tracked file or command history.

## DNS Records

Add these records at the DNS provider. Mail records must be DNS-only, never proxied through Cloudflare.

```text
A       mail.7anrps48.com       <public-server-ip>
MX      7anrps48.com            10 mail.7anrps48.com
TXT     7anrps48.com            v=spf1 mx -all
TXT     _dmarc.7anrps48.com     v=DMARC1; p=quarantine; rua=mailto:postmaster@7anrps48.com
```

Docker Mailserver generates the DKIM public key after DKIM is enabled. Add its generated TXT record at the selector shown by Docker Mailserver.

The server IP also needs a reverse DNS (PTR) record pointing to `mail.7anrps48.com`. Configure this in the server provider panel. Without a valid PTR, many receivers will reject or spam-folder messages.

## TLS And Firewall

Use a trusted TLS certificate whose name matches `mail.7anrps48.com`. Port `587` is the application submission port and must use STARTTLS. Keep these publicly reachable only when the mail server is intended to receive external email:

```text
25   SMTP receiving
465  SMTPS, optional
587  SMTP submission with STARTTLS
993  IMAPS, optional
```

Do not use port `465` with the application's `SmtpClient`; it uses STARTTLS on port `587`.

## Application Configuration

Create a non-tracked `.env` file beside `docker-compose.yml`:

```dotenv
EMAIL_HOST=host.docker.internal
EMAIL_PORT=587
EMAIL_ENABLE_SSL=true
EMAIL_USERNAME=info@7anrps48.com
EMAIL_PASSWORD=<mailbox-password>
EMAIL_FROM_EMAIL=info@7anrps48.com
EMAIL_FROM_NAME=7ANRPS48
```

`host.docker.internal` is configured in this project's Compose file and reaches the host-published mailserver port. If both containers are attached to the same Docker network, use `EMAIL_HOST=mailserver` instead.

Apply the variables by recreating the web container:

```powershell
docker compose up -d --force-recreate web
```

Then use the **Mail Infrastructure Test** action on `/Admin/Ayarlar` to send a test email. Check the mailserver logs if delivery fails.

## Important Notes

- Docker Mailserver is open source, but the server, static IP, DNS, TLS certificate, and outbound port 25 availability are infrastructure requirements.
- Some hosting providers block outbound port 25. Request removal of the restriction or use an SMTP relay for outbound delivery.
- Never proxy mail DNS records through Cloudflare.
- Keep mailbox passwords only in `.env`, Docker secrets, or another secret manager.
