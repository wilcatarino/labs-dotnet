using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace ApacheKafkaProducer;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Testando o envio de mensagens com Kafka");

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

        try
        {
            ProducerConfig producerConfig = new ProducerConfig()
            {
                BootstrapServers = bootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = saslUsername,
                SaslPassword = saslPassword,
                EnableIdempotence = true,
                ClientId = "test-apache-kafka-producer"
            };

            using (IProducer<Null, string> producerBuilder = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                for (int i = 0; i < 10; i++)
                {
                    Message<Null, string> message = new Message<Null, string>()
                    {
                        Value = $"[{DateTime.Now.ToString("s")}] Envio {i}"
                    };
                    DeliveryResult<Null, string> produceResult = await producerBuilder.ProduceAsync(topicName, message);

                    Console.WriteLine($"Mensagem enviada: {message.Value}");
                    Console.WriteLine($"Status: {produceResult.Status.ToString()} | Offset: {produceResult.Offset} | Partition: {produceResult.Partition}");
                    Console.WriteLine("-------------------------");
                }
            }

            Console.WriteLine("Todas mensagens foram enviadas");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
        }
    }
}
