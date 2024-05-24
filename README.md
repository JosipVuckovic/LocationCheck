# LocationCheck

Simple API that calls Google Places API to look up places around a given coordinate.

Solution includes 
- API
- RequestNotifyConsole (SignalR console that receives updates on each request)
- Security part
- Data sources
- External (Google API)

To use, appsettings must be provided with the required data.
Requires a working Google Places API key and SQL database.

Each request in Swagger must be provided with a RequestId Guid.
RequestId is used to ensure idempotency of all requests. 
(All operations without HTTP verb restrictions will return existing responses for same key, didn't implement stalling so old gets will also return their response)

Api uses BasicAuth handler which checks for api key, can be set globally in Swagger using "Basic {testUserKey}". A test API user is seeded with a migration.

RequestNotifyConsole is provided with a default value for SignarR hub, check before use if the port is the same.
