const { CosmosClient } = require("@azure/cosmos");

const endpoint = process.env["CosmosEndpoint"];
const key = process.env["CosmosKey"];
const client = new CosmosClient({ endpoint, key });


module.exports = async function (context, documents) {

    const { database } = await client.databases.createIfNotExists({ id: "bhoojal_outlets" });
    const { container } = await database.containers.createIfNotExists({ id: "outlet" });

    const { resources } = await container.items.query("SELECT * from c").fetchAll();
    const outlet = resources.find(({id}) => id == documents[0].outletId);
    const alertJson = documents[0];
    const type = alertJson.alert.metrics[0].metric;
    const value = alertJson.alert.metrics[0].value;
    const threshold = alertJson.alert.metrics[0].threshold;  

    var emailBody = "<html><body>Hello,<br /><br />There is an alert raised on your registered ground water outlet <b>" 
        + outlet.id + 
        "</b>.<br />Alert raised for <b>"
        + type + 
        "</b>.<br />Metrics <b>" 
        + Math.round(value)
        + "</b> has crossed the threshold <b>"
        + threshold + 
        "</b>.<br /><br />Thank You,<br /><br />Bhoojal Team";


    var message = {
        "personalizations": [ { "to": [ { "email": outlet.email } ] } ],
       from: { email: "rohan.nevrikar@gmail.com" },        
       subject: "Bhoojal: Alert - Registered outlet needs attention",
       content: [{
           type: 'text/html',
           value: emailBody
       }]
   };

   context.done(null, {message});
}
