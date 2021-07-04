'use strict';

require('dotenv').config()

// Set connection string
console.log(process.env.IOTHUB_CONNECTIONSTRING)
var connectionString = process.env.IOTHUB_CONNECTIONSTRING;


var Mqtt = require('azure-iot-device-mqtt').Mqtt;
var DeviceClient = require('azure-iot-device').Client
var Message = require('azure-iot-device').Message;
const chalk = require('chalk');

var bhoojalMeterConfig = {
  outletId: null,
  city: null,
  quota: 0
};

var client = DeviceClient.fromConnectionString(connectionString, Mqtt);

// Create a message and send it to the IoT hub every second
let consumedQuota = 0;
setInterval(function(){
  // Simulate telemetry.
  consumedQuota += 50 + (Math.random() * 20);
  // always safe metrics
  let phLevel = 6.5 + (Math.random() * 2.5);
  // Usually moderate hard water, rarely exceeding harmful levels
  let hardness = 80 + (Math.random() * 120);
  var message = new Message(JSON.stringify({
    consumedQuota: consumedQuota,
    hardness: hardness,
    phLevel: phLevel,
    timestamp: new Date()
  }));

  // Add a custom application property to the message.
  // An IoT hub can filter on these properties without access to the message body.
  message.contentType = "application/json";
  message.contentEncoding = "utf-8";
  //message.properties.add('consumptionAlert', (consumptionAlert > 30000) ? 'true' : 'false');

  console.log('Sending message: ' + message.getData());

  // Send the message.
  client.sendEvent(message, function (err) {
    if (err) {
      console.error('send error: ' + err.toString());
    } else {
      console.log('message sent');
    }
  });
}, 5000);

client.getTwin(function(err, twin) {
  if (err) {
    console.error(chalk.red('Could not get device twin'));
  } else {
    console.log(chalk.green('Device twin created'));
    console.log(twin.properties.desired);
    // Handle all desired property updates
    twin.on('properties.desired', function(delta) {
      console.log(chalk.yellow('\nNew desired properties received in patch:'));
      console.log(delta);
      bhoojalMeterConfig.outletId = delta.outletId;
      bhoojalMeterConfig.city = delta.city;
      bhoojalMeterConfig.quota = delta.quota;
    });
  }
});

client.open(function (err) {
  if (err) {
    console.log("Erroring opening device connection");
  }
  else {
    console.log("Device connection opened")
    client.on("message", function (msg) {
      console.log('Id: ' + msg.messageId);
      console.log("Data follows:");
      console.log(JSON.parse(msg.data));
      // When using MQTT the following line is a no-op.
      client.complete(msg, printResultFor('completed'));
    })
  }
})

// Helper function to print results in the console
function printResultFor(op) {
  return function printResult(err, res) {
    if (err) console.log(op + ' error: ' + err.toString());
    if (res) console.log(op + ' status: ' + res.constructor.name);
  };
}