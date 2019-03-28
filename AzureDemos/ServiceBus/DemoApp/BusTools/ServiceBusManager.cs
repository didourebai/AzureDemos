﻿using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureDemos.ServiceBus.DemoApp.BusTools
{
    class ServiceBusManager
    {
        private string connectionString;
        private ManagementClient mgtClient;

        public ServiceBusManager(string serviceBusConnectionString)
        {
            this.connectionString = serviceBusConnectionString;
            mgtClient = new ManagementClient(serviceBusConnectionString);
        }

        #region Queue Management

        public async Task CreateQueue(string queuePath)
        {
            await CreateQueue(new QueueDescription(queuePath), false);
        }
        public async Task CreateQueue(string queuePath, TimeSpan messageTTL, bool enableDeadLetteringOnMessageExpiration, bool deleteAndRemakeIfExists)
        {
            await CreateQueue(new QueueDescription(queuePath)
            {
                DefaultMessageTimeToLive = messageTTL,
                EnableDeadLetteringOnMessageExpiration = enableDeadLetteringOnMessageExpiration
            }, deleteAndRemakeIfExists);
        }

        public async Task CreateQueue(QueueDescription description, bool deleteAndRemakeIfExists)
        {
            var exists = await mgtClient.QueueExistsAsync(description.Path);

            if (exists && deleteAndRemakeIfExists)
            {
                await mgtClient.DeleteQueueAsync(description.Path);
                exists = false;
            }

            if (!exists)
            {
                await mgtClient.CreateQueueAsync(description);
            }

        }

        #endregion

    }
}