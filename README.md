# Database MQTT Monitor

A .NET application that monitors a database file size and publishes the information to an MQTT broker. This tool is useful for tracking database growth and integrating database size metrics into your monitoring systems.

## Features

- Monitors database file size
- Publishes size information to MQTT broker
- Configurable through environment variables
- Supports MQTT authentication
- Retains last message on MQTT broker

## Prerequisites

- .NET 6.0 or later
- MQTT broker
- Access to the database file you want to monitor

## Configuration

Create a `.env` file in the project root with the following environment variables:

```env
DB_PATH=/path/to/your/database/file
MQTT_BROKER=your.mqtt.broker.address
MQTT_TOPIC=your/mqtt/topic
MQTT_USERNAME=your_mqtt_username
MQTT_PASSWORD=your_mqtt_password
```

### Environment Variables

- `DB_PATH`: Path to the database file you want to monitor
- `MQTT_BROKER`: Address of your MQTT broker
- `MQTT_TOPIC`: MQTT topic to publish the database size information
- `MQTT_USERNAME`: Username for MQTT authentication
- `MQTT_PASSWORD`: Password for MQTT authentication

## Building and Running

1. Clone the repository
2. Create and configure your `.env` file
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

## MQTT Message Format

The application publishes JSON messages in the following format:

```json
{
  "timestamp": "2024-03-21T12:00:00Z",
  "db_size_bytes": 1234567
}
```

- `timestamp`: UTC timestamp of when the measurement was taken
- `db_size_bytes`: Size of the database file in bytes

## MQTT Configuration

- Quality of Service: At Least Once (QoS 1)
- Retain Flag: Enabled (last message is retained on the broker)
- Keep Alive Period: 60 seconds
- Connection Timeout: 10 seconds

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright (c) 2025 Richard Dyer
