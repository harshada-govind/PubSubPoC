﻿
using System;
using System.Data.SqlClient;
using NServiceBus;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace MessageSubscriber.Configuration
{
    public class NServiceBusConfiguration
    {
        protected readonly string EndpointName;
        protected readonly EndpointConfiguration EndpointConfiguration;
        protected readonly IConfiguration _configuration;
        public NServiceBusConfiguration(string endpointName, IConfiguration configuration)
        {
            EndpointName = endpointName;
            _configuration = configuration;
            EndpointConfiguration = new EndpointConfiguration(endpointName);
        }
        public EndpointConfiguration Build()
        {
            string nsbLicense = $"{ AppDomain.CurrentDomain.BaseDirectory }/license/license.xml";
            string messageSubscriberConnection = _configuration.GetConnectionString("MessageSubscriberDBContext");

            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            endpointConfiguration.LicensePath(nsbLicense);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.AddDeserializer<XmlSerializer>();
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            serialization.Settings(settings);
            string NServiceBusConnection = _configuration.GetConnectionString("NServiceBus_Transport");
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.ConnectionString(NServiceBusConnection);
            transport.SubscriptionSettings().DisableSubscriptionCache();
            var subscriptions = transport.SubscriptionSettings();
            subscriptions.SubscriptionTableName(
                tableName: "SubscriptionRouting",
                schemaName: "dbo",
                catalogName: "MessageSubscriber");
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.ConnectionBuilder(() => new SqlConnection(messageSubscriberConnection));
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();

            return endpointConfiguration;
        }
    }
}