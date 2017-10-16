# QlikAuthentication

Prerequisite:
1. Export the Qlik certificate and place it to your local dev server
2. Import the Qlik certificate (Client.pfx) to Personal folder. Make sure you use the Local Computer account when importing the certificate
3. Add the authority to your current Default App Pool which you use it in your IIS web application. Just click Managa Private key, and add the Default App Pool
4. In IIS, make sure the identity of Default App Pool is ApplicationPoolIdentity


Qlik Session will only work if the Qlik Server is one domain of web-app. Injecting session cookie to different domain is prohibited by W3 standard. I am thinking of using Reverse Proxy so user only point to a single domain for both web-app and Qlik Server:

Example:
qlikserver.web-url.com
application.web-url.com

As above servers are behind reverse proxy, it means we can inject same cookie for both application servers.
