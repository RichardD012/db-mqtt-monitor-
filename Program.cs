using System.Text.Json;
using MQTTnet;
using MQTTnet.Protocol;

DotNetEnv.Env.Load(".env");

string GetRequiredEnvVar(string name)
{
    var value = Environment.GetEnvironmentVariable(name);
    if (string.IsNullOrEmpty(value))
    {
        Console.WriteLine($"Error: Required environment variable '{name}' is not set");
        Environment.Exit(1);
    }
    return value;
}

long GetFileSize(string path)
{
    try
    {
        FileInfo fileInfo = new FileInfo(path);
        return fileInfo.Length;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return 0;
    }
}

var dbPath = GetRequiredEnvVar("DB_PATH");
var mqttBroker = GetRequiredEnvVar("MQTT_BROKER");
var mqttTopic = GetRequiredEnvVar("MQTT_TOPIC");
var mqttUsername = GetRequiredEnvVar("MQTT_USERNAME");
var mqttPassword = GetRequiredEnvVar("MQTT_PASSWORD");


var mqttFactory = new MqttClientFactory();

using (var mqttClient = mqttFactory.CreateMqttClient())
{
    var mqttClientOptions = new MqttClientOptionsBuilder()
        .WithTcpServer(mqttBroker)
        .WithCredentials(mqttUsername, mqttPassword)
        .WithClientId($"plex-db-monitor")
        .WithCleanSession()
        .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
        .WithTimeout(TimeSpan.FromSeconds(10))
        .Build();

    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

    var deviceConfig = new
    {
        timestamp = DateTime.UtcNow,
        db_size_bytes = GetDbSize(dbPath),
    };

    await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
        .WithTopic(mqttTopic)
        .WithPayload(JsonSerializer.Serialize(deviceConfig))
        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
        .WithRetainFlag()
        .Build());

    await mqttClient.DisconnectAsync();

    Console.WriteLine("MQTT application message is published.");
}