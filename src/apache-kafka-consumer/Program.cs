using Confluent.Kafka;
using System;
using System.Threading;

namespace ApacheKafkaConsumer;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Testando o consumo de mensagens com Kafka");

        string bootstrapServers = args[0];
        string saslUsername = args[1];
        string saslPassword = args[2];
        string topicName = args[3];
        string groupId = args[4];

        Console.WriteLine("-------------------------");
        Console.WriteLine($"BootstrapServers = {bootstrapServers}");
        Console.WriteLine($"SaslUsername = {saslUsername}");
        Console.WriteLine($"SaslPassword = {saslPassword}");
        Console.WriteLine($"Topic = {topicName}");
        Console.WriteLine($"GroupId = {groupId}");
        Console.WriteLine("-------------------------");

        ConsumerConfig consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = bootstrapServers,
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
            SaslUsername = saslUsername,
            SaslPassword = saslPassword,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            // HeartbeatIntervalMs = 300000,
            // MaxPollIntervalMs = 60000,
            ClientId = "test-apache-kafka-consumer"
        };

        CancellationTokenSource cancellation = new CancellationTokenSource();
        Console.CancelKeyPress += (_, cancellationEvent) =>
        {
            cancellationEvent.Cancel = true;
            cancellation.Cancel();
        };

        try
        {
            using (IConsumer<Ignore, string> consumerBuilder = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                Console.WriteLine($"Subcrevendo no topic {topicName}");
                consumerBuilder.Subscribe(topicName);

                try
                {
                    while (true)
                    {
                        ConsumeResult<Ignore, string> consumerResult = consumerBuilder.Consume(cancellation.Token);
                        Console.WriteLine($"Mensagem lida: {consumerResult.Message.Value}");
                        Console.WriteLine($"Offset: {consumerResult.Offset} | Partition: {consumerResult.Partition}");
                        Console.WriteLine("-------------------------");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                    Console.WriteLine("Cancelada a execução do Consumer");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
        }
    }
}
