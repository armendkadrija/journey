# journey

This app will store telemetry data from a MQTT broker (mqtt.hsl.fi) and make them searcheable from http endpoint.

# Migrations

dotnet ef migrations add "init" --project src/Infrastructure --startup-project src/WebUI --output-dir Persistence/Migrations
