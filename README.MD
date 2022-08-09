This is a sample of an .net console application that utilizes Auth0 device code flow to get an access token.
To try this out replace the config an appsettings.json:

- `ClientId` - your Auth0 application client id (must be enabled for device code flow)
- `Domain` - your Auth0 tenants domain - find it in the management dashboard
- `Audience` - the Audience of the API you want to access - find it in the management dashboard

You need a Tenant with a API you want to access and a application that is configured to use Device Flow.
General documentation regarding device code flow and how to setup your tenant can be found here: https://auth0.com/docs/quickstart/native/device/01-login


